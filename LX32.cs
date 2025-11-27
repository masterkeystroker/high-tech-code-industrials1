using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Creta.ModBus;

namespace PLC
{
    public class clsLX32 : IDisposable
    {
        #region General

        private clsMBMaestro master;
        private System.Threading.Thread thServo;

        private const double factorConversion = 10000.0;
        private const double Tolerancia = 0.1;

        public event delSuceso Suceso;

        private volatile int paso = 0;
        private volatile int mbTimeOut = 50;
        private volatile bool _CalidadOK = false;
        private volatile bool _Alarma = false;

        private eEstadoLX32Deseado _ModoDeseado = eEstadoLX32Deseado.Cero;
        private eEstadoLX32Deseado _ModoActual = eEstadoLX32Deseado.Cero; //Integer = 0  4  '1= Manual, 2= Automático
        private eEstadoLX32Real _EstadoReal = eEstadoLX32Real.Cero;

        public clsLX32(clsMBMaestro MB, byte Direccion)
        {
            master = MB;
            _DirMb = Direccion;
            Scan();
        }

        /// <summary>
        /// Ciclo de scan del objeto
        /// </summary>
        public void Scan()
        {
            try
            {
                if (thServo != null && thServo.IsAlive == false)
                {
                    thServo = null;
                }
                if (thServo == null && disposedValue == false)
                {
                    thServo = new System.Threading.Thread(Servo);
                    thServo.IsBackground = true;
                    thServo.Name = "Servo:" + master.Puerto.ToString() + "." + _DirMb.ToString();
                    thServo.Start();
                }
            }
            catch (Exception ex)
            {
                if (Suceso != null) Suceso(100, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Hilo principal
        /// </summary>
        private void Servo()
        {
            _EstadoReal = eEstadoLX32Real.Cero;
            bool Primera = false;

            while (disposedValue == false)
            {
                try
                {
                    //Subproceso del servo
                    if (master.Conectar)
                    {
                        //Si hay fallo de comunicacion quita el inicializado
                        if (!_CalidadOK) paso = 0;

                        //Escribe el controlWord (solo si ha cambiado)
                        if (!EscribeControlWord()) paso = 0;

                        //Lee el status word cada 300ms, y los errores si hay
                        if (DateTime.Now > tLeerStatusWord.AddMilliseconds(250)) LeeStatusWord();

                        //Alarma en el lexium
                        if (DateTime.Now >= tLeeError.AddMilliseconds(1000)) LeeError();

                        //Lee la posicion actual cada segundo
                        if (DateTime.Now > tLeerPosActual.AddMilliseconds(1000)) LeePosicionActual();

                        //si no voltage paso=0
                        if (!_SWVoltage) paso = 0;

                        //si no tengo el control escribo el 282
                        if (!_SWRemoto)
                        {
                            //Intenta tomar el control del servo
                            escribirModbus(282, 1);
                            LeeStatusWord();
                            paso = 0;
                        }
                        else
                        {
                            //Modo general de trabajo
                            if (_ModoDeseado != _ModoActual)
                            {
                                //Al cambio de modo se obliga a pasar por el estado 0
                                if (_ModoActual == eEstadoLX32Deseado.Cero)
                                {
                                    _ModoActual = _ModoDeseado;
                                }
                                else
                                {
                                    _ModoActual = eEstadoLX32Deseado.Cero;
                                    paso = 1;
                                    _PosicionadoSeguro = false;
                                }
                            }

                            if (paso == 0)
                            {
                                //Espera a que la comunicacion funcione
                                if (Inicializar() && _CalidadOK) paso = 1;
                            }
                            else if (paso == 1)
                            {
                                //Pone el servo en ready
                                _EstadoReal = eEstadoLX32Real.Cero;

                                if (EstableceRdy())
                                    paso = 2;
                                else
                                    paso = 0;
                            }
                            else if (paso == 2)
                            {
                                //Si el estado es diferente de 0 pone el servo en marcha y pasa al siguiente paso
                                _EstadoReal = eEstadoLX32Real.Cero;
                                Primera = true;
                                if (_ModoActual != eEstadoLX32Deseado.Cero && !_Alarma) paso = 3;
                            }
                            else if (paso == 3)
                            {
                                if (!_Alarma)
                                {
                                    if (_ModoActual == eEstadoLX32Deseado.Autotune)
                                    {
                                        autotuning(ref Primera);
                                    }
                                    else if (_ModoActual == eEstadoLX32Deseado.Homing)
                                    {
                                        homing(ref Primera);
                                    }
                                    else if (_ModoActual == eEstadoLX32Deseado.Posicionado)
                                    {
                                        if (_SWReferenciado)
                                            Posicionar(ref Primera);
                                        else
                                            _EstadoReal = eEstadoLX32Real.PosicionadoFinErr;
                                    }
                                    else if (_ModoActual == eEstadoLX32Deseado.Jog)
                                    {
                                        jog(ref Primera);
                                    }
                                    else
                                    {
                                        _EstadoReal = eEstadoLX32Real.Cero;
                                    }
                                }
                                else
                                {
                                    paso = 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        //No esta conectado al modbus
                        paso = 0;
                        System.Threading.Thread.Sleep(500);
                    }
                }
                catch (Exception ex)
                {
                    if (Suceso != null) Suceso(100, ex.Message, ex.StackTrace);
                    System.Threading.Thread.Sleep(200);
                }
                finally
                {
                    _CWFaultReset = false;
                    System.Threading.Thread.Sleep(5);
                }
            }
        }

        private DateTime HoraRun = DateTime.Now;

        private bool Inicializar()
        {
            //leemos las resolución del encoder definida en el lexium.
            sLeerDatosDobles datosLeidos;
            if (leerModbus(1550, 1, out datosLeidos))
            {
                double ScalePOSdenom = datosLeidos.Datos[0];

                if (leerModbus(1322, 1, out datosLeidos))
                {
                    _ResolucionEncoder = (ScalePOSdenom / datosLeidos.Datos[0]) * 4;
                    _ResolucionEncoder = (25000 / datosLeidos.Datos[0]) * 4;

                    //Escribe el node warding
//#if DEBUG
//                    if (!escribirModbus(5644, 0))
//#else
//                    if (!escribirModbus(5644, 3000))
//#endif
//                    {
                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private bool EstableceRun()
        {
            //escribimos el PosReg1Start, los límites por Sw y la ganancia.
            if (escribirModbus(2820, 1) &&
                escribirModbus(1542, new int[] { 3, _positiveLimit, _negativeLimit }) &&
                escribirModbus(4394, _CTRL_GlobGain))
            {
                _CWSwitchOn = true;
                _CWVoltage = true;
                _CWEnableOperation = true;
                _CWOperatingMode = 0;
                EscribeControlWord();

                HoraRun = DateTime.Now.AddMilliseconds(300);
                System.Threading.Thread.Sleep(100);

                LeeStatusWord();
                if (_SWStatus4 == 7) //7= RUN
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        private bool EstableceRdy()
        {
            //inicializa el servo
            _CWSwitchOn = false;
            _CWVoltage = false;
            _CWEnableOperation = false;
            _CWOperatingMode = 0;
            return EscribeControlWord();
        }
        private bool guardarEnEPROM()
        {
            return escribirModbus(1026, 3);
        }
        
        #endregion
        #region Comunicaciones

        private byte _DirMb;
        private int _ContTx = 0;

        private bool escribirModbus(int direccionModbus, int valor)
        {
            _ContTx++;
            if (master.EscribirPalabrasDobles(_DirMb, direccionModbus, new int[] { valor }, mbTimeOut) != 0)
            {
                _CalidadOK = false;
                paso = 0;
                System.Threading.Thread.Sleep(200);
                return false;
            }
            else
            {
                _CalidadOK = true;
                return true;
            }
        }
        private bool escribirModbus(int direccionModbus, int[] valores)
        {
            _ContTx++;
            if (master.EscribirPalabrasDobles(_DirMb, direccionModbus, valores, mbTimeOut) != 0)
            {
                _CalidadOK = false;
                paso = 0;
                System.Threading.Thread.Sleep(200);
                return false;
            }
            else
            {
                _CalidadOK = true;
                return true;
            }
        }
        private bool leerModbus(int DirMemoria, int npalabras, out sLeerDatosDobles lectura)
        {
            _ContTx++;
            lectura = master.LeerPalabrasDobles(_DirMb, DirMemoria, npalabras, mbTimeOut);
            if (lectura.Erro != 0)
            {
                _CalidadOK = false;
                paso = 0;
                System.Threading.Thread.Sleep(200);
                return false;
            }
            else
            {
                _CalidadOK = true;
                return true;
            }
        }

        public byte DirMb
        {
            get
            {
                return _DirMb;
            }
        }
        public int ContTx
        {
            get
            {
                return _ContTx;
            }
        }

        #endregion
        #region Leer StatusWord

        private DateTime tLeerStatusWord;
        private bool _SWVoltage;
        private bool _SWWarning;
        private bool _SWHALT;
        private bool _SWRemoto;
        private bool _SWTargetReached;
        private bool _SWReserved;
        private bool _SWOperatingMode;
        private bool _SWError;
        private bool _SWEnd;
        private bool _SWReferenciado;
        private int _SWStatus3;
        private int _SWStatus4;
        private int _operatingModeActivo;

        public bool SWVoltage
        {
            get
            {
                return _SWVoltage;
            }
        }
        public bool SWWarning
        {
            get
            {
                return _SWWarning;
            }
        }
        public bool SWHALT
        {
            get
            {
                return _SWHALT;
            }
        }
        public bool SWRemoto
        {
            get
            {
                return _SWRemoto;
            }
        }
        public bool SWTargetReached
        {
            get
            {
                return _SWTargetReached;
            }
        }
        public bool SWReserved
        {
            get
            {
                return _SWReserved;
            }
        }
        public bool SWOperatingMode
        {
            get
            {
                return _SWOperatingMode;
            }
        }
        public bool SWError
        {
            get
            {
                return _SWError;
            }
        }
        public bool SWEnd
        {
            get
            {
                return _SWEnd;
            }
        }
        public bool Referenciado
        {
            get
            {
                return _SWReferenciado;
            }
        }
        public int SWStatus3
        {
            get
            {
                return _SWStatus3;
            }
        }
        public int SWSTatus4
        {
            get
            {
                return _SWStatus4;
            }
        }

        private bool LeerPosActualAux1;

        private bool LeeStatusWord()
        {
            tLeerStatusWord = DateTime.Now;

            sLeerDatosDobles datosLeidos;
            if (leerModbus(6916, 3, out datosLeidos))
            {
                //recopilo status word (6916)
                _SWVoltage = BitOfLong(datosLeidos.Datos[0], 4);
                _SWWarning = BitOfLong(datosLeidos.Datos[0], 7);
                _SWHALT = BitOfLong(datosLeidos.Datos[0], 8);
                _SWRemoto = BitOfLong(datosLeidos.Datos[0], 9);
                _SWTargetReached = BitOfLong(datosLeidos.Datos[0], 10);
                _SWReserved = BitOfLong(datosLeidos.Datos[0], 11);
                _SWOperatingMode = BitOfLong(datosLeidos.Datos[0], 12);
                _SWError = BitOfLong(datosLeidos.Datos[0], 13);
                _SWEnd = BitOfLong(datosLeidos.Datos[0], 14);
                _SWReferenciado = BitOfLong(datosLeidos.Datos[0], 15);

                //pagina 205 del manual

                //8 fallo
                //7 run
                //0 disable (sin seguridad seta)

                _SWStatus4 = 0;
                _SWStatus4 = BitToLong(_SWStatus4, 0, BitOfLong(datosLeidos.Datos[0], 0));
                _SWStatus4 = BitToLong(_SWStatus4, 1, BitOfLong(datosLeidos.Datos[0], 1));
                _SWStatus4 = BitToLong(_SWStatus4, 2, BitOfLong(datosLeidos.Datos[0], 2));
                _SWStatus4 = BitToLong(_SWStatus4, 3, BitOfLong(datosLeidos.Datos[0], 3));

                _SWStatus3 = 0;
                _SWStatus3 = BitToLong(_SWStatus3, 0, BitOfLong(datosLeidos.Datos[0], 5));
                _SWStatus3 = BitToLong(_SWStatus3, 1, BitOfLong(datosLeidos.Datos[0], 6));

                //recopilo la active operating mode (6920)
                _operatingModeActivo = datosLeidos.Datos[2];

                if (_ModoActual == eEstadoLX32Deseado.Posicionado && (_SWEnd || _SWError))
                {
                    if (!LeerPosActualAux1)
                    {
                        LeePosicionActual();
                        LeerPosActualAux1 = this.HaLlegado;
                    }
                }
                else
                {
                    LeerPosActualAux1 = false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
        #region Escribir ControlWord

        private int _ultimoControlWord = -1;

        private bool _CWSwitchOn = false;
        private bool _CWVoltage = false;
        private bool _CWStop = false;
        private bool _CWEnableOperation = false;
        private bool _CWFaultReset = false;
        private bool _CWHALT = false;
        private bool _CWChangeSetPoint = false;
        private short _CWOperatingMode = 0;
        private short _CWReserved = 0;

        private bool EscribeControlWord()
        {
            try
            {
                int valor = 0;
                valor = BitToLong(valor, 0, _CWSwitchOn);
                valor = BitToLong(valor, 1, _CWVoltage);
                valor = BitToLong(valor, 2, !_CWStop); //Parada rápida
                valor = BitToLong(valor, 3, _CWEnableOperation);
                valor = BitToLong(valor, 7, _CWFaultReset);
                valor = BitToLong(valor, 8, _CWHALT);
                valor = BitToLong(valor, 9, _CWChangeSetPoint);
                valor = BitToLong(valor, 4, BitOfShort(_CWOperatingMode, 0));
                valor = BitToLong(valor, 5, BitOfShort(_CWOperatingMode, 1));
                valor = BitToLong(valor, 6, BitOfShort(_CWOperatingMode, 2));
                valor = BitToLong(valor, 10, BitOfShort(_CWReserved, 0));
                valor = BitToLong(valor, 11, BitOfShort(_CWReserved, 1));
                valor = BitToLong(valor, 12, BitOfShort(_CWReserved, 2));
                valor = BitToLong(valor, 13, BitOfShort(_CWReserved, 3));
                valor = BitToLong(valor, 14, BitOfShort(_CWReserved, 4));
                valor = BitToLong(valor, 15, BitOfShort(_CWReserved, 5));

                _CWFaultReset = false;

                if (valor != _ultimoControlWord)
                {
                    if (escribirModbus(6914, valor))
                    {
                        _ultimoControlWord = valor;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (Suceso != null) Suceso(100, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public bool CWSwitchOn
        {
            get
            {
                return _CWSwitchOn;
            }
        }
        public bool CWVoltage
        {
            get
            {
                return _CWVoltage;
            }
        }
        public bool CWEnableOperation
        {
            get
            {
                return _CWEnableOperation;
            }
        }
        public bool CWChangeSetPoint
        {
            get
            {
                return _CWChangeSetPoint;
            }
        }
        public short CWOperatingMode
        {
            get
            {
                return _CWOperatingMode;
            }
        }

        public void Reset()
        {
            _CWFaultReset = true;
        }
        public bool CWStop
        {
            get
            {
                return _CWStop;
            }
            set
            {
                _CWStop = value;
            }
        }
        public bool CWHALT
        {
            get
            {
                return _CWHALT;
            }
            set
            {

                _CWHALT = value;
            }
        }

        #endregion
        #region Leer errores

        private int _LastError;

        //WarnLatched
        private bool _WLGeneral = false;
        private bool _WLOutRange = false;
        private bool _WLActiveOperatingMode = false;
        private bool _WLCOmissioningInterface = false;
        private bool _WLFieldbus = false;
        private bool _WLFollowingWarningLimit = false;
        private bool _WLInputs = false;
        private bool _WLLowVoltage = false;
        private bool _WLEncoderInteface = false;
        private bool _WLTemperatureMotorHigh = false;
        private bool _WLTemperaturePowerHigh = false;
        private bool _WLMemoryCard = false;
        private bool _WLFieldbusMode = false;
        private bool _WLFieldbusEncoder = false;
        private bool _WLFieldbusSafety = false;
        private bool _WLBrakingResistorOverload = false;
        private bool _WLPowerStageOverload = false;
        private bool _WLMotorOverload = false;

        private DateTime tLeeError;

        private bool LeeError()
        {
            sLeerDatosDobles datosLeidos;
            tLeeError = DateTime.Now;

            if (leerModbus(7178, 1, out datosLeidos))
            {
                //Error causing a stop
                _LastError = datosLeidos.Datos[0];

                if (leerModbus(7192, 1, out datosLeidos))
                {
                    //Saved warnings, bit-coded (392)
                    _WLGeneral = BitOfLong(datosLeidos.Datos[0], 0);
                    _WLOutRange = BitOfLong(datosLeidos.Datos[0], 2);
                    _WLActiveOperatingMode = BitOfLong(datosLeidos.Datos[0], 4);
                    _WLCOmissioningInterface = BitOfLong(datosLeidos.Datos[0], 5);
                    _WLFieldbus = BitOfLong(datosLeidos.Datos[0], 6);
                    _WLFollowingWarningLimit = BitOfLong(datosLeidos.Datos[0], 8);
                    _WLInputs = BitOfLong(datosLeidos.Datos[0], 10);
                    _WLLowVoltage = BitOfLong(datosLeidos.Datos[0], 13);
                    _WLEncoderInteface = BitOfLong(datosLeidos.Datos[0], 16);
                    _WLTemperatureMotorHigh = BitOfLong(datosLeidos.Datos[0], 17);
                    _WLTemperaturePowerHigh = BitOfLong(datosLeidos.Datos[0], 18);
                    _WLMemoryCard = BitOfLong(datosLeidos.Datos[0], 20);
                    _WLFieldbusMode = BitOfLong(datosLeidos.Datos[0], 21);
                    _WLFieldbusEncoder = BitOfLong(datosLeidos.Datos[0], 22);
                    _WLFieldbusSafety = BitOfLong(datosLeidos.Datos[0], 23);
                    _WLBrakingResistorOverload = BitOfLong(datosLeidos.Datos[0], 29);
                    _WLPowerStageOverload = BitOfLong(datosLeidos.Datos[0], 30);
                    _WLMotorOverload = BitOfLong(datosLeidos.Datos[0], 31);

                    if (leerModbus(7184, 1, out datosLeidos))
                    {
                        //TODO extraer los bits para hacer el diagnostico
                        if (datosLeidos.Datos[0] == 0)
                        {
                            if (_EstadoReal == eEstadoLX32Real.Alarma) _EstadoReal = eEstadoLX32Real.Cero;
                            _Alarma = false;
                        }
                        else
                        {
                            _EstadoReal = eEstadoLX32Real.Alarma;
                            _Alarma = true;
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion
        #region Leer posicion

        private DateTime tLeerPosActual;
        private double _posicionActual;

        private bool LeePosicionActual()
        {
            try
            {
                tLeerPosActual = DateTime.Now;
                sLeerDatosDobles datosLeidos;
                if (leerModbus(7706, 1, out datosLeidos))
                {
                    _posicionActual = datosLeidos.Datos[0] / factorConversion;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (Suceso != null) Suceso(100, ex.Message, ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Devuelve la distancia del servo hasta el punto indicado
        /// </summary>
        /// <param name="Punto">Punto que se quiere medir</param>
        /// <returns>Distancia del punto al servo</returns>
        public double DistanciaHasta(double Punto)
        {
            return Math.Abs(_posicionActual - Punto);
        }

        #endregion

        #region Jog

        //características del Jog
        private int _ultimoJog = -1;
        private bool _Jog = false;
        private bool _direccionJog;
        private bool _velocidadJog;
        private double _vRapidaJog = 100.0;
        private double _vLentaJog = 100.0;
        private double _vRapidaJog2 = 100.0;
        private double _vLentaJog2 = 100.0;

        /// <summary>
        /// Velocidad Jog lenta
        /// </summary>
        public double velocidadLentaJog
        {
            get
            {
                return _vLentaJog;
            }
            set
            {
                _vLentaJog = value;
            }
        }

        /// <summary>
        /// Velocidad Jog rápida
        /// </summary>
        public double velocidadRapidaJog
        {
            get
            {
                return _vRapidaJog;
            }
            set
            {
                _vRapidaJog = value;
            }
        }

        /// <summary>
        /// Selecciona la velocidad del Jog
        /// 1 rápida, 0 lenta
        /// </summary>
        public bool velocidadJog
        {
            get
            {
                return _velocidadJog;
            }
            set
            {
                _velocidadJog = value;
            }
        }

        /// <summary>
        /// Direccion en manual.
        /// true: dirección positiva; false: dirección negativa
        /// </summary>
        public bool direccionJog
        {
            get
            {
                return _direccionJog;
            }
            set
            {
                _direccionJog = value;
            }
        }

        /// <summary>
        /// Marcha del movimiento
        /// </summary>
        public bool moviendoJog
        {
            get
            {
                return _Jog;
            }
            set
            {
                _Jog = value;
            }
        }

        private void jog(ref bool Primera)
        {
            _EstadoReal = eEstadoLX32Real.Jog;

            if (_operatingModeActivo == -1 && !Primera && _SWStatus4 == 7)
            {
                int valor = 0;
                valor = BitToLong(valor, 0, _Jog && _direccionJog);
                valor = BitToLong(valor, 1, _Jog && !_direccionJog);
                valor = BitToLong(valor, 2, _Jog && _velocidadJog);

                if (valor != _ultimoJog)
                {
                    //Movimientos en manual
                    if (escribirModbus(6930, valor))
                        _ultimoJog = valor;
                    else
                        paso = 1;
                }
                else if (_vRapidaJog2 != _vRapidaJog || _vLentaJog2 != _vLentaJog)
                {
                    //Si ha cambiado la velocidad la escribe
                    if (escribirModbus(10504, new int[] { Convert.ToInt32(_vLentaJog), Convert.ToInt32(_vRapidaJog) }))
                    {
                        _vRapidaJog2 = _vRapidaJog;
                        _vLentaJog2 = _vLentaJog;
                    }
                }
            }
            else
            {
                //configura velocidades Jog y pasa a esperar movimiento.
                if (escribirModbus(10504, new int[] { Convert.ToInt32(_vLentaJog), Convert.ToInt32(_vRapidaJog) }))
                {
                    _vRapidaJog2 = _vRapidaJog;
                    _vLentaJog2 = _vLentaJog;

                    if (escribirModbus(6918, -1))
                    {
                        if (LeeStatusWord())
                        {
                            if (_operatingModeActivo == -1 && _SWStatus4 == 1 && EstableceRun())
                            {
                                _Jog = false;
                                Primera = false;
                            }
                            else
                            {
                                paso = 1;
                            }
                        }
                        else
                        {
                            paso = 1;
                        }
                    }
                    else
                    {
                        paso = 1;
                    }
                }
                else
                {
                    paso = 1;
                }
            }
        }

        #endregion
        #region Homing

        //TODO añadir variables de velocidad y otras relacionadas con el homming

        //características del homing
        private double _posicionHoming = 0.0;
        private int _modoHoming = 35;

        /// <summary>
        /// Homing method:
        ///  1: LIMN with index pulse
        ///  2: LIMP with index pulse
        ///  7: REF+ with index pulse, inv., outside
        ///  8: REF+ with index pulse, inv., inside
        ///  9: REF+ with index pulse, not inv., inside
        ///  10: REF+ with index pulse, not inv., outside
        ///  11: REF- with index pulse, inv., outside
        ///  12: REF- with index pulse, inv., inside
        ///  13: REF- with index pulse, not inv., inside
        ///  14: REF- with index pulse, not inv., outside
        ///  17: LIMN
        ///  18: LIMP
        ///  23: REF+, inv., outside
        ///  24: REF+, inv., inside
        ///  25: REF+, not inv., inside
        ///  26: REF+, not inv., outside
        ///  27: REF-, inv., outside
        ///  28: REF-, inv., inside
        ///  29: REF-, not inv., inside
        ///  30: REF-, not inv., outside
        ///  33: Index pulse neg. direction
        ///  34: Index pulse pos. direction
        ///  35: Position setting
        /// </summary>
        public int HomingModo
        {
            get
            {
                return _modoHoming;
            }
            set
            {
                _modoHoming = value;
            }
        }

        public double HomingPosicion
        {
            get
            {
                return _posicionHoming;
            }
            set
            {
                _posicionHoming = value;
            }
        }

        private void homing(ref bool Primera)
        {
            if (_operatingModeActivo == 6 && !Primera && _SWStatus4 == 7)
            {
                if (_SWReferenciado)
                {
                    _EstadoReal = eEstadoLX32Real.HomingFinOk;
                }
                else
                {
                    _EstadoReal = eEstadoLX32Real.HomingProceso;
                }
            }
            else if (_operatingModeActivo == 6 && !Primera && _SWStatus4 == 8)
            {
                _EstadoReal = eEstadoLX32Real.HomingFinErr;
            }
            else
            {
                _EstadoReal = eEstadoLX32Real.HomingProceso;

                //deshabilitar límites por Sw
                if (escribirModbus(1542, 0))
                {
                    if (escribirModbus(6918, 6))
                    {
                        if (LeeStatusWord())
                        {
                            if (_operatingModeActivo == 6 && _SWStatus4 == 1)
                            {
                                //EstableceRun() no sirve en este modo porque se activan los limites
                                _CWSwitchOn = true;
                                _CWVoltage = true;
                                _CWEnableOperation = true;
                                _CWOperatingMode = 0;
                                EscribeControlWord();

                                System.Threading.Thread.Sleep(100);

                                LeeStatusWord();
                                if (_SWStatus4 == 7)
                                {
                                    //7= RUN
                                    if (escribirModbus(10262, Convert.ToInt32(Math.Round(_posicionHoming * factorConversion))))
                                    {
                                        if (escribirModbus(6936, _modoHoming))
                                        {
                                            _CWOperatingMode = 1;
                                            if (EscribeControlWord()) Primera = false;
                                            _CWOperatingMode = 0;
                                            EscribeControlWord();
                                            LeeStatusWord();
                                        }
                                        else
                                            paso = 1;
                                    }
                                    else
                                        paso = 1;
                                }
                                else
                                    paso = 1;
                            }
                            else
                                paso = 1;
                        }
                        else
                            paso = 1;
                    }
                    else
                        paso = 1;
                }
                else
                    paso = 1;
            }
        }

        #endregion
        #region Posicionamiento

        //características del posicionamiento
        private double _PosicionadoDest = 0.0;
        private double _PosicionadoVel = 0.0;
        private double _PosicionadoDestUltima = 0;
        private double _PosicionadoVelUltima = 0;
        private bool _Posicionar = false;
        private bool _PosicionadoSeguro = false;

        private void Posicionar(ref bool Primera)
        {
            if (_operatingModeActivo == 1 && !Primera && _SWStatus4 == 7)
            {
                if (_PosicionadoSeguro && (_SWEnd || _SWError) && !this.HaLlegado) _Posicionar = true;

                if ((_Posicionar || _PosicionadoDest != _PosicionadoDestUltima || _PosicionadoVel != _PosicionadoVelUltima)) //DateTime.Now >= HoraRun && 
                {
                    int[] valores = new int[2];
                    valores[0] = Convert.ToInt32(Math.Round(_PosicionadoDest * factorConversion)); //velocidad de movimiento
                    valores[1] = Convert.ToInt32(_PosicionadoVel);    //posición de destino
                    if (escribirModbus(6940, valores))
                    {
                        _CWOperatingMode = 3;
                        if (EscribeControlWord())
                        {
                            _PosicionadoDestUltima = _PosicionadoDest;
                            _PosicionadoVelUltima = _PosicionadoVel;
                            _Posicionar = false;
                        }
                        else
                        {
                            paso = 1;
                        }
                        _CWOperatingMode = 0;
                        EscribeControlWord();
                    }
                    else
                    {
                        paso = 1;
                    }
                }

                if (_SWError)
                    _EstadoReal = eEstadoLX32Real.PosicionadoFinErr; //Fin error
                else if (_SWEnd)
                    _EstadoReal = eEstadoLX32Real.PosicionadoFinOk; //Fin Ok
                else
                    _EstadoReal = eEstadoLX32Real.PosicionadoProceso; //En proceso
            }
            else
            {
                //Pone el servo en modo posicionado
                if (escribirModbus(6918, 1))
                {
                    if (LeeStatusWord())
                    {
                        if (_operatingModeActivo == 1 && _SWStatus4 == 1 && EstableceRun())
                        {
                            _PosicionadoDestUltima = _PosicionadoDest;
                            _Posicionar = false;
                            Primera = false;
                        }
                        else
                        {
                            paso = 1;
                        }
                    }
                    else
                    {
                        paso = 1;
                    }
                }
                else
                {
                    paso = 1;
                }
            }
        }

        /// <summary>
        /// Orden de posicionar
        /// </summary>
        /// <param name="Velocidad">Velocidad de posicionado</param>
        /// <param name="Destino">Destino de posicionado</param>
        public void Posicionar(double Velocidad, double Destino)
        {
            //Solo actua si ha cambiado el valor
            if (_PosicionadoVel != Velocidad || _PosicionadoDest != Destino)
            {
                //Hacer para que cambie cuando cambia algun valor de estos
                _PosicionadoVel = Velocidad;
                _PosicionadoDest = Destino;

                //Solo activa el bit si el motor esta parado y la posicion indicada es diferente de la actual
                if (_PosicionadoDest < _posicionActual - Tolerancia || _PosicionadoDest > _posicionActual + Tolerancia) _Posicionar = true;
            }

            //Marca el bit para empezar ha hacer movimientos
            _PosicionadoSeguro = true;
        }

        /// <summary>
        /// Velocidad de posicionado
        /// </summary>
        public double PosVelocidad
        {
            get
            {
                return _PosicionadoVel;
            }
        }

        /// <summary>
        /// Destino del posicionado
        /// </summary>
        public double PosDestino
        {
            get
            {
                return _PosicionadoDest;
            }
        }

        /// <summary>
        /// Indica que el servo ha llegado a la posicion deseada
        /// </summary>
        public bool HaLlegado
        {
            get
            {
                //return _EstadoReal == eEstadoLX32Real.PosicionadoFinOk;
                return Convert.ToBoolean(_SWEnd && (_PosicionadoDest >= _posicionActual - Tolerancia) && (_PosicionadoDest <= _posicionActual + Tolerancia));
            }
        }

        #endregion
        #region Autotuning

        //características del autotuning
        private int _AT_dir = 1; //1-Positive Negative Home / 2-Negative Positive Home / 3-Positive Home / 4-Positive / 5-Negative Home / 6-Negative
        private int _tolVelocidadAuto = 10; //TODO buscar la documentacion correcta
        private int _AT_n_ref = 100; //Jump of speed of rotation for Autotuning
        private int _AT_dis = 2; //Movement range for Autotuning
        private int _AT_mechanical = 3; //1-Direct Coupling / 2-Belt Axis / 3-Spindle Axis
        private int _CTRL_GlobGain = 400; //Global gain factor
        private int _AT_start = 0; //0-Terminate / 1-Activate EasyTuning / 2-Activate ComfortTuning
        private int _AT_progress = 0;
        private int _AT_state = 0; //Bit 13: auto_tune_process / Bit 14: auto_tune_end / Bit 15: auto_tune_err
        private DateTime tLee_AT_progress;

        private void autotuning(ref bool Primera)
        {
            sLeerDatosDobles datosLeidos;

            if (_operatingModeActivo == -6 && !Primera && _SWStatus4 == 7)
            {
                //En proceso
                if (_EstadoReal == eEstadoLX32Real.AutotuneProceso && DateTime.Now >= tLee_AT_progress.AddMilliseconds(333) && leerModbus(12036, 1, out datosLeidos))
                {
                    tLee_AT_progress = DateTime.Now;
                    _AT_state = datosLeidos.Datos[0];

                    if (BitOfLong(_AT_state, 13))
                    {
                        //Si esta en proces
                        _EstadoReal = eEstadoLX32Real.AutotuneProceso;

                        //Lee el prograso del autotune
                        if (leerModbus(12054, 1, out datosLeidos)) _AT_progress = datosLeidos.Datos[0];
                    }
                    else if (BitOfLong(_AT_state, 14))
                    {
                        //Terminado Ok
                        _AT_progress = 100;

                        //escribimos el gain
                        if (escribirModbus(4394, _CTRL_GlobGain))
                        {
                            //guardamos los parámetros en la EPROM
                            if (guardarEnEPROM())
                            {
                                _EstadoReal = eEstadoLX32Real.AutotuneFinOk;
                                _AT_progress = 0;
                            }
                        }
                    }
                    else
                    {
                        //Terminado error
                        _AT_progress = 0;
                        _EstadoReal = eEstadoLX32Real.AutotuneFinErr;
                    }
                }
            }
            else
            {
                //configura autotuning y lo inicia
                _EstadoReal = eEstadoLX32Real.AutotuneProceso;

                if (escribirModbus(12038, new int[] { _AT_dis, _AT_dir }))
                {
                    if (escribirModbus(12044, _AT_n_ref))
                    {
                        if (escribirModbus(12058, _tolVelocidadAuto))
                        {
                            if (escribirModbus(12060, _AT_mechanical))
                            {
                                if (escribirModbus(6918, -6)) //Operating mode
                                {
                                    if (_SWStatus4 == 1 && EstableceRun())
                                    {
                                        if (escribirModbus(12034, _AT_start)) //Autotuning start
                                        {
                                            Primera = false;
                                            tLee_AT_progress = DateTime.Now;
                                            LeeStatusWord();
                                        }
                                        else
                                            paso = 1;
                                    }
                                    else
                                        paso = 1;
                                }
                                else
                                    paso = 1;
                            }
                            else
                                paso = 1;
                        }
                        else
                            paso = 1;
                    }
                    else
                        paso = 1;
                }
                else
                    paso = 1;
            }
        }

        //PROPIEDADES DEL AUTOTUNING
        public int AT_progress
        {
            get
            {
                return _AT_progress;
            }
        }
        public float CTRL_GlobGain
        {
            get
            {
                return _CTRL_GlobGain / 10.0f;
            }
            set
            {
                _CTRL_GlobGain = Convert.ToInt32(value * 10.0f);
            }
        }
        public int AT_dir
        {
            get
            {
                return _AT_dir;
            }
            set
            {
                if (value >= 1 && value <= 6) _AT_dir = value;
            }
        }
        public int AT_TolVelocidadAuto
        {
            get
            {
                return _tolVelocidadAuto;
            }
            set
            {
                _tolVelocidadAuto = value;
            }
        }
        public int AT_n_ref
        {
            get
            {
                return _AT_n_ref;
            }
            set
            {
                _AT_n_ref = value;
            }
        }
        public int AT_dis
        {
            get
            {
                return _AT_dis;
            }
            set
            {
                _AT_dis = value;
            }
        }
        public int AT_mechanical
        {
            get
            {
                return _AT_mechanical;
            }
            set
            {
                if (value >= 1 && value <= 3) _AT_mechanical = value;
            }
        }
        public int AT_start
        {
            get
            {
                return _AT_start;
            }
            set
            {
                if (value >= 1 || value <= 2) _AT_start = value;
            }
        }

        #endregion

        #region EntradasSalidas

        //características del servo
        private int _positiveLimit = 2000000000;
        private int _negativeLimit = -2000000000;
        private double _ResolucionEncoder = -1.0;

        //PROPIEDADES DEL SERVO
        public eEstadoLX32Deseado Modo
        {
            get
            {
                return _ModoDeseado;
            }
            set
            {
                _ModoDeseado = value;
            }
        }
        public eEstadoLX32Real Estado
        {
            get
            {
                return _EstadoReal;
            }
        }

        //PROPIEDADES STATUSWORD 
        public double LimitePositivo
        {
            get
            {
                return _positiveLimit / factorConversion;
            }
            set
            {
                _positiveLimit = Convert.ToInt32(value * factorConversion);
            }
        }
        public double LimiteNegativo
        {
            get
            {
                return _negativeLimit / factorConversion;
            }
            set
            {
                _negativeLimit = Convert.ToInt32(value * factorConversion);
            }
        }
        public int LastError
        {
            get
            {
                return _LastError;
            }
        }
        public double PosicionActual
        {
            get
            {
                return _posicionActual;
            }
        }
        public bool Alarma
        {
            get
            {
                return _Alarma;
            }
        }
        public bool Conectado
        {
            get
            {
                return _CalidadOK && master.Conectar;
            }
        }
        public double ResolucionEncoder
        {
            get
            {
                return _ResolucionEncoder;
            }
        }
        public bool Inicializado
        {
            get
            {
                return paso >= 2;
            }
        }

        #endregion
        #region Auxiliares

        public int BitToLong(int Palabra, int Bit, bool Valor)
        {
            if (Valor)
                return Palabra |= (0x0001 << Bit);
            else
                return Palabra &= ~(0x0001 << Bit);
        }
        public bool BitOfLong(int Palabra, int Bit)
        {
            return !Convert.ToBoolean((Palabra & (0x01 << Bit)) == 0);
        }
        public bool BitOfShort(short Palabra, int Bit)
        {
            return !Convert.ToBoolean((Palabra & (0x01 << Bit)) == 0); ;
        }

        #endregion
        #region IDisposable Support

        private bool disposedValue = false;

        public void Dispose()
        {
            this.disposedValue = true;
            if (thServo == null)
            {
                thServo.Join();
                thServo = null;
            }

            //Devuelve el control del Lexium
            escribirModbus(282, 0);
            escribirModbus(6914, 0);

            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public enum eEstadoLX32Deseado
    {
        Cero = 0,
        Jog = 1,
        Posicionado = 2,
        Autotune = 3,
        Velocidad = 4,
        Par = 5,
        Homing = 6
    }
    public enum eEstadoLX32Real
    {
        Cero = 0,
        AutotuneProceso = 1,
        AutotuneFinOk = 2,
        AutotuneFinErr = 3,
        HomingProceso = 4,
        HomingFinOk = 5,
        HomingFinErr = 6,
        PosicionadoProceso = 7,
        PosicionadoFinOk = 8,
        PosicionadoFinErr = 9,
        Jog = 10,
        Velocidad = 11,
        Par = 12,
        Alarma = 13
    }
}
