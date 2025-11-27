using System;
using CretaBase;
using System.Windows;
using System.Windows.Threading;

namespace CC3_GUI
{
    public class ComHmiMgr
    {
        private Creta.libcomhmi.clsClientTCP _comHmi;
        const string ERROR_NOT_CONNECTED = "No conectado";
        const int TIMEOUT = 100000;//10000;
        const int TIME_TO_RECONNECT = 10;
        private string _serverAdress = CretaTypes.COMHMI_SERVER_ADRESS;
        private DispatcherTimer _disconnectedTimer; //TODO: Hacer un hilo en vez del timer, cuando intenta reconectar se bloquea la aplicación (cada TIME_TO_RECONNECT)
        private DateTime _datStart;
        private string _KernelErrorConnection;
        private bool _bErrorDisplayed;

        public ComHmiMgr()
        {
            _KernelErrorConnection = LocalizationMgr.GetUIString("CretaBase:Strings:MAIN_KERNEL_ERROR");
            _bErrorDisplayed = false;
            _disconnectedTimer = new System.Windows.Threading.DispatcherTimer();
            _disconnectedTimer.Tick += new EventHandler(Update);
            _disconnectedTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
        }
        public void TryReconnect()
        {
            if (GlobalState.MainVM != null) GlobalState.MainVM.IsKernelConnectionError = true;
            _comHmi = null;
            _datStart = DateTime.Now;
            _disconnectedTimer.Start();
        }
        public void Update(object sender, EventArgs e)
        {
            TimeSpan elapsedTime = DateTime.Now.Subtract(_datStart);
            if (elapsedTime.Seconds >= TIME_TO_RECONNECT)
            {
                Connect();
                if (IsComHmiMgrOk())
                {
                    _disconnectedTimer.Stop();
                    if (GlobalState.MainVM != null) GlobalState.MainVM.IsKernelConnectionError = false;
                }
                else
                    _datStart = DateTime.Now;
            }
        }

        public void Connect()
        {
            //bool bError = false;
#if SIMULATION
            _comHmi = null;
#else
            _comHmi = new Creta.libcomhmi.clsClientTCP(_serverAdress, CretaTypes.COMHMI_PORT);
            if (_comHmi.Conectado)
                _comHmi.PeticionCadenaSinRespuesta += new Creta.libcomhmi.eventoPeticionCadenaSinRespuesta(RequestFromServer);
            else
            {
                //bError = true;
                if (_serverAdress != CretaTypes.COMHMI_SERVER_ADRESS)
                {
                    _comHmi = new Creta.libcomhmi.clsClientTCP(CretaTypes.COMHMI_SERVER_ADRESS, CretaTypes.COMHMI_PORT);
                    if (_comHmi.Conectado)
                    {
                        _comHmi.PeticionCadenaSinRespuesta += new Creta.libcomhmi.eventoPeticionCadenaSinRespuesta(RequestFromServer);
                        //bError = false;
                        _bErrorDisplayed = false;
                    }
                }
            }
/* Esto fallaba, si no se conecta a la primera que se cierre, que es lo que se hace en el mainwindow si connectionOK es false
            if (bError)
            {
                TryReconnect();
                if (!_bErrorDisplayed)
                {
                    _bErrorDisplayed = true;
                    DataError(_KernelErrorConnection);
                }
                _comHmi = null;
            }
 */ 
#endif
        }

        public void AddIp(string sIp)
        {
            _serverAdress = sIp;
        }

