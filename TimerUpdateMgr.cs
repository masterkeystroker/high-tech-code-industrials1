using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Threading;
using CretaBase;

namespace CC3_GUI
{
    public class TimerUpdateMgr
    {
        #region Members
        private bool _checkReturnToMain = false;
        private DateTime _returnToMainTime;
        const int TIME_TO_RETURN_MAIN = 1800;
        private bool _checkUserLogout = false;
        private DateTime _userLogoutTime;
        const int TIME_TO_LOGOUT_USER = 900;
        private DateTime _logsManagementTime;
        const int TIME_MINS_TO_MANAGE_LOGS = 1440;//minutes in 24 hours
        const int DAYS_TO_DELETE_LOGS = 60;//Max days to mantain the logs
        private DateTime _alignBackupManagementTime;
        const int DAYS_TO_DELETE_ALIGN_BACKUP = 60;//max days to mantain the config backup files
        private System.Threading.Thread _th = null;
        private DateTime _checkUsersUSBTime;
        private bool _bFinished = false;
        #endregion

        public TimerUpdateMgr()
        {
            //Start del hilo de gestión de los updates
            _th = new System.Threading.Thread(UpdateThread);
            _th.Name = "TimeUpdateMgr";
            _th.IsBackground = true;
            _th.Start();

            _logsManagementTime = DateTime.Now;
            _alignBackupManagementTime = DateTime.Now;
            _checkUsersUSBTime = DateTime.Now;
        }
        #region Properties
        #endregion

        private void UpdateThread()
        {
            while (!_bFinished)
            {
                //UpdateReturnToMain();

                UpdateUserLogout();

                UpdateLogsManagement();

                UpdateAlignBackupManagement();

                //Esta función chequea las unidades USB para ver si hay un usuario
                UpdateCheckUsersUSB();

                System.Threading.Thread.Sleep(CretaTypes.UPDATE_TIME_FAST);
            }
        }

        private void UpdateReturnToMain()
        {
            TimeSpan elapsedTime = DateTime.Now.Subtract(_returnToMainTime);
            if (_checkReturnToMain && elapsedTime.TotalSeconds >= TIME_TO_RETURN_MAIN)
            {
                _checkReturnToMain = false;
                NavigationMgr.GoToMainScreen();
            }
        }

        private void UpdateUserLogout()
        {
            TimeSpan elapsedTime = DateTime.Now.Subtract(_userLogoutTime);
            if (_checkUserLogout && elapsedTime.TotalSeconds >= TIME_TO_LOGOUT_USER)
            {
                bool bLoggedOut = false;
                //Para evitar que el comando de cambio de idioma lance una excepción por intervenir en el hilo principal desde otro hilo
                GlobalState.MainWindow.Dispatcher.Invoke((Action)(() =>
                {
                    bLoggedOut = GlobalState.MainVM.UserLogOut();
                    if (bLoggedOut)
                    {
                        if (NavigationMgr.CurrentScreen != CretaTypes.EScreenNames.SCREEN_MAIN)
                            NavigationMgr.GoToMainScreen();
                    }
                }));
            }
        }

