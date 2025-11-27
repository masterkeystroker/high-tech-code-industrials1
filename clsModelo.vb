Imports Creta.CC3.CC3Types
Imports System.Drawing

<Serializable()> Public Class clsModelo

    'Internamente todo trabaja en milimetros

#Region "General"

    '<Xml.Serialization.XmlIgnore()> Public Parent As clsBandeja

    Public Sub New()
        _Id = -1
        _Nombre = ""
        _Descripcion = ""
        _modModo = enumModoImpresion.Normal
        _modOffsetX = 0.0
        _modOffsetY = 0.0
        _modIncX = 0.0
        _modIncY = 0.0
        _modPiezasImagen = 1
        _modCambioImagen = 0
        _modFilas = 1
        _modColumnas = 1
        _modModo360H = False
        _modModo360V = False
        _calCalidad = -1
        _calRuido = 0.25!
        _Favorito = True
        _forX = 100
        _forY = 100
        _LockImprimiendo1 = False
        _LockImprimiendo2 = False
        _FechaCreacion = Now
        _FechaImpresion = Date.MinValue
        _debeRecalcularConsumo = False
        _isInkConsumptionActive = False
    End Sub
    Public Sub Dispose()
        eliminaEventosImagenes()
        For Each _img As clsModeloImagen In Imagenes
            _img = Nothing
        Next
        Imagenes = Nothing

        For Each _mt As clsModeloTas In TASlist
            _mt = Nothing
        Next
        TASlist = Nothing

        For Each _mod As clsModeloModificada In Modificadas
            _mod = Nothing
        Next
        Modificadas = Nothing

    End Sub

    Public Function Clone() As clsModelo
        Try
            'Serializa a XML
            Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(clsModelo))
            Dim file As New System.IO.StringWriter
            writer.Serialize(file, Me)
            Dim Objeto As String = file.ToString.Replace("\", "\\").Replace("'", "\'")
            file.Dispose()

            'Deserializa en XML
            Dim fs As New System.IO.StringReader(Objeto)
            Dim reader As New System.Xml.XmlTextReader(fs)
            Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(clsModelo))
            If serializer.CanDeserialize(reader) Then
                'Obtiene el objeto
                Dim Item As clsModelo = CType(serializer.Deserialize(reader), clsModelo)

                'Propiedades por defecto del clon
                Item.Id = 0
                Item.LockImprimiendo1 = False
                Item.LockImprimiendo2 = False
                Item.LockGuardando = False
                Item.LockEditando = False

                fs.Close()
                Return Item
            Else
                Return Nothing
            End If
        Catch ex As Exception
            'DB.Suceso(100, ex.Message, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString & ":" & System.Threading.Thread.CurrentThread.Name & vbCrLf & ex.StackTrace)
            RaiseEvent Excepcion(ex)
            System.Threading.Thread.Sleep(0)
            Return Nothing
        End Try
    End Function 'Clona este modelo FALTA PROBAR

#End Region
#Region "Vision"

    Public Vision As New List(Of clsModeloVision) 'Lista de puntos de vision

    'Tipo de origen de vision
    'Private _TipoVision As eTipoVision = eTipoVision.CamaraCognex
    'Public Property TipoVision As eTipoVision
    '    Get
    '        Return _TipoVision
    '    End Get
    '    Set(value As eTipoVision)
    '        If _TipoVision = value Then Modificado = True
    '        _TipoVision = value
    '    End Set
    'End Property

