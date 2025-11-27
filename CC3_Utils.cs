using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using CretaBase;
using System.Windows.Controls;
using Creta.CC3.CC3Types;

namespace CC3_GUI
{
    public static class CC3_Utils
    {
        public static string ModeNormal { get; set; }
        public static string ModeIncremental { get; set; }
        public static string ModeRandom { get; set; }
        public static string ModeTiling { get; set; }
        public static string ModeManual { get; set; }
        public static string ModeVision { get; set; }
        public static void ResetTranslationTokens()
        {
            ModeNormal = LocalizationMgr.GetUIString("CretaBase:Strings:MODEL_MODES_NORMAL");
            ModeIncremental = LocalizationMgr.GetUIString("CretaBase:Strings:MODEL_MODES_INCREMENTAL");
            ModeRandom = LocalizationMgr.GetUIString("CretaBase:Strings:MODEL_MODES_RANDOM");
            ModeTiling = LocalizationMgr.GetUIString("CretaBase:Strings:MODEL_MODES_TILING");
            ModeManual = LocalizationMgr.GetUIString("CretaBase:Strings:MODEL_MODES_MANUAL");
            ModeVision = LocalizationMgr.GetUIString("CretaBase:Strings:MODEL_MODES_CRETAVISION");
        }

        public static void RegisterAction(int iNum = -1, string sPlace = "-", string sParam = "", bool bSystemAction=false)
        {
            string sUser = bSystemAction ? "SYSTEM" : GlobalState.UserName;
            string sSend = CretaTypes.COM_ACTION_REGISTER + sPlace + CretaTypes.COM_CHAR_SEPARATOR_TOKENS_MSG + sParam +
                CretaTypes.COM_CHAR_SEPARATOR_TOKENS_MSG + sUser + CretaTypes.COM_CHAR_SEPARATOR_TOKENS_MSG + iNum.ToString();

            GlobalState.ComHmiMgr.StringWithoutRequest(sSend);
        }

        /// <summary>
        /// Gets the translated name corresponding to the enumModoImpresion
        /// </summary>
        public static string GetPrintModeName(Creta.CC3.CC3Types.Tipos.enumModoImpresion eMode)
        {
            string sName = "Mode error";
            switch (eMode)
            {
                case Creta.CC3.CC3Types.Tipos.enumModoImpresion.Normal:
                    sName = ModeNormal;
                    break;
                case Creta.CC3.CC3Types.Tipos.enumModoImpresion.Incremental:
                    sName = ModeIncremental;
                    break;
                case Creta.CC3.CC3Types.Tipos.enumModoImpresion.Random:
                    sName = ModeRandom;
                    break;
                case Creta.CC3.CC3Types.Tipos.enumModoImpresion.Cuadricula:
                    sName = ModeTiling;
                    break;
                case Creta.CC3.CC3Types.Tipos.enumModoImpresion.Manual:
                    sName = ModeManual;
                    break;
                case Creta.CC3.CC3Types.Tipos.enumModoImpresion.Vision:
                    sName = ModeVision;
                    break;
            }

            return sName;
        }

        /// <summary>
        /// Gets the translated name corresponding to the enumModoImpresion
        /// </summary>
        public static string GetPrintSimulationModeName(PrintModeSimulationInfo.EditSimulationMode eMode)
        {
            string sName = "Mode error";
            switch (eMode)
            {
                case PrintModeSimulationInfo.EditSimulationMode.SIMULATION_EDIT:
                    sName = LocalizationMgr.GetUIString("CretaBase:Strings:SIMULATION_IMG_MODE_EDIT");
                    break;
                case PrintModeSimulationInfo.EditSimulationMode.SIMULATION_ONLY_ONE:
                    sName = LocalizationMgr.GetUIString("CretaBase:Strings:SIMULATION_IMG_MODE_ONE");
                    break;
                case PrintModeSimulationInfo.EditSimulationMode.SIMULATION_ALL:
                    sName = LocalizationMgr.GetUIString("CretaBase:Strings:SIMULATION_IMG_MODE_ALL");
                    break;
            }
            return sName;
        }
        
