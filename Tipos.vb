
Imports System.drawing
Imports Creta.CC3.infoBarra

Public Module Constantes
    Public Const SEPARADORVARIABLES = "¤"
    Public Const SEPARADORVARIABLEVALOR = "="
    Public Const SEPARADORCADENAVARIABLE = "_"
    Public Const SEPARADORALARMAS = "¦"
    Public Const IDPLC = "PLC"
    Public Const IDPRINT = "PRT"
    Public Const IDALR = "ALR"
    Public Const IDPRO = "PRO"
    Public Const IDBDD = "BDD"
    Public Const IDCLD = "CLD"
    Public Const IDGNR = "GNR"
    Public Const IDPC = "PC"
    Public Const IDBAR = "BAR"
    Public Const IDPMB = "PMB"
    Public Const IDHEAD = "HEAD"
    Public Const IDROW = "ROW"
    Public Const IDTRIM = "TRIM"
    Public Const CADENAERROR = "#"
    Public Const PLC_VERSION_ERROR = "-1.-1.-1.-1"
    Public Const FACTORCONVERSION = 25.4
    'Public Const FACTORCONVERSIONGIS = 25.38
    Public Const WAVEFORMFILE = "wave_"
    Public Const WAVEFORMCRETAPATH = "Waveforms\"
    Public Const WAVEFORMCRETAPATH_SLAVES = "C:\GIS\Rip Files\Waveforms\"
    Public Const CONST_RUTATIF = "RUTATIF"
    Public Const ACTION_APP = "APP"
    Public Const ACTION_PRINT = "PRINT"
    Public Const ACTION_RENDER = "RENDER"

    'Constantes parametros para los accesos a la base de datos y para interpretar las peticiones de la interfaz.
    'GENERAL
    Public Const CONST_LOGFILESLEVEL = "LOGFILESLEVEL"
    Public Const CONST_KEEPPCSON = "KEEPPCSON"
    Public Const CONST_SECURITYLEVEL = "SECURITYLEVEL"
    Public Const CONST_INKCONSUMPTION = "INKCONSUMPTION"
    'Public Const CONST_ISFLUENTINKSYSTEM = "ISFLUENTINKSYSTEM"
    Public Const CONST_SUCTIONMODE = "SUCTIONMODE"
    Public Const CONST_SUCTIONLENGTH = "SUCTIONLENGTH"
    Public Const CONST_CONSENTSIGNALDELAY = "CONSENTSIGNALDELAY"
    Public Const CONST_NUMSLAVES = "NUMSLAVES"
    Public Const CONST_MATRICULA = "MATRICULA"
    Public Const CONST_KERNELVERSION = "KERNELVERSION"
    Public Const CONST_BDVERSION = "BDVERSION"
    Public Const CONST_RIPVERSION = "RIPVERSION"
    Public Const CONST_ROBOTVERSION = "ROBOTVERSION"
    Public Const CONST_GUIPACKAGEVERSION = "GUIPACKAGEVERSION"
    Public Const CONST_KERNELPACKAGEVERSION = "KERNELPACKAGEVERSION"
    Public Const CONST_PACKAGEFIERYVERSION = "CONST_PACKAGEFIERYVERSION"
    Public Const CONST_PACKAGEVERSION = "PACKAGEVERSION"
    Public Const CONST_BDPACKAGEVERSION = "BDPACKAGEVERSION"
    Public Const CONST_ROBOTPACKAGEVERSION = "ROBOTPACKAGEVERSION"
    Public Const CONST_PLCVERSION = "PLCVERSION"
    Public Const CONST_CARRIAGEID = "CARRIAGEID"
    Public Const CONST_DITHERENABLED = "DITHERENABLED"
    Public Const CONST_TIPOELECTRONICA = "TIPOELECTRONICA"
    Public Const CONST_TIPOMAQUINA = "TIPOMAQUINA"
    Public Const CONST_NUMCARRIAGES = "NUMCARRIAGES"
    Public Const CONST_OCULTAR = "OCULTAR"

    'DIAGNOSTICS
    Public Const CONST_VIDEOPROGRAM = "VIDEOPROGRAM"
    Public Const CONST_VIDEOFPGAREV = "VIDEOFPGAREV"
    Public Const CONST_VIDEOLINKTEST = "VIDEOLINKTEST"
    Public Const CONST_CARRIAGEPROGRAM = "CARRIAGEPROGRAM"
    Public Const CONST_CARRIAGEFPGAREV = "CARRIAGEFPGAREV"
    Public Const CONST_CARRIAGELINKTEST = "CARRIAGELINKTEST"
    Public Const CONST_IDENTITYPROGRAM = "IDENTITYPROGRAM"
    Public Const CONST_IDENTITYFPGAREV = "IDENTITYFPGAREV"
    Public Const CONST_IDENTITYLINKTEST = "IDENTITYLINKTEST"
    Public Const CONST_VOLTAGECONTROLLER = "VOLTAGECONTROLLER"
    Public Const CONST_TEMPERATURE = "TEMPERATURE"
    Public Const CONST_DEFAULTVOLT = "DEFAULTVOLT"
    Public Const CONST_ENCODERCOUNT = "ENCODERCOUNT"
    Public Const CONST_ENCODERRES = "ENCODERRES"
    Public Const CONST_PDCOUNT = "PDCOUNT"
    Public Const CONST_BELTSPEED = "BELTSPEED"
    Public Const PROGRAMMER_PATH = "Programmer\"

    'PRINT
    Public Const CONST_TILESDIRECTION = "TILESDIRECTION"
    Public Const CONST_REDUCTOR = "REDUCTOR"
    Public Const CONST_PERIMETER = "PERIMETER"
    Public Const CONST_ENCODERRESOLUTION = "ENCODERRESOLUTION"
    Public Const CONST_ENCODERPULSES = "ENCODERPULSES"
    Public Const CONST_PRINTMODE = "PRINTMODE"
    Public Const CONST_MAXBELTSPEED = "MAXBELTSPEED"
    Public Const CONST_NUMBARS = "NUMBARS"
    Public Const CONST_L1TILES = "L1TILES"
    Public Const CONST_L2TILES = "L2TILES"
    Public Const CONST_L1STATUS = "L1STATUS"
    Public Const CONST_L2STATUS = "L2STATUS"
    Public Const CONST_L1GISRTP = "L1GISRTP"
    Public Const CONST_L2GISRTP = "L2GISRTP"
    Public Const CONST_PRINTSTATUS = "PRINTSTATUS"
    Public Const CONST_PRINTSTATUSROBOT = "PRINTSTATUSROBOT"
    Public Const CONST_SYSTEMSTATUSROBOT = "SYSTEMSTATUSROBOT"

    Public Const CONST_IMAGEBUFFER = "IMAGEBUFFER"
    Public Const CONST_PRINTSERVERCONNECTION = "PRINTSERVERCONNECTION"
    Public Const CONST_OFFSETBANDAX = "Offset_Banda_X"
    Public Const CONST_XTILEADJUSTMENTL1 = "XTILEADJUSTMENTL1"
    Public Const CONST_YTILEADJUSTMENTL1 = "YTILEADJUSTMENTL1"
    Public Const CONST_XTILEADJUSTMENTL2 = "XTILEADJUSTMENTL2"
    Public Const CONST_YTILEADJUSTMENTL2 = "YTILEADJUSTMENTL2"
    Public Const CONST_CENTEREDL1 = "CENTEREDL1"
    Public Const CONST_CENTEREDL2 = "CENTEREDL2"
    Public Const CONST_PRINTCHANNEL1 = "PRINTCHANNEL1"
    Public Const CONST_PRINTCHANNEL2 = "PRINTCHANNEL2"
    Public Const CONST_RTPL1 = "RTPL1"
    Public Const CONST_RTPL2 = "RTPL2"
    Public Const CONST_PMB_RTP = "RTPPMB"
    Public Const CONST_SPITPRODUCTION = "SPITPRODUCTION"
    Public Const CONST_RUTAWAVEFORMMASTER = "RUTAWAVEFORMMASTER"
    Public Const CONST_RUTAWAVEFORMSLAVES = "RUTAWAVEFORMSLAVES"
    '  Public Const CONST_ESTADOPRINT = "ESTADOPRINT"
    Public Const MAX_BELT_SPEED = 100
    Public Const CONST_NUMTOTALHEADS = "NUMTOTALHEADS"
    Public Const CONST_PSAVE = "PSAVE" 'Nuevo comando que guardará los cambios en el xml de Robot
    Public Const CONST_LANEGLOBALXPROCESSOFFSET = "LANEGLOBALXPROCESSOFFSET"  'Movimiento de la imagen en Y
    Public Const CONST_PDGLOBALOFFSET = "PDGLOBALOFFSET" 'Movimiento de la imagen en X
    'BARRA
    Public Const CONST_COLORPLANE = "COLORPLANE"
    Public Const CONST_NUMHEADS = "NUMHEADS"
    Public Const CONST_NUMPMBS = "NUMPMBS"
    Public Const CONST_XRESOLUTIONL1 = "XRESOLUTIONL1"
    Public Const CONST_XRESOLUTIONL2 = "XRESOLUTIONL2"
    Public Const CONST_YRESOLUTION = "YRESOLUTION"
    Public Const CONST_PRINTGODISTANCE = "PRINTGODISTANCE"
    Public Const CONST_BARTYPE = "TYPEBAR"
    Public Const CONST_HEADTYPE = "TYPEHEAD"
    Public Const CONST_UNIONPMB = "UNIONPMB"
    Public Const CONST_INKCOLOR = "INKCOLOR"
    Public Const CONST_WAVEFORMPATH = "WAVEFORMPATH"
    Public Const CONST_WAVEFORMID = "WAVEFORMID"
    Public Const CONST_WAVEFORMMAXID = "WAVEFORMMAXID"
    Public Const CONST_L1ISENABLED = "L1ISENABLED"
    Public Const CONST_L2ISENABLED = "L2ISENABLED"
    Public Const CONST_DROPLETVOLUME = "DROPLETVOLUME"
    Public Const CONST_L1PRINTENABLED = "L1PRINTENABLED"
    Public Const CONST_L2PRINTENABLED = "L2PRINTENABLED"
    Public Const CONST_SPITPRODUCTIONROBOT = "SPITPRODUCTIONROBOT"
    Public Const CONST_SPITENABLED = "SPITENABLED"
    Public Const CONST_SPITFREQUENCY = "SPITFREQUENCY"
    Public Const CONST_SPITCYCLE = "SPITCYCLE"
    Public Const CONST_SPITTIME = "SPITTIME"
    Public Const CONST_NUMHEADSL1 = "NUMHEADSL1"  'RCP, modificar si es necesario, se crean estas variables para una implementación inicial
    Public Const CONST_NUMHEADSL2 = "NUMHEADSL2"
    Public Const CONST_NUMHEADSL3 = "NUMHEADSL3"
    Public Const CONST_NUMHEADSL4 = "NUMHEADSL4"
    Public Const CONST_NUMPCL1 = "NUMPCL1"
    Public Const CONST_NUMPCL2 = "NUMPCL2"
    Public Const CONST_ROBOTHMIPORTL1 = "ROBOTHMIPORTL1"
    Public Const CONST_ROBOTHMIPORTL2 = "ROBOTHMIPORTL2"
    Public Const CONST_BARRATEST = "TESTBARRA"
    Public Const CONST_CABEZALTEST = "TESTCABEZAL"
    Public Const CONST_MODOSTARTPLOTTER = "MODOSTARTPLOTTER"
    Public Const CONST_TIPOMOVIMIENTO = "TIPOMOVIMIENTO"
    Public Const CONST_MODOPLOTTER = "MODOPLOTTER"
    Public Const CONST_MOVXMMINI1 = "XMMINI1"
    Public Const CONST_MOVXMMINI2 = "XMMINI2"
    Public Const CONST_MOVXMMINI3 = "XMMINI3"
    Public Const CONST_MOVXMMINI4 = "XMMINI4"
    Public Const CONST_MOVYMMINI1 = "YMMINI1"
    Public Const CONST_MOVYMMINI2 = "YMMINI2"
    Public Const CONST_MOVYMMINI3 = "YMMINI3"
    Public Const CONST_MOVYMMINI4 = "YMMINI4"
    Public Const CONST_MOVYMMOFFSET1 = "YMMOFFSET1"
    Public Const CONST_MOVYMMOFFSET2 = "YMMOFFSET2"
    Public Const CONST_MOVYMMOFFSET3 = "YMMOFFSET3"
    Public Const CONST_MOVYMMOFFSET4 = "YMMOFFSET4"
    Public Const CONST_POSPRIMERHEAD = "POSPRIMERHEAD"
    Public Const CONST_DESFASEX0 = "DESFASEX0"
    Public Const CONST_DESFASEY0 = "DESFASEY0"
    Public Const CONST_CONTSIMULACION = "CONTSIMULACION"

    'PMB
    Public Const CONST_NUMPC = "NUMPC"
    Public Const CONST_NUMPORT = "NUMPORT"
    Public Const CONST_RTP = "RTP"
    Public Const CONST_PRINTING = "PRINTING"
    Public Const CONST_TRAFFIC = "TRAFFIC"
    Public Const CONST_TILE = "TILE"
    Public Const CONST_BUFFER = "BUFFER"
    Public Const CONST_STATUS = "STATUS"

    'HEAD
    Public Const CONST_ISENABLED = "ISENABLED"
    Public Const CONST_MIRRORED = "MIRRORED"
    Public Const CONST_FIREORDER = "FIREORDER"
    Public Const CONST_XPOSITION = "XPOSITION"
    Public Const CONST_YPOSITION = "YPOSITION"
    Public Const CONST_DISABLEDNOZZLESINI = "DISABLEDNOZZLESINI"
    Public Const CONST_DISABLEDNOZZLESFIN = "DISABLEDNOZZLESFIN"
    Public Const CONST_HEADOVERLAP = "HEADOVERLAP"
    Public Const CONST_DISABLEDNOZZLES = "DISABLEDNOZZLES"
    Public Const CONST_DISABLEDNOZZLESINI_END = "DISABLEDNOZZLESINIEND"
    Public Const CONST_NUMROWS = "NUMROWS"
    Public Const CONST_NUMTRIMS = "NUMTRIMS"
    Public Const CONST_SERIALNUMBER = "SERIALNUMBER"
    Public Const CONST_INTERROWDSESV = "INTERROWDSESV"
    'Public Const CONST_HEADID = "HEADID" 'ReadOnly
    Public Const CONST_LOGICALHEADID = "LOGICALHEADID"
    Public Const CONST_CARRIAGEPORT = "CARRIAGEPORT"
    Public Const CONST_CARRIAGESUBPORT = "CARRIAGESUBPORT"
    Public Const CONST_ESTADOCABEZAL = "ESTADOCABEZAL"
    Public Const CONST_DITHERWIDTH = "DITHERWIDTH"
    Public Const CONST_TRIMS = "TRIMS"
    'amc: comprobación de voltajes
    Public Const CONST_TRIMSOK = "TRIMSOK"
    Public Const CONST_PORTSUBPORT = "PORTSUBPORT"
    Public Const CONST_COLOROFFSETID = "COLOROFFSETID"
    Public Const CONST_HEADFEEDBACK = "FEEDBACK"

    'PC
    Public Const CONST_IP = "IP"
    Public Const CONST_PORT = "PORT"
    Public Const CONST_RIPPATH = "RIPPATH"
    Public Const CONST_TIMEON = "TIMEON"
    Public Const CONST_FREESPACE = "FREESPACE"
    Public Const CONST_TOTALSPACE = "TOTALSPACE"
    Public Const CONST_CONNECTED = "CONNECTED"

    Public Const CONST_STATE = "STATE"
    Public Const CONST_PERC = "PERC"
    Public Const CONST_DATE_CHANGED = "PRINTDATE"
    Public Const CONST_QUEUE = "QUEUE"
    Public Const CONST_STATETAS = "TASSTATE"
    Public Const CONST_STATEMOD = "MODSTATE"
    Public Const CONST_INK_CONS = "INKCONSCHANGED"

    'LINE
    Public Const CONST_LINENUMBER = "LINENUMBER"
    Public Const CONST_XTILEADJUSTMENT = "XTILEADJUSTMENT"
    Public Const CONST_YTILEADJUSTMENT = "YTILEADJUSTMENT"
    Public Const CONST_PRINTCHANNEL = "PRINTCHANNEL"
    Public Const CONST_CENTERED = "CENTERED"
    Public Const CONST_TILES = "TILES"
    Public Const CONST_LINESTATUS = "LINESTATUS"
    Public Const CONST_TILESIZEXMM = "TILESIZEXMM"
    Public Const CONST_TILESIZEYMM = "TILESIZEYMM"
    Public Const CONST_TILESIZEXPX = "TILESIZEXPX"
    Public Const CONST_TILESIZEYPX = "TILESIZEYPX"
    Public Const CONST_MODEL = "MODEL"
    Public Const CONST_MODELMODE = "MODELMODE"
    Public Const CONST_RESOLUCIONDPIX = "RESOLUCIONDPIX"
    Public Const MAXIMOLINEAS = 2 'TODO RCP, DTP implementación para 2 líneas primero, luego 4
    Public Const CONST_VIDEOFEEDBACK = "FEEDBACK"


    'SPIT
    Public Const CONST_SPIT_PRODUCTION_VALUE_MIN = 0 'Duración mínima del spit en producción
    Public Const CONST_SPIT_PRODUCTION_VALUE_MAX = 10 'Duración máxima del spit en producción
    Public Const CONST_SPIT_FREQUENCY_VALUE_MIN = 100 'Frecuencia mínima de disparo del spit
    Public Const CONST_SPIT_FREQUENCY_VALUE_MAX = 1000 'Frecuencia máxima de disparo del spit
    Public Const CONST_SPIT_CYCLE_VALUE_MIN = 60 'Periodicidad mínima con la que se realiza el spit en minutos
    Public Const CONST_SPIT_CYCLE_VALUE_MAX = 65535 'Periodicidad máxima con la que se realiza el spit en minutos
    Public Const CONST_SPIT_TIME_VALUE_MIN = 0 'Duración mínima del spit en segundos
    Public Const CONST_SPIT_TIME_VALUE_MAX = 0.1 'Duración máxima del spit en segundos