#End Region
#Region "Propiedades"

    Private _Id As Integer 'Se usa en el programa para saber cual es el Id en la base de datos
    Private _Nombre As String
    Private _Descripcion As String
    Private _Favorito As Boolean
    Private _forX As Double
    Private _forY As Double
    Private _modModo As enumModoImpresion
    Private _modOffsetX As Double
    Private _modOffsetY As Double
    Private _modIncX As Double
    Private _modIncY As Double
    Private _modPiezasImagen As UInteger
    Private _modCambioImagen As eModoCambioImagenes
    Private _modFilas As UInteger
    Private _modColumnas As UInteger
    Private _modModo360H As Boolean
    Private _modModo360V As Boolean
    'Private _calCalidad As String 'Nombre de la calidad/receta seleccionada
    Private _calCalidad As Integer 'Id de la calidad.
    'Private _calResolucionX As Integer 'Resolucion seleccionada (Calculada a partir de la velocidad)
    'Private _calResolucionY As Integer 'Resolución nativa del cabezal
    Private _calRuido As Single 'Nivel de ruido deseado
    Private _debeRecalcularConsumo As Boolean
    Private _consumoTintaCalculado As Boolean
    Private _isInkConsumptionActive As Boolean

    Private _FechaCreacion As Date
    Private _FechaImpresion As Date
    Private _Estado As eEstadosModelos = eEstadosModelos.NewBornModel
    'Public estadoAnterior As eEstadosModelos = eEstadosModelos.ModeloNoPreparado

    Public ReadOnly Property Rippeada() As Boolean
        Get
            'Si hay por lo menos una imagen ripeada y habilitada se da el modelo como preparado para imprimir
            For Each Img As clsModeloImagen In Imagenes
                If Img.BmpEstado = eEstadosImagenes.OK And Img.Habilitado Then Return True
            Next
            Return False
        End Get
    End Property 'Devuelve si hay alguna imagen completamente rippeada
    Public ReadOnly Property Rippeando() As Boolean
        Get
            'Lo primero es comprobar si hay alguna imagen ya rippeada para deshabilitar las funciones de calidad
            For Each Img As clsModeloImagen In Imagenes
                'If (Img.BmpEstado = eEstadosImagenes.OK And Img.Habilitado) Or Img.BmpEstado = eEstadosImagenes.Procesando Or Img.BmpEstado = eEstadosImagenes.EnCola Then Return True
                If Img.BmpEstado = eEstadosImagenes.Procesando Or Img.BmpEstado = eEstadosImagenes.EnCola Then Return True

                'For Each ImgM As clsModeloImagenModificada In Img.imagenesModificadas
                '    If (ImgM.BmpEstado = eEstadosImagenes.OK) Or ImgM.BmpEstado = eEstadosImagenes.Procesando Or ImgM.BmpEstado = eEstadosImagenes.EnCola Then Return True
                'Next
            Next
            'For Each modeloModificada As clsModeloModificada In Modificadas
            '    For Each ImgM As clsModeloImagenModificada In modeloModificada.imagenesModificadas
            '        If (ImgM.BmpEstado = eEstadosImagenes.OK) Or ImgM.BmpEstado = eEstadosImagenes.Procesando Or ImgM.BmpEstado = eEstadosImagenes.EnCola Then Return True
            '    Next
            'Next
            'For Each modeloTAS As clsModeloTas In TASlist
            '    For Each imagenTAS As clsModeloTasImagen In modeloTAS.Imagenes
            '        If (imagenTAS.BmpEstado = eEstadosImagenes.OK) Or imagenTAS.BmpEstado = eEstadosImagenes.Procesando Or imagenTAS.BmpEstado = eEstadosImagenes.EnCola Then Return True
            '    Next
            'Next
            Return False
        End Get
    End Property 'Devuelve si hay alguna imagen rippeada o ripeando
    Public Property DebeRecalcularConsumo() As Boolean
        Set(value As Boolean)
            _debeRecalcularConsumo = value
        End Set
        Get
            Return _debeRecalcularConsumo
        End Get
    End Property 'Indica si ha cambiado algún valor del modo de pintado para recalcular el consumo de tintas
    Public Property ConsumoTintaCalculado() As Boolean 'Indica si el modelo tiene un consumo de tintas guardado
        Set(value As Boolean)
            If _consumoTintaCalculado <> value Then
                _consumoTintaCalculado = value
                RaiseEvent ConsumoTintasCambiado(_Id, value)
            End If
        End Set
        Get
            Return _consumoTintaCalculado
        End Get
    End Property

    'Indica si las imagenes o sus modificadas estan en los esclavos correctamente
    'Private Function BmpOk() As Boolean

    Public ReadOnly Property BmpOk() As Boolean
        Get
            'Si no hay ninguna habilitada no esta correcto
            Dim Hab As Boolean = False

            'Repasa las imagenes
            For Each Imagen As clsModeloImagen In Imagenes
                'La original no esta correcta
                If Imagen.Habilitado = True Then
                    'Hay por lo menos una habilitada
                    Hab = True

                    'Si la original no esta correcta devuelve error
                    'TODO OJO NUEVA COMPROBACIÓN PARA EL ESTADO DEL MODELO, PODRIA BORRAR LOS BMPS: Imagen.BmpEstado <> eEstadosImagenes.OK
                    If (Imagen.BmpOk = False) Or (Imagen.BmpEstado <> eEstadosImagenes.OK) Then Return False
                End If
            Next

            'Si no hay ninguna habilitada o no hay imagenes devuelve error
            If Not Hab Then Return False

            'Devuelve el estado verdadero si no se ha encontrado ningun error
            Return True
        End Get

    End Property
    'End Function

    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            Try
                If _Nombre <> value Then
                    'Nombre del modelo cambiado
                    Dim NombreAnterior As String = _Nombre
                    'Modificado = True
                    _Nombre = value

                    'TODO Descomentar cuando se implemente el TAS
                    'Comprueba si hay algun TAS para cambiar el nombre del modelo
                    'If Parent IsNot Nothing Then
                    '    For Each tas As clsModeloTas In Parent.TAS
                    '        If tas.Modelo.Trim.ToLower = NombreAnterior.Trim.ToLower Then tas.Modelo = _Nombre
                    '    Next
                    'End If
                End If
            Catch ex As Exception
                RaiseEvent Excepcion(ex)
                'DB.Suceso(100, ex.Message, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString & ":" & System.Threading.Thread.CurrentThread.Name & vbCrLf & ex.StackTrace)
                System.Threading.Thread.Sleep(0)
            End Try
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            'If _Descripcion <> value Then Modificado = True
            _Descripcion = value
        End Set
    End Property
    Public Property Favorito() As Boolean
        Get
            Return _Favorito
        End Get
        Set(ByVal value As Boolean)
            'If _Favorito <> value Then Modificado = True
            _Favorito = value
        End Set
    End Property

    Public Property forX() As Double
        Get
            Return _forX
        End Get
        Set(ByVal value As Double)
            _forX = value
        End Set
    End Property
    Public Property forY() As Double
        Get
            Return _forY
        End Get
        Set(ByVal value As Double)
            _forY = value
        End Set
    End Property

    Public Property modModo() As enumModoImpresion
        Get
            Return _modModo
        End Get
        Set(ByVal value As enumModoImpresion)
            _modModo = value
        End Set
    End Property
    Public Property modOffsetX() As Double
        Get
            Return _modOffsetX
        End Get
        Set(ByVal value As Double)
            _modOffsetX = value
        End Set
    End Property
    Public Property modOffsetY() As Double
        Get
            Return _modOffsetY
        End Get
        Set(ByVal value As Double)
            _modOffsetY = value
        End Set
    End Property
    Public Property modIncX() As Double
        Get
            Return _modIncX
        End Get
        Set(ByVal value As Double)
            _modIncX = value
        End Set
    End Property
    Public Property modIncY() As Double
        Get
            Return _modIncY
        End Get
        Set(ByVal value As Double)
            _modIncY = value
        End Set
    End Property
    Public Property modPiezasImagen() As UInteger
        Get
            Return _modPiezasImagen
        End Get
        Set(ByVal value As UInteger)
            _modPiezasImagen = value
        End Set
    End Property
    Public Property modCambioImagen() As eModoCambioImagenes
        Get
            Return _modCambioImagen
        End Get
        Set(ByVal value As eModoCambioImagenes)
            If _modCambioImagen <> value Then
                If _Estado <> eEstadosModelos.NewBornModel Then
                    'Si cambio el modo de cambio de imagen a Manual desde otro modo o desde manual a otro modo, tengo que recalcular el consumo
                    If _modCambioImagen = eModoCambioImagenes.Manual Or value = eModoCambioImagenes.Manual Then
                        EncolaCalculoConsumoTintas()
                    End If
                End If
                _modCambioImagen = value
            End If
        End Set
    End Property
    Public Property modFilas() As UInteger
        Get
            Return _modFilas
        End Get
        Set(ByVal value As UInteger)
            _modFilas = value
        End Set
    End Property
    Public Property modColumnas() As UInteger
        Get
            Return _modColumnas
        End Get
        Set(ByVal value As UInteger)
            _modColumnas = value
        End Set
    End Property
    Public Property modModo360H() As Boolean
        Get
            Return _modModo360H
        End Get
        Set(ByVal value As Boolean)
            _modModo360H = value
        End Set
    End Property
    Public Property modModo360V() As Boolean
        Get
            Return _modModo360V
        End Get
        Set(ByVal value As Boolean)
            _modModo360V = value
        End Set
    End Property

    Public Property calCalidad() As Integer
        Get
            Return _calCalidad
        End Get
        Set(ByVal value As Integer)
            'If _calCalidad <> value Then Modificado = True
            _calCalidad = value
            For Each imagen As clsModeloImagen In Imagenes
                imagen.calCalidad = _calCalidad
            Next
            For Each modeloModificada As clsModeloModificada In Modificadas
                modeloModificada.calCalidad = _calCalidad
            Next
            For Each modeloTAS As clsModeloTas In TASlist
                modeloTAS.calCalidad = _calCalidad
            Next
        End Set
    End Property 'Nombre de la calidad con que ha sido rippeada
    'Public Property calResolucionX() As Integer
    '    Get
    '        Return _calResolucionX
    '    End Get
    '    Set(ByVal value As Integer)
    '        'If _calResolucionX <> value Then Modificado = True
    '        _calResolucionX = value
    '    End Set
    'End Property 'Resolucion
    'Public Property calResolucionY() As Integer
    '    Get
    '        Return _calResolucionY
    '    End Get
    '    Set(ByVal value As Integer)
    '        'If _calResolucionY <> value Then Modificado = True
    '        _calResolucionY = value
    '    End Set
    'End Property 'Resolucion
    Public Property calRuido() As Single
        Get
            Return _calRuido
        End Get
        Set(ByVal value As Single)
            'If _calRuido <> value Then Modificado = True
            _calRuido = value
        End Set
    End Property 'Nivel de ruido

    Public Property FechaCreacion() As Date
        Get
            Return _FechaCreacion
        End Get
        Set(ByVal value As Date)
            'If _FechaCreacion <> value Then Modificado = True
            _FechaCreacion = value
        End Set
    End Property
    Public Property FechaImpresion() As Date
        Get
            Return _FechaImpresion
        End Get
        Set(ByVal value As Date)
            If value > Date.MinValue Then
                _FechaImpresion = value
                RaiseEvent FechaImpresionCambiada(_Id, value)
            End If
        End Set
    End Property
    Public Property Estado As eEstadosModelos
        Get
            'Dim _eA As eEstadosModelos = _Estado
            'If _LockImprimiendo1 Then
            '    _Estado = eEstadosModelos.ImprimiendoL1
            'ElseIf _LockImprimiendo2 Then
            '    _Estado = eEstadosModelos.ImprimiendoL2
            'Else
            '    If BmpOk() Then
            '        _Estado = eEstadosModelos.OK
            '    Else
            '        If Rippeando Then
            '            _Estado = eEstadosModelos.Procesando
            '        Else
            '            _Estado = eEstadosModelos.ModeloNoPreparado
            '        End If
            '    End If
            'End If

            'If _Estado <> _eA Then
            '    RaiseEvent EstadoModeloCambiado(_Id, _Estado)
            'End If
            Return _Estado
        End Get
        Set(value As eEstadosModelos)
            If value <> _Estado Then
                Dim prevState As Tipos.eEstadosModelos = _Estado
                _Estado = value
                RaiseEvent EstadoModeloCambiado(_Id, value)
                'Aquí si cambia el estado a OK añado el cálculo de tintas
                If _Estado = eEstadosModelos.OK And prevState <> eEstadosModelos.NewBornModel Then
                    Debug.WriteLine("------------------------------------------------------")
                    Debug.WriteLine("Consumo de tintas por cambio de estado del modelo a OK")
                    EncolaCalculoConsumoTintas()
                End If
            End If
        End Set
    End Property

    <Xml.Serialization.XmlIgnore()> Public Property IsInkConsumptionActive As Boolean
        Get
            Return _isInkConsumptionActive
        End Get
        Set(value As Boolean)
            _isInkConsumptionActive = value
        End Set
    End Property

    Private _lastId As Integer = -1
    Public Property LastId As Integer
        Get
            Return _lastId
        End Get
        Set(value As Integer)
            _lastId = value
        End Set
    End Property

    Public Function newImageId() As Integer
        'Si el LasId es -1 y ya hay imágenes busco el id mayor de las imágenes que habían y lo aumento en 1
        Dim imageId As Integer = 0
        If LastId = -1 Then
            If Imagenes.Count > 0 Then
                'Si ya había imágenes busco el id mayor para aumentarlo en 1
                Dim imageInstance As clsModeloImagen
                For Each imageInstance In Imagenes
                    If imageInstance.id >= imageId Then
                        imageId = imageInstance.id + 1
                    End If
                Next
            End If
        Else
            imageId = LastId + 1
        End If
        LastId = imageId
        Return imageId
        'Dim imageId As Integer = 0
        'Dim idMayor As Integer = 0
        'Dim imageInstance As clsModeloImagen
        'For Each imageInstance In Imagenes
        '    If imageId <= imageInstance.id Then
        '        imageId = imageInstance.id + 1
        '    End If
        'Next
        'Return imageId
    End Function