        /// <summary>
        /// Returns the string corresponding to mode (Manual, Automatic or 0)
        /// </summary>
        public static string GetManAutoString(bool bMan, bool bAuto)
        {
            string sRes = "";

            if (bMan)
                sRes = LocalizationMgr.GetUIString("CretaBase:Strings:MAIN_MANUAL");
            else if (bAuto)
                sRes = LocalizationMgr.GetUIString("CretaBase:Strings:MAIN_AUTOMATIC");
            else
                sRes = "0";

            return sRes;
        }

        public static CretaTypes.enumTipoCabezal GetPrintheadTypeFromString(string sPrintHeadType)
        {
            CretaTypes.enumTipoCabezal eType = CretaTypes.enumTipoCabezal.X;

            if (sPrintHeadType == CretaTypes.enumTipoCabezal.T.ToString()) eType = CretaTypes.enumTipoCabezal.T;
            else if (sPrintHeadType == CretaTypes.enumTipoCabezal.X.ToString()) eType = CretaTypes.enumTipoCabezal.X;
            else if (sPrintHeadType == CretaTypes.enumTipoCabezal.S.ToString()) eType = CretaTypes.enumTipoCabezal.S;
            else if (sPrintHeadType == CretaTypes.enumTipoCabezal.XGS40.ToString()) eType = CretaTypes.enumTipoCabezal.XGS40;


            return eType;
        }

        #region ConvertTrayStateToImgSource(string sTrayState)
        public const string IMG_TRAY_CLEANING = "/CretaBase;component/Images/barra_limpieza.png";
        public const string IMG_TRAY_MAINTENANCE = "/CretaBase;component/Images/barra_mantenim.png";
        public const string IMG_TRAY_OVERTRAY = "/CretaBase;component/Images/barra_capped.png";
        public const string IMG_TRAY_EXTRACTION = "/CretaBase;component/Images/alert.png";
        public const string IMG_TRAY_PRINTING = "/CretaBase;component/Images/barra_pintado.png";
        public static string ConvertTrayStateToImgSource(string sTrayState)
        {
            string sRes = CretaTypes.IMG_BAD;
            int iState = CretaBase.CretaUtils.ToInt(sTrayState);
            switch (iState)
            {
                case 0: //Desconocido
                    sRes = CretaTypes.IMG_BAD   ;
                    break;
                case 1: //Yendo a mantenimiento
                case 2: //En mantenimiento
                    sRes = IMG_TRAY_MAINTENANCE;
                    break;
                case 3: //Yendo a sobrebandeja
                case 4: //En sobrebandeja
                    sRes = IMG_TRAY_OVERTRAY;
                    break;
                case 5: //Yendo a extracción
                case 6: //En extracción
                    sRes = IMG_TRAY_EXTRACTION;
                    break;
                case 7: //Yendo a pintado
                case 8: //En pintado
                    sRes = IMG_TRAY_PRINTING;
                    break;
                case 9: //Yendo a limpieza
                case 10: //En limpieza
                    sRes = IMG_TRAY_CLEANING;
                    break;
            }
            return sRes;
        }

#if PLOTTER
        private const string IMG_PLOTTER_AUTOTUNE = "/CretaBase;component/Images/autotune.png";
        private const string IMG_PLOTTER_HOMING = "/CretaBase;component/Images/referenciar.png";
        public static string ConvertTrayStateToImgSourcePlotter(string sTrayState)
        {
            string sRes = CretaTypes.IMG_BAD;
            int iState = CretaBase.CretaUtils.ToInt(sTrayState);
            switch (iState)
            {
                case 0: //0
                case 1: //Manual
                case 2: //Desconocido
                case 10: //Homing error
                case 13: //Autotune X error
                case 16: //Autotune Y error
                case 19: //Autotune Z error
                case 22: //Scanning error
                case 25: //Belt print error
                    sRes = CretaTypes.IMG_BAD;
                    break;
                case 3: //Yendo a sobrebandeja
                case 4: //En sobrebandeja
                    sRes = IMG_TRAY_OVERTRAY;
                    break;
                case 5: //Yendo a mantenimiento
                case 6: //En mantenimiento
                    sRes = IMG_TRAY_MAINTENANCE;
                    break;
                case 7: //En limpieza
                    sRes = IMG_TRAY_CLEANING;
                    break;
                case 8: //Homing
                case 9: //Homing OK
                    sRes = IMG_PLOTTER_HOMING;
                    break;
                case 11: //Autotune X
                case 12: //Autotune X OK
                case 14: //Autotune Y
                case 15: //Autotune Y OK
                case 17: //Autotune Z
                case 18: //Autotune Z OK
                    sRes = IMG_PLOTTER_AUTOTUNE;
                    break;
                case 20://Scanning
                case 21://Scanning OK
                    sRes = IMG_TRAY_PRINTING;
#warning Cambiar la imagen del modo Scanning
                    break;
                case 23://Ir pintado banda
                case 24://En pintado banda
                    sRes = IMG_TRAY_PRINTING;
                    break;
  
            }
            return sRes;
        }
#endif
        #endregion