End Module

Public Module Tipos


    Public Enum eEstadosBarra
        None 'Estado desconocido inicial
        Inicializando
        noOK 'Algun cabezal no esta OK
        OK 'Todos los cabezales estan OK
    End Enum
#Region "PRINT"
    Public Enum eStatusPrint As Integer
        NoPreparado = 0
        Stopp = 1
        Starting = 2
        Start = 3
        Stoping = 4
        Nothingg = 10
    End Enum 'Estado de la impresion
    Public Function toEnumStatusPrint(estadoPrint As String) As eStatusPrint

        Select Case estadoPrint
            Case eStatusPrint.NoPreparado.ToString
                Return eStatusPrint.NoPreparado
            Case eStatusPrint.Nothingg.ToString
                Return eStatusPrint.Nothingg
            Case eStatusPrint.Start.ToString
                Return eStatusPrint.Start
            Case eStatusPrint.Starting.ToString
                Return eStatusPrint.Starting
            Case eStatusPrint.Stoping.ToString
                Return eStatusPrint.Stoping
            Case eStatusPrint.Stopp.ToString
                Return eStatusPrint.Stopp
            Case Else
                Return eStatusPrint.NoPreparado
        End Select
    End Function



    Public Enum eEstadosCabezal
        None 'Estado desconocido
        Loading 'Cargando valores iniciales
        OK 'Las configuraciones son iguales
    End Enum
    Public Enum enumModoPintado
        UnaPieza
        DosPiezas
    End Enum 'Modo de pintado

    Public Function toEnumModoPintado(modo As String) As enumModoPintado
        Select Case modo
            Case enumModoPintado.DosPiezas.ToString
                Return enumModoPintado.DosPiezas
            Case enumModoPintado.UnaPieza.ToString
                Return enumModoPintado.UnaPieza
            Case Else
                Return enumModoPintado.UnaPieza
        End Select
    End Function

    Public Structure sProduccion
        Public Linea As Integer '0:Una sola linea 1:Linea1 2:Linea2
        Public FormatoX As Double
        Public FormatoY As Double
        Public Numero As Integer 'Numero de pieza
        Public Modelo As String 'Nombre del "Modelo" o "Modelo:Prueba"
        Public ModeloId As Integer 'Id del modelo
        Public Tipo As Integer 'Tipo de registro 0:Primer registro de produccion 1:Registro normal 2:Registro de prueba
        Public Fecha As Date
    End Structure
