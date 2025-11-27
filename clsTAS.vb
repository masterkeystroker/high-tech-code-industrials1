Imports Creta.CC3.CC3Types

<Serializable()> Public Class clsModeloTas
    'Representa una prueba de un modelo

    'Public Muestras As New List(Of clsModeloTasMuestra) 'Lista con las muestras
    Public Muestras As New List(Of clsTAS) 'Lista con las muestras
    Public Imagenes As New List(Of clsModeloTasImagen) 'Lista con las imagenes

    Public Desviaciones As New List(Of Double) 'Lista de desviaciones (por ahora 5, en un futuro pueden ser más)

    '<Xml.Serialization.XmlIgnore()> Public Parent As clsBandeja
    '<Xml.Serialization.XmlIgnore()> Public RipInmediatamente As Boolean 'Indica que se debe ripear justo al terminar de crear el tif
    <Xml.Serialization.XmlIgnore()> Public LockEditando As Boolean 'Indica que se esta editando

    Public Sub New()

    End Sub
    'Public Sub New(ByVal Prueba As clsModeloTas, ByVal ValIniCan1 As Double, ByVal ValIniCan2 As Double, ByVal ValIniCan3 As Double, ByVal ValIniCan4 As Double, ByVal ValIniCan5 As Double, ByVal ValIniCan6 As Double, ByVal ValIniCan7 As Double, ByVal ValIniCan8 As Double)
    '    _Id = 0
    '    _Modelo = Prueba.Modelo
    '    _Nombre = Prueba.Nombre & "-TAS"
    '    _Tipo = Prueba.Tipo
    '    _Fase = eModeloTasFase.Config
    '    _Descripcion = Prueba.Descripcion
    '    _Original = Prueba.Original
    '    _Canales = Prueba.Canales
    '    _X = Prueba.X
    '    _Y = Prueba.Y
    '    _1Canal = Prueba.Combinaciones1Canal
    '    _2Canal = Prueba.Combinaciones2Canal
    '    _CombinarCanal1 = Prueba.CombinarCanal1
    '    _CombinarCanal2 = Prueba.CombinarCanal2
    '    _CombinarCanal3 = Prueba.CombinarCanal3
    '    _CombinarCanal4 = Prueba.CombinarCanal4
    '    _CombinarCanal5 = Prueba.CombinarCanal5
    '    _CombinarCanal6 = Prueba.CombinarCanal6
    '    _CombinarCanal7 = Prueba.CombinarCanal7
    '    _CombinarCanal8 = Prueba.CombinarCanal8
    '    _forX = Prueba.ForX
    '    _forY = Prueba.ForY
    '    _Xmm = Prueba.Xmm
    '    _Ymm = Prueba.Ymm
    '    _Filas = Prueba.Filas
    '    _Columnas = Prueba.Columnas
    '    _FechaCreacion = Now
    '    _FechaImpresion = _FechaCreacion
    '    _Calidad = Prueba.Calidad
    '    '_ResolucionX = Prueba.ResolucionX
    '    '_ResolucionY = Prueba.ResolucionY
    '    _Ruido = Prueba.Ruido
    '    '_ValorIniCanal1 = ValIniCan1
    '    '_ValorIniCanal2 = ValIniCan2
    '    '_ValorIniCanal3 = ValIniCan3
    '    '_ValorIniCanal4 = ValIniCan4
    '    '_ValorIniCanal5 = ValIniCan5
    '    '_ValorIniCanal6 = ValIniCan6
    '    '_ValorIniCanal7 = ValIniCan7
    '    '_ValorIniCanal8 = ValIniCan8
    'End Sub
    'Public Sub New(ByVal Tipo As eTipoPrueba, ByVal Modelo As clsModelo)
    '    _Id = 0
    '    _Modelo = Modelo.Nombre
    '    _Nombre = Modelo.Nombre & "-TAS"
    '    _Tipo = Tipo
    '    _Fase = eModeloTasFase.Config
    '    _Descripcion = ""
    '    _Original = ""
    '    _Canales = 0
    '    _X = 0
    '    _Y = 0
    '    _1Canal = False
    '    _2Canal = False
    '    _CombinarCanal1 = False
    '    _CombinarCanal2 = False
    '    _CombinarCanal3 = False
    '    _CombinarCanal4 = False
    '    _CombinarCanal5 = False
    '    _CombinarCanal6 = False
    '    _CombinarCanal7 = False
    '    _CombinarCanal8 = False
    '    _forX = Modelo.forX
    '    _forY = Modelo.forY
    '    _Xmm = Modelo.modOffsetX
    '    _Ymm = Modelo.modOffsetY
    '    _Filas = 1
    '    _Columnas = 1
    '    _FechaCreacion = Now
    '    _FechaImpresion = _FechaCreacion
    '    _Calidad = Modelo.calCalidad
    '    '_ResolucionX = Modelo.calResolucionX
    '    '_ResolucionY = Modelo.calResolucionY
    '    _Ruido = Modelo.calRuido
    '    '_ValorIniCanal1 = 0.0
    '    '_ValorIniCanal2 = 0.0
    '    '_ValorIniCanal3 = 0.0
    '    '_ValorIniCanal4 = 0.0
    '    '_ValorIniCanal5 = 0.0
    '    '_ValorIniCanal6 = 0.0
    '    '_ValorIniCanal7 = 0.0
    '    '_ValorIniCanal8 = 0.0
    'End Sub

    'Private _Modificado As Boolean

    'Private _Modelo As String
    'Private _Modificada As String 'Para tener acceso a la modificada una vez terminado todo 
    Private _nombre As String
    Private _descripcion As String
    Private _id As Integer
    Private _tipo As eTipoPrueba
    Private _1Canal As Boolean
    Private _2Canal As Boolean
    Private _CombinarCanal1 As Boolean
    Private _CombinarCanal2 As Boolean
    Private _CombinarCanal3 As Boolean
    Private _CombinarCanal4 As Boolean
    Private _CombinarCanal5 As Boolean
    Private _CombinarCanal6 As Boolean
    Private _CombinarCanal7 As Boolean
    Private _CombinarCanal8 As Boolean
    Private _tileSizeX As Double 'en mm
    Private _tileSizeY As Double 'en mm
    Private _sampleOffsetX As Double 'en mm
    Private _sampleOffsetY As Double 'en mm
    Private _rowsTile As Integer
    Private _columnsTile As Integer    
    Private _fase As eModeloTasFase
    Private _calCalidad As Integer
    'Private _Calidad As String    
    'Private _ResolucionX As Integer
    'Private _ResolucionY As Integer
    'Private _Ruido As Single
    'Private _ValorIniCanal1 As Double
    'Private _ValorIniCanal2 As Double
    'Private _ValorIniCanal3 As Double
    'Private _ValorIniCanal4 As Double
    'Private _ValorIniCanal5 As Double
    'Private _ValorIniCanal6 As Double
    'Private _ValorIniCanal7 As Double
    'Private _ValorIniCanal8 As Double
    Private _FechaCreacion As Date
    Private _FechaImpresion As Date

    Private _Original As String 'Nombre de la imagen original
    Private _idOriginal As Integer
    Private _Canales As Integer 'Guarda los canales que tiene la imagen original
    Private _X As Integer 'Tamaño imagen original
    Private _Y As Integer 'Tamaño imagen original

    'Public Property Modelo As String
    '    Get
    '        Return _Modelo
    '    End Get
    '    Set(ByVal value As String)
    '        'If _Modelo <> value Then _Modificado = True
    '        _Modelo = value
    '    End Set
    'End Property
    'Public Property Modificada As String
    '    Get
    '        Return _Modificada
    '    End Get
    '    Set(ByVal value As String)
    '        'If _Modificada <> value Then _Modificado = True
    '        _Modificada = value
    '    End Set
    'End Property
    Public Property nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            'If _Nombre <> value Then Modificado = True
            _nombre = value
        End Set
    End Property
    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            'If _Id <> value Then Modificado = True
            _id = value
        End Set
    End Property
    Public Property tipo() As eTipoPrueba
        Get
            Return _tipo
        End Get
        Set(ByVal value As eTipoPrueba)
            'If _Tipo <> value Then Modificado = True
            _tipo = value
        End Set
    End Property
    Public Property descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            'If _Descripcion <> value Then Modificado = True
            _descripcion = value
        End Set
    End Property
    Public Property Combinaciones1Canal() As Boolean
        Get
            Return _1Canal
        End Get
        Set(ByVal value As Boolean)
            'If _2Canal <> value Then _Modificado = True
            _1Canal = value
        End Set
    End Property 'Indica que se quieren usar combinaciones de 1 en 1
    Public Property Combinaciones2Canal() As Boolean
        Get
            Return _2Canal
        End Get
        Set(ByVal value As Boolean)
            'If _2Canal <> value Then _Modificado = True
            _2Canal = value
        End Set
    End Property 'Indica que se quieren usar combinaciones de 2 en 2
    Public Property CombinarCanal1() As Boolean
        Get
            Return _CombinarCanal1
        End Get
        Set(ByVal value As Boolean)
            'If _CombinarCanal1 <> value Then _Modificado = True
            _CombinarCanal1 = value
        End Set
    End Property 'Indica que se quieren usar el canal 1 para combinar
    Public Property CombinarCanal2() As Boolean
        Get
            Return _CombinarCanal2
        End Get
        Set(ByVal value As Boolean)
            'If _CombinarCanal2 <> value Then _Modificado = True
            _CombinarCanal2 = value
        End Set
    End Property
    Public Property CombinarCanal3() As Boolean
        Get
            Return _CombinarCanal3
        End Get
        Set(ByVal value As Boolean)
            'If _CombinarCanal3 <> value Then _Modificado = True
            _CombinarCanal3 = value
        End Set
    End Property
    Public Property CombinarCanal4() As Boolean
        Get
            Return _CombinarCanal4
        End Get
        Set(ByVal value As Boolean)
            'If _CombinarCanal4 <> value Then _Modificado = True
            _CombinarCanal4 = value
        End Set
    End Property
    Public Property CombinarCanal5() As Boolean
        Get
            Return _CombinarCanal5
        End Get
        Set(ByVal value As Boolean)
            'If _CombinarCanal5 <> value Then _Modificado = True
            _CombinarCanal5 = value
        End Set
    End Property
    Public Property CombinarCanal6() As Boolean
        Get
            Return _CombinarCanal6
        End Get
        Set(ByVal value As Boolean)
            'If _CombinarCanal6 <> value Then _Modificado = True
            _CombinarCanal6 = value
        End Set
    End Property
    Public Property CombinarCanal7() As Boolean
        Get
            Return _CombinarCanal7
        End Get
        Set(ByVal value As Boolean)
            'If _CombinarCanal7 <> value Then _Modificado = True
            _CombinarCanal7 = value
        End Set
    End Property
    Public Property CombinarCanal8() As Boolean
        Get
            Return _CombinarCanal8
        End Get
        Set(ByVal value As Boolean)
            'If _CombinarCanal8 <> value Then _Modificado = True
            _CombinarCanal8 = value
        End Set
    End Property
    Public Property tileSizeX() As Double
        Get
            Return _tileSizeX
        End Get
        Set(ByVal value As Double)
            'If _forX <> value Then _Modificado = True
            _tileSizeX = value
            For Each imagenTAS As clsModeloTasImagen In Imagenes
                imagenTAS.Xmm = _tileSizeX
            Next
        End Set
    End Property 'Formato para la prueba
    Public Property tileSizeY() As Double
        Get
            Return _tileSizeY
        End Get
        Set(ByVal value As Double)
            'If _forY <> value Then _Modificado = True
            _tileSizeY = value
            For Each imagenTAS As clsModeloTasImagen In Imagenes
                imagenTAS.Ymm = _tileSizeY
            Next
        End Set
    End Property
    Public Property sampleOffsetX() As Double
        Get
            Return _sampleOffsetX
        End Get
        Set(ByVal value As Double)
            'If _Xmm <> value Then Modificado = True
            _sampleOffsetX = value
        End Set
    End Property 'Cotas donde empiezan a coger la imagen. El trozo de imagen se indica en el formato del modelo
    Public Property sampleOffsetY() As Double
        Get
            Return _sampleOffsetY
        End Get
        Set(ByVal value As Double)
            'If _Ymm <> value Then Modificado = True
            _sampleOffsetY = value
        End Set
    End Property
    Public Property rowsTile() As Integer
        Get
            Return _rowsTile
        End Get
        Set(ByVal value As Integer)
            'If _Filas <> value Then _Modificado = True
            _rowsTile = value
        End Set
    End Property 'Filas para cuando es nPuebasPieza
    Public Property columnsTile() As Integer
        Get
            Return _columnsTile
        End Get
        Set(ByVal value As Integer)
            'If _Columnas <> value Then _Modificado = True
            _columnsTile = value
        End Set
    End Property
    Public Property fase As eModeloTasFase
        Get
            Return _fase
        End Get
        Set(ByVal value As eModeloTasFase)
            'If _Fase <> value Then Modificado = True
            _fase = value
        End Set
    End Property
    Public Property calCalidad As Integer
        Get
            Return _calCalidad
        End Get
        Set(ByVal value As Integer)
            'If _Calidad <> value Then Modificado = True
            _calCalidad = value
            For Each imagenTas As clsModeloTasImagen In Imagenes
                imagenTas.calCalidad = _calCalidad
            Next
        End Set
    End Property

    Public Property FechaCreacion As Date
        Get
            Return _FechaCreacion
        End Get
        Set(ByVal value As Date)
            ' If _FechaCreacion <> value Then _Modificado = True
            _FechaCreacion = value
        End Set
    End Property
    Public Property FechaImpresion As Date
        Get
            Return _FechaImpresion
        End Get
        Set(ByVal value As Date)
            'If _FechaImpresion <> value Then _Modificado = True
            _FechaImpresion = value
        End Set
    End Property

    'Public Property ResolucionX As Integer
    '    Get
    '        Return _ResolucionX
    '    End Get
    '    Set(ByVal value As Integer)
    '        If _ResolucionX <> value Then Modificado = True
    '        _ResolucionX = value
    '    End Set
    'End Property
    'Public Property ResolucionY As Integer
    '    Get
    '        Return _ResolucionY
    '    End Get
    '    Set(ByVal value As Integer)
    '        If _ResolucionY <> value Then Modificado = True
    '        _ResolucionY = value
    '    End Set
    'End Property
    'Public Property Ruido As Single
    '    Get
    '        Return _Ruido
    '    End Get
    '    Set(ByVal value As Single)
    '        'If _Ruido <> value Then Modificado = True
    '        _Ruido = value
    '    End Set
    'End Property
    'Public Property ValorIniCanal1() As Double
    '    Get
    '        Return _ValorIniCanal1
    '    End Get
    '    Set(ByVal value As Double)
    '        If _ValorIniCanal1 <> value Then Modificado = True
    '        _ValorIniCanal1 = value
    '    End Set
    'End Property
    'Public Property ValorIniCanal2() As Double
    '    Get
    '        Return _ValorIniCanal2
    '    End Get
    '    Set(ByVal value As Double)
    '        If _ValorIniCanal2 <> value Then Modificado = True
    '        _ValorIniCanal2 = value
    '    End Set
    'End Property
    'Public Property ValorIniCanal3() As Double
    '    Get
    '        Return _ValorIniCanal3
    '    End Get
    '    Set(ByVal value As Double)
    '        If _ValorIniCanal3 <> value Then Modificado = True
    '        _ValorIniCanal3 = value
    '    End Set
    'End Property
    'Public Property ValorIniCanal4() As Double
    '    Get
    '        Return _ValorIniCanal4
    '    End Get
    '    Set(ByVal value As Double)
    '        If _ValorIniCanal4 <> value Then Modificado = True
    '        _ValorIniCanal4 = value
    '    End Set
    'End Property
    'Public Property ValorIniCanal5() As Double
    '    Get
    '        Return _ValorIniCanal5
    '    End Get
    '    Set(ByVal value As Double)
    '        If _ValorIniCanal5 <> value Then Modificado = True
    '        _ValorIniCanal5 = value
    '    End Set
    'End Property
    'Public Property ValorIniCanal6() As Double
    '    Get
    '        Return _ValorIniCanal6
    '    End Get
    '    Set(ByVal value As Double)
    '        If _ValorIniCanal6 <> value Then Modificado = True
    '        _ValorIniCanal6 = value
    '    End Set
    'End Property
    'Public Property ValorIniCanal7() As Double
    '    Get
    '        Return _ValorIniCanal7
    '    End Get
    '    Set(ByVal value As Double)
    '        If _ValorIniCanal7 <> value Then Modificado = True
    '        _ValorIniCanal7 = value
    '    End Set
    'End Property
    'Public Property ValorIniCanal8() As Double
    '    Get
    '        Return _ValorIniCanal8
    '    End Get
    '    Set(ByVal value As Double)
    '        If _ValorIniCanal8 <> value Then Modificado = True
    '        _ValorIniCanal8 = value
    '    End Set
    'End Property

    Public ReadOnly Property BmpOk() As Boolean
        Get
            'Si no hay ninguno habilitado no se da como ok
            For Each Item As clsModeloTasImagen In Imagenes
                If Item.BmpOk = False Then Return False
            Next
            Return True
        End Get
    End Property 'Indica que las imagenes de prueba que estan habilitadas estan en los esclavos
    Public ReadOnly Property TiffEstado() As eEstadosImagenes
        Get
            Dim Estado As eEstadosImagenes = eEstadosImagenes.SinProcesar
            For Each Item As clsModeloTasImagen In Imagenes
                If Item.TiffEstado = eEstadosImagenes.Procesando Then
                    Return eEstadosImagenes.Procesando
                ElseIf Item.TiffEstado = eEstadosImagenes.EnCola Then
                    Return eEstadosImagenes.EnCola
                ElseIf Item.TiffEstado = eEstadosImagenes.OK Then
                    Estado = eEstadosImagenes.OK
                End If
            Next
            Return Estado
        End Get
    End Property

    Private _estado As eEstadosModelos = eEstadosModelos.ModeloNoPreparado
    Public Property estado() As eEstadosModelos
        Get
            'Dim est As eEstadosModelos = _estado ' eEstadosModelos.ModeloNoPreparado
            'Dim contOK As Integer = 0
            'Dim contSinprocesar As Integer = 0
            'For Each imagenTAS As clsModeloTasImagen In Imagenes
            '    If imagenTAS.BmpEstado = eEstadosImagenes.SinProcesarError Or imagenTAS.BmpEstado = eEstadosImagenes.SinMemoria Or
            '        imagenTAS.BmpEstado = eEstadosImagenes.SinOriginal Then
            '        est = eEstadosModelos.ModeloNoPreparado
            '        Exit For
            '    ElseIf imagenTAS.BmpEstado = eEstadosImagenes.EnCola Or imagenTAS.BmpEstado = eEstadosImagenes.Procesando Then
            '        est = eEstadosModelos.Procesando
            '    ElseIf imagenTAS.BmpEstado = eEstadosImagenes.OK Then
            '        contOK += 1
            '    ElseIf imagenTAS.BmpEstado = eEstadosImagenes.SinProcesar Then
            '        contSinprocesar += 1
            '    End If
            'Next

            'If contOK = Imagenes.Count Then
            '    est = eEstadosModelos.OK
            'ElseIf contSinprocesar = Imagenes.Count Then
            '    est = eEstadosModelos.ModeloNoPreparado
            'End If

            'If _estado <> est Then
            '    _estado = est
            '    RaiseEvent EstadoTASCambiado(_id, _estado)
            'End If
            Return _estado
        End Get
        Set(value As eEstadosModelos)
            If _estado <> value Then
                _estado = value
                RaiseEvent EstadoTASCambiado(_id, _estado)
            End If
        End Set
    End Property
    '<Xml.Serialization.XmlIgnore()> Public Property Modificado() As Boolean
    '    Get
    '        If _Modificado Then
    '            Return True
    '        Else
    '            For Each img As clsModeloTasImagen In Imagenes
    '                If img.Modificado Then Return True
    '            Next
    '            For Each mue As clsModeloTasMuestra In Muestras
    '                If mue.Modificado Then Return True
    '            Next
    '        End If
    '        Return False
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _Modificado = value
    '        For Each img As clsModeloTasImagen In Imagenes
    '            img.Modificado = value
    '        Next
    '        For Each mue As clsModeloTasMuestra In Muestras
    '            mue.Modificado = value
    '        Next
    '    End Set
    'End Property

    Public Property Original() As String
        Get
            Return _Original
        End Get
        Set(ByVal value As String)
            If _Original <> value Then
                LeerDimensiones(True)
                'Modificado = True
            End If
            _Original = value
        End Set
    End Property 'Imagen original para hacer la prueba
    Public Property idOriginal As Integer
        Get
            Return _idOriginal
        End Get
        Set(value As Integer)
            _idOriginal = value
        End Set
    End Property
    Public Property Canales As Integer
        Get
            LeerDimensiones(False)
            Return _Canales
        End Get
        Set(ByVal value As Integer)
            'If _Canales <> value Then Modificado = True
            _Canales = value
            For Each imagenTAS As clsModeloTasImagen In Imagenes
                imagenTAS.Canales = _Canales
            Next
        End Set
    End Property
    Public Property X() As Integer
        Get
            LeerDimensiones(False)
            Return _X
        End Get
        Set(ByVal value As Integer)
            'If _X <> value Then Modificado = True
            _X = value
        End Set
    End Property 'Tamaño en pixeles de la imagen original
    Public Property Y() As Integer
        Get
            LeerDimensiones(False)
            Return _Y
        End Get
        Set(ByVal value As Integer)
            'If _Y <> value Then Modificado = True
            _Y = value
        End Set
    End Property
    'Lee la informacion de la imagen solo si faltan datos
    Private Sub LeerDimensiones(ByVal Forzar As Boolean)
        Try
            'Obtiene las dimensiones del TIFF seleccionado
            If (_X <= 0.0 Or _Y <= 0.0 Or _Canales <= 0 Or Forzar) AndAlso My.Computer.FileSystem.FileExists(_Original) Then

                'Lee la informacion de la imagen
                Dim info As Creta.Tiff.TiffInfo = Creta.Tiff.cTiff.Info(_Original)
                _Canales = info.CanalesPixel

                'Calcula los mm de la imagen a partir de los pixeles
                _X = info.Width
                _Y = info.Heigh
                'Modificado = True
            End If
        Catch ex As Exception
            'DB.Suceso(100, ex.Message, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString & ":" & System.Threading.Thread.CurrentThread.Name & vbCrLf & ex.StackTrace)

            System.Threading.Thread.Sleep(0)
        End Try
    End Sub

