

<Serializable()> Public Class clsGISInfoBarra

    Private _numBarra As Integer
    Private _numPMBs As Integer
    'Private _rutaRip As String
    Private _colorPlane As Integer
    Private _numCabezales As Integer
    Private _listaCabezales As List(Of clsInfoCabezal)
    Private _listaCabezalesPorLinea As List(Of Integer)

    Private _listaPMBs As List(Of clsInfoPMB)
    'Private _alineamientoX As Long
    Private _resolucionXL1 As Short
    Private _resolucionXL2 As Short
    Private _resolucionY As Short
    Private _espacioDisponible As Double
    Private _distanciaSensor As Double
    Private _tipoBarra As TypesIB.enumTipoBarra
    Private _tipoCabezal As TypesIB.enumTipoCabezal
    Private _union As TypesIB.enumUnionPMB
    Private _l1IsEnabled As Boolean
    Private _l2IsEnabled As Boolean
    Private _inkColor As String
    Private _waveformPath As String
    Private _waveformNumber As Integer
    Private _dropletVolume As Double
    Private _l1PrintEnabled As Boolean
    Private _l2PrintEnabled As Boolean
    Private _spitEnabled As Boolean
    Private _spitFrequency As Integer
    Private _spitCycle As Integer
    Private _spitTime As Single
    Private _tipoElectronica As TypesIB.enumTipoElectronica
    Private _numCabezalesL1 As Integer
    Private _numCabezalesL2 As Integer
    Private _numPCL1Robot As Integer
    Private _numPCL2Robot As Integer
    Private _robotHMIPortL1 As Integer
    Private _robotHMIPortL2 As Integer
    Private _numCBs As Integer
    Private _ditherEnabled As Boolean
    Private _firstHeadPosisiton As TypesIB.enumPosPrimerCabezal



    Public Property numBarra As Integer
        Get
            Return _numBarra
        End Get
        Set(ByVal value As Integer)
            _numBarra = value
        End Set
    End Property
    Public Property numPMBs As Integer
        Get
            Return _numPMBs
        End Get
        Set(ByVal value As Integer)
            _numPMBs = value
        End Set
    End Property
    Public Property colorPlane As Integer
        Get
            Return _colorPlane
        End Get
        Set(ByVal value As Integer)
            _colorPlane = value


        End Set
    End Property
    Public Property numCabezales As Integer
        Get
            If tipoElectronica = TypesIB.enumTipoElectronica.GIS Then
                Dim contNumCabezales As Integer = 0
                For Each _ipmb As clsInfoPMB In _listaPMBs
                    contNumCabezales += _ipmb.numCabezales
                Next
                Return contNumCabezales
            Else
                Return numCabezalesL1 + numCabezalesL2
            End If
        End Get
        Set(ByVal value As Integer)
            _numCabezales = value
        End Set
    End Property
    Public Property listaCabezales As List(Of clsInfoCabezal)
        Get
            Return _listaCabezales
        End Get
        Set(ByVal value As List(Of clsInfoCabezal))
            _listaCabezales = value
        End Set
    End Property

    Public Property listaCabezalesPorLinea As List(Of Integer)
        Get
            Return _listaCabezalesPorLinea
        End Get
        Set(ByVal value As List(Of Integer))
            _listaCabezalesPorLinea = value
        End Set
    End Property
  
    Public Property listaPMBs As List(Of clsInfoPMB)
        Get
            Return _listaPMBs
        End Get
        Set(ByVal value As List(Of clsInfoPMB))
            _listaPMBs = value
        End Set
    End Property
    'Public Property alineamientoX As Long
    '    Get
    '        Return _alineamientoX
    '    End Get
    '    Set(ByVal value As Long)
    '        _alineamientoX = value
    '    End Set
    'End Property
    Public Property resolucionXL1 As Short
        Get
            Return _resolucionXL1
        End Get
        Set(ByVal value As Short)
            _resolucionXL1 = value
        End Set
    End Property
    Public Property resolucionXL2 As Short
        Get
            Return _resolucionXL2
        End Get
        Set(ByVal value As Short)
            _resolucionXL2 = value
        End Set
    End Property
    Public Property resolucionY As Short
        Get
            Return _resolucionY
        End Get
        Set(ByVal value As Short)
            _resolucionY = value
        End Set
    End Property
    Public Property distanciaSensor As Double
        Get
            Return _distanciaSensor
        End Get
        Set(ByVal value As Double)
            _distanciaSensor = value
        End Set
    End Property
    Public Property tipoBarra As TypesIB.enumTipoBarra
        Get
            Return _tipoBarra
        End Get
        Set(value As TypesIB.enumTipoBarra)
            _tipoBarra = value
        End Set
    End Property
    Public Property tipoCabezal As TypesIB.enumTipoCabezal
        Get
            Return _tipoCabezal
        End Get
        Set(value As TypesIB.enumTipoCabezal)
            _tipoCabezal = value
            Select Case _tipoCabezal
                Case TypesIB.enumTipoCabezal.T
                    resolucionY = 300
                Case TypesIB.enumTipoCabezal.X, TypesIB.enumTipoCabezal.XGS40
                    resolucionY = 360
                Case TypesIB.enumTipoCabezal.D
                    resolucionY = 400
            End Select
        End Set
    End Property
    Public Property union As TypesIB.enumUnionPMB
        Get
            Return _union
        End Get
        Set(value As TypesIB.enumUnionPMB)
            _union = value
        End Set
    End Property
    Public Property l1IsEnabled As Boolean
        Get
            Return _l1IsEnabled
        End Get
        Set(value As Boolean)
            _l1IsEnabled = value
        End Set
    End Property
    Public Property l2IsEnabled As Boolean
        Get
            Return _l2IsEnabled
        End Get
        Set(value As Boolean)
            _l2IsEnabled = value
        End Set
    End Property
    Public Property inkColor As String
        Get
            Return _inkColor
        End Get
        Set(value As String)
            _inkColor = value
        End Set
    End Property
    Public Property waveformPath As String
        Get
            Return _waveformPath
        End Get
        Set(value As String)
            _waveformPath = value
        End Set
    End Property
    Public Property waveformNumber As Integer
        Get
            Return _waveformNumber
        End Get
        Set(value As Integer)
            _waveformNumber = value
        End Set
    End Property
    Public Property dropletVolume As Double
        Get
            Return _dropletVolume
        End Get
        Set(value As Double)
            _dropletVolume = value
        End Set
    End Property
    Public Property l1PrintEnabled As Boolean
        Get
            Return _l1PrintEnabled
        End Get
        Set(value As Boolean)
            _l1PrintEnabled = value
        End Set
    End Property
    Public Property l2PrintEnabled As Boolean
        Get
            Return _l2PrintEnabled
        End Get
        Set(value As Boolean)
            _l2PrintEnabled = value
        End Set
    End Property
    ''' <summary>
    ''' Indica si el spit de limpieza está habilitado o no
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property spitEnabled As Boolean
        Get
            Return _spitEnabled
        End Get
        Set(value As Boolean)
            _spitEnabled = value
        End Set
    End Property

    Public Property spitFrequency As Integer
        Get
            Return _spitFrequency
        End Get
        Set(value As Integer)
            _spitFrequency = value
        End Set
    End Property
    Public Property spitCycle As Integer
        Get
            Return _spitCycle
        End Get
        Set(value As Integer)
            _spitCycle = value
        End Set
    End Property
    Public Property spitTime As Single
        Get
            Return _spitTime
        End Get
        Set(value As Single)
            _spitTime = value
        End Set
    End Property
    Public Property tipoElectronica As TypesIB.enumTipoElectronica
        Get
            Return _tipoElectronica
        End Get
        Set(value As TypesIB.enumTipoElectronica)
            _tipoElectronica = value
        End Set
    End Property
    Public Property numCabezalesL1 As Integer
        Get
            Return _numCabezalesL1
        End Get
        Set(value As Integer)
            _numCabezalesL1 = value
        End Set
    End Property
    Public Property numCabezalesL2 As Integer
        Get
            Return _numCabezalesL2
        End Get
        Set(value As Integer)
            _numCabezalesL2 = value
        End Set
    End Property
    Public Property numPCL1Robot As Integer
        Get
            Return _numPCL1Robot
        End Get
        Set(value As Integer)
            _numPCL1Robot = value
        End Set
    End Property
    Public Property numPCL2Robot As Integer
        Get
            Return _numPCL2Robot
        End Get
        Set(value As Integer)
            _numPCL2Robot = value
        End Set
    End Property
    Public Property robotHMIPortL1() As Integer
        Get
            Return (_robotHMIPortL1)
        End Get
        Set(value As Integer)
            _robotHMIPortL1 = value
        End Set
    End Property
    Public Property robotHMIPortL2() As Integer
        Get
            Return (_robotHMIPortL2)
        End Get
        Set(value As Integer)
            _robotHMIPortL2 = value
        End Set
    End Property
    Public Property numCBs() As Integer
        Get
            Return _numCBs
        End Get
        Set(value As Integer)
            _numCBs = value
        End Set
    End Property
    Public Property ditherEnabled As Boolean
        Get
            Return _ditherEnabled
        End Get
        Set(value As Boolean)
            _ditherEnabled = value
        End Set
    End Property
    Public Property FirstHeadPosisiton As TypesIB.enumPosPrimerCabezal
        Get
            Return _firstHeadPosisiton
        End Get
        Set(value As TypesIB.enumPosPrimerCabezal)
            _firstHeadPosisiton = value
        End Set
    End Property



    Public Sub New(numeroBarra As Integer)
        _numBarra = numeroBarra
        _listaPMBs = New List(Of clsInfoPMB)
        _listaCabezales = New List(Of clsInfoCabezal)
        _listaCabezalesPorLinea = New List(Of Integer)
        _numPMBs = 1
        _colorPlane = 0
        '_numCabezales = 10
        _resolucionXL1 = 360
        _resolucionXL2 = 360
        _resolucionY = 300
        _espacioDisponible = 999999999999999
        _distanciaSensor = 200.0
        _tipoBarra = TypesIB.enumTipoBarra.DECORACION
        _tipoCabezal = TypesIB.enumTipoCabezal.T
        _union = TypesIB.enumUnionPMB.Unir01
        _l1IsEnabled = True
        _l2IsEnabled = True
        _l1PrintEnabled = True
        _l2PrintEnabled = True
    End Sub
    Public Sub Dispose()
        For Each auxPMB As clsInfoPMB In _listaPMBs
            auxPMB.Dispose()
            auxPMB = Nothing
        Next
        _listaPMBs = Nothing

        GC.SuppressFinalize(Me)
    End Sub