        #region Convert Language names to enum and Language enums to names
        public static CretaTypes.ELanguages ConvertLanguageNameToEnum(string sLang)
        {
            CretaTypes.ELanguages eLang = CretaTypes.ELanguages.LANG_ENGLISH_EN;

            if (sLang == CretaTypes.STR_SPANISH)
                eLang = CretaTypes.ELanguages.LANG_SPANISH_ES;
            else if (sLang == CretaTypes.STR_ENGLISH)
                eLang = CretaTypes.ELanguages.LANG_ENGLISH_EN;
            else if (sLang == CretaTypes.STR_ITALIAN)
                eLang = CretaTypes.ELanguages.LANG_ITALIAN_IT;
            else if (sLang == CretaTypes.STR_POLSKI)
                eLang = CretaTypes.ELanguages.LANG_POLSKI_PL;
            else if (sLang == CretaTypes.STR_CHINESE)
                eLang = CretaTypes.ELanguages.LANG_CHINESE_ZH;
            else if (sLang == CretaTypes.STR_CZECK)
                eLang = CretaTypes.ELanguages.LANG_CZECH_CS;
            else if (sLang == CretaTypes.STR_FRENCH)
                eLang = CretaTypes.ELanguages.LANG_FRENCH_FR;
            else if (sLang == CretaTypes.STR_TURK)
                eLang = CretaTypes.ELanguages.LANG_TURK_TR;
            else if (sLang == CretaTypes.STR_RUSSIAN)
                eLang = CretaTypes.ELanguages.LANG_RUSSIAN_RU;
            else if (sLang == CretaTypes.STR_INDONESIAN)
                eLang = CretaTypes.ELanguages.LANG_INDONESIAN_ID;
            else if (sLang == CretaTypes.STR_KOREAN)
                eLang = CretaTypes.ELanguages.LANG_KOREAN_KO;
            else if (sLang == CretaTypes.STR_PORTUGUESE)
                eLang = CretaTypes.ELanguages.LANG_PORTUGUESE_PT;
            else if (sLang == CretaTypes.STR_GERMAN)
                eLang = CretaTypes.ELanguages.LANG_GERMAN_DE;

            return eLang;
        }
        public static string ConvertLangEnumToString(CretaTypes.ELanguages eLang)
        {
            string sLang = "";
            switch (eLang)
            {
                case CretaTypes.ELanguages.LANG_SPANISH_ES:
                    sLang = CretaTypes.STR_SPANISH;
                    break;
                case CretaTypes.ELanguages.LANG_ENGLISH_EN:
                    sLang = CretaTypes.STR_ENGLISH;
                    break;
                case CretaTypes.ELanguages.LANG_ITALIAN_IT:
                    sLang = CretaTypes.STR_ITALIAN;
                    break;
                case CretaTypes.ELanguages.LANG_POLSKI_PL:
                    sLang = CretaTypes.STR_POLSKI;
                    break;
                case CretaTypes.ELanguages.LANG_CHINESE_ZH:
                    sLang = CretaTypes.STR_CHINESE;
                    break;
                case CretaTypes.ELanguages.LANG_CZECH_CS:
                    sLang = CretaTypes.STR_CZECK;
                    break;
                case CretaTypes.ELanguages.LANG_FRENCH_FR:
                    sLang = CretaTypes.STR_FRENCH;
                    break;
                case CretaTypes.ELanguages.LANG_TURK_TR:
                    sLang = CretaTypes.STR_TURK;
                    break;
                case CretaTypes.ELanguages.LANG_RUSSIAN_RU:
                    sLang = CretaTypes.STR_RUSSIAN;
                    break;
                case CretaTypes.ELanguages.LANG_INDONESIAN_ID:
                    sLang = CretaTypes.STR_INDONESIAN;
                    break;
                case CretaTypes.ELanguages.LANG_KOREAN_KO:
                    sLang = CretaTypes.STR_KOREAN;
                    break;
                case CretaTypes.ELanguages.LANG_PORTUGUESE_PT:
                    sLang = CretaTypes.STR_PORTUGUESE;
                    break;
                case CretaTypes.ELanguages.LANG_GERMAN_DE:
                    sLang = CretaTypes.STR_GERMAN;
                    break;
            }

            return sLang;
        }
        public static CretaTypes.ELanguages ConvertLanguageDatabaseToEnum(string sLang)
        {
            CretaTypes.ELanguages eLang = CretaTypes.ELanguages.LANG_ENGLISH_EN;

            switch (sLang)
            {
                case CretaTypes.STR_SPANISH_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_SPANISH_ES;
                    break;
                case CretaTypes.STR_ENGLISH_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_ENGLISH_EN;
                    break;
                case CretaTypes.STR_ITALIAN_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_ITALIAN_IT;
                    break;
                case CretaTypes.STR_POLSKI_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_POLSKI_PL;
                    break;
                case CretaTypes.STR_CHINESE_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_CHINESE_ZH;
                    break;
                case CretaTypes.STR_CZECK_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_CZECH_CS;
                    break;
                case CretaTypes.STR_FRENCH_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_FRENCH_FR;
                    break;
                case CretaTypes.STR_TURK_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_TURK_TR;
                    break;
                case CretaTypes.STR_RUSSIAN_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_RUSSIAN_RU;
                    break;
                case CretaTypes.STR_INDONESIAN_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_INDONESIAN_ID;
                    break;
                case CretaTypes.STR_KOREAN_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_KOREAN_KO;
                    break;
                case CretaTypes.STR_PORTUGUESE_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_PORTUGUESE_PT;
                    break;
                case CretaTypes.STR_GERMAN_USER_DB:
                    eLang = CretaTypes.ELanguages.LANG_GERMAN_DE;
                    break;
            }

            return eLang;
        }
        public static string ConvertLangEnumToLangDatabase(CretaTypes.ELanguages eLang)
        {
            string sLang = CretaTypes.STR_ENGLISH_USER_DB;
            switch (eLang)
            {
                case CretaTypes.ELanguages.LANG_SPANISH_ES:
                    sLang = CretaTypes.STR_SPANISH_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_ENGLISH_EN:
                    sLang = CretaTypes.STR_ENGLISH_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_ITALIAN_IT:
                    sLang = CretaTypes.STR_ITALIAN_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_POLSKI_PL:
                    sLang = CretaTypes.STR_POLSKI_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_CHINESE_ZH:
                    sLang = CretaTypes.STR_CHINESE_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_CZECH_CS:
                    sLang = CretaTypes.STR_CZECK_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_FRENCH_FR:
                    sLang = CretaTypes.STR_FRENCH_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_TURK_TR:
                    sLang = CretaTypes.STR_TURK_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_RUSSIAN_RU:
                    sLang = CretaTypes.STR_RUSSIAN_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_INDONESIAN_ID:
                    sLang = CretaTypes.STR_INDONESIAN_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_KOREAN_KO:
                    sLang = CretaTypes.STR_KOREAN_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_PORTUGUESE_PT:
                    sLang = CretaTypes.STR_PORTUGUESE_USER_DB;
                    break;
                case CretaTypes.ELanguages.LANG_GERMAN_DE:
                    sLang = CretaTypes.STR_GERMAN_USER_DB;
                    break;
            }

            return sLang;
        }
        #endregion

