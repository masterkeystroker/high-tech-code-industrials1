using System;
using CretaBase;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;

namespace CC3_GUI
{
    public static class GlobalState
    {
        public static MainWindow MainWindow;
        public static ComHmiMgr ComHmiMgr;
        //public static PopUpMgr PopUpMgr;
        public static TimerUpdateMgr TimerUpdateMgr;
        public static CretaMessageBox CretaMessageBox;
        public static LoadingDataMgr LoadingDataMgr;
        public static FieryImageEditorProcessMgr FieryImageEditorMgr;
        public static MachineState MachineState;
        //DataContext ViewModels
        public static ModelsVM ModelsVM;
        public static MainVM MainVM;
        public static ActivationVM ActivationVM;
        public static PrinterDescriptionVM PrinterDescVM;
        public static PrintHeads_AlignmentVM PrintHeads_AlignmentVM;

        //Basic information
        public static bool IsRightHand { get; set; } //True is a Right Direction, false Left
        //public static bool IsOneLine { get; set; }
        public static int NumOfBars { get; set; }
        //Users
        public static string UserName { get; set; }
        
        
       

        private static CretaTypes.ESecurityLevel _userLevel;
        public static CretaTypes.ESecurityLevel UserLevel 
        {
            get { return _userLevel; }
            set
            {
                if (_userLevel != value)
                {
                    _userLevel = value;
                    MachineState.IsAdminLevelUser = _userLevel == CretaTypes.ESecurityLevel.LEVEL_ADMIN;
                    MachineState.IsHighLevelUser = MachineState.IsAdminLevelUser || _userLevel == CretaTypes.ESecurityLevel.LEVEL_HIGH;
                    MachineState.IsMedLevelUser = MachineState.IsHighLevelUser || _userLevel == CretaTypes.ESecurityLevel.LEVEL_MED;
                    GlobalState.MainVM.MenuQualitiesVisible = _userLevel >= CretaTypes.ESecurityLevel.LEVEL_MED ? Visibility.Visible : Visibility.Collapsed;
                    if (_userLevel >= CretaTypes.ESecurityLevel.LEVEL_HIGH)
                    {
                        if (GlobalState.MachineState.ElectronicsType == CretaTypes.enumTipoElectronica.ROBOT)
                        {
                            GlobalState.MainVM.MenuEfiDiagnosticsVisible = Visibility.Visible;
                        }
                        else 
                        {
                            GlobalState.MainVM.MenuEfiDiagnosticsVisible = Visibility.Collapsed;
                        }
                        
                    }
                    else
                    {
                        GlobalState.MainVM.MenuEfiDiagnosticsVisible = Visibility.Collapsed;
                    }
                       
                }
            }
        }
        /// <summary>
        /// El nivel devuelto variará según el nivel de seguridad ajustado en descripción de máquina
        ///     Si Creta.CC3.CC3Types.Tipos.eModoSeguridad == Minima
        ///         LEVEL_ZERO == LEVEL_LOW == LEVEL_MED == LEVEL_HIGH
        ///     Si Creta.CC3.CC3Types.Tipos.eModoSeguridad == Media
        ///         LEVEL_MED == LEVEL_HIGH
        ///     Si Creta.CC3.CC3Types.Tipos.eModoSeguridad == Maxima
        ///         cada nivel corresponde al suyo
        /// </summary>
        /// 




       