End Class

<Serializable()> Public Class clsInfoPMB
    Private _listaCabezales As List(Of clsInfoCabezal)
    'Private _alineamientoX As Decimal
    Private _numPC As Integer 'The PC id could be a number (1, 2, 3, ...) or the IP direction.
    Private _numCabezales As Integer
    Private _puertoPMB As Integer
    Private _numPMB As Integer
    Private _numBarra As Integer
    'Private _colorPlane As Integer

    Public Property listaCabezales As List(Of clsInfoCabezal)
        Get
            Return _listaCabezales
        End Get
        Set(ByVal value As List(Of clsInfoCabezal))
            _listaCabezales = value
        End Set
    End Property
    'Public Property alineamientoX As Decimal
    '    Get
    '        Return _alineamientoX
    '    End Get
    '    Set(ByVal value As Decimal)
    '        _alineamientoX = value
    '    End Set
    'End Property
    Public Property numPC As Integer
        Get
            Return _numPC
        End Get
        Set(ByVal value As Integer)
            _numPC = value
        End Set
    End Property
    'Public Property ip As String
    '    Get
    '        Return _ip
    '    End Get
    '    Set(ByVal value As String)
    '        _ip = value
    '    End Set
    'End Property
    Public Property numCabezales As Integer
        Get
            Return _numCabezales
        End Get
        Set(ByVal value As Integer)
            _numCabezales = value
        End Set
    End Property
    Public Property puertoPMB As Integer
        Get
            Return _puertoPMB
        End Get
        Set(ByVal value As Integer)
            _puertoPMB = value
        End Set
    End Property
    Public Property numPMB As Integer
        Get
            Return _numPMB
        End Get
        Set(value As Integer)
            _numPMB = value
        End Set
    End Property
    Public Property numBarra As Integer
        Get
            Return _numBarra
        End Get
        Set(value As Integer)
            _numBarra = value
        End Set
    End Property
    'Public Property colorPlane As Integer
    '    Get
    '        Return _colorPlane
    '    End Get
    '    Set(value As Integer)
    '        _colorPlane = value
    '    End Set
    'End Property

    Public Sub New(PMB As Integer, Barra As Integer)
        _listaCabezales = New List(Of clsInfoCabezal)
        _numPMB = PMB
        _numBarra = Barra
        '_alineamientoX = 200
        _numPC = 1
        _numCabezales = 0
        _puertoPMB = 2001
    End Sub
    Public Sub Dispose()
        For Each auxCabezal As clsInfoCabezal In listaCabezales
            auxCabezal = Nothing
        Next
        listaCabezales = Nothing

        GC.SuppressFinalize(Me)
    End Sub