#End Region
    Public Enum eErrorCodes
        'Generales
        OK = 0
        ComandoDesconocido = -1
        ParametrosIncorrectos = -5

        'De la aplicacion
        Aplicacion = -10
        TimeOut = -11
        FalloAck = -12

        'Server
        ServerLoadConfiguration = -2
        ServerSeveConfig = -170
        ServerSaveConfigAs = -171
        ServerApplyMode = -186

        'Render
        RenderGetParam = -6
        RenderGeneralFailure = -100
        RenderBusy = -101
        RenderLoadConfig = -102
        RenderInitialise = -104
        RenderCreateLogDir = -106
        RenderLoadVPI = -110
        RenderFailed = -121
        RenderGetPreview = -127
        RenderOnDemand = -131
        RenderStartRenderingOnDemand = -132
        RenderEndRenderingOnDemand = -133
        RenderAddOnDemand = -134
        RenderSetParam = -140
        RenderApplyMode = -150

        'Print
        PrintBusy = -201
        PrintLoadConfig = -203
        PrintCreateLogDir = -204
        PrintInitialise = -206
        PrintTransportMecIni = -207
        PrintManagerBoardIni = -208
        PrintFailed = -210
        PrintTransportMecPrinting = -212
        PrintManagerBoardPrinting = -213
        PrintStartQueueMode = -214
        PrintEndQueueMode = -215
        PrintNotInitialised = -216
        PrintSecurityFailed = -217
        PrintAddItemQueue = -220
        PrintSpitFailed = -230
        PrintTurnHeatersOn = -260
        PrintTurnHeatersOff = -270
        PrintSetTargetHeadTemp = -280
    End Enum

    Public Enum eStatusCodes
        None = 0
        Ready = 1
        Initialising = 2
        ErrorInitPmb = 4
        ErrorLoadConfig = 5
        ErrorCreateLogger = 6
        ErrorInitFail = 7
        ErrorTransport = 9
        InitialiseComplete = 10
        LoadingConfig = 11
        Printing = 12
        StartingPrint = 13
        FinishingPrint = 14
        PrintFinished = 15
        InitialisingPrint = 16
        Moving = 17
        Spitting = 18
        ErrorSecurity = 19
    End Enum

    Public Enum eTrafficLightCodes
        Ready = 0
        Preparing = 1
        Printing = 2
        Errorr = 3
    End Enum

    Public Enum eEstadoRendering
        EnCola
        SinOriginal
        SinMemoria
        Renderizando
        RenderizadoOK
        RenderizadoError
        Cancelado
    End Enum

    Public Enum eModoSeguridad
        Minima
        Media
        Maxima
    End Enum
    Public Function toEnumModoSeguridad(modo As String) As eModoSeguridad
        Select Case modo.ToUpper
            Case eModoSeguridad.Maxima.ToString.ToUpper
                Return eModoSeguridad.Maxima
            Case eModoSeguridad.Media.ToString.ToUpper
                Return eModoSeguridad.Media
            Case eModoSeguridad.Minima.ToString.ToUpper
                Return eModoSeguridad.Minima
            Case Else
                Return eModoSeguridad.Media
        End Select
    End Function
    Public Structure sUserFile
        Public File As String
        Public Idioma As String
        Public User As String
        Public Nivel As Integer
        Public Valido As Boolean
    End Structure

    Public Class clsVariablesPLC
        Public costasAspiracion(7) As Double
        Public distanciasTotal(7) As Double
        Public tempsSetpointBarra(7) As Double
        Public tempsSetpointTanque(7) As Double
        Public tiemposMovCarroLimp(7) As Integer
        Public tiemposGoteo(7) As Integer
        Public offsetsUsuario(7) As Double
        Public offsetsAspiracion(7) As Double
        Public estadoRealBarras(7) As Integer
    End Class

    Public Enum eModeloInkbox
        Hijet
        Creta
    End Enum