        private void UpdateLogsManagement()
        {
            TimeSpan elapsedTime = DateTime.Now.Subtract(_logsManagementTime);
            if (elapsedTime.TotalMinutes >= TIME_MINS_TO_MANAGE_LOGS) //Cada 24 horas
            {
                //Borrar los logs de x días atras, de momento probar con los últimos de un mes, borrar los que tengan una fecha anterior a (datetime.now - 30 días) 
                if (System.IO.Directory.Exists(GlobalState.LogsFilePath))
                {
                    string[] filePaths = System.IO.Directory.GetFiles(GlobalState.LogsFilePath,"Log*");
                    foreach (string file in filePaths)
                    {
                        System.IO.FileInfo info = new System.IO.FileInfo(file);
                        if (info.LastWriteTime < DateTime.Now.AddDays(-DAYS_TO_DELETE_LOGS))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                }
                _logsManagementTime = DateTime.Now;
            }
        }

        private void UpdateAlignBackupManagement()
        {
            const int MIN_FILES_TO_MANTAIN = 20;
            TimeSpan elapsedTime = DateTime.Now.Subtract(_alignBackupManagementTime);
            if (elapsedTime.TotalMinutes >= TIME_MINS_TO_MANAGE_LOGS) //cada 24h
            {
                //Borrar los backups automáticos de x días atrás, borrar los que tengan una fecha anterior a (datetime.now - 60 días) 
                if (System.IO.Directory.Exists(GlobalState.ConfigsFilePath))
                {
                    //string[] filePaths = System.IO.Directory.GetFiles(GlobalState.ConfigsFilePath,"Config_*");
                    List<string> ConfigFiles = System.IO.Directory.GetFiles(GlobalState.ConfigsFilePath, "Config_*").ToList();
                    ConfigFiles.Sort();
                    //Si las configuraciones son muy antiguas (de una puesta en marcha de hace 1 año) y desde entonces no se han guardado más
                    //  nos quedaríamos sin puntos de restauración, en ese caso, al estar ordenados los archivos por fecha de menor a mayor 
                    //  siempre dejaré almenos los últimos 20 archivos.
                    int iFilesRemaining = ConfigFiles.Count;
                    foreach (string file in ConfigFiles)
                    {
                        System.IO.FileInfo info = new System.IO.FileInfo(file);
                        if (info.LastWriteTime < DateTime.Now.AddDays(-DAYS_TO_DELETE_ALIGN_BACKUP))
                        {
                            if (iFilesRemaining > MIN_FILES_TO_MANTAIN)
                            {
                                System.IO.File.Delete(file);
                                iFilesRemaining--;
                            }
                            else
                                break;
                        }
                    }
                    if(ConfigFiles.Count != iFilesRemaining)
                        GlobalState.WriteLogEvent(String.Format("Se han borrado {0} logs de backup automático", ConfigFiles.Count - iFilesRemaining), CretaTypes.ELogTypes.LOG_INFO);
                }
                _alignBackupManagementTime = DateTime.Now;
            }
        }

        /// <summary>
        /// User activity to reset the timers
        /// </summary>
        public void UserActivityRegistered()
        {
            if (_checkReturnToMain)
                _returnToMainTime = DateTime.Now;

            if (_checkUserLogout)
                _userLogoutTime = DateTime.Now;
        }

        public void StartReturnToMainTimer()
        {
            _checkReturnToMain = true;
            _returnToMainTime = DateTime.Now;
        }
        public void StopReturnMainTimer()
        {
            _checkReturnToMain = false;
        }

        public void StartLogoutUserTimer()
        {
            _checkUserLogout = true;
            _userLogoutTime = DateTime.Now;
        }
        public void StopLogoutUserTimer()
        {
            _checkUserLogout = false;
        }

        #region Check Users USB
        /// <summary>
        /// Obtiene el contenido del archivo de usuario, devuelve cadena vacía si no es el formato esperado
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string ReadUserFile(string filePath)
        {
            string sRes = "";
            try
            {
                byte[] _Key = new byte[32] { 12, 84, 65, 185, 80, 79, 230, 250, 152, 173, 36, 180, 56, 86, 50, 51, 230, 78, 65, 198, 27, 36, 240, 37, 98, 81, 125, 25, 37, 78, 156, 64 };
                byte[] _IV = new byte[16] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                if (File.Exists(filePath))
                {
                    string s = System.Text.Encoding.UTF8.GetString(CretaEncrypt.Decrypt_AES(File.ReadAllBytes(filePath), _Key, _IV));
                    if (s.StartsWith(""))
                        sRes = s;
                }
            }
            catch (Exception ex)
            {
                string sMessage = "Error Read User USB file: " + ex.Message + ex.StackTrace;
                GlobalState.WriteLogEvent(sMessage, CretaTypes.ELogTypes.LOG_EXCEPTION);
            }
            return sRes;
        }

        class cDriveUSB : IEquatable<cDriveUSB>
        {
            public cDriveUSB(string name, string label) { sName = name; sLabel = label; }
            public string sName;
            public string sLabel;
            public bool Equals(cDriveUSB other)
            {
                if (this.sName == other.sName && this.sLabel == other.sLabel)
                    return true;
                else
                    return false;
            }
        };
        class cUserUSBFile
        {
            public string sFilePath;
            public string sUserName;
            public string sPassword;
            public int iNivel;
            public bool bConnected;
            public void Clear() { sFilePath = ""; sUserName = ""; sPassword = ""; iNivel = 0; bConnected = false; }
        };
        cUserUSBFile _currentUSer = new cUserUSBFile();
        private List<cDriveUSB> _unidadesUsb = new List<cDriveUSB>(); //Guarda las unidades de usb para buscar solo al conectar
        /// <summary>
        /// Comprueba cada segundo las unidades USB y busca si hay un usuario distinto del actual y si debe cambiarlo
        /// </summary>
        private void UpdateCheckUsersUSB()
        {
            TimeSpan elapsedTime = DateTime.Now.Subtract(_checkUsersUSBTime);
            if (elapsedTime.TotalMilliseconds >= CretaTypes.UPDATE_TIME_FAST) //cada medio segundo
            {
                try
                {
                    foreach (DriveInfo drive in DriveInfo.GetDrives())
                    {
                        if (drive.DriveType == DriveType.Removable && drive.IsReady)
                        {
                            cDriveUSB driveUSB = new cDriveUSB(drive.Name, drive.VolumeLabel);
                            if (!_unidadesUsb.Contains(driveUSB))
                            {
                                //Detectada nueva unidad usb conectada
                                _unidadesUsb.Add(driveUSB);
                                string sActionParam = "USB IN -> " + driveUSB.sLabel  + " (" + driveUSB.sName + ")";
                                CC3_Utils.RegisterAction((int)CretaTypes.EActionsGUI.ACTION_USB_IN_OUT, CretaTypes.COM_ACTION_APP, sActionParam);

                                bool bNewUserConnected = false;
                                //Busco en el directorio raiz el archivo de tipo usuario
                                string[] aFiles = Directory.GetFiles(drive.Name);
                                foreach (string sFile in aFiles)
                                {
                                    if (sFile.Substring(sFile.Length - 4) == ".cpu")
                                    {
                                        //Lee y añade el archivo a la lista de usuarios
                                        string sData = ReadUserFile(sFile);
                                        if (sData != "")
                                        {
                                            //El fichero de usuario es correcto
                                            string[] aSeparator = new string[] { Environment.NewLine };
                                            string[] aLineas = sData.Split(aSeparator, StringSplitOptions.RemoveEmptyEntries);
                                            if (aLineas.Length == 4 && (aLineas[0] == "SECURITY USER" &&
                                                aLineas[1].StartsWith("USER:") && aLineas[2].StartsWith("PASS:") && aLineas[3].StartsWith("DATE:")))
                                            {
                                                //Usuario leído correctamente
                                                string sName = aLineas[1].Substring(5);
                                                string sPass = aLineas[2].Substring(5);
                                                //Este tiempo es por si por seguridad queremos que caduque el fichero
                                                DateTime Date = new DateTime((long)CretaUtils.ToDouble(aLineas[3].Substring(5)));

                                                //Intento conectarme con el user si no he conectado con ningún usuario de este nuevo usb
                                                if (!bNewUserConnected)
                                                {
                                                    bool bLoginOk = false;
                                                    //Para evitar que el comando de cambio de idioma lance una excepción por intervenir en el hilo principal desde otro hilo
                                                    GlobalState.MainWindow.Dispatcher.Invoke((Action)(() =>
                                                    {
                                                        bLoginOk = GlobalState.MainVM.Login(sName, sPass, true);
                                                    }));
                                                    if (bLoginOk)
                                                    {
                                                        bNewUserConnected = true;
                                                        _currentUSer.sFilePath = sFile;
                                                        _currentUSer.sUserName = sName;
                                                        _currentUSer.sPassword = sPass;
                                                        _currentUSer.bConnected = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //Recorre todos los archivos de la raiz del usb
                                }
                            }
                            //Recorre los usbs conectados
                        }
                        //Revisa todas las unidades 
                    }

                    //Recorro las unidades por si se han quitado borrarlas de la lista
                    for (int i = _unidadesUsb.Count - 1; i >= 0; --i)
                    {
                        string sUsb = _unidadesUsb[i].sName;
                        if (!Directory.Exists(sUsb))
                        {
                            string sActionParam = "USB OUT <- " + _unidadesUsb[i].sLabel + " (" + _unidadesUsb[i].sName + ")";
                            CC3_Utils.RegisterAction((int)CretaTypes.EActionsGUI.ACTION_USB_IN_OUT, CretaTypes.COM_ACTION_APP, sActionParam);
                            _unidadesUsb.RemoveAt(i);
                        }
                    }

                    //Compruebo si el usuario actual está aún conectado al usb para sacarlo
                    if (_currentUSer.bConnected)
                    {
                        //Primero miro si aunque tenga que está conectado es el mismo nombre de usuario que está realmente conectado
                        if (GlobalState.UserName == _currentUSer.sUserName)
                        {
                            //En caso de estar realmente conectado compruebo si está aún la usb
                            //  si no está lo desconecto
                            if (!File.Exists(_currentUSer.sFilePath))
                            {
                                //Para evitar que el comando de cambio de idioma lance una excepción por intervenir en el hilo principal desde otro hilo
                                GlobalState.MainWindow.Dispatcher.Invoke((Action)(() =>
                                {
                                    GlobalState.MainVM.UserLogOut();
                                }));
                                _currentUSer.Clear();
                            }
                        }
                        else //Si no coinciden los nombres de usuario es que se ha desconectado
                            _currentUSer.Clear();
                    }
                }
                catch (Exception ex)
                {
                    string sMessage = "Error Update Check Users USB: " + ex.Message + ex.StackTrace;
                    GlobalState.WriteLogEvent(sMessage, CretaTypes.ELogTypes.LOG_EXCEPTION);
                }
                _checkUsersUSBTime = DateTime.Now;
            }
        }
        #endregion
    }
}