#Region "Evento"
    Public Sub setImagenTASEvents()
        For Each imagenTAS As clsModeloTasImagen In Imagenes
            setImagenTASEvents(imagenTAS)
        Next
    End Sub
    Public Sub setImagenTASEvents(ByRef img As clsModeloTasImagen)
        AddHandler img.Excepcion, AddressOf imagenTASExcepcion
        'AddHandler img.PorcentajeRipCambiado, AddressOf imagenTASPorcentajeRIPCambiado
        'AddHandler img.EstadoImagenCambiado, AddressOf imagenTASEstadoCambiado
    End Sub
    Public Sub removeImagenTASEvents(ByRef img As clsModeloTasImagen)
        RemoveHandler img.Excepcion, AddressOf imagenTASExcepcion
        'RemoveHandler img.PorcentajeRipCambiado, AddressOf imagenTASPorcentajeRIPCambiado
        'RemoveHandler img.EstadoImagenCambiado, AddressOf imagenTASEstadoCambiado
    End Sub

    Public Event Excepcion(ByVal ex As Exception)
    'Public Event PorcentajeRipCambiado(ByVal imageId As Integer, ByVal percent As Integer)
    Public Event EstadoTASCambiado(ByVal TASId As Integer, ByVal imageState As eEstadosModelos)

    Private Sub imagenTASExcepcion(ByVal ex As Exception)
        RaiseEvent Excepcion(ex)
    End Sub
    'Private Sub imagenTASPorcentajeRIPCambiado(ByVal imageId As Integer, ByVal percent As Integer)
    '    RaiseEvent PorcentajeRipCambiado(imageId, percent)
    'End Sub
    Private Sub imagenTASEstadoCambiado(ByVal imageId As Integer, ByVal imageState As eEstadosModelos)
        Dim est As eEstadosModelos = _estado
        Dim contOK As Integer = 0
        Dim contSinprocesar As Integer = 0
        For Each imagenTAS As clsModeloTasImagen In Imagenes
            If imagenTAS.BmpEstado = eEstadosImagenes.SinProcesarError Or imagenTAS.BmpEstado = eEstadosImagenes.SinMemoria Or
                imagenTAS.BmpEstado = eEstadosImagenes.SinOriginal Then
                est = eEstadosModelos.ModeloNoPreparado
                Exit For
            ElseIf imagenTAS.BmpEstado = eEstadosImagenes.EnCola Or imagenTAS.BmpEstado = eEstadosImagenes.Procesando Then
                est = eEstadosModelos.Procesando
            ElseIf imagenTAS.BmpEstado = eEstadosImagenes.OK Then
                contOK += 1
            ElseIf imagenTAS.BmpEstado = eEstadosImagenes.SinProcesar Then
                contSinprocesar += 1
            End If
        Next

        If contOK = Imagenes.Count Then
            est = eEstadosModelos.OK
        ElseIf contSinprocesar = Imagenes.Count Then
            est = eEstadosModelos.ModeloNoPreparado
        End If

        If est <> _estado Then
            estado = est
        End If

    End Sub