        #region Convert Security Level enum to String
        public static string ConvertSecurityLevelEnumToString(CretaTypes.ESecurityLevel eLevel)
        {
            string sLevel = "";
            switch (eLevel)
            {
                case CretaTypes.ESecurityLevel.LEVEL_LOW:
                    sLevel = LocalizationMgr.GetUIString("CretaBase:Strings:M_USER_LEVEL_LOW");
                    break;
                case CretaTypes.ESecurityLevel.LEVEL_MED:
                    sLevel = LocalizationMgr.GetUIString("CretaBase:Strings:M_USER_LEVEL_MED");
                    break;
                case CretaTypes.ESecurityLevel.LEVEL_HIGH:
                    sLevel = LocalizationMgr.GetUIString("CretaBase:Strings:M_USER_LEVEL_HIGH");
                    break;
                case CretaTypes.ESecurityLevel.LEVEL_ADMIN:
                    sLevel = LocalizationMgr.GetUIString("CretaBase:Strings:M_USER_LEVEL_ADMIN");
                    break;
            }
            return sLevel;
        }
        public static CretaTypes.ESecurityLevel GetSecurityLevelEnumFromInt(int iLevel)
        {
            CretaTypes.ESecurityLevel eLevel = CretaTypes.ESecurityLevel.LEVEL_ZERO;
            
            if (iLevel == (int)CretaTypes.ESecurityLevel.LEVEL_LOW)
                eLevel = CretaTypes.ESecurityLevel.LEVEL_LOW;
            else if (iLevel == (int)CretaTypes.ESecurityLevel.LEVEL_MED)
                eLevel = CretaTypes.ESecurityLevel.LEVEL_MED;
            else if (iLevel == (int)CretaTypes.ESecurityLevel.LEVEL_HIGH)
                eLevel = CretaTypes.ESecurityLevel.LEVEL_HIGH;
            else if (iLevel == (int)CretaTypes.ESecurityLevel.LEVEL_ADMIN)
                eLevel = CretaTypes.ESecurityLevel.LEVEL_ADMIN;

            return eLevel;
        }
        #endregion

