using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Creta.ModBus;
using System.Runtime.InteropServices;

namespace PLC
{
    public class clsOTB : IDisposable
    {
        #region General

        private clsMBMaestro MB;
        private System.Threading.Thread thOTB;
        private volatile bool _Calidad = false;
        private volatile byte _Direccion = 1;
        private volatile short[] _SalidasAux = new short[3];

        public event delSuceso Suceso;

        public clsOTB(clsMBMaestro Master, byte Direccion)
        {
            //Comunicacion
            _Direccion = Direccion;
            MB = Master;

            //Inicia el subproceso
            Scan();
        }
        public void Scan()
        {
            try
            {
                if (thOTB != null && thOTB.IsAlive == false)
                {
                    thOTB.Join();
                    thOTB = null;
                }

                if (thOTB == null && disposedValue == false)
                {
                    thOTB = new System.Threading.Thread(OTB);
                    thOTB.Name = "OTB";
                    thOTB.Start();
                }
            }
            catch (Exception ex)
            {
                if (Suceso != null) Suceso(100, ex.Message, ex.StackTrace);
            }
        }

        private void OTB()
        {
            int mbTimeOut = 50;
            int Paso = 0;
            int errorEscritura = 0;
            DateTime tLectura = DateTime.Now;

            while (disposedValue == false)
            {
                try
                {
                    //Subproceso de la OTB

                    //Si no esta conectado pone el paso 0
                    if (!MB.Conectar) Paso = 0;

                    switch (Paso)
                    {
                        case 0:
                            //Espera a que la comunicacion funcione
                            _Calidad = false;
                            if (MB.Conectar) Paso = 1;
                            break;

                        case 1:
                            //Internal bus stop 1005=1
                            errorEscritura = MB.EscribirPalabras(_Direccion, 1005, new short[] { 1 }, 1000);
                            if (errorEscritura == 0) Paso = 2; else Paso = 0;
                            break;

                        case 2:
                            //Inicializacion 1
                            errorEscritura = MB.EscribirPalabras(_Direccion, 200, new short[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 255, 0, -1, 0, -1, 0 }, 500);
                            if (errorEscritura == 0) Paso = 3; else Paso = 0;
                            break;

                        case 3:
                            //Para las analogicas
                            errorEscritura = MB.EscribirPalabras(_Direccion, 218, new short[] { 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000 }, 500);
                            if (errorEscritura == 0) Paso = 4; else Paso = 0;
                            break;

                        case 4:
                            //Inicializacion 2
#if DEBUG
                            errorEscritura = MB.EscribirPalabras(_Direccion, 1006, new short[] { 0 }, 500);
#else
                            errorEscritura = MB.EscribirPalabras(_Direccion, 1006, new short[] { 2000 }, mbTimeOut);
#endif
                            if (errorEscritura == 0) Paso = 5; else Paso = 0;
                            break;

                        case 5:
                            //Salvar parametros 1002=1
                            errorEscritura = MB.EscribirPalabras(_Direccion, 1002, new short[] { 1 }, 500);
                            if (errorEscritura == 0) Paso = 6; else Paso = 0;
                            break;

                        case 6:
                            //Salvar parametros 1002=0
                            errorEscritura = MB.EscribirPalabras(_Direccion, 1002, new short[] { 0 }, 500);
                            if (errorEscritura == 0) Paso = 7; else Paso = 0;
                            break;

                        case 7:
                            //Restart internal bus 1005=0
                            errorEscritura = MB.EscribirPalabras(_Direccion, 1005, new short[] { 0 }, 1000);
                            if (errorEscritura == 0) Paso = 10; else Paso = 0;
                            break;

                        case 10:
                            //Ciclo normal
                            short[] _Salidas = new short[3];
                            Union U;

                            U.Short0 = 0;
                            U.Int0 = 0;
                            if (_Q000) U.Int0 |= 0x0001;
                            if (_Q001) U.Int0 |= 0x0002;
                            if (_Q002) U.Int0 |= 0x0004;
                            if (_Q003) U.Int0 |= 0x0008;
                            if (_Q004) U.Int0 |= 0x0010;
                            if (_Q005) U.Int0 |= 0x0020;
                            if (_Q006) U.Int0 |= 0x0040;
                            if (_Q007) U.Int0 |= 0x0080;
                            _Salidas[0] = U.Short0;

                            U.Short0 = 0;
                            U.Int0 = 0;
                            if (_Q300) U.Int0 |= 0x0001;
                            if (_Q301) U.Int0 |= 0x0002;
                            if (_Q302) U.Int0 |= 0x0004;
                            if (_Q303) U.Int0 |= 0x0008;
                            if (_Q304) U.Int0 |= 0x0010;
                            if (_Q305) U.Int0 |= 0x0020;
                            if (_Q306) U.Int0 |= 0x0040;
                            if (_Q307) U.Int0 |= 0x0080;
                            if (_Q308) U.Int0 |= 0x0100;
                            if (_Q309) U.Int0 |= 0x0200;
                            if (_Q310) U.Int0 |= 0x0400;
                            if (_Q311) U.Int0 |= 0x0800;
                            if (_Q312) U.Int0 |= 0x1000;
                            if (_Q313) U.Int0 |= 0x2000;
                            if (_Q314) U.Int0 |= 0x4000;
                            if (_Q315) U.Int0 |= 0x8000;
                            _Salidas[1] = U.Short0;

                            U.Short0 = 0;
                            U.Int0 = 0;
                            if (_Q400) U.Int0 |= 0x0001;
                            if (_Q401) U.Int0 |= 0x0002;
                            if (_Q402) U.Int0 |= 0x0004;
                            if (_Q403) U.Int0 |= 0x0008;
                            if (_Q404) U.Int0 |= 0x0010;
                            if (_Q405) U.Int0 |= 0x0020;
                            if (_Q406) U.Int0 |= 0x0040;
                            if (_Q407) U.Int0 |= 0x0080;
                            if (_Q408) U.Int0 |= 0x0100;
                            if (_Q409) U.Int0 |= 0x0200;
                            if (_Q410) U.Int0 |= 0x0400;
                            if (_Q411) U.Int0 |= 0x0800;
                            if (_Q412) U.Int0 |= 0x1000;
                            if (_Q413) U.Int0 |= 0x2000;
                            if (_Q414) U.Int0 |= 0x4000;
                            if (_Q415) U.Int0 |= 0x8000;
                            _Salidas[2] = U.Short0;

                            //Ciclo de escritura. Escribe solo cuando ha cambiado alguna salida
                            if (_SalidasAux[0] != _Salidas[0] || _SalidasAux[1] != _Salidas[1] || _SalidasAux[2] != _Salidas[2])
                            {
                                errorEscritura = MB.EscribirPalabras(_Direccion, 100, _Salidas, mbTimeOut);
                                if (errorEscritura == 0)
                                {
                                    _SalidasAux[0] = _Salidas[0];
                                    _SalidasAux[1] = _Salidas[1];
                                    _SalidasAux[2] = _Salidas[2];
                                }
                            }

                            //Ciclo de lectura. Lee por tiempo
                            if (DateTime.Now >= tLectura.AddMilliseconds(100))
                            {
                                tLectura = DateTime.Now;

                                sLeerDatos datosLeidos = MB.LeerPalabras(_Direccion, 0, 13, mbTimeOut);
                                if (datosLeidos.Erro == 0)
                                {
                                    Array.Copy(datosLeidos.Datos, _Entradas, 13);
                                    _Calidad = true;
                                }
                                else
                                {
                                    _Calidad = false;
                                    Paso = 0;
                                    System.Threading.Thread.Sleep(500);
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    if (Suceso != null) Suceso(100, ex.Message, ex.StackTrace);
                }
                finally
                {
                    System.Threading.Thread.Sleep(5);
                }
            }

            //Escribe las salidas a 0 al salir del bucle
            try
            {
                MB.EscribirPalabras(_Direccion, 100, new short[] { 0, 0, 0 }, mbTimeOut);
            }
            catch (Exception ex)
            {
                if (Suceso != null) Suceso(100, ex.Message, ex.StackTrace);
            }
        }
        private void OTBold()
        {
            int mbTimeOut = 50;
            int Paso = 0;
            int errorEscritura = 0;
            DateTime tLectura = DateTime.Now;

            while (disposedValue == false)
            {
                try
                {
                    //Subproceso de la OTB

                    //Si no esta conectado pone el paso 0
                    if (!MB.Conectar) Paso = 0;

                    switch (Paso)
                    {
                        case 0:
                            //Espera a que la comunicacion funcione
                            _Calidad = false;
                            if (MB.Conectar) Paso = 1;
                            break;

                        case 1:
                            //Inicializacion 1
                            errorEscritura = MB.EscribirPalabras(_Direccion, 200, new short[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 255, 0, -1, 0, -1, 0 }, mbTimeOut);
                            if (errorEscritura == 0) Paso = 2; else Paso = 0;
                            break;

                        case 2:
                            //Para las analogicas
                            errorEscritura = MB.EscribirPalabras(_Direccion, 218, new short[] { 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000, 0x0008, 0x0002, -2000, 6000 }, mbTimeOut);
                            if (errorEscritura == 0) Paso = 3; else Paso = 0;
                            break;

                        case 3:
                            //Inicializacion 2
#if DEBUG
                            errorEscritura = MB.EscribirPalabras(_Direccion, 1006, new short[] { 0 }, mbTimeOut);
#else
                            errorEscritura = MB.EscribirPalabras(_Direccion, 1006, new short[] { 2000 }, mbTimeOut);
#endif
                            if (errorEscritura == 0) Paso = 10; else Paso = 0;
                            break;

                        case 4:
                            //Inicializacion 3
                            errorEscritura = MB.EscribirPalabras(_Direccion, 1002, new short[] { 1 }, mbTimeOut);
                            if (errorEscritura == 0) Paso = 5;
                            break;

                        case 5:
                            //Inicializacion 4
                            errorEscritura = MB.EscribirPalabras(_Direccion, 1005, new short[] { 1 }, mbTimeOut);
                            if (errorEscritura == 0) Paso = 10;
                            break;

                        case 10:
                            //Ciclo normal
                            short[] _Salidas = new short[3];
                            Union U;

                            U.Short0 = 0;
                            U.Int0 = 0;
                            if (_Q000) U.Int0 |= 0x0001;
                            if (_Q001) U.Int0 |= 0x0002;
                            if (_Q002) U.Int0 |= 0x0004;
                            if (_Q003) U.Int0 |= 0x0008;
                            if (_Q004) U.Int0 |= 0x0010;
                            if (_Q005) U.Int0 |= 0x0020;
                            if (_Q006) U.Int0 |= 0x0040;
                            if (_Q007) U.Int0 |= 0x0080;
                            _Salidas[0] = U.Short0;

                            U.Short0 = 0;
                            U.Int0 = 0;
                            if (_Q300) U.Int0 |= 0x0001;
                            if (_Q301) U.Int0 |= 0x0002;
                            if (_Q302) U.Int0 |= 0x0004;
                            if (_Q303) U.Int0 |= 0x0008;
                            if (_Q304) U.Int0 |= 0x0010;
                            if (_Q305) U.Int0 |= 0x0020;
                            if (_Q306) U.Int0 |= 0x0040;
                            if (_Q307) U.Int0 |= 0x0080;
                            if (_Q308) U.Int0 |= 0x0100;
                            if (_Q309) U.Int0 |= 0x0200;
                            if (_Q310) U.Int0 |= 0x0400;
                            if (_Q311) U.Int0 |= 0x0800;
                            if (_Q312) U.Int0 |= 0x1000;
                            if (_Q313) U.Int0 |= 0x2000;
                            if (_Q314) U.Int0 |= 0x4000;
                            if (_Q315) U.Int0 |= 0x8000;
                            _Salidas[1] = U.Short0;

                            U.Short0 = 0;
                            U.Int0 = 0;
                            if (_Q400) U.Int0 |= 0x0001;
                            if (_Q401) U.Int0 |= 0x0002;
                            if (_Q402) U.Int0 |= 0x0004;
                            if (_Q403) U.Int0 |= 0x0008;
                            if (_Q404) U.Int0 |= 0x0010;
                            if (_Q405) U.Int0 |= 0x0020;
                            if (_Q406) U.Int0 |= 0x0040;
                            if (_Q407) U.Int0 |= 0x0080;
                            if (_Q408) U.Int0 |= 0x0100;
                            if (_Q409) U.Int0 |= 0x0200;
                            if (_Q410) U.Int0 |= 0x0400;
                            if (_Q411) U.Int0 |= 0x0800;
                            if (_Q412) U.Int0 |= 0x1000;
                            if (_Q413) U.Int0 |= 0x2000;
                            if (_Q414) U.Int0 |= 0x4000;
                            if (_Q415) U.Int0 |= 0x8000;
                            _Salidas[2] = U.Short0;

                            //Ciclo de escritura. Escribe solo cuando ha cambiado alguna salida
                            if (_SalidasAux[0] != _Salidas[0] || _SalidasAux[1] != _Salidas[1] || _SalidasAux[2] != _Salidas[2])
                            {
                                errorEscritura = MB.EscribirPalabras(_Direccion, 100, _Salidas, mbTimeOut);
                                if (errorEscritura == 0)
                                {
                                    _SalidasAux[0] = _Salidas[0];
                                    _SalidasAux[1] = _Salidas[1];
                                    _SalidasAux[2] = _Salidas[2];
                                }
                            }

                            //Ciclo de lectura. Lee por tiempo
                            if (DateTime.Now >= tLectura.AddMilliseconds(100))
                            {
                                tLectura = DateTime.Now;

                                sLeerDatos datosLeidos = MB.LeerPalabras(_Direccion, 0, 13, mbTimeOut);
                                if (datosLeidos.Erro == 0)
                                {
                                    Array.Copy(datosLeidos.Datos, _Entradas, 13);
                                    _Calidad = true;
                                }
                                else
                                {
                                    _Calidad = false;
                                    Paso = 0;
                                    System.Threading.Thread.Sleep(500);
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    if (Suceso != null) Suceso(100, ex.Message, ex.StackTrace);
                }
                finally
                {
                    System.Threading.Thread.Sleep(5);
                }
            }

            //Escribe las salidas a 0 al salir del bucle
            try
            {
                MB.EscribirPalabras(_Direccion, 100, new short[] { 0, 0, 0 }, mbTimeOut);
            }
            catch (Exception ex)
            {
                if (Suceso != null) Suceso(100, ex.Message, ex.StackTrace);
            }
        }

        public byte Direccion
        {
            get
            {
                return _Direccion;
            }
            set
            {
                _Direccion = value;
            }
        }

        #endregion
        #region EntradasSalidas

        private volatile short[] _Entradas = new short[13];

        private volatile bool _Q000;
        private volatile bool _Q001;
        private volatile bool _Q002;
        private volatile bool _Q003;
        private volatile bool _Q004;
        private volatile bool _Q005;
        private volatile bool _Q006;
        private volatile bool _Q007;
        private volatile bool _Q300;
        private volatile bool _Q301;
        private volatile bool _Q302;
        private volatile bool _Q303;
        private volatile bool _Q304;
        private volatile bool _Q305;
        private volatile bool _Q306;
        private volatile bool _Q307;
        private volatile bool _Q308;
        private volatile bool _Q309;
        private volatile bool _Q310;
        private volatile bool _Q311;
        private volatile bool _Q312;
        private volatile bool _Q313;
        private volatile bool _Q314;
        private volatile bool _Q315;
        private volatile bool _Q400;
        private volatile bool _Q401;
        private volatile bool _Q402;
        private volatile bool _Q403;
        private volatile bool _Q404;
        private volatile bool _Q405;
        private volatile bool _Q406;
        private volatile bool _Q407;
        private volatile bool _Q408;
        private volatile bool _Q409;
        private volatile bool _Q410;
        private volatile bool _Q411;
        private volatile bool _Q412;
        private volatile bool _Q413;
        private volatile bool _Q414;
        private volatile bool _Q415;

        public bool CalidadOk
        {
            get
            {
                return _Calidad;
            }
        }
        
        public bool I000
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0001);
            }
        }
        public bool I001
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0002);
            }
        }
        public bool I002
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0004);
            }
        }
        public bool I003
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0008);
            }
        }
        public bool I004
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0010);
            }
        }
        public bool I005
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0020);
            }
        }
        public bool I006
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0040);
            }
        }
        public bool I007
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0080);
            }
        }
        public bool I008
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0100);
            }
        }
        public bool I009
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0200);
            }
        }
        public bool I010
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0400);
            }
        }
        public bool I011
        {
            get
            {
                return Convert.ToBoolean(_Entradas[0] & 0x0800);
            }
        }

        public bool I100
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0001);
            }
        }
        public bool I101
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0002);
            }
        }
        public bool I102
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0004);
            }
        }
        public bool I103
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0008);
            }
        }
        public bool I104
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0010);
            }
        }
        public bool I105
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0020);
            }
        }
        public bool I106
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0040);
            }
        }
        public bool I107
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0080);
            }
        }
        public bool I108
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0100);
            }
        }
        public bool I109
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0200);
            }
        }
        public bool I110
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0400);
            }
        }
        public bool I111
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x0800);
            }
        }
        public bool I112
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x1000);
            }
        }
        public bool I113
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x2000);
            }
        }
        public bool I114
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x4000);
            }
        }
        public bool I115
        {
            get
            {
                return Convert.ToBoolean(_Entradas[1] & 0x8000);
            }
        }
        public bool I116
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0001);
            }
        }
        public bool I117
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0002);
            }
        }
        public bool I118
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0004);
            }
        }
        public bool I119
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0008);
            }
        }
        public bool I120
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0010);
            }
        }
        public bool I121
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0020);
            }
        }
        public bool I122
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0040);
            }
        }
        public bool I123
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0080);
            }
        }
        public bool I124
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0100);
            }
        }
        public bool I125
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0200);
            }
        }
        public bool I126
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0400);
            }
        }
        public bool I127
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x0800);
            }
        }
        public bool I128
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x1000);
            }
        }
        public bool I129
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x2000);
            }
        }
        public bool I130
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x4000);
            }
        }
        public bool I131
        {
            get
            {
                return Convert.ToBoolean(_Entradas[2] & 0x8000);
            }
        }

        public bool I200
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0001);
            }
        }
        public bool I201
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0002);
            }
        }
        public bool I202
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0004);
            }
        }
        public bool I203
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0008);
            }
        }
        public bool I204
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0010);
            }
        }
        public bool I205
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0020);
            }
        }
        public bool I206
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0040);
            }
        }
        public bool I207
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0080);
            }
        }
        public bool I208
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0100);
            }
        }
        public bool I209
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0200);
            }
        }
        public bool I210
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0400);
            }
        }
        public bool I211
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x0800);
            }
        }
        public bool I212
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x1000);
            }
        }
        public bool I213
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x2000);
            }
        }
        public bool I214
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x4000);
            }
        }
        public bool I215
        {
            get
            {
                return Convert.ToBoolean(_Entradas[3] & 0x8000);
            }
        }
        public bool I216
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0001);
            }
        }
        public bool I217
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0002);
            }
        }
        public bool I218
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0004);
            }
        }
        public bool I219
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0008);
            }
        }
        public bool I220
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0010);
            }
        }
        public bool I221
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0020);
            }
        }
        public bool I222
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0040);
            }
        }
        public bool I223
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0080);
            }
        }
        public bool I224
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0100);
            }
        }
        public bool I225
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0200);
            }
        }
        public bool I226
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0400);
            }
        }
        public bool I227
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x0800);
            }
        }
        public bool I228
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x1000);
            }
        }
        public bool I229
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x2000);
            }
        }
        public bool I230
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x4000);
            }
        }
        public bool I231
        {
            get
            {
                return Convert.ToBoolean(_Entradas[4] & 0x8000);
            }
        }

        public bool Q000
        {
            get
            {
                return _Q000;
            }
            set
            {
                _Q000 = value;
            }
        }
        public bool Q001
        {
            get
            {
                return _Q001;
            }
            set
            {
                _Q001 = value;
            }
        }
        public bool Q002
        {
            get
            {
                return _Q002;
            }
            set
            {
                _Q002 = value;
            }
        }
        public bool Q003
        {
            get
            {
                return _Q003;
            }
            set
            {
                _Q003 = value;
            }
        }
        public bool Q004
        {
            get
            {
                return _Q004;
            }
            set
            {
                _Q004 = value;
            }
        }
        public bool Q005
        {
            get
            {
                return _Q005;
            }
            set
            {
                _Q005 = value;
            }
        }
        public bool Q006
        {
            get
            {
                return _Q006;
            }
            set
            {
                _Q006 = value;
            }
        }
        public bool Q007
        {
            get
            {
                return _Q007;
            }
            set
            {
                _Q007 = value;
            }
        }

        public bool Q300
        {
            get
            {
                return _Q300;
            }
            set
            {
                _Q300 = value;
            }
        }      
        public bool Q301
        {
            get
            {
                return _Q301;
            }
            set
            {
                _Q301 = value;
            }
        }
        public bool Q302
        {
            get
            {
                return _Q302;
            }
            set
            {
                _Q302 = value;
            }
        }
        public bool Q303
        {
            get
            {
                return _Q303;
            }
            set
            {
                _Q303 = value;
            }
        }
        public bool Q304
        {
            get
            {
                return _Q304;
            }
            set
            {
                _Q304 = value;
            }
        }
        public bool Q305
        {
            get
            {
                return _Q305;
            }
            set
            {
                _Q305 = value;
            }
        }
        public bool Q306
        {
            get
            {
                return _Q306;
            }
            set
            {
                _Q306 = value;
            }
        }
        public bool Q307
        {
            get
            {
                return _Q307;
            }
            set
            {
                _Q307 = value;
            }
        }
        public bool Q308
        {
            get
            {
                return _Q308;
            }
            set
            {
                _Q308 = value;
            }
        }
        public bool Q309
        {
            get
            {
                return _Q309;
            }
            set
            {
                _Q309 = value;
            }
        }
        public bool Q310
        {
            get
            {
                return _Q310;
            }
            set
            {
                _Q310 = value;
            }
        }
        public bool Q311
        {
            get
            {
                return _Q311;
            }
            set
            {
                _Q311 = value;
            }
        }
        public bool Q312
        {
            get
            {
                return _Q312;
            }
            set
            {
                _Q312 = value;
            }
        }
        public bool Q313
        {
            get
            {
                return _Q313;
            }
            set
            {
                _Q313 = value;
            }
        }
        public bool Q314
        {
            get
            {
                return _Q314;
            }
            set
            {
                _Q314 = value;
            }
        }
        public bool Q315
        {
            get
            {
                return _Q315;
            }
            set
            {
                _Q315 = value;
            }
        }

        public bool Q400
        {
            get
            {
                return _Q400;
            }
            set
            {
                _Q400 = value;
            }
        }      
        public bool Q401
        {
            get
            {
                return _Q401;
            }
            set
            {
                _Q401 = value;
            }
        }
        public bool Q402
        {
            get
            {
                return _Q402;
            }
            set
            {
                _Q402 = value;
            }
        }
        public bool Q403
        {
            get
            {
                return _Q403;
            }
            set
            {
                _Q403 = value;
            }
        }
        public bool Q404
        {
            get
            {
                return _Q404;
            }
            set
            {
                _Q404 = value;
            }
        }
        public bool Q405
        {
            get
            {
                return _Q405;
            }
            set
            {
                _Q405 = value;
            }
        }
        public bool Q406
        {
            get
            {
                return _Q406;
            }
            set
            {
                _Q406 = value;
            }
        }
        public bool Q407
        {
            get
            {
                return _Q407;
            }
            set
            {
                _Q407 = value;
            }
        }
        public bool Q408
        {
            get
            {
                return _Q408;
            }
            set
            {
                _Q408 = value;
            }
        }
        public bool Q409
        {
            get
            {
                return _Q409;
            }
            set
            {
                _Q409 = value;
            }
        }
        public bool Q410
        {
            get
            {
                return _Q410;
            }
            set
            {
                _Q410 = value;
            }
        }
        public bool Q411
        {
            get
            {
                return _Q411;
            }
            set
            {
                _Q411 = value;
            }
        }
        public bool Q412
        {
            get
            {
                return _Q412;
            }
            set
            {
                _Q412 = value;
            }
        }
        public bool Q413
        {
            get
            {
                return _Q413;
            }
            set
            {
                _Q413 = value;
            }
        }
        public bool Q414
        {
            get
            {
                return _Q414;
            }
            set
            {
                _Q414 = value;
            }
        }
        public bool Q415
        {
            get
            {
                return _Q415;
            }
            set
            {
                _Q415 = value;
            }
        }
        
        public short I500
        {
            get
            {
                return _Entradas[5];
            }
        }
        public short I501
        {
            get
            {
                return _Entradas[6];
            }
        }
        public short I502
        {
            get
            {
                return _Entradas[7];
            }
        }
        public short I503
        {
            get
            {
                return _Entradas[8];
            }
        }
        public short I600
        {
            get
            {
                return _Entradas[9];
            }
        }
        public short I601
        {
            get
            {
                return _Entradas[10];
            }
        }
        public short I602
        {
            get
            {
                return _Entradas[11];
            }
        }
        public short I603
        {
            get
            {
                return _Entradas[12];
            }
        }

        #endregion
        #region IDisposable Support

        private bool disposedValue = false;

        public void Dispose()
        {
            this.disposedValue = true;

            if (thOTB == null)
            {
                thOTB.Join();
                thOTB = null;
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
  
    //public class EBOOL
    //{
    //    private volatile bool _estado = false;
    //    private volatile bool _subida = false;
    //    private volatile bool _bajada = false;
    //    private volatile bool _ForzadoOn = false;
    //    private volatile bool _ForzadoOff = false;

    //    public EBOOL()
    //    {
    //        _estado = false;
    //        _subida = false;
    //        _bajada = false;
    //    }

    //    /// <summary>
    //    /// Entrada de valor
    //    /// </summary>
    //    public bool I
    //    {
    //        get
    //        {
    //            return _estado;
    //        }
    //        set
    //        {
    //            //Si hay algun forzado ignora la entrada
    //            if (!_ForzadoOn && !_ForzadoOff)
    //            {
    //                //calculo flanco de subida
    //                if (value && !_estado)
    //                    _subida = true;
    //                else
    //                    _subida = false;

    //                //calculo flanco de bajada
    //                if (!value && _estado)
    //                    _bajada = true;
    //                else
    //                    _bajada = false;

    //                //refresco valor
    //                _estado = value;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Salida de valor
    //    /// </summary>
    //    public bool Q
    //    {
    //        get
    //        {
    //            return _estado;
    //        }
    //    }

    //    /// <summary>
    //    /// Salida flanco de subida
    //    /// </summary>
    //    public bool P
    //    {
    //        get
    //        {
    //            return _subida;
    //        }
    //    }

    //    /// <summary>
    //    /// Salida flanco de bajada
    //    /// </summary>
    //    public bool N
    //    {
    //        get
    //        {
    //            return _bajada;
    //        }
    //    }

    //    /// <summary>
    //    /// Entrada Set si valor verdadero
    //    /// </summary>
    //    public bool S
    //    {
    //        set
    //        {
    //            if (value)
    //            {
    //                this.I = true;
    //            }
    //            else
    //            {
    //                //Gestiona los flancos cuando no se esta poneiendo a 1
    //                _subida = false;
    //                _bajada = false;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Entrada Reset si valor verdadero
    //    /// </summary>
    //    public bool R
    //    {
    //        set
    //        {
    //            if (value)
    //            { 
    //                I = false;
    //            }
    //            else
    //            {
    //                //Gestiona los flancos cuando no se esta poneiendo a 0
    //                _subida = false;
    //                _bajada = false;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Forzado a SET
    //    /// </summary>
    //    public bool ForzadoSet
    //    {
    //        get
    //        {
    //            return _ForzadoOn;
    //        }
    //        set
    //        {
    //            _ForzadoOn = value;
    //            if (_ForzadoOn) _ForzadoOff = false;

    //            _estado = true;
    //            _subida = false;
    //            _bajada = false;
    //        }
    //    }

    //    /// <summary>
    //    /// Forzado a RESET
    //    /// </summary>
    //    public bool ForzadoReset
    //    {
    //        get
    //        {
    //            return _ForzadoOff;
    //        }
    //        set
    //        {
    //            _ForzadoOff = value;
    //            if (_ForzadoOff) _ForzadoOn = false;

    //            _estado = false;
    //            _subida = false;
    //            _bajada = false;
    //        }
    //    }

    //}

    public interface ITemporizador
    {
        bool I { get; set; }
        int T { get; set; } //En milisegundos
        bool Q { get; }
        int E { get; }
    }
    public class clsTON : ITemporizador
    {
        private volatile bool _I;
        private volatile bool _Q;
        private volatile int _Tiempo; //Milisegundos
        private DateTime _Inicio; //Hora de inicio de la temporizacion

        public clsTON()
        {
            _I = false;
            _Q = false;
            _Tiempo = 1000;
            _Inicio = DateTime.Now;
        }
        public clsTON(int Tiempo)
        {
            _I = false;
            _Q = false;
            _Tiempo = Tiempo;
            _Inicio = DateTime.Now;
        }

        public bool I
        {
            get
            {
                return _I;
            }
            set
            {
                if (value)
                {
                    //El temporizador esta activo
                    if (!_I)
                    {
                        //Primer ciclo del temporizador
                        _I = true;
                        _Inicio = DateTime.Now;
                    }

                    //Lee la salida para saber si se ha cumplido el tiempo
                    bool b = Q;
                }
                else
                {
                    //El temporizador no esta activo
                    _Q = false;
                    _I = false;
                }
            }
        }
        public int T
        {
            get
            {
                return _Tiempo;
            }
            set
            {
                _Tiempo = value;
            }
        }

        public bool Q
        {
            get
            {
                if (_Q)
                {
                    //El tiempo ya ha finalizado
                    return true;
                }
                else
                {
                    //Comprueba si el tiempo ha finalizado
                    if (_I)
                    {
                        //La estrada esta activada

                        //Fin del tiempo
                        if (DateTime.Now >= _Inicio.AddMilliseconds(_Tiempo)) _Q = true;

                        return _Q;
                    }
                    else
                    {
                        //La entrada no esta activada
                        return false;
                    }
                }
            }
        }
        public int E
        {
            get
            {
                //Lee el tiempo transcurrido
                if (_I)
                {
                    //En marcha
                    if (Q)
                    {
                        //Fin del tiempo
                        return _Tiempo;
                    }
                    else
                    {
                        //Contando... calcula el tiempo transcurrido
                        int i = _Tiempo - Math.Abs(Convert.ToInt32(Convert.ToDouble(DateTime.Now.Ticks - _Inicio.AddMilliseconds(_Tiempo).Ticks) / 10000.0));
                        return i;
                    }
                }
                else
                {
                    //Parado
                    return 0;
                }
            }
        }
    }
    public class clsTOFF : ITemporizador
    {
        private volatile bool _I;
        private volatile bool _Q;
        private volatile int _Tiempo; //Milisegundos
        private DateTime _Inicio; //Hora de inicio de la temporizacion

        public clsTOFF()
        {
            _I = false;
            _Q = false;
            _Tiempo = 1000;
            _Inicio = DateTime.Now;
        }
        public clsTOFF(int Tiempo)
        {
            _I = false;
            _Q = false;
            _Tiempo = Tiempo;
            _Inicio = DateTime.Now;
        }

        public bool I
        {
            get
            {
                return _I;
            }
            set
            {
                if (value)
                {
                    //El temporizador no esta activo
                    _Q = true;
                    _I = true;
                }
                else
                {
                    //El temporizador esta activo
                    if (_I)
                    {
                        //Primer ciclo del temporizador
                        _I = false;
                        _Inicio = DateTime.Now;
                    }

                    //Lee la salida para saber si se ha cumplido el tiempo
                    bool b = Q;
                }
            }
        }
        public int T
        {
            get
            {
                return _Tiempo;
            }
            set
            {
                _Tiempo = value;
            }
        }

        public bool Q
        {
            get
            {
                if (_I)
                {
                    //Si esta la entrada devuelve verdadero
                    return true;
                }
                else
                {
                    if (_Q)
                    {
                        //Comprueba si el tiempo ha finalizado
                        if (DateTime.Now >= _Inicio.AddMilliseconds(_Tiempo))
                        {
                            //Fin del tiempo
                            _Q = false;
                        }
                    }

                    return _Q;
                }
            }
        }
        public int E
        {
            get
            {
                //Lee el tiempo transcurrido
                if (_I == false)
                {
                    //En marcha
                    if (_Q == false)
                    {
                        //Fin del tiempo
                        return 0;
                    }
                    else
                    {
                        //Contando... calcula el tiempo transcurrido
                        int i = _Tiempo - Math.Abs(Convert.ToInt32(Convert.ToDouble(DateTime.Now.Ticks - _Inicio.AddMilliseconds(_Tiempo).Ticks) / 10000.0));
                        return i;
                    }
                }
                else
                {
                    //Parado
                    return 0;
                }
            }
        }

    }
    public class clsTP : ITemporizador
    {
        private volatile bool _I;
        private volatile bool _Q;
        private volatile bool _F;
        private volatile int _Tiempo; //Milisegundos
        private DateTime _Inicio; //Hora de inicio de la temporizacion

        public clsTP()
        {
            _I = false;
            _Q = false;
            _F = false;
            _Tiempo = 1000;
            _Inicio = DateTime.Now;
        }
        public clsTP(int Tiempo)
        {
            _I = false;
            _Q = false;
            _F = false;
            _Tiempo = Tiempo;
            _Inicio = DateTime.Now;
        }

        public bool I
        {
            get
            {
                return _I;
            }
            set
            {
                _I = value;

                if (_I == true && _F == false)
                {
                    //Primer ciclo del temporizador
                    _Q = true;
                    _F = true;
                    _Inicio = DateTime.Now;
                }

                //Lee la salida para saber si se ha cumplido el tiempo
                bool b = Q;
            }
        }
        public int T
        {
            get
            {
                return _Tiempo;
            }
            set
            {
                _Tiempo = value;
            }
        }

        public bool Q
        {
            get
            {
                if (_Q || _F)
                {
                    //Comprueba si el tiempo ha finalizado
                    if (DateTime.Now >= _Inicio.AddMilliseconds(_Tiempo))
                    {
                        //Fin del tiempo
                        _Q = false;
                        if (_I == false) _F = false;
                    }

                    return _Q;
                }
                else
                {
                    //Si esta la salida apagada el tiempo ha finalizado
                    return false;
                }
            }
        }
        public int E
        {
            get
            {
                //Lee el tiempo transcurrido
                if (_F)
                {
                    //En marcha
                    if (_Q == false)
                    {
                        //Fin del tiempo
                        return 0;
                    }
                    else
                    {
                        //Contando... calcula el tiempo transcurrido
                        int i = _Tiempo - Math.Abs(Convert.ToInt32(Convert.ToDouble(DateTime.Now.Ticks - _Inicio.AddMilliseconds(_Tiempo).Ticks) / 10000.0));
                        return i;
                    }
                }
                else
                {
                    //Parado
                    return 0;
                }
            }
        }

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Union
    {
        [FieldOffset(0)] public byte Byte1;
        [FieldOffset(1)] public byte Byte2;
        [FieldOffset(2)] public byte Byte3;
        [FieldOffset(3)] public byte Byte4;

        [FieldOffset(0)] public short Short0;
        [FieldOffset(2)] public short Short1;

        [FieldOffset(0)] public ushort UShort0;
        [FieldOffset(2)] public ushort UShort1;

        [FieldOffset(0)] public int Int0;

        [FieldOffset(0)] public uint UInt0;
    }

}
