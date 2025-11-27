using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CC3_GUI
{
    /// <summary>
    /// Types and Constants
    /// </summary>
    public class CretaTypes
    {
        #region Constants

        // ComHmi
        //public const string COMHMI_SERVER_ADRESS = "10.138.64.93"; //Carlos
        //public const string COMHMI_SERVER_ADRESS = "10.138.64.132"; // Master
        public const string COMHMI_SERVER_ADRESS = "localhost";
        public const int COMHMI_PORT = 32000;

        public const int INVALID_VALUE = -9999999;
        public const string INVALID_STR = "ERROR_STR";
        public const double DIFFERENCE_TO_CONSIDER = 0.0001;
        public const int BIG_NUMBER = 100000;

        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 1024;

        public const string LANG_FILE = "lng.cfg";

        // ComHmi Messages
        public const int COM_START_INDEX_MSG = 5;
        public const string COM_GET_VARS = "GVARS";
        public const string COM_SET_VARS = "SVARS";
        public const string COM_ALARMS_RESET = "RALRM";
        public const string COM_ALARM_MUTE = "ASSON";
        public const string COM_ALARM_CONFIRM = "AALRM";
        public const string COM_MODEL_GET = "GMODL";
        public const string COM_MODEL_SET = "SMODL";
        public const string COM_MODEL_DELETE = "DMODL";
        public const string COM_MODEL_COPY = "CMODL";
        public const string COM_MODEL_FAVORITE = "FMODL";
        public const string COM_MODEL_RENDER = "RMODL";
        public const string COM_MODEL_GET_STATE = "GMDST";
        public const string COM_MODEL_RENDER_STOP = "RSTOP";
        public const string COM_MODEL_RENDER_DELETE = "RDELT";
        public const string COM_MODEL_RENDER_ABORT = "RABRT";
        public const string COM_MODEL_IMAGE_ADD = "AIMGS";
        public const string COM_MODEL_IMAGE_DELETE = "DIMGS";
        public const string COM_GET_QUALITY = "GQALT";
        public const string COM_SET_QUALITY = "SQALT";
        public const string COM_DELETE_QUALITY = "DQALT";
        public const string COM_SLAVE_RESTART = "CRSTR";
        public const string COM_SLAVE_SHUTDOWN = "CSTDW";
        public const string COM_SLAVES_PC = "PC";
        public const string COM_CAMERA = "CAM";
        public const string COM_USERS_LOGIN = "ULOGN";
        public const string COM_USERS_LOGOUT = "ULOGO";
        public const string COM_USERS_GET = "GUSER";
        public const string COM_USERS_SET = "SUSER";
        public const string COM_USERS_DEL = "DUSER";
        public const string COM_LICENSE_DAYS_LEFT = "KGTIM"; //Devuelve los dias restantes
        public const string COM_LICENSE_IS_FIERY_ACTIVE = "KGFIE"; //Devuelve si está activo Fiery
        public const string COM_LICENSE_OK = "KGLOK"; //Devuelve si hay archivo de licencia
        public const string COM_LICENSE_GET = "KGKEY"; //Devuelve el código de licencia
        public const string COM_LICENSE_SET = "KSKEY"; //Envía la licencia
        public const string COM_RESET_RPROD = "RPROD";
        public const string COM_EFI_GET_DIAGNOSTICS = "GDIAG";
        public const string COM_EFI_SET_DIAGNOSTICS = "SDIAG";
        public const char COM_CHAR_SEPARATOR_MSGS_ALARMS = '¦';  //Alt+221 - ¦
        public const char COM_CHAR_SEPARATOR_TOKENS_MSG = '¤';  //Alt+207 - ¤
        public const char COM_CHAR_VAR_ASSIGNATION = '=';
        public const char COM_CHAR_SEPARATOR = '_';
        public const string COM_CHAR_ERROR = "#";
        public const string COM_TRUE = "True";
        public const string COM_FALSE = "False";
        public const string COM_TRUE_1 = "1";
        public const string COM_FALSE_0 = "0";
        public const string COM_PRT = "PRT_";
        public const string COM_PLC = "PLC_";
        public const string COM_GNR = "GNR_";
        public const string COM_BDD = "BDD_";
        public const string COM_BDD_VERSION = "VER_";
        public const string COM_VERSION_RELEASE = "RELEASE";
        public const string COM_VERSION_PACKAGE = "PACKAGE";
        public const string COM_VERSION_DATE = "DATE";
        public const string COM_VERSION_CARRIAGE = "CARRIAGE";
        public const string COM_VERSION_VIDEO = "VIDEO";
        public const string COM_VERSION_ALHAMBRA = "ALHAMBRA";
        public const string COM_VERSION_LEON = "LEON";
        public const string COM_VERSION_PEGASO = "PEGASO";
        public const string COM_PRO = "PRO_";
        public const string COM_GET_PRINTMODETABLE = "LPMTB";
        public const string COM_CLEAR_PRINTMODETABLE = "CPMTB";

        //Identificador de calidades fiery
        public const string FIERY_QUALITY_ID = "_FIE_";

        //Actions (Auditoria)
        public const string COM_ACTION_REGISTER = "RACCI";
        public const string COM_ACTION_GET_REPORT = "HACCI";

        public const string COM_ACTION_SCREEN = "SCREEN";
        public const string COM_ACTION_POPUP = "POPUP";
        public const string COM_ACTION_BUTTON = "BUTTON";
        public const string COM_ACTION_APP = "APP";

        //Images
        public const string IMG_BUTTON_PLAY = "/CretaBase;component/Images/media_play_green.png";
        public const string IMG_BUTTON_STOP = "/CretaBase;component/Images/media_stop_red.png";
        public const string IMG_OK = "/CretaBase;component/Images/check_ok.png";
        public const string IMG_BAD = "/CretaBase;component/Images/Close2.png";
        public const string IMG_LOCK_LOCKED = "/CretaBase;component/Images/lock.png";
        public const string IMG_LOCK_UNLOCKED = "/CretaBase;component/Images/lock_unlock.png";

        public const string IMG_TRANSPARENT = "/CretaBase;component/Images/Void.png";

        //Logo
        public const string LOGO_MAINSCREEN = "LogoWhite.png";
        //Previews
        public const string PREVIEW_BLANK = "NoPreview.jpg";
        public const string PREVIEW_TAS = "PreviewTAS.jpg";
        //Modes simulation 970x565 - Main simulation 700x660
        public const int MAX_PREVIEW_WIDTH = 970;
        public const int MAX_PREVIEW_HEIGHT = 660;
        public const int MAX_THUMBNAIL_WIDTH = 100;
        public const int MAX_THUMBNAIL_HEIGHT = 75;

        //Differents time values to Update the thread polling, in milliseconds
        public const int UPDATE_TIME_FAST = 500;
        public const int UPDATE_TIME_MED = 1000;
        public const int UPDATE_TIME_SLOW = 5000;
        public const int UPDATE_THREAD_TIME = 10;

        //Time to update special properties (bools from PLC with state and command) to avoid the change of state
        public const int MILLISECONDS_TO_ALLOW_UPDATE = 1000;

        // Console Info - Max length of queue
        public const int CONSOLE_MAX_INFO_QUEUE = 50;

        //PrintHeads Alignment and PTrims
        public const int PRINT_MAX_BARS = 8;
        public const int PRINT_MAX_PRINTHEADS_PER_BAR = 26;
        public const int PRINT_MAX_ROWS_PER_PRINTHEAD = 8;
        public const int PRINT_MAX_TRIMS_PER_ROW = 8;
        public const int PRINT_MAX_PMBS = 4;
        public const int PRINT_NUM_UNSELECTED = -1;
        public const string PRINT_PRINTHEAD_ENABLED = "enable";
        public const string PRINT_PRINTHEAD_DISABLED = "disable";
        public const float PRINT_PRINTHEAD_TRIM_OFFSET_DEFAULT = 0.1f;
        public const float PRINT_PRINTHEAD_TRIM_MIN_OFFSET = 0.01f;
        public const int PRINT_PRINTHEAD_MOVEMENT_OFFSET = 1;
        //PrintHeadTypes Rows and Trims
        public const int PRINT_PRINTHEAD_TYPE_TOSHIBA_ROWS = 2;
        public const int PRINT_PRINTHEAD_TYPE_TOSHIBA_TRIMS = 1;
        public const int PRINT_PRINTHEAD_TYPE_XAAR_ROWS = 2;
        public const int PRINT_PRINTHEAD_TYPE_XAAR_TRIMS = 8;
        public const int PRINT_PRINTHEAD_TYPE_DIMATIX_ROWS = 8;
        public const int PRINT_PRINTHEAD_TYPE_DIMATIX_TRIMS = 1;
        public const double PRINT_PRINTHEAD_VOLT_MIN = 14.0;
        public const double PRINT_PRINTHEAD_VOLT_MAX = 31.0;
        public const double PRINT_PRINTHEAD_VOLT_MIN_ROBOT_XAAR = 15.0;
        public const double PRINT_PRINTHEAD_VOLT_MAX_ROBOT_XAAR = 29.0;
        public const int PRINT_PRINTHEAD_NOZZLE_MIN = 0;
        public const int PRINT_PRINTHEAD_NOZZLE_MAX = 30;
        public const int PRINT_PRINTHEAD_NOZZLE_MAX_SEIKO = 120;
        //Printer Description - Slaves
        public const string PRINT_DESC_SLAVES_IP = "192.168.127.5";
        public const int PRINT_DESC_PORT = 2010;
        public const string PRINT_DESC_ADDRESS = "\\Rip Files\\";
        
        //Printer Desciption - Bars
        public const int PRINT_DESC_BAR_DIST_1 = 390;
        public const int PRINT_DESC_BAR_DIST_2 = 790;
        public const int PRINT_DESC_BAR_DIST_3 = 1190;
        public const int PRINT_DESC_BAR_DIST_4 = 1590;
        public const int PRINT_DESC_BAR_DIST_5 = 1990;
        public const int PRINT_DESC_BAR_DIST_6 = 2390;
        public const int PRINT_DESC_BAR_DIST_7 = 2790;
        public const int PRINT_DESC_BAR_DIST_8 = 3190;

        //Adjust Tile Position
        public const double EDIT_ADJUST_TILE_POS_OFFSET = 0.5;
        #endregion

        #region Types
        /// <summary>
        /// NavigationMgr Screens
        /// El número del enumerado debe aparecer ya que se utiliza para traducir las acciones de cambio de pantalla, se pueden añadir números entre los existentes y al final
        /// </summary>
        public enum EScreenNames
        {
            SCREEN_MAIN                     = 100,
            SCREEN_FILE_SLAVES              = 102,
            SCREEN_FILE_PRODUCTION_REPORT   = 104,
            SCREEN_FILE_CONSOLEINFO         = 106,
            SCREEN_FILE_BACKUP              = 108,
            SCREEN_FILE_CLEANSCREEN         = 110,
            SCREEN_FILE_ADJUST_TILE         = 112,
            SCREEN_EDIT_PRINTHEADS_ALIGNMENT = 114,
            SCREEN_EDIT_PRINTER_DESCRIPTION = 116,
            SCREEN_EDIT_QUALITIES           = 118,
            SCREEN_EDIT_FIERY_QUALITIES     = 119,
            SCREEN_ALARMS                   = 120,
            SCREEN_ALARMS_REPORT            = 122,
            SCREEN_ACTIONS_REPORT           = 124,
            SCREEN_BAR_MANAGEMENT           = 126,
            SCREEN_MODELS_MAIN              = 128,
            SCREEN_CLEANING_MAIN            = 130,
            SCREEN_MODELS_EDIT_DESIGNS      = 132,
            SCREEN_USERS_EDIT               = 134,
            SCREEN_TAS                      = 136,
            SCREEN_PRINTSERVER_STATUS       = 138,
            SCREEN_QR_MENU                  = 140,
            SCREEN_EFIDIAGNOSTICS           = 142,

            SCREEN_INVALID                  = -1,
        }

        //PopupMgr PopUps
        public enum EPopoupNames
        {
            POPUP_CANOPEN               = 150,
            POPUP_INPUT_LASER           = 152,
            POPUP_EDIT_ADJUST_MOTOR     = 154,
            POPUP_FILE_START            = 156,
            POPUP_LANGUAGES             = 158,
            POPUP_LINEMODE              = 160,
            POPUP_PRINTHEADS_T1         = 162,
            POPUP_PRINTHEADS_X1         = 164,
            POPUP_PRINTHEADS_D1         = 166,
            POPUP_PRINTHEADS_S          = 167,
            POPUP_MODEL_NEW             = 168,
            POPUP_MODEL_COPY            = 170,
            POPUP_MODEL_DELETE          = 172,
            POPUP_MODEL_INK_CONSUM      = 174,
            POPUP_QUALITY_NEW_FIERY     = 175,
            POPUP_QUALITY_NEW           = 176,
            POPUP_QUALITY_COPY          = 177,
            POPUP_QUALITY_DELETE        = 178,
            POPUP_QUALITY_SAVECHANGES   = 179,
            POPUP_HELP_SEND             = 180,
            POPUP_HELP_INFO             = 182,
            POPUP_MAIN_MODELS           = 184,
            POPUP_MAIN_CONTINUOUS       = 186,
            POPUP_HELP_ASSISTANCE       = 187,
            POPUP_USERS_LOGIN           = 188,
            POPUP_ACTIVATE_LICENSE      = 189,
            POPUP_TAS_ASSISTANT         = 190,      
            POPUP_TAS_MANUAL            = 192,
            POPUP_WAITING               = 194,
            POPUP_ADVANCED_INK          = 195,
            POPUP_WIZARD_HELP           = 196,

            POPUP_INVALID               = -1,
        }

        /// <summary>
        /// Machine state - Main screen - (Stop, Start, ByPass, Laboratory, Pause)
        /// </summary>
        public enum EMachineStateMode
        {
            MACHINE_STATE_MODE_STOP = 0,
            MACHINE_STATE_MODE_START,
            MACHINE_STATE_MODE_BYPASS,
            MACHINE_STATE_MODE_LAB,
            MACHINE_STATE_MODE_PAUSE,
            MACHINE_STATE_INVALID = -1,
        }
        //Movement PrintHeads, Bars...
        public enum EMovementDirection
        {
            MOVEMENT_DIRECTION_UP = 0,
            MOVEMENT_DIRECTION_DOWN,
            MOVEMENT_DIRECTION_LEFT,
            MOVEMENT_DIRECTION_RIGHT,
        }

        //PrintHeadTypes
        public enum enumTipoCabezal
        {
            //VSA (10/04/2015): El enumerado de tipo de cabezal está replicado en el GUI para evitar problemas entre los distintos kernel. 
            //       Si se desea añadir/modificar/eliminar algún tipo de cabezal hay que revisar el mismo enumerado del resto de proyectos de Kernel de C3.

            X = 0,  //XAAR GS12
            T = 1,  //TTEC y TTEC XL
            D = 2,  //Dimatix
            XGS40 = 3,  //XAAR GS40
            S = 4,  // Seiko
        }
        public enum enumTipoElectronica
        {
            GIS = 0,
            ROBOT = 1,
        }
       


        /// <summary>
        /// Tank Ink Levels
        /// </summary>
        public enum EInkLevel
        {
            INK_LEVEL_EMPTY = 0,
            INK_LEVEL_MIN,
            INK_LEVEL_MED,
            INK_LEVEL_FULL,
            INK_LEVEL_UNKNOWN,
        }

        /// <summary>
        /// Tank Ink Levels for Fluent
        /// </summary>
        public enum EInkLevelFluent
        {
            INK_LEVEL_EMPTY = 0,
            INK_LEVEL_MIN,
            INK_LEVEL_FULL,
            INK_LEVEL_UNKNOWN,
        }

        /// <summary>
        /// General two states enum
        /// </summary>
        public enum EBool
        {
            FALSE = 0,
            TRUE = 1,

            INVALID = -1,
        }

        public enum EMDescriptionBarDistToSensor
        {
            DIST_1 = 0,
            DIST_2,
            DIST_3,
            DIST_4,
            DIST_5,
            DIST_6,
            DIST_7,
            DIST_8,

            DIST_INVALID = -1,
        }

        public enum FluentStates
        {
            FLUENT_PARADO = 1,
            FLUENT_CARGA_DRENAJE = 2,
            FLUENT_ARRANQUE = 3,
            FLUENT_RECIRCULANDO = 4,
            FLUENT_REVERSE_MANUAL = 5,
            FLUENT_LIMPIEZA = 6,
            FLUENT_VACIADO_COMPLETO = 7,
            FLUENT_VACIADO_COLECTORES = 8,
        }

        #region Actions

        // (int)CretaTypes.EActionsGUI.ACTION_BAR_EXTRACTION

        public enum EActionsGUI
        { 
            //App
            ACTION_APP_START            =   2,
            ACTION_APP_EXIT             =   5,
            ACTION_APP_CONSENT          =   8,
            //Popup
            ACTION_POPUP_CLOSE          =   199,
            //Pantalla principal
            ACTION_MAIN_START_MODEL	    =   200,
            ACTION_MAIN_STOP_MODEL      =   201,
            ACTION_MAIN_TRIG            =   202,
            ACTION_MAIN_PARCIALES       =   203,
            ACTION_MAIN_MODO_MAQUINA    =   204,
            ACTION_MAIN_PRINT           =   205,
            ACTION_MAIN_OVER_TRAY       =   206,
            ACTION_MAIN_MAINTENANCE     =   207,
            ACTION_MAIN_CLEANING        =   208,
            ACTION_MAIN_RESET_ALRM      =   210,
            ACTION_MAIN_MUTE_ALRM	    =	211,
            ACTION_MAIN_LOAD_BAR_INFO	=	212,
            ACTION_MAIN_CONTINOUS_PRINT	=	215, //
            ACTION_MAIN_HEADS_ON_OFF	=	216,
            ACTION_MAIN_PRINT_ON_OFF_L1	=	217,
            ACTION_MAIN_PRINT_ON_OFF_L2	=	218,//
            ACTION_MAIN_HOMING_LASER	=	219,
            ACTION_MAIN_HOMING_BAR  	=	220,
            ACTION_MAIN_LOGOUT      	=	221,
            ACTION_MAIN_RESET_COM_SLVS  =   222,
            // amc 15/01/2015: Registro de la acción de cambio del margen de pintado
            ACTION_MAIN_JETTING_GAP     =   223,
            // amc 15/01/2015: Registro de la acción de cambio del grosor de pieza
            ACTION_MAIN_TILE_THICKNESS  =   224,
            // amc 15/01/2015: Registro de la acción de cambio de la velocidad de banda
            ACTION_MAIN_BELT_SPEED      =   225,
            //Barras
            ACTION_BAR_START_GNRL   	=	250,
            ACTION_BAR_EXTRACTION	    =	251,
            ACTION_BAR_VALIDATE_AUTO    =	252,
            ACTION_BAR_TRAY	            =	253,
            ACTION_BAR_CLEANING_CARRIER	=	254,
            ACTION_BAR_VENTURI	        =	255,
            ACTION_BAR_DRAIN	        =	256,
            ACTION_BAR_START_INKS	    =	257,
            ACTION_BAR_RESIST_BAR 	    =	258,
            ACTION_BAR_RESIST_TANK	    =	259,
            ACTION_BAR_STIRRER      	=	260,
            ACTION_BAR_SP_OFFSET        =   264,
            ACTION_BAR_SP_INK_TEMP      =   265,
            ACTION_BAR_SP_VACUUM        =   266,
            ACTION_BAR_SP_WEIR          =   267,
            ACTION_BAR_SP_OVERFLOW      =   268,
            ACTION_BAR_SP_POSITIVE      =   269,
            ACTION_BAR_SP_BAR_TEMP      =   270,
            ACTION_BAR_SP_TANK_TEMP     =   271,
            ACTION_BAR_TEST_MODE        =   272,
            ACTION_BAR_TEMP_CAMBIO_MENISCO = 273,
            ACTION_BAR_TEMP_DRAINAGE_TIME = 274,
            ACTION_BAR_TIME_REVERSE_FLOW = 275,
            ACTION_BAR_TIEMPO_VACIADO_COLECTOR = 276,
            ACTION_BAR_TIEMPO_SONDA_MINIMO = 277,
            ACTION_BAR_SP_VACUUM_COLD_MENISCUS = 278,
            ACTION_BAR_DC_COLD_MENISCUS = 279,
            //Slaves
            ACTION_SLAVES_RESTART     	=	280,
            ACTION_SLAVES_SHUTDOWN  	=	281,
            //Models
            ACTION_MODEL_NEW        	=	282,
            ACTION_MODEL_COPY	        =	283,
            ACTION_MODEL_DELETE	        =	284,
            ACTION_MODEL_RENDER_ALL	    =	285,
            ACTION_MODEL_RENDER_IMG	    =	286,
            ACTION_MODEL_ABORT_RIP	    =	287,
            ACTION_MODEL_STOP_RIP	    =	288,
            ACTION_MODEL_DELETE_RIP	    =	289,
            ACTION_MODEL_ADD_IMG	    =	290,
            ACTION_MODEL_DELETE_IMG	    =	291,
            ACTION_MODEL_PRINT_MODE	    =	292,
            ACTION_MODEL_TAS_NEW	    =	293,
            ACTION_MODEL_TAS_VALIDATE_SAMPLE	=	294,
            ACTION_MODEL_TAS_DELETE	    =	295,
            ACTION_MODEL_TAS_CONVERT_TO_JOB	=	296,
            ACTION_MODEL_TAS_EXTRACT_TIFF	=	297,
            ACTION_MODEL_TAS_NEW_MODIFICADA	=	298,
            ACTION_MODEL_TAS_DEL_MODIFICADA	=	299,
            ACTION_MODEL_DESIGN     	=	300,
            //Cleaning Menu 
            ACTION_CLEANING_SAVE        = 310,
            ACTION_CLEANING_FORCE_MANUAL = 311,
            ACTION_CLEANING_SPIT        = 312,
            //Calidades
            ACTION_QUALITY_SAVE	        =	315,
            ACTION_QUALITY_DELETE   	=	316,
            //Desc. máquina
            ACTION_PRINT_DESC_SAVE      =	320,
            //Alignment
            ACTION_ALIGNMENT_SAVE   	=	325,
            //Tile pos
            ACTION_TILEPOS_SAVE         =	330,
            //Production report
            ACTION_PRODUCTION_DELETE	=	335,
            //Backup
            ACTION_BACKUP_SAVE_FILE     =	340,
            ACTION_BACKUP_OPEN_FILE	    =	341,
            ACTION_BACKUP_RESTORE     	=	342,
            //License Activation
            ACTION_LICENSE_DAYS_LEFT    =   345,
            ACTION_LICENSE_ACTIVATION   =   346,
            //USB in/out
            ACTION_USB_IN_OUT           =   350,

            ACTION_BAR_ESTAB_SOLE       = 351,
            ACTION_BAR_DESAH_PID        = 352,
            ACTION_BAR_SP_GOTEO         = 353,
            ACTION_BAR_SP_REVERSE       = 354,
            ACTION_BAR_SP_VACIADO       = 355,
            //EFI diagnostics
            ACTION_DIAGNOSTICS_RESET_CB = 400,
            ACTION_DIAGNOSTICS_RESET_SWITCH = 402,
            ACTION_MAIN_STOP_MODEL_BYPASS_TRANSITION = 403
        }

        #endregion

        #region SecurityLevel
        public enum ESecurityLevel
        {
            LEVEL_ZERO  = 0,  //0 - No login
            LEVEL_LOW   = 1,  //1 - Usuario de fábrica
            LEVEL_MED   = 2,  //2 - Técnico de fábrica
            LEVEL_HIGH  = 4,  //4 - Técnico Creta   
            LEVEL_ADMIN = 5,  //5 - SuperAdmin   

        }
        #endregion

        #region languages
        public enum ELanguages
        {
            LANG_SPANISH_ES = 0,
            LANG_ENGLISH_EN,
            LANG_ITALIAN_IT,
            LANG_POLSKI_PL,
            LANG_CHINESE_ZH,
            LANG_CZECH_CS,
            LANG_FRENCH_FR,
            LANG_TURK_TR,
            LANG_RUSSIAN_RU,
            LANG_INDONESIAN_ID,
            LANG_KOREAN_KO,
            LANG_PORTUGUESE_PT,
            LANG_GERMAN_DE,
        }
        public const string STR_SPANISH = "Español";
        public const string STR_ENGLISH = "English";
        public const string STR_ITALIAN = "Italiano";
        public const string STR_POLSKI = "Polski";
        public const string STR_CHINESE = "中文";
        public const string STR_CZECK = "České";
        public const string STR_FRENCH = "Français";
        public const string STR_TURK = "Türk";
        public const string STR_RUSSIAN = "Rusă";
        public const string STR_INDONESIAN = "Indonesio";
        public const string STR_KOREAN = "Korean";
        public const string STR_PORTUGUESE = "Portuguese";
        public const string STR_GERMAN = "Deutsch";

        public const string STR_SPANISH_USER_DB = "Espanol";
        public const string STR_ENGLISH_USER_DB = "Ingles";
        public const string STR_ITALIAN_USER_DB = "Italiano";
        public const string STR_POLSKI_USER_DB = "Polaco";
        public const string STR_CHINESE_USER_DB = "Chino";
        public const string STR_CZECK_USER_DB = "Checo";
        public const string STR_FRENCH_USER_DB = "Frances";
        public const string STR_TURK_USER_DB = "Turco";
        public const string STR_RUSSIAN_USER_DB = "Ruso";
        public const string STR_INDONESIAN_USER_DB = "Indonesio";
        public const string STR_KOREAN_USER_DB = "Coreano";
        public const string STR_PORTUGUESE_USER_DB = "Portugues";
        public const string STR_GERMAN_USER_DB = "Aleman";

        public const string STR_SPANISH_LANG_ID = "es-ES";
        public const string STR_ENGLISH_LANG_ID = "en-US";
        public const string STR_ITALIAN_LANG_ID = "it-IT";
        public const string STR_POLSKI_LANG_ID = "pl-PL";
        public const string STR_CHINESE_LANG_ID = "zh-CN";
        public const string STR_CZECK_LANG_ID = "cs-CZ";
        public const string STR_FRENCH_LANG_ID = "fr-FR";
        public const string STR_TURK_LANG_ID = "tr-TR";
        public const string STR_RUSSIAN_LANG_ID = "ru-RU";
        public const string STR_INDONESIAN_LANG_ID = "id-ID";
        public const string STR_KOREAN_LANG_ID = "ko-KR";
        public const string STR_PORTUGUESE_LANG_ID = "pt-PT";
        public const string STR_GERMAN_LANG_ID = "de-DE";
        #endregion

        /// <summary>
        /// Different kind of log information, this enum will be showed at the begining of each log line
        /// </summary>
        public enum ELogTypes
        {
            LOG_INFO = 0,
            LOG_WARNING,       
            LOG_ERROR,
            LOG_EXCEPTION,
            LOG_UNHANDLED_EXCEPTION,
        }

        #region TAS enums
        public enum eTAS_AssistantSection
        {
            TAS_ASSISTANT_CONFIG_TONE_INTENSITY = 0,    //Step 1
            TAS_ASSISTANT_SAMPLES,                      //Step 2
            TAS_ASSISTANT_WAITING_PRINT,                    //waiting to print the samples
            TAS_ASSISTANT_SELECT_SAMPLE,                //Step 3 
            TAS_ASSISTANT_CONVERT_TO_PRODUCTION,            //Finish the assistant
            TAS_ASSISTANT_INVALID = -1
        }
        public enum eTAS_TypeOfInit
        {
            TAS_TONE = 0,
            TAS_INTENSITY,
            TAS_MANUAL,
            TAS_ASSISTANT_STARTED,
        }
        #endregion

        #endregion
    }
}