#End Region

End Class

'<Serializable()> Public Class clsModeloTasMuestra
'    'Representa un Item de una prueba de una imagen de un modelo

'    Public TAS As clsTAS 'Tas de este item

'    <Xml.Serialization.XmlIgnore()> Public Parent As clsModeloTas
'    <Xml.Serialization.XmlIgnore()> Public Modificado As Boolean 'Indica que se ha modificado esta clase

'    Public Sub New()

'    End Sub
'    Public Sub New(ByVal Padre As clsModeloTas, ByVal Etiqueta As String)
'        Parent = Padre
'        TAS = New clsTAS
'        TAS.Descripcion = Etiqueta
'    End Sub
'    Public Sub New(ByVal Padre As clsModeloTas, ByVal Etiqueta As String, ByVal ValCan1 As Double, ByVal ValCan2 As Double, ByVal ValCan3 As Double, ByVal ValCan4 As Double, ByVal ValCan5 As Double, ByVal ValCan6 As Double, ByVal ValCan7 As Double, ByVal ValCan8 As Double)
'        Parent = Padre
'        TAS = New clsTAS
'        TAS.Descripcion = Etiqueta
'        TAS.C1 = ValCan1
'        TAS.C2 = ValCan2
'        TAS.C3 = ValCan3
'        TAS.C4 = ValCan4
'        TAS.C5 = ValCan5
'        TAS.C6 = ValCan6
'        TAS.C7 = ValCan7
'        TAS.C8 = ValCan8
'    End Sub

