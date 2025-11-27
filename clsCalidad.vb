Imports System.Drawing

<Serializable()> Public Class clsCalidad

    Private _Id As Integer
    Private _Nombre As String
    Private _Desc As String
    Private _Noise As Double 'No se usan, para un futuro
    Private _Difussion As Double
    Private _BPP As eBPP
    Private _isEnabled As Boolean = True

    'Public Canales As New List(Of clsCanal)
    Public Canales As List(Of clsCanal) = Nothing

    Public Sub New()
        'Canales = New List(Of clsCanal)
    End Sub

    Public Sub New(ByVal n As String)
        _Id = -1
        _Nombre = n
        _Desc = ""
        _Noise = 0.25
        _Difussion = 1.0
        _BPP = eBPP.BPP4
        _isEnabled = True
        Canales = New List(Of clsCanal)
        For i As Integer = 0 To 7
            'Toshiba siempre 7  niveles
            'TODO para Xaar puede que se tengan que definir calidades con menos de 7 niveles, segun la waveform, para pintar más rápido.
            Canales.Add(New clsCanal(i, 7))
        Next
        'Canales.Clear()
    End Sub

    Public Sub Dispose()
        Dim _c As clsCanal
        If Canales IsNot Nothing Then
            For Each _c In Canales
                RemoveHandler _c.Excepcion, AddressOf calidadExcepcion
                _c = Nothing
            Next
            Canales = Nothing
        End If


    End Sub

    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return _Desc
        End Get
        Set(ByVal value As String)
            _Desc = value
        End Set
    End Property
    Public Property Noise() As Double
        Get
            Return _Noise
        End Get
        Set(ByVal value As Double)
            If value > 1.0 Then
                _Noise = 1.0
            ElseIf value < 0.0 Then
                _Noise = 0.0
            Else
                _Noise = value
            End If
        End Set
    End Property
    Public Property Diffusion() As Double
        Get
            Return _Difussion
        End Get
        Set(ByVal value As Double)
            If value > 1.0 Then
                _Difussion = 1.0
            ElseIf value < 0.0 Then
                _Difussion = 0.0
            Else
                _Difussion = value
            End If
        End Set
    End Property
    Public Property BPP() As eBPP
        Get
            Return _BPP
        End Get
        Set(ByVal value As eBPP)
            _BPP = value
        End Set
    End Property
    Public Property Id As Integer
        Get
            Return _Id
        End Get
        Set(value As Integer)
            _Id = value
        End Set
    End Property
    Public Property IsEnabled As Boolean
        Get
            Return _isEnabled
        End Get
        Set(value As Boolean)
            _isEnabled = value
        End Set
    End Property

#Region "Eventos"
    Public Event Excepcion(ByVal ex As Exception)

    Private Sub calidadExcepcion(ex As Exception)
        RaiseEvent Excepcion(ex)
    End Sub