        #region Convert Camera origin enum to string 
        public static Creta.CC3.CC3Types.Tipos.eOrigenVision ConvertStringToOrigenVisionEnum(string sOrigen)
        {
            Creta.CC3.CC3Types.Tipos.eOrigenVision eOrigen = Creta.CC3.CC3Types.Tipos.eOrigenVision.Ninguno;

            if (Creta.CC3.CC3Types.Tipos.eOrigenVision.CamaraCongnex.ToString() == sOrigen)
                eOrigen = Creta.CC3.CC3Types.Tipos.eOrigenVision.CamaraCongnex;
            else if (Creta.CC3.CC3Types.Tipos.eOrigenVision.EntradasPLC1.ToString() == sOrigen)
                eOrigen = Creta.CC3.CC3Types.Tipos.eOrigenVision.EntradasPLC1;
            else if (Creta.CC3.CC3Types.Tipos.eOrigenVision.Sync.ToString() == sOrigen)
                eOrigen = Creta.CC3.CC3Types.Tipos.eOrigenVision.Sync;
            
            return eOrigen;
        }
        #endregion

        public static void ScaleContentToScreenSize(UserControl UsrCtrl)
        {
            //double ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            //double ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double ScreenWidth = App.Current.MainWindow.Width;
            double ScreenHeight = App.Current.MainWindow.Height;

            double WidthScale = ScreenWidth / CretaTypes.SCREEN_WIDTH;
            double HeightScale = ScreenHeight / CretaTypes.SCREEN_HEIGHT;
            double Scale = WidthScale < HeightScale ? WidthScale : HeightScale;
            //Scale = 0.75;
            if (Scale < 1)
            {
                ScaleTransform scaleTransform1 = new ScaleTransform(Scale, Scale);
                UsrCtrl.LayoutTransform = scaleTransform1;
            }
        }