'    'Etiqueta del Tiff
'    Public Property Label() As String
'        Get
'            Return TAS.Descripcion
'        End Get
'        Set(ByVal value As String)
'            If TAS.Descripcion <> value Then Modificado = True
'            TAS.Descripcion = value
'        End Set
'    End Property

'End Class
<Serializable()> Public Class clsModeloTasImagen
    'Representa un Item de una prueba de una imagen de un modelo

    'Lista de tamaños de los bundle files por canal
    Public BndlSizeXpx As New List(Of Integer)
    Public BndlSizeYpx As New List(Of Integer)

    Private _id As Integer
    Public Property id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    'Numero del BMP
    Private _NumeroBmp As String
    Public Property NumeroBmp() As String
        Get
            Return _NumeroBmp
        End Get
        Set(ByVal value As String)
            _NumeroBmp = value
        End Set
    End Property

    'Numero del TIFF
    Private _NumeroTiff As Integer
    Public Property NumeroTiff() As Integer
        Get
            Return _NumeroTiff
        End Get
        Set(ByVal value As Integer)
            _NumeroTiff = value
        End Set
    End Property

    Private _TiffEstado As eEstadosImagenes
    Public Property TiffEstado() As eEstadosImagenes
        Get
            Return _TiffEstado
        End Get
        Set(ByVal value As eEstadosImagenes)
            _TiffEstado = value
        End Set
    End Property

    Private _BmpEstado As eEstadosImagenes = eEstadosImagenes.SinProcesar
    Public Property BmpEstado() As eEstadosImagenes
        Get
            Return _BmpEstado
        End Get
        Set(ByVal value As eEstadosImagenes)
            If _BmpEstado <> value Then
                _BmpEstado = value
                'RaiseEvent EstadoImagenCambiado(_id, _BmpEstado)
            End If
        End Set
    End Property

    'Indica si la imagen modificada esta en su sitio en los escalvos
    Private _BmpOk As Boolean = False
    Public Property BmpOk() As Boolean
        Get
            Return _BmpOk
        End Get
        Set(ByVal value As Boolean)
            _BmpOk = value
        End Set
    End Property

    Private _calCalidad As Integer
    Public Property calCalidad As Integer
        Get
            Return _calCalidad
        End Get
        Set(value As Integer)
            _calCalidad = value
        End Set
    End Property

    ''Calculos segun el tif y la resolucion a la que se debe imprimmir
    'Private _tiffMm As PointD
    'Public Property TiffMm() As PointD
    '    Get
    '        'getTiffInfo()
    '        Return _tiffMm
    '    End Get
    '    Set(value As PointD)
    '        _tiffMm = value
    '    End Set
    'End Property 'Tamaño de la imagen en milimetros
    Private _Xmm As Double
    Public Property Xmm As Double
        Get
            Return _Xmm
        End Get
        Set(value As Double)
            _Xmm = value
        End Set
    End Property
    Private _Ymm As Double
    Public Property Ymm As Double
        Get
            Return _Ymm
        End Get
        Set(value As Double)
            _Ymm = value
        End Set
    End Property

    Private _tiffDpiX As Integer
    Public Property TiffDpiX() As Integer 'Resolucion original
        Get
            Return _tiffDpiX
        End Get
        Set(ByVal value As Integer)
            _tiffDpiX = value
        End Set
    End Property
    Private _tiffDpiY As Integer
    Public Property TiffDpiY() As Integer 'Resolucion original
        Get
            Return _tiffDpiY
        End Get
        Set(ByVal value As Integer)
            _tiffDpiY = value
        End Set
    End Property

    Private _Canales As Integer 'Canales de la imagen original
    Public Property Canales As Integer
        Get
            Return _Canales
        End Get
        Set(value As Integer)
            _Canales = value
        End Set
    End Property

    Private _porcentajeRip As Integer = 0
    Public Property porcentajeRIP As Integer
        Get
            Return _porcentajeRip
        End Get
        Set(value As Integer)
            If _BmpEstado = eEstadosImagenes.Procesando Then
                'End If
                'If _porcentajeRip <> value Then
                _porcentajeRip = value
                'RaiseEvent PorcentajeRipCambiado(_id, value)
            End If
        End Set
    End Property