#End Region
End Class
<Serializable()> Public Class clsCanal

    Private _Canal As Integer
    Private _Niveles As Integer

    Private _DpiX As Integer = 300
    Public Property DpiX As Integer
        Get
            Return _DpiX
        End Get
        Set(value As Integer)
            _DpiX = value
        End Set
    End Property

    Private _DoubleYDpi As Boolean = False
    Public Property DoubleYDpi As Boolean
        Get
            Return _DoubleYDpi
        End Get
        Set(value As Boolean)
            _DoubleYDpi = value
        End Set
    End Property

    Private _DpiY As Integer = 300
    Public Property DpiY As Integer
        Get
            Return _DpiY
        End Get
        Set(value As Integer)
            _DpiY = value
        End Set
    End Property

    Public Gotas As New List(Of clsCanalGota)

    Public Sub New()
        'Para la serializacion
        Defecto()
    End Sub
    Public Sub New(ByVal vCanal As Integer, ByVal vNiveles As Integer)
        Defecto()

        'Calcula los niveles de gota
        For i As Integer = 1 To 15
            Gotas.Add(New clsCanalGota(i, True, 100.0))
        Next

        'Numero de canal
        Canal = vCanal
        Niveles = vNiveles

        'Hace el calculo de niveles equitativamente
        CalcDensidad()
    End Sub
    Private Sub Defecto()
        _P0 = New PointF(0.0!, 0.0!)
        _P5 = New PointF(5.0!, 5.0!)
        _P10 = New PointF(10.0!, 10.0!)
        _P15 = New PointF(15.0!, 15.0!)
        _P20 = New PointF(20.0!, 20.0!)
        _P25 = New PointF(25.0!, 25.0!)
        _P30 = New PointF(30.0!, 30.0!)
        _P35 = New PointF(35.0!, 35.0!)
        _P40 = New PointF(40.0!, 40.0!)
        _P45 = New PointF(45.0!, 45.0!)
        _P50 = New PointF(50.0!, 50.0!)
        _P55 = New PointF(55.0!, 55.0!)
        _P60 = New PointF(60.0!, 60.0!)
        _P65 = New PointF(65.0!, 65.0!)
        _P70 = New PointF(70.0!, 70.0!)
        _P75 = New PointF(75.0!, 75.0!)
        _P80 = New PointF(80.0!, 80.0!)
        _P85 = New PointF(85.0!, 85.0!)
        _P90 = New PointF(90.0!, 90.0!)
        _P95 = New PointF(95.0!, 95.0!)
        _P100 = New PointF(100.0!, 100.0!)
    End Sub

    Public Property Canal() As Integer
        Get
            Return _Canal
        End Get
        Set(ByVal value As Integer)
            If value > 8 Then
                _Canal = 8
            ElseIf value < 1 Then
                _Canal = 1
            Else
                _Canal = value
            End If
        End Set
    End Property
    Public Property Niveles() As Integer
        Get
            Return _Niveles
        End Get
        Set(ByVal value As Integer)

            If value > 15 Then
                _Niveles = 15
            ElseIf value < 1 Then
                _Niveles = 1
            Else
                _Niveles = value
            End If
        End Set
    End Property

    Public Sub CalcDensidad()
        Try
            'Calcula los niveles de gota
            Dim Paso As Double = 100.0 / Niveles
            Dim Densidad As Double = 0.0
            For i As Integer = 0 To 14
                If i >= Niveles Then
                    Densidad = 100.0
                Else
                    Densidad += Paso
                End If
                Gotas(i).Density = Densidad

                If i >= Niveles Then
                    Gotas(i).Habilitado = False
                End If
            Next
        Catch ex As Exception
            'DB.Suceso(100, ex.Message, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString & ":" & System.Threading.Thread.CurrentThread.Name & vbCrLf & ex.StackTrace)
            RaiseEvent Excepcion(ex)
            System.Threading.Thread.Sleep(0)
        End Try
    End Sub