        public static CretaTypes.ESecurityLevel GetUserLevelAccordingToSecurity()
        {
            CretaTypes.ESecurityLevel ResUsrLevel = UserLevel;

            if (SecurityLevel == Creta.CC3.CC3Types.Tipos.eModoSeguridad.Minima)
            {
                //Se trata todo como si solo hubiese dos niveles, ADMIN y HIGH para el resto
                if (UserLevel != CretaTypes.ESecurityLevel.LEVEL_ADMIN)
                    ResUsrLevel = CretaTypes.ESecurityLevel.LEVEL_HIGH;
            }
            else if (SecurityLevel == Creta.CC3.CC3Types.Tipos.eModoSeguridad.Media)
            {
                //El nivel MED y HIGH son HIGH
                if (UserLevel == CretaTypes.ESecurityLevel.LEVEL_MED)
                    ResUsrLevel = CretaTypes.ESecurityLevel.LEVEL_HIGH;
            }
            /* Si la seguridad es maxima se trata todo con su seguridad
            else if (SecurityLevel == Creta.CC3.CC3Types.Tipos.eModoSeguridad.Maxima)*/

            return ResUsrLevel;
        }
        public static Creta.CC3.CC3Types.Tipos.eModoSeguridad SecurityLevel { get; set; }
        public static string GeneralLang { get; set; }
        public static string CurrentLang { get; set; }
        //Directories
        public static string PreviewsFilePath { get; set; }
        public static string LogsFilePath { get; set; }
        public static string ConfigsFilePath { get; set; }
        //Licencia de la máquina
        /// <summary>
        /// Es "true" cuando la licencia ha expirado completamente y a la máquina ya no se le permite imprimir (o no hay archivo de licencia o ha expirado el tiempo de la misma)
        /// </summary>
        public static bool IsInvalidLicense { get; set; }
        /// <summary>
        /// Devuelve "true" cuando se tiene que mostrar el tiempo restante hasta que la licencia expire
        /// </summary>
        public static bool MustShowLicenseTimeLeft { get; set; }
        /// <summary>
        /// Devuelve "True" si en la licencia de la aplicación aparece la 
        /// </summary>
        public static bool IsFieryLicenseEnabled { get; set; }
        //Save LogFiles
        public static void WriteLogEvent(string sMessage, CretaTypes.ELogTypes logType = CretaTypes.ELogTypes.LOG_INFO)
        {
            string sFinalText = logType.ToString() + ";" + sMessage;
            DateTime dt = DateTime.Now;
            string sFile = GlobalState.LogsFilePath + "LogGui_" + dt.Year + "_" + dt.Month + "_" + dt.Day + ".txt";
            CretaUtils.WriteLogEvent(sFinalText, sFile);
        }

        //ChangeLanguage 
        public static void ChangeLang(string sLang)
        {
            LocalizationMgr.ChangeLanguage(sLang);
            CurrentLang = sLang;

            if (UserName == "" && sLang != "")
            {
                GeneralLang = sLang;

                if (System.IO.File.Exists(GlobalState.LogsFilePath + CretaTypes.LANG_FILE))
                {
                    System.IO.File.Delete(GlobalState.LogsFilePath + CretaTypes.LANG_FILE);
                }
                System.IO.StreamWriter file = new System.IO.StreamWriter(GlobalState.LogsFilePath + CretaTypes.LANG_FILE);
                file.WriteLine(sLang);
                file.Close();
            }
            else if (UserName != "")
                GlobalState.MainVM.StrUser = LocalizationMgr.GetUIString("CretaBase:Strings:MAIN_USER") + " " + GlobalState.UserName;
                        
            GlobalState.MainVM.TranslateLowerInfo();
        }

        //Show error or warning messages
        public static void ShowKernelErrorWarningMsg(string sNum)
        {
            string sMsg = "CC3_GUI:Errors:ERROR_" + CretaUtils.AddStartChars(sNum, 4, "0");
            int iNum = CretaUtils.ToInt(sNum);
            if (iNum >= 999)
                CretaMessageBox.ShowError(sMsg,MessageBoxButton.OK,false);
            else if (iNum > 0)
                CretaMessageBox.ShowWarning(sMsg, MessageBoxButton.OK, false);
            else if (iNum < 0)
            {
#if DEBUG
                MessageBox.Show("El valor pasado es menor que 0, puede que la conversión a entero haya fallado (este mensaje sólo aparece en Debug)");
#endif
                WriteLogEvent("ShowKernelErrorWarningMsg:El valor pasado es menor que 0, puede que la conversión a entero haya fallado",CretaTypes.ELogTypes.LOG_WARNING);
            }
        }
        public static bool IsPrintServerConnected()
        {
            bool bConnected = false;
            string sSend = CretaTypes.COM_GET_VARS + CretaTypes.COM_PRT +   PrintStatusRobotVM.PRINT_CONNECTION_OK + CretaTypes.COM_CHAR_SEPARATOR_TOKENS_MSG;
            string sReply = GlobalState.ComHmiMgr.StringRequest(sSend);
            if (sReply.Length > CretaTypes.COM_START_INDEX_MSG + 1)
            {
                sReply = sReply.Substring(CretaTypes.COM_START_INDEX_MSG);
                bConnected = CretaUtils.AreEqual(sReply, CretaTypes.COM_TRUE);
            }
            return bConnected;
        }
    }
}