#Region "Eventos"
    Public Event Excepcion(ByVal ex As Exception)
    'Public Event PorcentajeRipCambiado(ByVal imageId As Integer, ByVal percent As Integer)
    'Public Event EstadoImagenCambiado(ByVal imageId As Integer, ByVal imageState As eEstadosModelos)
#End Region
End Class

<Serializable()> Public Class clsTAS
    'Representa los datos modificados en el TAS

    Public Sub New()
        _Descripcion = ""
        _C1 = 1.0
        _C2 = 1.0
        _C3 = 1.0
        _C4 = 1.0
        _C5 = 1.0
        _C6 = 1.0
        _C7 = 1.0
        _C8 = 1.0
    End Sub

    Private _Descripcion As String
    Private _C1 As Double
    Private _C2 As Double
    Private _C3 As Double
    Private _C4 As Double
    Private _C5 As Double
    Private _C6 As Double
    Private _C7 As Double
    Private _C8 As Double

    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
        End Set
    End Property
    Public Property C1() As Double
        Get
            Return _C1
        End Get
        Set(ByVal value As Double)
            _C1 = value
        End Set
    End Property
    Public Property C2() As Double
        Get
            Return _C2
        End Get
        Set(ByVal value As Double)
            _C2 = value
        End Set
    End Property
    Public Property C3() As Double
        Get
            Return _C3
        End Get
        Set(ByVal value As Double)
            _C3 = value
        End Set
    End Property
    Public Property C4() As Double
        Get
            Return _C4
        End Get
        Set(ByVal value As Double)
            _C4 = value
        End Set
    End Property
    Public Property C5() As Double
        Get
            Return _C5
        End Get
        Set(ByVal value As Double)
            _C5 = value
        End Set
    End Property
    Public Property C6() As Double
        Get
            Return _C6
        End Get
        Set(ByVal value As Double)
            _C6 = value
        End Set
    End Property
    Public Property C7() As Double
        Get
            Return _C7
        End Get
        Set(ByVal value As Double)
            _C7 = value
        End Set
    End Property
    Public Property C8() As Double
        Get
            Return _C8
        End Get
        Set(ByVal value As Double)
            _C8 = value
        End Set
    End Property

    Public Function Clone() As clsTAS
        Dim T As New clsTAS
        T.Descripcion = _Descripcion
        T.C1 = _C1
        T.C2 = _C2
        T.C3 = _C3
        T.C4 = _C4
        T.C5 = _C5
        T.C6 = _C6
        T.C7 = _C7
        T.C8 = _C8
        Return T
    End Function

End Class

Public Enum eModeloTasFase
    Config 'Indica que esta en la primera fase de configuracion
    Ripeando 'Se esta ripeando ahora
    Ripeado 'Se ha terminado de ripear
    Impreso 'Indica que se impreso
    Seleccionado 'Indica que ya se ha seleccionado una etiqueta como buena
End Enum
Public Enum eTipoPrueba
    Tono
    Intensidad
End Enum