        /// <summary>
        /// Fills the color of each channel (avoiding the bar possition)
        /// </summary>
        public static void FillTASManualColors(List<Color> ChannelClr, List<Color> ChannelClrBrdr, List<bool> ChannelExist)
        {
            ChannelClr.Clear();
            ChannelClrBrdr.Clear();
            ChannelExist.Clear();
            for (int i = 0; i < CretaTypes.PRINT_MAX_BARS; ++i)
            {
                Color c = Color.FromArgb(0, 0, 0, 0);
                Color cBorder = Color.FromArgb(0, 0, 0, 0);
                bool bFound = false;
                for (int j = 0; j < GlobalState.PrinterDescVM.Desc_BarsList.Count && !bFound; j++)
                {
                    if (GlobalState.PrinterDescVM.Desc_BarsList[j].ColorPlane == i)
                    {
                        bFound = true;
                        c = GlobalState.PrinterDescVM.Desc_BarsList[j].BarColor.Color;
                        cBorder = Color.FromArgb(255, 51, 51, 51);
                    }
                }
                ChannelClr.Add(c);
                ChannelClrBrdr.Add(cBorder);
                ChannelExist.Add(bFound);
            }
        }

        /// <summary>
        /// Fills the color of each Bar (not channel)
        /// </summary>
        public static void FillBarsChannelColorsC4(List<Color> ChannelClr, List<Color> ChannelClrBrdr, List<bool> ChannelExist)
        {
            ChannelClr.Clear();
            ChannelClrBrdr.Clear();
            ChannelExist.Clear();
            for (int i = 0; i < CretaTypes.PRINT_MAX_BARS; ++i)
            {
                Color c = Color.FromArgb(0, 0, 0, 0);
                Color cBorder = Color.FromArgb(0, 0, 0, 0);
                bool bFound = false;
                if (i < GlobalState.MachineState.NumOfBars)
                {
                    c = GlobalState.PrinterDescVM.Desc_BarsList[i].BarColor.Color;
                    cBorder = Color.FromArgb(255, 51, 51, 51);
                    bFound = true;
                }
                ChannelClr.Add(c);
                ChannelClrBrdr.Add(cBorder);
                ChannelExist.Add(bFound);
            }
        }