#Region "Linearizacion"

    Private _P0 As PointF
    Private _P5 As PointF
    Private _P10 As PointF
    Private _P15 As PointF
    Private _P20 As PointF
    Private _P25 As PointF
    Private _P30 As PointF
    Private _P35 As PointF
    Private _P40 As PointF
    Private _P45 As PointF
    Private _P50 As PointF
    Private _P55 As PointF
    Private _P60 As PointF
    Private _P65 As PointF
    Private _P70 As PointF
    Private _P75 As PointF
    Private _P80 As PointF
    Private _P85 As PointF
    Private _P90 As PointF
    Private _P95 As PointF
    Private _P100 As PointF

    Public Property P0() As PointF
        Get
            Return _P0
        End Get
        Set(ByVal value As PointF)
            _P0 = value
            If _P0.X > 100.0! Then _P0.X = 100.0!
            If _P0.Y > 100.0! Then _P0.Y = 100.0!
            If _P0.X < 0.0! Then _P0.X = 0.0!
            If _P0.Y < 0.0! Then _P0.Y = 0.0!
        End Set
    End Property
    Public Property P5() As PointF
        Get
            Return _P5
        End Get
        Set(ByVal value As PointF)
            _P5 = value
            If _P5.X > 100.0! Then _P5.X = 100.0!
            If _P5.Y > 100.0! Then _P5.Y = 100.0!
            If _P5.X < 0.0! Then _P5.X = 0.0!
            If _P5.Y < 0.0! Then _P5.Y = 0.0!
        End Set
    End Property
    Public Property P10() As PointF
        Get
            Return _P10
        End Get
        Set(ByVal value As PointF)
            _P10 = value
            If _P10.X > 100.0! Then _P10.X = 100.0!
            If _P10.Y > 100.0! Then _P10.Y = 100.0!
            If _P10.X < 0.0! Then _P10.X = 0.0!
            If _P10.Y < 0.0! Then _P10.Y = 0.0!
        End Set
    End Property
    Public Property P15() As PointF
        Get
            Return _P15
        End Get
        Set(ByVal value As PointF)
            _P15 = value
            If _P15.X > 100.0! Then _P15.X = 100.0!
            If _P15.Y > 100.0! Then _P15.Y = 100.0!
            If _P15.X < 0.0! Then _P15.X = 0.0!
            If _P15.Y < 0.0! Then _P15.Y = 0.0!
        End Set
    End Property
    Public Property P20() As PointF
        Get
            Return _P20
        End Get
        Set(ByVal value As PointF)
            _P20 = value
            If _P20.X > 100.0! Then _P20.X = 100.0!
            If _P20.Y > 100.0! Then _P20.Y = 100.0!
            If _P20.X < 0.0! Then _P20.X = 0.0!
            If _P20.Y < 0.0! Then _P20.Y = 0.0!
        End Set
    End Property
    Public Property P25() As PointF
        Get
            Return _P25
        End Get
        Set(ByVal value As PointF)
            _P25 = value
            If _P25.X > 100.0! Then _P25.X = 100.0!
            If _P25.Y > 100.0! Then _P25.Y = 100.0!
            If _P25.X < 0.0! Then _P25.X = 0.0!
            If _P25.Y < 0.0! Then _P25.Y = 0.0!
        End Set
    End Property
    Public Property P30() As PointF
        Get
            Return _P30
        End Get
        Set(ByVal value As PointF)
            _P30 = value
            If _P30.X > 100.0! Then _P30.X = 100.0!
            If _P30.Y > 100.0! Then _P30.Y = 100.0!
            If _P30.X < 0.0! Then _P30.X = 0.0!
            If _P30.Y < 0.0! Then _P30.Y = 0.0!
        End Set
    End Property
    Public Property P35() As PointF
        Get
            Return _P35
        End Get
        Set(ByVal value As PointF)
            _P35 = value
            If _P35.X > 100.0! Then _P35.X = 100.0!
            If _P35.Y > 100.0! Then _P35.Y = 100.0!
            If _P35.X < 0.0! Then _P35.X = 0.0!
            If _P35.Y < 0.0! Then _P35.Y = 0.0!
        End Set
    End Property
    Public Property P40() As PointF
        Get
            Return _P40
        End Get
        Set(ByVal value As PointF)
            _P40 = value
            If _P40.X > 100.0! Then _P40.X = 100.0!
            If _P40.Y > 100.0! Then _P40.Y = 100.0!
            If _P40.X < 0.0! Then _P40.X = 0.0!
            If _P40.Y < 0.0! Then _P40.Y = 0.0!
        End Set
    End Property
    Public Property P45() As PointF
        Get
            Return _P45
        End Get
        Set(ByVal value As PointF)
            _P45 = value
            If _P45.X > 100.0! Then _P45.X = 100.0!
            If _P45.Y > 100.0! Then _P45.Y = 100.0!
            If _P45.X < 0.0! Then _P45.X = 0.0!
            If _P45.Y < 0.0! Then _P45.Y = 0.0!
        End Set
    End Property
    Public Property P50() As PointF
        Get
            Return _P50
        End Get
        Set(ByVal value As PointF)
            _P50 = value
            If _P50.X > 100.0! Then _P50.X = 100.0!
            If _P50.Y > 100.0! Then _P50.Y = 100.0!
            If _P50.X < 0.0! Then _P50.X = 0.0!
            If _P50.Y < 0.0! Then _P50.Y = 0.0!
        End Set
    End Property
    Public Property P55() As PointF
        Get
            Return _P55
        End Get
        Set(ByVal value As PointF)
            _P55 = value
            If _P55.X > 100.0! Then _P55.X = 100.0!
            If _P55.Y > 100.0! Then _P55.Y = 100.0!
            If _P55.X < 0.0! Then _P55.X = 0.0!
            If _P55.Y < 0.0! Then _P55.Y = 0.0!
        End Set
    End Property
    Public Property P60() As PointF
        Get
            Return _P60
        End Get
        Set(ByVal value As PointF)
            _P60 = value
            If _P60.X > 100.0! Then _P60.X = 100.0!
            If _P60.Y > 100.0! Then _P60.Y = 100.0!
            If _P60.X < 0.0! Then _P60.X = 0.0!
            If _P60.Y < 0.0! Then _P60.Y = 0.0!
        End Set
    End Property
    Public Property P65() As PointF
        Get
            Return _P65
        End Get
        Set(ByVal value As PointF)
            _P65 = value
            If _P65.X > 100.0! Then _P65.X = 100.0!
            If _P65.Y > 100.0! Then _P65.Y = 100.0!
            If _P65.X < 0.0! Then _P65.X = 0.0!
            If _P65.Y < 0.0! Then _P65.Y = 0.0!
        End Set
    End Property
    Public Property P70() As PointF
        Get
            Return _P70
        End Get
        Set(ByVal value As PointF)
            _P70 = value
            If _P70.X > 100.0! Then _P70.X = 100.0!
            If _P70.Y > 100.0! Then _P70.Y = 100.0!
            If _P70.X < 0.0! Then _P70.X = 0.0!
            If _P70.Y < 0.0! Then _P70.Y = 0.0!
        End Set
    End Property
    Public Property P75() As PointF
        Get
            Return _P75
        End Get
        Set(ByVal value As PointF)
            _P75 = value
            If _P75.X > 100.0! Then _P75.X = 100.0!
            If _P75.Y > 100.0! Then _P75.Y = 100.0!
            If _P75.X < 0.0! Then _P75.X = 0.0!
            If _P75.Y < 0.0! Then _P75.Y = 0.0!
        End Set
    End Property
    Public Property P80() As PointF
        Get
            Return _P80
        End Get
        Set(ByVal value As PointF)
            _P80 = value
            If _P80.X > 100.0! Then _P80.X = 100.0!
            If _P80.Y > 100.0! Then _P80.Y = 100.0!
            If _P80.X < 0.0! Then _P80.X = 0.0!
            If _P80.Y < 0.0! Then _P80.Y = 0.0!
        End Set
    End Property
    Public Property P85() As PointF
        Get
            Return _P85
        End Get
        Set(ByVal value As PointF)
            _P85 = value
            If _P85.X > 100.0! Then _P85.X = 100.0!
            If _P85.Y > 100.0! Then _P85.Y = 100.0!
            If _P85.X < 0.0! Then _P85.X = 0.0!
            If _P85.Y < 0.0! Then _P85.Y = 0.0!
        End Set
    End Property
    Public Property P90() As PointF
        Get
            Return _P90
        End Get
        Set(ByVal value As PointF)
            _P90 = value
            If _P90.X > 100.0! Then _P90.X = 100.0!
            If _P90.Y > 100.0! Then _P90.Y = 100.0!
            If _P90.X < 0.0! Then _P90.X = 0.0!
            If _P90.Y < 0.0! Then _P90.Y = 0.0!
        End Set
    End Property
    Public Property P95() As PointF
        Get
            Return _P95
        End Get
        Set(ByVal value As PointF)
            _P95 = value
            If _P95.X > 100.0! Then _P95.X = 100.0!
            If _P95.Y > 100.0! Then _P95.Y = 100.0!
            If _P95.X < 0.0! Then _P95.X = 0.0!
            If _P95.Y < 0.0! Then _P95.Y = 0.0!
        End Set
    End Property
    Public Property P100() As PointF
        Get
            Return _P100
        End Get
        Set(ByVal value As PointF)
            _P100 = value
            If _P100.X > 100.0! Then _P100.X = 100.0!
            If _P100.Y > 100.0! Then _P100.Y = 100.0!
            If _P100.X < 0.0! Then _P100.X = 0.0!
            If _P100.Y < 0.0! Then _P100.Y = 0.0!
        End Set
    End Property