#Region "BANDEJAS"
    Public Structure sPuntoVision
        Public Imagen As String
        Public X As Double
        Public Y As Double
    End Structure
    Public Structure sPuntoVision2
        Public Imagen As Integer
        Public X As Double
        Public Y As Double
    End Structure

    Public Enum eRespuestaBandeja
        OK = 0
        Bloqueo 'La imagen que se intenta quitar de la cola se está ripeando
        ImgNoEncontrada 'No se ha encontrado el Tiff (o la imagen)
        ErrorGrave 'Excepción o error desconocido al ripear
        CalidadNoEncontrada 'No se encuentra la calidad
        NoPreparado ' Cuando la función de ripear devuelve False por algún motivo en funciones de TAS y Modificadas
        EspacioInsuficiente 'No hay sufieciente espacio en el master
        NoRenderServer 'El servidor de ripeado NO está OK
        ModeloNoEncontrado 'No se ha encontrado el Id del modelo
    End Enum
    Public Enum eRespuestaBandejaAbrir
        OK = 0
        noOK
        ErrorGrave
        ErrorDB
        PassError
        FicheroCorrupto
        BmpYaExiste
    End Enum
    Public Enum eModoCambioImagenes
        Incremental
        Random
        Manual
    End Enum
    Public Enum enumModoImpresion
        Normal
        Incremental
        Random
        Cuadricula
        Vision
        Manual
        Prueba
    End Enum

    Public Function toEnumModoImpresion(modo As String) As enumModoImpresion
        Select Case modo
            Case enumModoImpresion.Cuadricula.ToString
                Return enumModoImpresion.Cuadricula
            Case enumModoImpresion.Incremental.ToString
                Return enumModoImpresion.Incremental
            Case enumModoImpresion.Manual.ToString
                Return enumModoImpresion.Manual
            Case enumModoImpresion.Normal.ToString
                Return enumModoImpresion.Normal
            Case enumModoImpresion.Prueba.ToString
                Return enumModoImpresion.Prueba
            Case enumModoImpresion.Random.ToString
                Return enumModoImpresion.Random
            Case enumModoImpresion.Vision.ToString
                Return enumModoImpresion.Vision
            Case Else
                Return enumModoImpresion.Normal
        End Select
    End Function

    'Se usa para marcar el estado de los tifs y de los bmps
    'Public Enum eEstadosImagenes
    '    Sin 'No existe el fichero
    '    EnCola 'El fichero esta en una pila para ser tradato
    '    Procesando 'El fichero se esta procesando (Ripeando)
    '    Con 'El fichero existe y es correcto
    'End Enum
    Public Enum eEstadosModelos
        ImprimiendoL1 = 0
        ImprimiendoL2 = 1
        OK = 2
        ModeloNoPreparado = 3
        Procesando = 4
        'CalculandoConsumoTinta = 5

        NewBornModel = -1
    End Enum
    Public Enum eEstadosImagenes
        SinProcesar 'No existe el fichero
        SinOriginal 'El fichero original no existe
        SinMemoria 'El master no tiene memoria suficiente para realizar el proceso
        EnCola 'El fichero esta en una pila para ser tradato
        Procesando 'El fichero se esta procesando (Ripeando)
        OK 'El fichero existe y es correcto
        SinProcesarError 'Por si despues de generar el procesado ha fallado
    End Enum

    Public Enum enumMaquinaMano
        Derecha
        Izquierda
    End Enum

    Public Function toEnumMaquinaMano(valor As String) As enumMaquinaMano
        Select Case valor
            Case enumMaquinaMano.Derecha.ToString
                Return enumMaquinaMano.Derecha
            Case enumMaquinaMano.Izquierda.ToString
                Return enumMaquinaMano.Izquierda
            Case Else
                Return enumMaquinaMano.Derecha
        End Select
    End Function

    Public Structure CalculoDpiXY
        'Contiene dos calculos para tener los dos ejes

        Private X As CalculoDpi
        Private Y As CalculoDpi

        Public Sub New(ByVal TiffPxX As Integer)
            X.TiffPx = TiffPxX
            Y.TiffPx = 0
        End Sub
        Public Sub New(ByVal TiffPxX As Integer, ByVal TiffPxY As Integer)
            X.TiffPx = TiffPxX
            Y.TiffPx = TiffPxY
        End Sub

        Public Property TiffPxX() As Integer
            Get
                Return X.TiffPx
            End Get
            Set(ByVal value As Integer)
                X.TiffPx = value
            End Set
        End Property
        Public Property TiffPxY() As Integer
            Get
                Return Y.TiffPx
            End Get
            Set(ByVal value As Integer)
                Y.TiffPx = value
            End Set
        End Property

        Public Property TiffResolutionX As Integer
            Get
                Return X.Resolucion
            End Get
            Set(value As Integer)
                X.Resolucion = value
            End Set
        End Property
        Public Property TiffResolutionY As Integer
            Get
                Return Y.Resolucion
            End Get
            Set(value As Integer)
                Y.Resolucion = value
            End Set
        End Property

        Public ReadOnly Property TiffMm() As PointD
            Get
                'Devuelve el tamaño en mm del tif original
                Return New PointD(X.TiffMm, Y.TiffMm)
            End Get
        End Property
        Public ReadOnly Property BmpPx(ByVal DpiX As Integer, ByVal DpiY As Integer) As Point
            Get
                'Tamaño en pixeles de la imagen ya rippeada
                Return New Point(X.BmpPx(DpiX), Y.BmpPx(DpiY))
            End Get
        End Property

        'Public Shared Function ToPixeles(ByVal CotaX As Double, ByVal CotaY As Double, ByVal DpiX As Integer, ByVal DpiY As Integer) As Point
        '    Return New Point(CalculoDpi.PixelToMm2(CotaX, DpiX), CalculoDpi.PixelToMm2(CotaY, DpiY))

        'End Function
        Public Shared Function PixelPitch(ByVal DpiX As Integer, ByVal DpiY As Integer) As PointD
            'PixelPichX para ripear e imprimir
            Return New PointD(CalculoDpi.PixelPitch(DpiX), CalculoDpi.PixelPitch(DpiY))
        End Function

    End Structure
    Public Structure CalculoDpi
        'Hace los calculos de DPI a MM segun GIS

        Private _resolucion As Integer
        Private _pxTiff As Integer
        Private _tamPixel As Double

        'Public Sub New(ByVal TiffPx As Integer)
        '    _pxTiff = TiffPx
        'End Sub

        Public Sub New(ByVal TiffPx As Integer, ByVal res As Integer)
            _pxTiff = TiffPx
            _resolucion = res
            '_tamPixel = FACTORCONVERSIONGIS / _resolucion
            _tamPixel = FACTORCONVERSION / _resolucion
        End Sub

        Public Property TiffPx() As Integer
            Get
                Return _pxTiff
            End Get
            Set(ByVal value As Integer)
                _pxTiff = value
            End Set
        End Property
        Public Property Resolucion As Integer
            Get
                Return _resolucion
            End Get
            Set(value As Integer)
                _resolucion = value
                '_tamPixel = FACTORCONVERSIONGIS / _resolucion
                _tamPixel = FACTORCONVERSION / _resolucion
            End Set
        End Property

        Public ReadOnly Property TiffMm() As Double
            '            Get
            '                'Devuelve el tamaño en mm del tif original
            '#If Cabezal = "T" Then
            '            Return _pxTiff / (300.0 / 25.4)
            '#Else
            '                Return _pxTiff / (360.0 / 25.4)
            '#End If
            '            End Get
            Get
                Return PixelToMm2(_pxTiff, _resolucion)
            End Get
        End Property
        Public ReadOnly Property BmpPx(ByVal Dpi As Integer) As Integer
            '            Get
            '                'Tamaño en pixeles de la imagen ya rippeada
            '#If Cabezal = "T" Then
            '            Return CInt(Fix(_pxTiff * 0.084667 / PixelPitch(Dpi)))
            '#Else
            '                Return CInt(Fix(_pxTiff * 0.0705 / PixelPitch(Dpi)))
            '#End If
            '            End Get
            Get
                Return CInt(Fix(_pxTiff * _tamPixel / PixelPitch(Dpi)))
            End Get
        End Property

        Public Shared Function PixelPitch(ByVal Dpi As Integer) As Double
            Return Math.Round(FACTORCONVERSION / Dpi, 4)
        End Function
        ''' <summary>
        ''' Transforma los mm a pixel según la resolución que se le pasa por parámetro.
        ''' </summary>
        ''' <param name="mm"></param>
        ''' <param name="resolution"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function MmToPixel2(ByVal mm As Double, ByVal resolution As Integer) As Integer
            Return CInt((mm * resolution) / FACTORCONVERSION)
        End Function
        ''' <summary>
        ''' Transforma los pixels a mm según la resolución que se la pasa por parámetro
        ''' </summary>
        ''' <param name="pixels"></param>
        ''' <param name="resolution"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function PixelToMm2(ByVal pixels As Integer, ByVal resolution As Integer) As Double
            'vsa: se ha dejado esta versión porque es la misma que utiliza photoshop y la que obtiene los valores más exactos
            '   p.e: 567px con una resolución de 360dpi obtendría con la antigua fórmula 40.004999999999995, en cambio con la nueva obtiene 40.005
            Return (pixels * (FACTORCONVERSION * 10.0)) / (resolution * 10.0)
            'Return (pixels * FACTORCONVERSION) / resolution
        End Function
    End Structure
    Public Structure PointD
        Public Sub New(ByVal X_ As Double, ByVal Y_ As Double)
            Me.X = X_
            Me.Y = Y_
        End Sub
        Public X As Double
        Public Y As Double
    End Structure