#End Region
#Region "Funciones publicas"


    Public Function addImage(ByRef img As clsModeloImagen) As Boolean
        Try
            If Imagenes IsNot Nothing Then
                'TODO: Anyado la misma imagen a todas las modificadas
                For Each Modific As clsModeloModificada In Modificadas
                    If Imagenes.Count = Modific.imagenesModificadas.Count Then
                        Dim imgModificada As New clsModeloImagenModificada

                        imgModificada = New clsModeloImagenModificada(Modific)
                        imgModificada.BmpOk = False
                        imgModificada.NumeroBmp = "0"
                        imgModificada.NumeroTiff = 0
                        imgModificada.TiffEstado = eEstadosImagenes.SinOriginal
                        imgModificada.TiffMm = img.TiffMm
                        imgModificada.Canales = img.Canales
                        Modific.imagenesModificadas.Add(imgModificada)

                    End If
                Next
                'Pongo la imagen en la lista
                setImageEvents(img)
                Imagenes.Add(img)

                'Cuando añado una imagen al modelo, esta iamgen no está OK y sí está habilitada. Por lo que el consumo de tintas debe marcarse como no hecho
                ConsumoTintaCalculado = False

                Return True
            End If
            Return False
        Catch ex As Exception
            RaiseEvent Excepcion(ex)
            Return False
        End Try
    End Function
    Public Function deleteImage(ByVal imageId As Integer) As Boolean
        Try
            If Imagenes IsNot Nothing Then
                For i = 0 To Imagenes.Count - 1
                    Dim img As clsModeloImagen = Imagenes(i)
                    If img.id = imageId Then
                        'Cuando borro una imagen tengo que mirar si estaba habilitada o no. Si estaba habilitada tengo que recalcular consumo, sino no
                        If img.Habilitado = True Then
                            EncolaCalculoConsumoTintas()
                        End If

                        'Borro la misma imagen de sus modificadas
                        For Each Modific As clsModeloModificada In Modificadas
                            If Imagenes.Count = Modific.imagenesModificadas.Count Then
                                Modific.imagenesModificadas.RemoveAt(i)
                            End If
                        Next
                        'Borro la imagen
                        RemoveHandler img.PorcentajeRipCambiado, AddressOf IMG_PorcentajeRipCambiado
                        Imagenes.Remove(img)
                        img = Nothing

                        Return True
                    End If
                Next

            End If
            Return False
        Catch ex As Exception
            RaiseEvent Excepcion(ex)
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Esta función lanza un hilo que calcula el consumo de tintas para el propio modelo, lanzará un hilo 
    '''     y comprobará que no hubiese uno anterior lanzado, de lo contrario lo matará.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub EncolaCalculoConsumoTintas()

        'Marco el modelo como que no tiene el consumo calculado
        ConsumoTintaCalculado = False
        'Si está el tick de consumo de tintas activado encolará el modelo para recalcularlo
        If IsInkConsumptionActive Then
            Debug.WriteLine(DateTime.Now)
            Debug.WriteLine("-Llamada la función de consumo de tintas-")
            
            'Lanzo un evento para que se encole el modelo y se calcule el consumo de tintas, el hilo que desencola y procesa el modelo escibirá en el modelo
            RaiseEvent EncolarCalculoTintas(Id)
        End If
        DebeRecalcularConsumo = False
    End Sub
#End Region
#Region "Bloqueos"

    Private _LockImprimiendo1 As Boolean 'Bloqueo de este objeto para la linea 1
    Private _LockImprimiendo2 As Boolean 'Bloqueo de este objeto para la linea 2
    Private _LockGuardando As Boolean 'Bloqueo de este objeto para guardarlo en un fichero
    Private _LockEditando As Boolean 'Bloqueo de este objeto para guardarlo en un fichero

    Public Property LockImprimiendo1() As Boolean
        Get
            Return _LockImprimiendo1
        End Get
        Set(ByVal value As Boolean)
            _LockImprimiendo1 = value
        End Set
    End Property
    Public Property LockImprimiendo2() As Boolean
        Get
            Return _LockImprimiendo2
        End Get
        Set(ByVal value As Boolean)
            _LockImprimiendo2 = value
        End Set
    End Property
    Public Property LockGuardando() As Boolean
        Get
            Return _LockGuardando
        End Get
        Set(ByVal value As Boolean)
            _LockGuardando = value
        End Set
    End Property
    Public Property LockEditando() As Boolean
        Get
            Return _LockEditando
        End Get
        Set(ByVal value As Boolean)
            _LockEditando = value
        End Set
    End Property

#End Region
#Region "Modificado"

    'Gestiona para saber si este modelo se ha modificado desde la ultima vez que se guardo

    'Private _Modificado As Boolean 'Indica que se ha modificado esta clase
    '<Xml.Serialization.XmlIgnore()> Public Property Modificado() As Boolean
    '    Get
    '        If _Modificado Then
    '            Return True
    '        Else
    '            'Comprueba las clases hijas
    '            For Each Img As clsModeloImagen In Imagenes
    '                If Img.Modificado Then Return True
    '                'For Each ImgM As clsModeloImagenModificada In Img.Modificadas
    '                '    If ImgM.Modificado Then Return True
    '                'Next
    '            Next
    '            Return False
    '        End If
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _Modificado = value
    '        For Each Img As clsModeloImagen In Imagenes
    '            Img.Modificado = value
    '            For Each ImgM As clsModeloImagenModificada In Img.Modificadas
    '                ImgM.Modificado = value
    '            Next
    '        Next
    '    End Set
    'End Property

#End Region
#Region "Listas"

    'Lista de imagenes
    Public Imagenes As New List(Of clsModeloImagen)

    'Lista de modificadas
    Public Modificadas As New List(Of clsModeloModificada)

    'Lista de TAS
    Public TASlist As New List(Of clsModeloTas)

    'Lista de consumo de tintas estimado
    Public InkCons(7) As Double
#End Region
#Region "Eventos"
    Public Sub setImageEvents()
        Dim img As clsModeloImagen = Nothing
        For Each img In Imagenes
            setImageEvents(img)
        Next
    End Sub
    Public Sub setImageEvents(ByRef img As clsModeloImagen)
        AddHandler img.PorcentajeRipCambiado, AddressOf IMG_PorcentajeRipCambiado
        AddHandler img.EstadoImagenCambiado, AddressOf IMG_EstadoImagenCambiado
        AddHandler img.Excepcion, AddressOf IMG_Excepcion
    End Sub
    Public Sub setModeloModificadaEvents(ByRef modeloModificada As clsModeloModificada)
        'AddHandler modeloModificada.PorcentajeRipCambiado, AddressOf IMG_PorcentajeRipCambiado
        AddHandler modeloModificada.EstadoModificadaCambiado, AddressOf MODIF_EstadoModificadaCambiado
        AddHandler modeloModificada.Excepcion, AddressOf IMG_Excepcion
    End Sub
    Public Sub setModeloTASEvents(ByRef modeloModificada As clsModeloTas)
        'AddHandler modeloModificada.PorcentajeRipCambiado, AddressOf IMG_PorcentajeRipCambiado
        AddHandler modeloModificada.EstadoTASCambiado, AddressOf TAS_EstadoTASCambiado
        AddHandler modeloModificada.Excepcion, AddressOf IMG_Excepcion
    End Sub

    Public Sub eliminaEventosImagenes()
        Dim img As clsModeloImagen = Nothing
        For Each img In Imagenes
            RemoveHandler img.PorcentajeRipCambiado, AddressOf IMG_PorcentajeRipCambiado
            RemoveHandler img.EstadoImagenCambiado, AddressOf IMG_EstadoImagenCambiado
            RemoveHandler img.Excepcion, AddressOf IMG_Excepcion
        Next
    End Sub

    Public Event Excepcion(ByVal ex As Exception)
    Public Event Suceso(ByVal Numero As Integer, ByVal Mensaje As String, ByVal Lugar As String)
    Public Event AuditoriaSYS(ByVal nivel As Integer, ByVal lugar As String, ByVal texto As String)
    Public Event FechaImpresionCambiada(ByVal modelId As Integer, fecha As Date)
    Public Event EstadoModeloCambiado(ByVal modelId As Integer, modelState As eEstadosModelos)
    Public Event ConsumoTintasCambiado(ByVal modelId As Integer, ByVal bCalculated As Boolean)
    Public Event PorcentajeRipCambiado(ByVal modelId As Integer, ByVal imageId As Integer, ByVal percent As Integer)
    Public Event EstadoImagenCambiado(ByVal modelId As Integer, ByVal imageId As Integer, ByVal imageState As eEstadosImagenes)
    Public Event EstadoModificadaCambiado(ByVal modelId As Integer, ByVal modificadaId As Integer, ByVal imageState As eEstadosModelos)
    Public Event EstadoTASCambiado(ByVal modelId As Integer, ByVal TASId As Integer, ByVal imageState As eEstadosModelos)
    Public Event EncolarCalculoTintas(ByVal modelId As Integer)

    Private Sub IMG_EstadoImagenCambiado(ByVal imageId As Integer, ByVal imageState As eEstadosImagenes)
        RaiseEvent EstadoImagenCambiado(_Id, imageId, imageState)
    End Sub
    Private Sub IMG_PorcentajeRipCambiado(ByVal imageId As Integer, ByVal percent As Integer)
        RaiseEvent PorcentajeRipCambiado(_Id, imageId, percent)
    End Sub
    Private Sub IMG_Excepcion(ByVal ex As Exception)
        RaiseEvent Excepcion(ex)
    End Sub

    Private Sub MODIF_EstadoModificadaCambiado(ByVal modificadaId As Integer, ByVal imageState As eEstadosModelos)
        RaiseEvent EstadoModificadaCambiado(_Id, modificadaId, imageState)
    End Sub
    Private Sub TAS_EstadoTASCambiado(ByVal TASId As Integer, ByVal imageState As eEstadosModelos)
        RaiseEvent EstadoTASCambiado(_Id, TASId, imageState)
    End Sub

#End Region

End Class


<Serializable()> Public Class clsModeloVision
    'Representa un Id de camara

    'Lista de puntos de este Id
    Public Puntos As New List(Of sPuntoVision)

    'Para marcar el numero que llegará del origen de deteccion de relieve (el entero que devuelve la máquina de origen, en la cámara coincide con el id, pero en los rodillos es otro número dependiendo de la cara)
    Public Numero As Integer = -1 'TODO Convalor -1 indica que no se debe usar este valor

    'Almacena el ultimo punto impreso
    <Xml.Serialization.XmlIgnore()> Public Ultimo As Integer = 0

    'Acceso al padre
    <Xml.Serialization.XmlIgnore()> Public Parent As clsModelo
End Class

''' <summary>
''' Imagen original. Contiene información referente a una imagen original del modelo
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class clsModeloImagen
    'Representa una imagen de un diseño

#Region "General y propiedades"

    Private _id As Integer
    Private _Original As String 'Nombre y ruta completa a la imagen original
    Private _Descripcion As String 'Comentario de esta imagen introducido por el usuario
    Private _Habilitado As Boolean 'Para indicar si esta habilitada para la impresion
    Private _Orden As Integer 'Orden de uso de la imagen dentro del modelo
    Private _NumeroBmpOriginal As String 'Nombre de la imagen cuando ya es BMP
    Private _Canales As Integer 'Canales de la imagen original
    Private _Tamano As CalculoDpiXY 'Medidas de la imagen original
    Private _calCalidad As Integer 'Id de la calidad del modelo.
    Private _IsRpf As Boolean 'Indica que la imagen no es un tiff, es un rpf
    Private _Repeticiones As Integer 'Indica el número de repeticiones en modo de cambio de imágenes 
    Private _consumoTintaCalculadoImg As Boolean 'Indica si se ha calculado el consumo de tintas de la imagen

    'Private _BmpXpx As Integer 'Medidas de la imagen ya rippeada
    'Private _BmpYpx As Integer 'Medidas de la imagen ya rippeada
    'Private _DpiX As Integer 'DpiX original
    'Private _DpiY As Integer 'DpiY original

    <Xml.Serialization.XmlIgnore()> Public Parent As clsModelo
    '<Xml.Serialization.XmlIgnore()> Public Modificado As Boolean 'Indica que se ha modificado esta clase

    Public ListaManual As New List(Of PointF) 'Lista de puntos en modo manual para esta imagen

    'Lista de número total de gotas por canal. ¡¡OJO!! aquí se guardan los valores por canal, no por barra.
    Public InkDropsPerChannel_Img(15) As ULong

    'Lista de tamaños de los bundle files por canal
    Public BndlSizeXpx As New List(Of Integer)
    Public BndlSizeYpx As New List(Of Integer)

    Public Sub New(ByVal modelo As clsModelo)
        Parent = modelo
        _id = Parent.newImageId
        _Original = ""
        _Descripcion = ""
        _Habilitado = True
        _BmpEstado = eEstadosImagenes.SinProcesar
        _NumeroBmpOriginal = 0
        'Render = Nothing
        _Tamano.TiffPxX = 0
        _Tamano.TiffPxY = 0
        '_BmpXpx = 0
        '_BmpYpx = 0
        _Canales = 0
        _IsRpf = False
        _Repeticiones = 1
        _anularComprobacionesTiff = False
    End Sub 'Nueva desde XML

    Public Sub New()
        _Original = ""
        _Descripcion = ""
        _Habilitado = True
        _BmpEstado = eEstadosImagenes.SinProcesar
        _NumeroBmpOriginal = 0
        'Render = Nothing
        _Tamano.TiffPxX = 0
        _Tamano.TiffPxY = 0
        '_BmpXpx = 0
        '_BmpYpx = 0
        _Canales = 0
        _IsRpf = False
        _Repeticiones = 1
        _anularComprobacionesTiff = False
    End Sub 'Nueva desde XML

    Public Property id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    Private _idParent As Integer = -1
    Public Property IdParent As Integer
        Get
            Return _idParent
        End Get
        Set(value As Integer)
            _idParent = value
        End Set
    End Property

    Public Property Original() As String
        Get
            Return _Original
        End Get
        Set(ByVal value As String)
            'If _Original <> value Then Modificado = True
            _Original = value
            'getTiffInfo()
        End Set
    End Property 'Ruta de la imagen original
    Public Property NumeroBmp() As String
        Get
            Return _NumeroBmpOriginal
        End Get
        Set(ByVal value As String)
            'If _NumeroBmpOriginal <> value Then Modificado = True
            _NumeroBmpOriginal = value
        End Set
    End Property

    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            'If _Descripcion <> value Then Modificado = True
            _Descripcion = value
        End Set
    End Property
    Public Property Habilitado() As Boolean
        Get
            Return _Habilitado
        End Get
        Set(ByVal value As Boolean)
            'Si la imagen se habilita y tenia el consumo de tintas calculado tendré que recalcularlo, 
            '   si no lo tenía marco el consumo calculado del modelo como false
            If Parent IsNot Nothing And _Habilitado <> value Then
                If value = True Then
                    Parent.EncolaCalculoConsumoTintas()
                Else 'La img se está deshabilitando
                    'Si se está deshabilitando la imagen 
                    If Parent.ConsumoTintaCalculado = True Then
                        'Recalcular el consumo
                        Parent.EncolaCalculoConsumoTintas()
                    End If
                End If
            End If
            'If _Habilitado <> value Then Modificado = True
            _Habilitado = value
        End Set
    End Property
    Public Property Orden() As Integer
        Get
            Return _Orden
        End Get
        Set(ByVal value As Integer)
            'If _Orden <> value Then Modificado = True
            _Orden = value
        End Set
    End Property
    Public Property TiffDpiX() As Integer 'Resolucion original
        Get
            getTiffInfo()
            Return _Tamano.TiffResolutionX
        End Get
        Set(ByVal value As Integer)
            'SOLO PARA RESTAURACION DESDE LA BASE DE DATOS
            'If _Dpi <> value Then Modificado = True
            'vsa 06/05/13 - Esta comprobación es para evitar que cuando no se encuentra el tiff original (borrado, en un pendrive, renombrado...) 
            '   machaque los datos con '0', no pudiendo pintar esa imagen al no tener dimensiones y no encontrar un punto válido para pintar
            If value > 0 Then
                _Tamano.TiffResolutionX = value
            End If
        End Set
    End Property
    Public Property TiffDpiY() As Integer 'Resolucion original
        Get
            getTiffInfo()
            Return _Tamano.TiffResolutionY
        End Get
        Set(ByVal value As Integer)
            'SOLO PARA RESTAURACION DESDE LA BASE DE DATOS
            'If _Dpi <> value Then Modificado = True
            'vsa 06/05/13 - Esta comprobación es para evitar que cuando no se encuentra el tiff original (borrado, en un pendrive, renombrado...) 
            '   machaque los datos con '0', no pudiendo pintar esa imagen al no tener dimensiones y no encontrar un punto válido para pintar
            If value > 0 Then
                _Tamano.TiffResolutionY = value
            End If
        End Set
    End Property
    Public Property TiffXpx() As Integer 'Width
        Get
            getTiffInfo()
            Return _Tamano.TiffPxX
        End Get
        Set(ByVal value As Integer)
            'SOLO PARA RESTAURACION DESDE LA BASE DE DATOS
            'If _Tamano.TiffPxX <> value Then Modificado = True
            'vsa 06/05/13 - Esta comprobación es para evitar que cuando no se encuentra el tiff original (borrado, en un pendrive, renombrado...) 
            '   machaque los datos con '0', no pudiendo pintar esa imagen al no tener dimensiones y no encontrar un punto válido para pintar
            If value > 0 Then
                _Tamano.TiffPxX = value
            End If
        End Set
    End Property
    Public Property TiffYpx() As Integer 'Heigh
        Get
            getTiffInfo()
            Return _Tamano.TiffPxY
        End Get
        Set(ByVal value As Integer)
            'SOLO PARA RESTAURACION DESDE LA BASE DE DATOS
            'If _Tamano.TiffPxY <> value Then Modificado = True
            'vsa 06/05/13 - Esta comprobación es para evitar que cuando no se encuentra el tiff original (borrado, en un pendrive, renombrado...) 
            '   machaque los datos con '0', no pudiendo pintar esa imagen al no tener dimensiones y no encontrar un punto válido para pintar
            If value > 0 Then
                _Tamano.TiffPxY = value
            End If
        End Set
    End Property
    Public Property calCalidad As Integer
        Get
            Return _calCalidad
        End Get
        Set(value As Integer)
            _calCalidad = value
        End Set
    End Property
    'Ahora no tiene sentido porque cada canal pueder ser ripeado a una resolución por lo que cada canal tendrá
    'un número de pixels distinto.
    'Public Property BmpXpx() As Integer 'Width
    '    Get
    '        getTiffInfo()
    '        Return _BmpXpx
    '    End Get
    '    Set(ByVal value As Integer)
    '        'SOLO PARA RESTAURACION DESDE LA BASE DE DATOS
    '        'If _BmpXpx <> value Then Modificado = True
    '        _BmpXpx = value
    '    End Set
    'End Property
    'Public Property BmpYpx() As Integer 'Heigh
    '    Get
    '        getTiffInfo()
    '        Return _BmpYpx
    '    End Get
    '    Set(ByVal value As Integer)
    '        'SOLO PARA RESTAURACION DESDE LA BASE DE DATOS
    '        'If _BmpYpx <> value Then Modificado = True
    '        _BmpYpx = value
    '    End Set
    'End Property
    Public Property Canales() As Integer
        Get
            getTiffInfo()
            Return _Canales
        End Get
        Set(ByVal value As Integer)
            'SOLO PARA RESTAURACION DESDE LA BASE DE DATOS
            'If _Canales <> value Then Modificado = True
            'vsa 06/05/13 - Esta comprobación es para evitar que cuando no se encuentra el tiff original (borrado, en un pendrive, renombrado...) 
            '   machaque los datos con '0', no pudiendo pintar esa imagen al no tener dimensiones y no encontrar un punto válido para pintar
            If value > 0 Then
                _Canales = value
            End If
        End Set
    End Property 'Numero de capas que tiene cada pixel

    Public Property ConsumoTintaCalculadoImg() As Boolean 'Indica si la imagen tiene un consumo de tintas guardado
        Set(value As Boolean)
            If _consumoTintaCalculadoImg <> value Then
                _consumoTintaCalculadoImg = value
            End If
        End Set
        Get
            Return _consumoTintaCalculadoImg
        End Get
    End Property

    Private _BmpEstado As eEstadosImagenes
    Public Property BmpEstado() As eEstadosImagenes
        Get
            Return _BmpEstado
        End Get
        Set(ByVal value As eEstadosImagenes)
            If _BmpEstado <> value Then
                If Parent IsNot Nothing Then
                    'Si la imagen estaba OK y pasa a estar en cualquier otro estado marco el flag de consumo de tintas de la imagen a false
                    If _BmpEstado = eEstadosImagenes.OK Then 'AndAlso value <> eEstadosImagenes.OK Then 'Sólo entra aquí si el valor es distinto del anterior
                        ConsumoTintaCalculadoImg = False
                        'Además, si la imagen estaba habilitada marco el modelo como que el consumo de tintas no
                        If Habilitado = True Then
                            'Si la imagen que ha dejado de estar OK estaba habilitada, marco como que el consumo no está calculado
                            Parent.ConsumoTintaCalculado = False
                        End If
                    ElseIf value = eEstadosImagenes.OK Then
                        'Si la imagen pasa a estar OK y estaba habilitada debería recalcular el consumo
                        If Habilitado = True Then
                            Parent.EncolaCalculoConsumoTintas()
                        End If
                    End If

                End If
                _BmpEstado = value
                RaiseEvent EstadoImagenCambiado(_id, value)
            End If
        End Set
    End Property

    Private _BmpOk As Boolean
    Public Property BmpOk() As Boolean
        Get
            Return _BmpOk
        End Get
        Set(ByVal value As Boolean)
            If value <> _BmpOk Then
                _BmpOk = value
                'RaiseEvent EstadoImagenCambiado(_id, _BmpEstado)
            End If
        End Set
    End Property

    Private _porcentajeRip As Integer
    Public Property porcentajeRIP As Integer
        Get
            Return _porcentajeRip
        End Get
        Set(value As Integer)
            If _BmpEstado = eEstadosImagenes.Procesando Then
                'End If
                'If _porcentajeRip <> value Then
                _porcentajeRip = value
                RaiseEvent PorcentajeRipCambiado(_id, value)
            End If
        End Set
    End Property

    Public Property IsRpf() As Boolean
        Get
            Return _IsRpf
        End Get
        Set(ByVal value As Boolean)
            _IsRpf = value
        End Set
    End Property

    Public Property Repeticiones() As Integer
        Get
            Return _Repeticiones
        End Get
        Set(value As Integer)
            _Repeticiones = value
        End Set
    End Property

    Private _anularComprobacionesTiff As Boolean 'Para evitar que se calculen valores del TIFF
    <Xml.Serialization.XmlIgnore()> Public Property AnularComprobacionesTiff() As Boolean
        Get
            Return _anularComprobacionesTiff
        End Get
        Set(ByVal value As Boolean)
            _anularComprobacionesTiff = value
        End Set
    End Property
#End Region
#Region "Imagen original"

    'Calculos segun el tif y la resolucion a la que se debe imprimmir
    Public ReadOnly Property TiffMm() As PointD
        Get
            getTiffInfo()
            Return _Tamano.TiffMm
        End Get
    End Property 'Tamaño de la imagen en milimetros

    '    Public ReadOnly Property BmpPx(ByVal DpiX As Integer) As Point 'Width
    '        Get
    '            getTiffInfo()
    '            If _BmpXpx <= 0 Or _BmpYpx <= 0 Then
    '#If Cabezal = "T" Then
    '                Return _Tamano.BmpPx(DpiX, 300)
    '#Else
    '                Return _Tamano.BmpPx(DpiX, 360)
    '#End If
    '            Else
    '                Return New Point(_BmpXpx, _BmpYpx)
    '            End If
    '        End Get
    '    End Property 'Tamaño de la imagen BMP en pixeles segun la resolucion

    ''Lee la informacion de la imagen solo si faltan datos
    'Private Sub LeerDimensiones()
    '    Try
    '        'Obtiene las dimensiones del TIFF seleccionado
    '        If (_Tamano.TiffMm.X <= 0.0 Or _Tamano.TiffMm.Y <= 0.0 Or _Canales <= 0) And My.Computer.FileSystem.FileExists(_Original) Then

    '            'Lee la informacion de la imagen
    '            Dim info As Creta.Tiff.TiffInfo = Creta.Tiff.cTiff.Info(_Original)
    '            _Canales = info.CanalesPixel

    '            'Calcula los mm de la imagen a partir de los pixeles
    '            _Tamano.TiffPxX = info.Width
    '            _Tamano.TiffPxY = info.Heigh
    '            _Dpi = info.ResX
    '            'Modificado = True
    '        End If

    '        'Obtiene las dimensiones del BMP original
    '        If Parent._bandeja IsNot Nothing AndAlso (_BmpXpx <= 0 Or _BmpYpx <= 0) Then
    '            Dim file As String = Parent._bandeja.RutaBmp(_NumeroBmpOriginal, 0)
    '            If My.Computer.FileSystem.FileExists(file) Then
    '                'Abre el fichero para averiguar el tamaño
    '                Dim bmp As New Creta.Bmp.clibBmp(file)
    '                If bmp.IsOpen Then
    '                    _BmpYpx = Convert.ToInt32(bmp.Heigth)
    '                    _BmpXpx = Convert.ToInt32(bmp.Width)
    '                    bmp.Cerrar()
    '                    'Modificado = True
    '                End If
    '            Else
    '                'El fichero no existe
    '                _BmpXpx = 0
    '                _BmpYpx = 0
    '            End If
    '        End If
    '    Catch ex As Exception
    '        'DB.Suceso(100, ex.Message, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString & ":" & System.Threading.Thread.CurrentThread.Name & vbCrLf & ex.StackTrace)
    '        RaiseEvent Excepcion(ex)
    '        System.Threading.Thread.Sleep(0)
    '    End Try
    'End Sub

    'Lee la informacion de la imagen solo si faltan datos
    Private Sub getTiffInfo()
        If Not _anularComprobacionesTiff Then
            Try
                If _Original.ToLower().EndsWith(".rpf") Then
                    'Es un tiff encriptado de Fiery
                    _IsRpf = True

                    If (_Tamano.TiffPxX <= 0.0 Or _Tamano.TiffPxY <= 0.0 Or _Canales <= 0) AndAlso My.Computer.FileSystem.FileExists(_Original) Then
                        'Monta el nombre para la preview
                        Dim Rut As String = "C:\ProgramData\Cretaprint\CC3\Previews\"
                        Dim Destino As String = Rut & Parent.Id.ToString & "_" & _id.ToString & ".jpg"

                        Dim W As Integer
                        Dim H As Integer
                        Dim C As Integer
                        Dim rX As Integer
                        Dim rY As Integer
                        Dim halftone As Integer
                        If System.IO.File.Exists(Destino) Then System.IO.File.Delete(Destino)
                        If Fiery.GenerarPreview(_Original, Destino, W, H, C, halftone, rX, rY) Then
                            'Imagen Ok
                            _Canales = C
                            _Tamano.TiffPxX = W
                            _Tamano.TiffPxY = H
                            _Tamano.TiffResolutionX = rX
                            _Tamano.TiffResolutionY = rY
                        End If
                    End If
                Else
                    _IsRpf = False

                    'Obtiene las dimensiones del TIFF seleccionado
                    If (_Tamano.TiffPxX <= 0.0 Or _Tamano.TiffPxY <= 0.0 Or _Canales <= 0) AndAlso My.Computer.FileSystem.FileExists(_Original) Then

                        'Lee la informacion de la imagen
                        Dim info As Creta.Tiff.TiffInfo = Creta.Tiff.cTiff.Info(_Original)
                        _Canales = info.CanalesPixel

                        'Calcula los mm de la imagen a partir de los pixeles
                        _Tamano.TiffPxX = info.Width
                        _Tamano.TiffPxY = info.Heigh
                        _Tamano.TiffResolutionX = info.ResX
                        _Tamano.TiffResolutionY = info.ResY
                    End If
                End If

            Catch ex As Exception
                'DB.Suceso(100, ex.Message, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString & ":" & System.Threading.Thread.CurrentThread.Name & vbCrLf & ex.StackTrace)
                RaiseEvent Excepcion(ex)
                System.Threading.Thread.Sleep(0)
            End Try
        End If
    End Sub

#End Region
#Region "Eventos"
    Public Event Excepcion(ByVal ex As Exception)
    Public Event PorcentajeRipCambiado(ByVal imageId As Integer, ByVal percent As Integer)
    Public Event EstadoImagenCambiado(ByVal imageId As Integer, ByVal imageState As eEstadosImagenes)
#End Region

End Class

''' <summary>
''' Especificación de la desviación de una prueba de TAS.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class clsModeloModificada
    'Representa una modificada de un modelo que se aplica a todas las imagenes

    <Xml.Serialization.XmlIgnore()> Public Parent As clsModelo
    '<Xml.Serialization.XmlIgnore()> Public Modificado As Boolean 'Indica que se ha modificado esta clase

    Public TAS As clsTAS 'Incica que tipo de modificacion tiene esta imagen

    Private _Id As Integer
    Public Property Id As Integer
        Get
            Return _Id
        End Get
        Set(value As Integer)
            _Id = value
        End Set
    End Property

    Private _Nombre As String
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            Try
                If _Nombre <> value Then
                    'Cambia el nombre de esta modificacion
                    Dim NombreAnterior As String = _Nombre
                    'Modificado = True
                    _Nombre = value

                    'TODO Descomentar cuando se implemente el TAS
                    ''Comprueba si hay algun TAS para cambiar el nombre del modelo
                    'If Parent IsNot Nothing AndAlso Parent.Parent IsNot Nothing Then
                    '    For Each tas As clsModeloTas In Parent.Parent.TAS
                    '        If tas.Modificada.Trim.ToLower = NombreAnterior.Trim.ToLower Then tas.Modificada = _Nombre
                    '    Next
                    'End If
                End If
            Catch ex As Exception
                'DB.Suceso(100, ex.Message, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString & ":" & System.Threading.Thread.CurrentThread.Name & vbCrLf & ex.StackTrace)
                RaiseEvent Excepcion(ex)
                System.Threading.Thread.Sleep(0)
            End Try
        End Set
    End Property

    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            'If _Descripcion <> value Then Modificado = True
            _Descripcion = value
        End Set
    End Property

    'Indica que se deben mantener los tifs despues de generar los bmps
    Private _GenerarTifs As Boolean
    Public Property GenerarTifs() As Boolean
        Get
            Return _GenerarTifs
        End Get
        Set(ByVal value As Boolean)
            'If _GenerarTifs <> value Then Modificado = True
            _GenerarTifs = value
        End Set
    End Property

    'Indica que se desean hacer los bmps de esta modificada
    Private _GenerarBmps As Boolean
    Public Property GenerarBmps() As Boolean
        Get
            Return _GenerarBmps
        End Get
        Set(ByVal value As Boolean)
            'If _GenerarBmps <> value Then Modificado = True
            _GenerarBmps = value
        End Set
    End Property

    'Indica que todos los Bmps estan en su lugar en los esclavos
    Public ReadOnly Property BmpOk As Boolean
        Get
            'Si no hay ninguna habilitada no esta correcto
            Dim Hab As Boolean = False
            Dim i As Integer = 0
            Dim Imagen As clsModeloImagen = Nothing
            'Repasa las imagenes
            For i = 0 To Parent.Imagenes.Count - 1
                'La original esta habilitada
                Imagen = Parent.Imagenes(i)
                If Imagen.Habilitado = True Then
                    'Hay por lo menos una habilitada
                    Hab = True

                    'Si la modificada no esta correcta devuelve error
                    If i < imagenesModificadas.Count Then
                        If imagenesModificadas(i).BmpOk = False Then Return False
                    Else
                        Debug.WriteLine("Se está intentando acceder a un elemento de la lista que no existe\n Inconsistencia entre la lista de imagenesModificadas y Parent.Imagenes")
                    End If
                End If
            Next
            'For Each modeloModificada As clsModeloModificada In Parent.Modificadas
            '    'Si la modificada no esta correcta devuelve error
            '    If Not modeloModificada.imagenesModificadas(_Indice).BmpOk Then Return False
            'Next
            'Si no hay ninguna habilitada o no hay imagenes devuelve error
            If Not Hab Then Return False

            'Devuelve el estado verdadero si no se ha encontrado ningun error
            Return True
        End Get
    End Property

    'Se usa para saber cual es el indice de esta modificada dentro de la lista de modificadas
    Private _Indice As Integer
    Public Property Indice() As Integer
        Get
            Return _Indice
        End Get
        Set(ByVal value As Integer)
            'If _Indice <> value Then Modificado = True
            _Indice = value
        End Set
    End Property

    'Lista de imagenes modificadas
    ''' <summary>
    ''' Lista de imagenes modificadas, a las que se les ha aplicado una desviación respecto de la original.
    ''' </summary>
    ''' <remarks></remarks>
    Public imagenesModificadas As New List(Of clsModeloImagenModificada)

    Private _calCalidad As Integer
    Public Property calCalidad As Integer
        Get
            Return _calCalidad
        End Get
        Set(value As Integer)
            _calCalidad = value
            For Each imagenModificada As clsModeloImagenModificada In imagenesModificadas
                imagenModificada.calCalidad = _calCalidad
            Next
        End Set
    End Property

    Private _estado As eEstadosModelos = eEstadosModelos.ModeloNoPreparado
    Public Property estado As eEstadosModelos
        Get
            'Dim est As eEstadosModelos = eEstadosModelos.ModeloNoPreparado
            'Dim contOK As Integer = 0
            'Dim contSinprocesar As Integer = 0
            'For Each imagenModificada As clsModeloImagenModificada In imagenesModificadas
            '    If imagenModificada.BmpEstado = eEstadosImagenes.SinProcesarError Or imagenModificada.BmpEstado = eEstadosImagenes.SinMemoria Or
            '        imagenModificada.BmpEstado = eEstadosImagenes.SinOriginal Then
            '        est = eEstadosModelos.ModeloNoPreparado
            '        Exit For
            '    ElseIf imagenModificada.BmpEstado = eEstadosImagenes.EnCola Or imagenModificada.BmpEstado = eEstadosImagenes.Procesando Then
            '        est = eEstadosModelos.Procesando
            '    ElseIf imagenModificada.BmpEstado = eEstadosImagenes.OK Then
            '        contOK += 1
            '    ElseIf imagenModificada.BmpEstado = eEstadosImagenes.SinProcesar Then
            '        contSinprocesar += 1
            '    End If
            'Next

            'If contOK = imagenesModificadas.Count Then
            '    est = eEstadosModelos.OK
            'ElseIf contSinprocesar = imagenesModificadas.Count Then
            '    est = eEstadosModelos.ModeloNoPreparado
            'End If

            'If _estado <> est Then
            '    _estado = est
            '    RaiseEvent EstadoModificadaCambiado(_Id, _estado)
            'End If
            Return _estado
        End Get
        Set(value As eEstadosModelos)
            If _estado <> value Then
                _estado = value
                RaiseEvent EstadoModificadaCambiado(_Id, _estado)
            End If
        End Set
    End Property

    Public Function newImageModificadaId() As Integer
        Dim imageModificadaId As Integer = 0
        Dim imageModificadaInstance As clsModeloImagenModificada
        For Each imageModificadaInstance In imagenesModificadas
            If imageModificadaId <= imageModificadaInstance.id Then
                imageModificadaId = imageModificadaInstance.id + 1
            End If
        Next
        Return imageModificadaId
    End Function


#Region "Eventos"
    Public Sub setImagenesModificadaEvents()
        For Each imagenModificada As clsModeloImagenModificada In imagenesModificadas
            setImagenesModificadaEvents(imagenModificada)
        Next
    End Sub
    Public Sub setImagenesModificadaEvents(ByRef img As clsModeloImagenModificada)
        AddHandler img.Excepcion, AddressOf imagenModificadaExcepcion
        'AddHandler img.PorcentajeRipCambiado, AddressOf imagenModificadaPorcentajeRIPCambiado
        'AddHandler img.EstadoImagenModificadaCambiado, AddressOf imagenModificadaEstadoCambiado
    End Sub
    Public Sub removeImageModificadaEvent(ByRef img As clsModeloImagenModificada)
        RemoveHandler img.Excepcion, AddressOf imagenModificadaExcepcion
        'RemoveHandler img.PorcentajeRipCambiado, AddressOf imagenModificadaPorcentajeRIPCambiado
        'RemoveHandler img.EstadoImagenModificadaCambiado, AddressOf imagenModificadaEstadoCambiado
    End Sub

    Public Event Excepcion(ByVal ex As Exception)
    'Public Event PorcentajeRipCambiado(ByVal imageId As Integer, ByVal percent As Integer)
    Public Event EstadoModificadaCambiado(ByVal modificadaId As Integer, ByVal imageState As eEstadosModelos)

    Private Sub imagenModificadaExcepcion(ByVal ex As Exception)
        RaiseEvent Excepcion(ex)
    End Sub
    'Private Sub imagenModificadaPorcentajeRIPCambiado(ByVal imageId As Integer, ByVal percent As Integer)
    '    RaiseEvent PorcentajeRipCambiado(imageId, percent)
    'End Sub
    'Private Sub imagenModificadaEstadoCambiado(ByVal imageId As Integer, ByVal imageState As eEstadosImagenes)
    '    Dim est As eEstadosModelos = _estado
    '    Dim contOK As Integer = 0
    '    Dim contSinprocesar As Integer = 0
    '    For Each imagenModificada As clsModeloImagenModificada In imagenesModificadas
    '        If imagenModificada.BmpEstado = eEstadosImagenes.SinProcesarError Or imagenModificada.BmpEstado = eEstadosImagenes.SinMemoria Or
    '            imagenModificada.BmpEstado = eEstadosImagenes.SinOriginal Then
    '            est = eEstadosModelos.ModeloNoPreparado
    '            Exit For
    '        ElseIf imagenModificada.BmpEstado = eEstadosImagenes.EnCola Or imagenModificada.BmpEstado = eEstadosImagenes.Procesando Then
    '            est = eEstadosModelos.Procesando
    '            'ElseIf imagenModificada.BmpEstado = eEstadosImagenes.SinProcesar And estado = eEstadosImagenes.SinProcesar Then
    '            'estado = eEstadosImagenes.SinProcesar
    '        ElseIf imagenModificada.BmpEstado = eEstadosImagenes.OK Then
    '            contOK += 1
    '        ElseIf imagenModificada.BmpEstado = eEstadosImagenes.SinProcesar Then
    '            contSinprocesar += 1
    '        End If
    '    Next

    '    If contOK = imagenesModificadas.Count Then
    '        est = eEstadosModelos.OK
    '    ElseIf contSinprocesar = imagenesModificadas.Count Then
    '        est = eEstadosModelos.ModeloNoPreparado
    '    End If

    '    If est <> _estado Then
    '        estado = est
    '    End If

    'End Sub
#End Region
End Class

''' <summary>
''' Imagen modificada. Contiene información sobre una imagen modificada (imagen original a la que se la ha aplicado la desviación definida en clsModeloModificada.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class clsModeloImagenModificada
    'Representa una imagen modificada a partir de la original

    '<Xml.Serialization.XmlIgnore()> Public Parent As clsModeloImagen

    '<Xml.Serialization.XmlIgnore()> Public Render As clsGISRIPEngingItem 'Es una copia del objeto mientras se esta renderizando para obtener informacion
    'se elimina porque desde la interfaz ya tiene que venir bien.
    '<Xml.Serialization.XmlIgnore()> Public Modificado As Boolean 'Indica que se ha modificado esta clase

    'Lista de tamaños de los bundle files por canal
    Public BndlSizeXpx As New List(Of Integer)
    Public BndlSizeYpx As New List(Of Integer)

    Public Sub New(ByRef modeloModificada As clsModeloModificada)
        _id = modeloModificada.newImageModificadaId
        _NumeroTiff = "0"
        _NumeroBmp = "0"
        _TiffEstado = eEstadosImagenes.SinProcesar
        _BmpEstado = eEstadosImagenes.SinProcesar
        _BmpOk = False
    End Sub

    Public Sub New()
        _NumeroTiff = 0
        _NumeroBmp = 0
        _TiffEstado = eEstadosImagenes.SinProcesar
        _BmpEstado = eEstadosImagenes.SinProcesar
        _BmpOk = False
    End Sub

    Private _id As Integer
    Public Property id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    Private _NumeroTiff As Integer
    Public Property NumeroTiff() As Integer
        Get
            Return _NumeroTiff
        End Get
        Set(ByVal value As Integer)
            ' If _NumeroTiff <> value Then Modificado = True
            _NumeroTiff = value
        End Set
    End Property

    Private _NumeroBmp As String
    Public Property NumeroBmp() As String
        Get
            Return _NumeroBmp
        End Get
        Set(ByVal value As String)
            'If _NumeroBmp <> value Then Modificado = True
            _NumeroBmp = value
        End Set
    End Property

    Private _TiffEstado As eEstadosImagenes
    Public Property TiffEstado() As eEstadosImagenes
        Get
            Return _TiffEstado
        End Get
        Set(ByVal value As eEstadosImagenes)
            'If _TiffEstado <> value Then Modificado = True
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
                'RaiseEvent EstadoImagenModificadaCambiado(_id, _BmpEstado)
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

    'Calculos segun el tif y la resolucion a la que se debe imprimmir
    Private _tiffMm As PointD
    Public Property TiffMm() As PointD
        Get
            'getTiffInfo()
            Return _tiffMm
        End Get
        Set(value As PointD)
            _tiffMm = value
        End Set
    End Property 'Tamaño de la imagen en milimetros

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
    Public Property Canales() As Integer
        Get
            Return _Canales
        End Get
        Set(ByVal value As Integer)
            _Canales = value
        End Set
    End Property 'Numero de capas que tiene cada pixel

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
    'Public Event EstadoImagenModificadaCambiado(ByVal imageId As Integer, ByVal imageState As eEstadosImagenes)
#End Region

End Class