        public static double CalculaAnchoImpresion(CretaTypes.enumTipoCabezal tipoCabezal, int numCabezales)
        {
            double _anchoBarra = 0.0;
            switch (tipoCabezal)
            {
                case CretaTypes.enumTipoCabezal.X:
                    _anchoBarra = numCabezales * PrintHeads_AlignmentVM.XAAR_Y;
                   break;
                case CretaTypes.enumTipoCabezal.XGS40:
                   _anchoBarra = numCabezales * PrintHeads_AlignmentVM.XAAR_Y;
                   break;
                case CretaTypes.enumTipoCabezal.T:
                    _anchoBarra = numCabezales * PrintHeads_AlignmentVM.TTEC_Y;
                    break;
                case CretaTypes.enumTipoCabezal.S:
                    _anchoBarra = numCabezales * PrintHeads_AlignmentVM.SEIKO_Y;
                    break;
                default:
                    _anchoBarra = 0.0;
                    break;
            }
            return _anchoBarra;
        }
        public static double AnchoLineaMin(int linea)
        {
            double _minAnchoLinea = Double.MaxValue;

            if (GlobalState.MachineState.IsOneLine && (linea == 2)) return 0.0;

            for (int i = 0; i <= GlobalState.PrinterDescVM.Desc_General.NumBars - 1; i++)
            {
                CretaTypes.enumTipoCabezal tipoCabezal = GlobalState.PrinterDescVM.Desc_BarsList[i].PrintHeadType;
                int numCabezales;
                if (GlobalState.MachineState.IsOneLine)
                {
                    numCabezales = GlobalState.PrinterDescVM.Desc_BarsList[i].NumRobotHeadsL1 + GlobalState.PrinterDescVM.Desc_BarsList[i].NumRobotHeadsL2;
                }
                else
                {
                    numCabezales = (linea == 1) ? GlobalState.PrinterDescVM.Desc_BarsList[i].NumRobotHeadsL1 : GlobalState.PrinterDescVM.Desc_BarsList[i].NumRobotHeadsL2;
                }
                double anchoImpresion = CalculaAnchoImpresion(tipoCabezal, numCabezales);
                if (anchoImpresion < _minAnchoLinea)
                {
                    _minAnchoLinea = anchoImpresion;
                }
            }
            return _minAnchoLinea;
        }

        private const string M_EDIT_ADJUST_TILE_Y = "YTILEADJUSTMENT";
        public static bool OffsetExceedsLineWidth(double L1AdjustY, double L2AdjustY)
        {
            double anchoImpresionLinea1 = CC3_Utils.AnchoLineaMin(1);
            double anchoImpresionLinea2 = CC3_Utils.AnchoLineaMin(2);
            if ((anchoImpresionLinea1 > 0) && (Math.Abs(L1AdjustY) >= anchoImpresionLinea1))
            {
                return true;
            }
            if ((anchoImpresionLinea2 > 0) && (Math.Abs(L2AdjustY) >= anchoImpresionLinea2))
            {
                return true;
            }
            return false;
        }
        public static bool OffsetExceedsLineWidth()
        {
            //Coger datos del kernel, poner los offsets y si es centrado 
            string sSend = CretaTypes.COM_GET_VARS;
            string sReply = "";
            string[] asReply = null;
            int iCountVars = 0;
            bool bMustExit = false;
            double L1AdjustY = 0.0;
            double L2AdjustY = 0.0;

            for (int z = 0; z < 2 && !bMustExit; ++z)
            {
                bool bRequest = z == 0;

                if (bRequest) { sSend += CretaTypes.COM_PRT + M_EDIT_ADJUST_TILE_Y + "L1" + CretaTypes.COM_CHAR_SEPARATOR_TOKENS_MSG; }
                else { L1AdjustY = CretaUtils.ToDouble(asReply[iCountVars]); iCountVars++; }
                if (bRequest) { sSend += CretaTypes.COM_PRT + M_EDIT_ADJUST_TILE_Y + "L2" + CretaTypes.COM_CHAR_SEPARATOR_TOKENS_MSG; }
                else { L2AdjustY = CretaUtils.ToDouble(asReply[iCountVars]); iCountVars++; }
                if (bRequest)
                {
                    //Send Data to kernel
                    sReply = GlobalState.ComHmiMgr.StringRequest(sSend);
                    if (sReply.Length > CretaTypes.COM_START_INDEX_MSG + 1)
                    {
                        sReply = sReply.Substring(CretaTypes.COM_START_INDEX_MSG);
                        asReply = sReply.Split(CretaTypes.COM_CHAR_SEPARATOR_TOKENS_MSG);
                    }
                    else
                        bMustExit = true;
                }
            }
            return OffsetExceedsLineWidth(L1AdjustY, L2AdjustY);
        }
    
    }
}