End Class

<Serializable()> Public Class clsInfoCabezal

    Private _listaRows As List(Of clsInfoRow)
    Private _alineamientoX As Double
    Private _alineamientoY As Double
    Private _numRows As Integer
    Private _numTrims As Integer
    Private _voltajeMedio As Decimal
    Private _numCabezal As Integer
    Private _habilitado As Boolean
    Private _anuladosInicio As Integer
    Private _anuladosFin As Integer
    Private _interrowOffsetDesviation As Integer
    Private _logicalHeadID As Integer
    Private _carriagePort As Integer
    Private _carriageSubport As Integer
    Private _serialNumber As String
    Private _cadenaHEAD As String = "HEAD"
    Private _estado As TypesIB.enumEstadosCabezalRobot
    Private _waveformPath As String
    Private _tipoCabezal As TypesIB.enumTipoCabezal
    Private _mirrored As Boolean
    Private _fireOrder As Boolean



    Public Property listaRows As List(Of clsInfoRow)
        Get
            Return _listaRows
        End Get
        Set(value As List(Of clsInfoRow))
            _listaRows = value
        End Set
    End Property
    Public Property alineamientoX As Double
        Get
            Return _alineamientoX
        End Get
        Set(value As Double)
            _alineamientoX = value
            RaiseEvent setHeadParametroBD("XPOSITION", value, LogicalHeadID)
        End Set
    End Property
    Public Property alineamientoY As Double
        Get
            Return _alineamientoY
        End Get
        Set(value As Double)
            _alineamientoY = value
            RaiseEvent setHeadParametroBD("YPOSITION", value, LogicalHeadID)
        End Set
    End Property
    Public Property numRows As Integer
        Get
            Return _numRows
        End Get
        Set(value As Integer)

            If TipoCabezal = TypesIB.enumTipoCabezal.T Or TipoCabezal = TypesIB.enumTipoCabezal.X Or TipoCabezal = TypesIB.enumTipoCabezal.XGS40 Then
                _numRows = 2
            ElseIf TipoCabezal = TypesIB.enumTipoCabezal.D Then
                _numRows = 4
            End If
            Dim auxRow As clsInfoRow = Nothing
            While listaRows.Count < _numRows
                auxRow = New clsInfoRow(listaRows.Count, _numTrims)
                AddHandler auxRow.SetRowParametroBD, AddressOf Row_setParametro
                listaRows.Add(auxRow)
            End While
            While listaRows.Count > _numRows
                listaRows.RemoveAt(listaRows.Count - 1)
            End While

            RaiseEvent setHeadParametroBD("NUMROWS", _numRows, LogicalHeadID)
        End Set
    End Property
    Public Property numTrims As Integer
        Get
            Return _numTrims
        End Get
        Set(value As Integer)
            If TipoCabezal = TypesIB.enumTipoCabezal.T Then
                _numTrims = 1
            ElseIf TipoCabezal = TypesIB.enumTipoCabezal.X Or TipoCabezal = TypesIB.enumTipoCabezal.XGS40 Then
                _numTrims = 8
            ElseIf TipoCabezal = TypesIB.enumTipoCabezal.D Then
                _numTrims = 1
            End If
            For Each _r As clsInfoRow In listaRows
                _r.numTrims = _numTrims
            Next
            RaiseEvent setHeadParametroBD("NUMTRIMS", _numTrims, LogicalHeadID)
        End Set
    End Property
    Public ReadOnly Property voltajeMedio As Decimal
        Get
            Dim _voltajeTotal As Decimal = 0
            Dim i As Integer = 0
            For Each _row As clsInfoRow In listaRows
                For Each _trim As Decimal In _row.listaTrims
                    i += 1
                    _voltajeTotal = _voltajeTotal + _trim
                Next
            Next
            _voltajeMedio = CShort(_voltajeTotal / i)
            Return _voltajeMedio
        End Get
    End Property
    Public Property numCabezal As Integer
        Get
            Return _numCabezal
        End Get
        Set(value As Integer)
            _numCabezal = value
        End Set
    End Property
    Public Property habilitado As Boolean
        Get
            Return _habilitado
        End Get
        Set(value As Boolean)
            _habilitado = value
            RaiseEvent setHeadParametroBD("ISENABLED", value.ToString.ToUpper, LogicalHeadID)
        End Set
    End Property
    Public Property anuladosInicio As Integer
        Get
            Return _anuladosInicio
        End Get
        Set(value As Integer)
            _anuladosInicio = value
            RaiseEvent setHeadParametroBD("DISABLEDNOZZLESINI", value, LogicalHeadID)
        End Set
    End Property
    Public Property anuladosFin As Integer
        Get
            Return _anuladosFin
        End Get
        Set(value As Integer)
            _anuladosFin = value
            RaiseEvent setHeadParametroBD("DISABLEDNOZZLESFIN", value, LogicalHeadID)
        End Set
    End Property
    Public Property interrowOffsetDesviation As Integer
        Get
            Return _interrowOffsetDesviation
        End Get
        Set(value As Integer)
            _interrowOffsetDesviation = value
            RaiseEvent setHeadParametroBD("INTERROWDSESV", value, LogicalHeadID)
        End Set
    End Property
    Public Property LogicalHeadID As Integer
        Get
            Return _logicalHeadID
        End Get
        Set(value As Integer)
            _logicalHeadID = value
            RaiseEvent setHeadParametroBD("LOGICALHEADID", value, LogicalHeadID)

        End Set
    End Property
    Public Property CarriagePort As Integer
        Get
            Return _carriagePort
        End Get
        Set(value As Integer)
            _carriagePort = value
            RaiseEvent setHeadParametroBD("CARRIAGEPORT", value, LogicalHeadID)
        End Set
    End Property
    Public Property CarriageSubport As Integer
        Get
            Return _carriageSubport
        End Get
        Set(value As Integer)
            _carriageSubport = value
            RaiseEvent setHeadParametroBD("CARRIAGESUBPORT", value, LogicalHeadID)
        End Set
    End Property
    Public Property SerialNumber() As String
        Get
            Return _serialNumber
        End Get
        Set(value As String)
            _serialNumber = value
            RaiseEvent setHeadParametroBD("SERIALNUMBER", value, LogicalHeadID)
        End Set
    End Property
    Public Property Estado() As TypesIB.enumEstadosCabezalRobot
        Get
            Return _estado
        End Get
        Set(value As TypesIB.enumEstadosCabezalRobot)
            _estado = value
        End Set
    End Property
    Public Property WaveformPath As String
        Get
            Return _waveformPath
        End Get
        Set(value As String)
            _waveformPath = value
            RaiseEvent setHeadParametroBD("WAVEFORMPATH", value, LogicalHeadID)
        End Set
    End Property
    Public Property TipoCabezal As TypesIB.enumTipoCabezal
        Get
            Return _tipoCabezal
        End Get
        Set(value As TypesIB.enumTipoCabezal)
            _tipoCabezal = value
        End Set
    End Property
    Public Property Mirrored As Boolean
        Get
            Return _mirrored
        End Get
        Set(value As Boolean)
            _mirrored = value
            RaiseEvent setHeadParametroBD("MIRRORED", value.ToString.ToUpper, LogicalHeadID)
        End Set
    End Property
    Public Property FireOrder As Boolean
        Get
            Return _fireOrder
        End Get
        Set(value As Boolean)
            _fireOrder = value
            RaiseEvent setHeadParametroBD("FIREORDER", value.ToString.ToUpper, LogicalHeadID)
        End Set
    End Property






    Public Event setHeadParametroBD(ByVal parametro As String, ByVal valor As String, ByVal logicalHeadId As Integer)
    Public Sub New(nc As Integer)

        _listaRows = New List(Of clsInfoRow)

        'AddHandler _infoBarra.listaCabezales(nHead - 1).setHeadParametroBD, AddressOf HEAD_setParametro

        _numCabezal = nc
        _alineamientoX = 0
        _alineamientoY = 0
        _numRows = 2
        _numTrims = 1
        _voltajeMedio = 20.1
        _anuladosFin = 0
        _anuladosInicio = 0
        _interrowOffsetDesviation = 0
        LogicalHeadID = -1
        CarriagePort = -1
        CarriageSubport = -1
        SerialNumber = ""

    End Sub
    Public Sub Dispose()
        For Each auxRow As clsInfoRow In _listaRows
            auxRow = Nothing
        Next
        _listaRows = Nothing

        GC.SuppressFinalize(Me)
    End Sub
    Public Sub Row_setParametro(ByVal parametro As String, ByVal valorLeido As String)
        RaiseEvent setHeadParametroBD(parametro, valorLeido, LogicalHeadID)
    End Sub