#End Region

#Region "BARRAS"

#End Region

#Region "USUARIOS"
    Public Class clsUser
        Public Id As String
        Public Pass As String
        Public Description As String
        Public Enabled As Boolean
        Public Level As Integer
        Public Language As String

        Public Sub New(userId As String, userPass As String, userDescripton As String, userEnabled As Boolean, userLevel As Integer, userLanguage As String)
            Id = userId
            Pass = userPass
            Description = userDescripton
            Enabled = userEnabled
            Level = userLevel
            Language = userLanguage
        End Sub
    End Class
#End Region

#Region "ALARMAS"
    Public Structure sAlarma
        Public id As Integer
        Public horaIni As Date
        Public horaAck As Date
    End Structure
#End Region

#Region "COMUNICACION INTERFAZ"
    Public Enum ErroresInterfaz
        OK 'Ok
        RegistroNoGuardado 'Registro no añadido correctamente

    End Enum

    Public Enum mensajesInterfaz
        modelStateUpdated 'Modificado el estado del modelo
        imageStateUpdated 'Modificado el estado de la imagen

    End Enum
#End Region

#Region "QR"
    Public Enum enumQRstatus
        Invalido
        Esperando
        GarrafaPosError
        GarrafaOK
    End Enum
    Public Enum enumQRlabel
        Ocupado
        FormatoError
        Usada
        NoDisponible
        OK_Garrafa
        OK_Usuario
    End Enum
#End Region

#Region "Vision"

    Public Enum eOrigenVision
        Ninguno
        CamaraCongnex
        EntradasPLC1
        Test
        Sync
    End Enum

#End Region

    ''' <summary>
    ''' Lista modos de pintado en el Plotter
    ''' </summary>
    Public Enum eModoStartPlotter
        Test
        Simulacion
        Normal
    End Enum

    ''' <summary>
    ''' Tipo de movimiento en scaning
    ''' </summary>
    Public Enum eTipoMovimiento
        m1Yx1
        m1Yx2
        m1Yx4
        m2Yx1
    End Enum

    ''' <summary>
    ''' Lista de modo del ploter
    ''' </summary>
    Public Enum eModoPlotter
        MultiPass
        SinglePass
    End Enum

End Module