#End Region
#Region "Eventos"
    Public Event Excepcion(ByVal ex As Exception)
#End Region
End Class
<Serializable()> Public Class clsCanalGota

    Private _GreyLevel As Integer
    Private _Hab As Boolean
    Private _Den As Double
    Private _Over As Double

    Public Sub New()
        _GreyLevel = 1
        _Hab = True
        _Den = 100.0
        _Over = 0.0
    End Sub
    Public Sub New(ByVal vGreyLevel As Integer, ByVal vHabilitado As Boolean, ByVal vDensity As Double)
        GreyLevel = vGreyLevel
        Habilitado = vHabilitado
        Density = vDensity
    End Sub

    Public Property GreyLevel() As Integer
        Get
            Return _GreyLevel
        End Get
        Set(ByVal value As Integer)
            If value > 15 Then
                _GreyLevel = 15
            ElseIf value < 1 Then
                _GreyLevel = 1
            Else
                _GreyLevel = value
            End If
        End Set
    End Property
    Public Property Habilitado() As Boolean
        Get
            Return _Hab
        End Get
        Set(ByVal value As Boolean)
            _Hab = value
        End Set
    End Property
    Public Property Density() As Double
        Get
            Return _Den
        End Get
        Set(ByVal value As Double)
            If value > 100.0 Then
                _Den = 100.0
            ElseIf value < 0.0 Then
                _Den = 0.0
            Else
                _Den = value
            End If
        End Set
    End Property
    Public Property Overload() As Double
        Get
            Return _Over
        End Get
        Set(ByVal value As Double)
            If value > 100.0 Then
                _Over = 100.0
            ElseIf value < 0.0 Then
                _Over = 0.0
            Else
                _Over = value
            End If
        End Set
    End Property

End Class

Public Enum eBPP
    BPP1 = 1
    BPP2 = 2
    BPP4 = 4
End Enum