End Class

<Serializable()> Public Class clsInfoRow
    Private _rutaWaveform As String
    Private _numeroWaveform As Integer
    Private _numTrims As Integer
    Private _listaTrims As List(Of Double)
    Private _numRow As Integer

    Public Property rutaWaveform As String
        Get
            Return _rutaWaveform
        End Get
        Set(value As String)
            _rutaWaveform = value
        End Set
    End Property
    Public Property numeroWaveform As Integer
        Get
            Return _numeroWaveform
        End Get
        Set(value As Integer)
            _numeroWaveform = value
        End Set
    End Property
    Public Property numTrims As Integer
        Get
            Return _numTrims
        End Get
        Set(value As Integer)
            _numTrims = value
            While _listaTrims.Count < _numTrims
                _listaTrims.Add(20.0)
            End While
            While _listaTrims.Count > _numTrims
                _listaTrims.RemoveAt(_listaTrims.Count - 1)
            End While
        End Set
    End Property

    Public Property listaTrims(indice As Integer) As Double
        Get
            Return _listaTrims(indice)
        End Get
        Set(value As Double)
            _listaTrims(indice) = value
            RaiseEvent SetRowParametroBD("ROW" & numRow + 1 & "." & "TRIM" & indice + 1, value.ToString.ToUpper)
        End Set
    End Property
    Public Property listaTrims As List(Of Double)
        Get
            Return _listaTrims
        End Get
        Set(value As List(Of Double))
            _listaTrims = value
           
        End Set
    End Property
    Public Property numRow As Integer
        Get
            Return _numRow
        End Get
        Set(value As Integer)
            _numRow = value
        End Set
    End Property
    Public Event SetRowParametroBD(ByVal parametro As String, ByVal valor As String)
    Public Sub New(nr As Integer, nt As Integer)
        _numRow = nr

        _listaTrims = New List(Of Double)

        _rutaWaveform = "C:\GIS\Waveforms"
        _numeroWaveform = 1

        numTrims = nt

    End Sub
    Public Sub Dispose()
        For Each _trim As Short In _listaTrims
            _trim = Nothing
        Next
        _listaTrims = Nothing

        GC.SuppressFinalize(Me)
    End Sub
End Class