        const string ID_ALARMS = "ALARM";
        const string ID_INFO = "I";
        const string ID_MODEL = "MDL";
        const string ID_PRINT = "SVARSPRT_";
        //Indica el modelo que se está imprimiendo y la infomación a mostrar en la pantalla principal
        const string ID_MODEL_PRINTING = "PMODL"; //P|MODL| linea_produccion¤id_model¤id_imagen¤posicion_x¤posicion_y
        const string ID_MODIFIED_PRINTING = "PMMOD"; // P|MMOD| linea_produccion¤id_model¤ id_model_modificada¤id_imagen¤posicion_x¤posicion_y
        const string ID_TAS_PRINTING = "PMTAS"; //P|MTAS| linea_produccion¤id_model¤ id_model_TAS
        void RequestFromServer(String sData)
        {
            if (GlobalState.MainWindow != null)
            {
                try
                {
                    //Evento cuando el servidor envía información sin esperar respuesta
                    if (sData.StartsWith(ID_ALARMS))
                        GlobalState.MainWindow.ServerRequestAlarms(sData);
                    else if (sData.StartsWith(ID_INFO))
                        GlobalState.MainWindow.ServerRequestInfo(sData);
                    else if (sData.StartsWith(ID_MODEL))
                        GlobalState.MainWindow.ServerRequestModels(sData);
                    else if (sData.StartsWith(ID_PRINT))
                    {
                        string sPrintCommand = sData.Substring(sData.IndexOf(CretaTypes.COM_CHAR_SEPARATOR) + 1);
                        GlobalState.MainWindow.ServerRequestPrint(sPrintCommand);
                    }
                    else if (sData.StartsWith(ID_MODEL_PRINTING) || sData.StartsWith(ID_TAS_PRINTING) || sData.StartsWith(ID_MODIFIED_PRINTING))
                    {
                        //PMODL|linea_produccion ¤ id_model ¤                       id_imagen ¤ posicion_x ¤ posicion_y
                        //PMMOD|linea_produccion ¤ id_model ¤ id_model_modificada ¤ id_imagen ¤ posicion_x ¤ posicion_y
                        //PMTAS|linea_produccion ¤ id_model ¤ id_model_TAS
                        string sPrinting = sData.Substring(5);
                        string[] asPrintTokens = sPrinting.Split(CretaTypes.COM_CHAR_SEPARATOR_TOKENS_MSG);
                        int iLine = -1;
                        int iMdl = -1;
                        int iImg = -1;
                        int idModTAS = -1;
                        double dPosX = -1;
                        double dPosY = -1;
                        if (asPrintTokens.Length > 0)
                            iLine = CretaUtils.ToInt(asPrintTokens[0]);
                        if (asPrintTokens.Length > 1)
                            iMdl = CretaUtils.ToInt(asPrintTokens[1]);
                        if (sData.StartsWith(ID_MODEL_PRINTING)) //Print modelo normal
                        {
                            if (asPrintTokens.Length > 2)
                                iImg = CretaUtils.ToInt(asPrintTokens[2]);
                            if (asPrintTokens.Length > 3)
                                dPosX = CretaUtils.ToDouble(asPrintTokens[3]);
                            if (asPrintTokens.Length > 4)
                                dPosY = CretaUtils.ToDouble(asPrintTokens[4]);
                        }
                        else // TAS o modificadas
                        {
                            if (asPrintTokens.Length > 2)
                                idModTAS = CretaUtils.ToInt(asPrintTokens[2]);
                            if (asPrintTokens.Length > 3)
                                iImg = CretaUtils.ToInt(asPrintTokens[3]);
                            if (asPrintTokens.Length > 4)
                                dPosX = CretaUtils.ToDouble(asPrintTokens[4]);
                            if (asPrintTokens.Length > 5)
                                dPosY = CretaUtils.ToDouble(asPrintTokens[5]);
                        }

                        System.Diagnostics.Debug.WriteLine("---Enviado desde el kernel el modelo a pintar---");
                        System.Diagnostics.Debug.WriteLine(sData);
                        int idTAS = sData.StartsWith(ID_TAS_PRINTING) ? idModTAS : -1;
                        int idMod = sData.StartsWith(ID_MODIFIED_PRINTING) ? idModTAS : -1;
                        if (iLine == 1)
                        {
                            GlobalState.MainVM.MainSimulationL1.ChangeImage(iMdl, iImg, dPosX, dPosY, idModTAS);
                            GlobalState.MainVM.ModelInfoL1.FillData(iMdl, idTAS, idMod);
                            //GlobalState.MainVM.ModelL1Perc = 100;
                        }
                        else
                        {
                            GlobalState.MainVM.MainSimulationL2.ChangeImage(iMdl, iImg, dPosX, dPosY, idModTAS);
                            GlobalState.MainVM.ModelInfoL2.FillData(iMdl, idTAS, idMod);
                            //GlobalState.MainVM.ModelL2Perc = 100;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message + ex.StackTrace;
                    //TODO: guardarlo en logs, si el archivo es muy grande crear otro archivo
                    //MessageBox.Show(String.Format("ComHmiMgr (Avisar a Víctor) - Error al recibir la información del Kernel: {0} \n\t{1}", sData,s));
                    GlobalState.WriteLogEvent(String.Format("ComHmiMgr - Error RequestFromServer(): {0} \n\t{1}", sData, s), CretaTypes.ELogTypes.LOG_EXCEPTION);
                }
            }
        }
        public void Close()
        {
            if (_comHmi != null)
            {
                _comHmi.Dispose();
                _disconnectedTimer.Stop();
            }
        }

        public string StringRequest(string sVar, int Timeout=TIMEOUT)
        {
            try
            {
                string sReply = "";
                //Quito del final de la cadena cualquier carácter sobrante de separador
                sVar = sVar.Trim(CretaTypes.COM_CHAR_SEPARATOR_TOKENS_MSG);
                //Si estoy haciendo un SVARS o un GVARS compruebo que a continuación del token hay algo
                bool CanSend = true;
                if(sVar.StartsWith(CretaTypes.COM_SET_VARS) || sVar.StartsWith(CretaTypes.COM_GET_VARS))
                    CanSend = sVar.Length > CretaTypes.COM_START_INDEX_MSG;
                if (_comHmi != null && CanSend)
                {
                    if (_comHmi.Conectado)
                    {
                        int iLasCharIdx = sVar.Length - 1;
                        if (sVar[sVar.Length - 1] == CretaTypes.COM_CHAR_SEPARATOR_TOKENS_MSG)
                            sVar = sVar.Substring(0, sVar.Length - 1);
                        try
                        {
                            sReply = _comHmi.HacerPeticionConRespuesta(sVar, Timeout);
                        }
                        catch
                        {
                            GlobalState.WriteLogEvent(String.Format("ComHmiMgr - Error StringRequest,HacerPeticionConRespuesta(): {0}", sVar), CretaTypes.ELogTypes.LOG_EXCEPTION);
                        }
                    }
                    else
                    {
                        TryReconnect();
                        if (!_bErrorDisplayed)
                        {
                            _bErrorDisplayed = true;
                            DataError(_KernelErrorConnection);
                        }
                    }
                }
                if (CanSend && sReply == "" && _comHmi!=null)
                    GlobalState.WriteLogEvent(String.Format("ComHmiMgr - No hay respuesta - StringRequest,HacerPeticionConRespuesta(): {0}", sVar), CretaTypes.ELogTypes.LOG_ERROR);
                return sReply;
            }
            catch
            {
                GlobalState.WriteLogEvent(String.Format("ComHmiMgr - Error StringRequest(): {0}", sVar), CretaTypes.ELogTypes.LOG_EXCEPTION);
                return "#-1";
            }
        }
        public void StringWithoutRequest(string sVar)
        {
            try
            {
                if (_comHmi != null)
                {
                    if (_comHmi.Conectado)
                    {
                        _comHmi.HacerPeticionSinRespuesta(sVar);
                        //_comHmi.HacerPeticionConRespuesta(sVar, TIMEOUT);
                    }
                    else
                    {
                        TryReconnect();
                        if (!_bErrorDisplayed)
                        {
                            _bErrorDisplayed = true;
                            DataError(_KernelErrorConnection);
                        }
                    }
                }
            }
            catch
            {
                if (_comHmi.Conectado)
                    GlobalState.WriteLogEvent(String.Format("ComHmiMgr - Error StringWithoutRequest(): {0}", sVar), CretaTypes.ELogTypes.LOG_EXCEPTION);
               // MessageBox.Show(String.Format("ComHmiMgr (Avisar a Víctor) - Error al enviar la cadena sin respuesta: {0}", sVar));
            }
        }
        /// <summary>
        /// Check if The ComHmi Manager is Ok
        /// </summary>
        /// <returns></returns>
        public bool IsComHmiMgrOk()
        {
            return (_comHmi != null) && _comHmi.Conectado;
        }

        public void DataError()
        {
            DataError("ComHmi communication error");
        }

        public void DataError(string sText)
        {
            GlobalState.WriteLogEvent(sText,CretaTypes.ELogTypes.LOG_ERROR);
#if DEBUG
            MessageBox.Show(sText);
#else
            GlobalState.CretaMessageBox.ShowError("CretaBase:Strings:G_ERROR_COMMAND", MessageBoxButton.OK, false);
#endif
        }
    }
}
