using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Creta.ModBus;
using PLC;

namespace PLC
{
    public class clsATV32 : IDisposable
    {
        #region General

        private clsMBMaestro master;
        private System.Threading.Thread thAtv32;
        private bool _CalidadOK = false;
        private bool _Marcha = false;
        private bool _Habilitado = false;

        public event delSuceso Suceso;

        public clsATV32(clsMBMaestro MB, byte Direccion)
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
                if (thAtv32 != null && thAtv32.IsAlive == false)
                {
                    thAtv32 = null;
                }
                if (thAtv32 == null && disposedValue == false)
                {
                    thAtv32 = new System.Threading.Thread(Variador);
                    thAtv32.IsBackground = true;
                    thAtv32.Name = "ATV32:" + master.Puerto.ToString() + "." + _DirMb.ToString();
                    thAtv32.Start();
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
        private void Variador()
        {
            while (disposedValue == false)
            {
                try
                {
                    //Subproceso del servo
                    if (master.Conectar && _Habilitado)
                    {
                        //Lee el status word cada 1000ms, y los errores si hay
                        if (DateTime.Now > tLeerStatusWord.AddMilliseconds(1000)) LeeStatusWord();

                        //Habilita el variador
                        _cwSwitchOn = _swReadyOn;

                        //Pone en marcha el variador segun el bit de marcha
                        if (_swSwitchedOn)
                            _cwEnableOperation = _Marcha;
                        else
                            _cwEnableOperation = false;

                        EscribeControlWord();
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
                catch (Exception ex)
                {
                    if (Suceso != null) Suceso(100, ex.Message, ex.StackTrace);
                    System.Threading.Thread.Sleep(1000);
                }
                finally
                {
                    System.Threading.Thread.Sleep(5);
                }
            }

            //Para el variador
            _cwEnableOperation = false;
            _cwSwitchOn = false;
            _Marcha = false;
            EscribeControlWord();
        }

        /// <summary>
        /// Marcha del motor
        /// </summary>
        public bool Marcha
        {
            get
            {
                return _Marcha;
            }
            set
            {
                _Marcha = value;
            }
        }

        /// <summary>
        /// Habilita el modulo para comunicar
        /// </summary>
        public bool Habilitado
        {
            get
            {
                return _Habilitado;
            }
            set
            {
                _Habilitado = value;
            }
        }

        #endregion
        #region Comunicaciones

        private byte _DirMb;
        private int _ContTx = 0;

        private bool escribirModbus(int direccionModbus, short valor)
        {
            _ContTx++;
            if (master.EscribirPalabras(_DirMb, direccionModbus, new short[] { valor }) != 0)
            {
                _CalidadOK = false;
                _Marcha = false;
                return false;
            }
            else
            {
                _CalidadOK = true;
                return true;
            }
        }
        private bool escribirModbus(int direccionModbus, short[] valores)
        {
            _ContTx++;
            if (master.EscribirPalabras(_DirMb, direccionModbus, valores) != 0)
            {
                _CalidadOK = false;
                _Marcha = false;
                return false;
            }
            else
            {
                _CalidadOK = true;
                return true;
            }
        }
        private bool leerModbus(int DirMemoria, int npalabras, out sLeerDatos lectura)
        {
            _ContTx++;
            lectura = master.LeerPalabras(_DirMb, DirMemoria, npalabras);
            if (lectura.Erro != 0)
            {
                _CalidadOK = false;
                _Marcha = false;
                System.Threading.Thread.Sleep(100);
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
        public bool CalidadOK
        {
            get
            {
                return _CalidadOK;
            }
        }

        #endregion
        #region Leer StatusWord

        private DateTime tLeerStatusWord;

        private bool _swReadyOn;
        private bool _swSwitchedOn;
        private bool _swOperationEnabled;
        private bool _swFault;
        private bool _swVoltageEnabled;
        private bool _swQuickStop;
        private bool _swSwitchOnDisabled;
        private bool _swWarning;
        private bool _swRemote;
        private bool _swTargetReached;

        private short _swVelocidad;

        public bool swReadyOn
        {
            get
            {
                return _swReadyOn;
            }
        }
        public bool swSwitchedOn
        {
            get
            {
                return _swSwitchedOn;
            }
        }
        public bool swOperationEnabled
        {
            get
            {
                return _swOperationEnabled;
            }
        }
        public bool swFault
        {
            get
            {
                return _swFault;
            }
        }
        public bool swVoltageEnabled
        {
            get
            {
                return _swVoltageEnabled;
            }
        }
        public bool swQuickStop
        {
            get
            {
                return _swQuickStop;
            }
        }
        public bool swSwitchOnDisabled
        {
            get
            {
                return _swSwitchOnDisabled;
            }
        }
        public bool swWarning
        {
            get
            {
                return _swWarning;
            }
        }
        public bool swRemote
        {
            get
            {
                return _swRemote;
            }
        }
        public bool swTargetReached
        {
            get
            {
                return _swTargetReached;
            }
        }
        public short swVelocidad
        {
            get
            {
                return _swVelocidad;
            }
        }

        private bool LeeStatusWord()
        {
            tLeerStatusWord = DateTime.Now;

            sLeerDatos datosLeidos;
            if (leerModbus(12741, 2, out datosLeidos))
            {
                //recopilo status word (12741)
                _swReadyOn = BitOfLong(datosLeidos.Datos[0], 0);
                _swSwitchedOn = BitOfLong(datosLeidos.Datos[0], 1);
                _swOperationEnabled = BitOfLong(datosLeidos.Datos[0], 2);
                _swFault = BitOfLong(datosLeidos.Datos[0], 3);
                _swVoltageEnabled = BitOfLong(datosLeidos.Datos[0], 4);
                _swQuickStop = BitOfLong(datosLeidos.Datos[0], 5);
                _swSwitchOnDisabled = BitOfLong(datosLeidos.Datos[0], 6);
                _swWarning = BitOfLong(datosLeidos.Datos[0], 7);
                _swRemote = BitOfLong(datosLeidos.Datos[0], 9);
                _swTargetReached = BitOfLong(datosLeidos.Datos[0], 10);

                _swVelocidad = datosLeidos.Datos[1];

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
        #region Escribir ControlWord

        private short _ultimoCW = 0;
        private short _ultimoCWvel = 0;
        private DateTime _tEscribeLsp = DateTime.Now.AddSeconds(-100);

        private bool _cwSwitchOn = false;
        private bool _cwStop = false;
        private bool _cwEnableOperation = false;
        private bool _cwFaultReset = false;
        private bool _cwHALT = false;
        private bool _cwDireccion = false;
        private short _cwVelocidad = 0;

        private bool EscribeControlWord()
        {
            try
            {
                short[] valor = new short[2];

                valor[0] = BitToShort(valor[0], 0, _cwSwitchOn);
                valor[0] = BitToShort(valor[0], 1, true);
                valor[0] = BitToShort(valor[0], 2, !_cwStop); //Parada rápida
                valor[0] = BitToShort(valor[0], 3, _cwEnableOperation);
                valor[0] = BitToShort(valor[0], 4, false);
                valor[0] = BitToShort(valor[0], 5, false);
                valor[0] = BitToShort(valor[0], 6, false);
                valor[0] = BitToShort(valor[0], 7, _cwFaultReset);
                valor[0] = BitToShort(valor[0], 8, _cwHALT);
                valor[0] = BitToShort(valor[0], 9, false);
                valor[0] = BitToShort(valor[0], 10, false);
                valor[0] = BitToShort(valor[0], 11, _cwDireccion);
                valor[0] = BitToShort(valor[0], 12, false);
                valor[0] = BitToShort(valor[0], 13, false);
                valor[0] = BitToShort(valor[0], 14, false);
                valor[0] = BitToShort(valor[0], 15, false);

                valor[1] = _cwVelocidad;

                if (valor[0] != _ultimoCW || valor[1] != _ultimoCWvel)
                {
                    if (escribirModbus(12761, valor))
                    {
                        _cwFaultReset = false;
                        _ultimoCW = valor[0];
                        _ultimoCWvel = valor[1];

                        //Lee el status despues de escribir el control
                        return LeeStatusWord();
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

        public void Reset()
        {
            _cwFaultReset = true;
        }
        public bool Stop
        {
            get
            {
                return _cwStop;
            }
            set
            {
                _cwStop = value;
            }
        }
        public bool Halt
        {
            get
            {
                return _cwHALT;
            }
            set
            {

                _cwHALT = value;
            }
        }
        public bool Direccion
        {
            get
            {
                return _cwDireccion;
            }
            set
            {
                _cwDireccion = value;
            }
        }
        public short SpVelocidad
        {
            get
            {
                return _cwVelocidad;
            }
            set
            {
                _cwVelocidad = value;
            }
        }

        #endregion
        #region Auxiliares

        public short BitToShort(short Palabra, int Bit, bool Valor)
        {
            Union U;
            U.Int0 = 0;
            U.Short0 = Palabra;

            if (Valor)
                U.Int0 |= (0x0001 << Bit);
            else
                U.Int0 &= ~(0x0001 << Bit);

            return U.Short0;
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
            if (thAtv32 == null)
            {
                thAtv32.Join();
                thAtv32 = null;
            }
        }

        #endregion
    }
}
