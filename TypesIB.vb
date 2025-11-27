Imports Creta.CC3.infoBarra
'Imports Creta.libMbOpc
'Imports Creta.CC3.CC3Types
Public Class TypesIB
    Public Shared Function toEnumTipoCabezal(tipo As String) As enumTipoCabezal
        'RCP 2014 04 08 Traduce el tipo de cabezal antiguo a la nueva nomenclatura
        Select Case tipo.ToUpper
            Case enumTipoCabezal.T.ToString, "T1"
                Return enumTipoCabezal.T
            Case enumTipoCabezal.X.ToString, "X1", "X2"
                Return enumTipoCabezal.X
            Case enumTipoCabezal.XGS40.ToString
                Return enumTipoCabezal.XGS40
            Case enumTipoCabezal.D.ToString, "D1", "D2"
                Return enumTipoCabezal.D
            Case Else
                Throw New Exception("Unknown printhead type: " & tipo)
        End Select
    End Function
    Public Shared Function toEnumTipoBarra(tipo As String) As enumTipoBarra
        Select Case tipo.ToUpper
            Case enumTipoBarra.DECORACION.ToString
                Return enumTipoBarra.DECORACION
            Case enumTipoBarra.ESPESORADA.ToString
                Return enumTipoBarra.ESPESORADA
            Case Else
                Throw New Exception("Unknown bar type: " & tipo)
        End Select
    End Function
    Public Shared Function toEnumUnionPMB(valor As String) As enumUnionPMB
        Select Case valor
            Case enumUnionPMB.Unir01.ToString
                Return enumUnionPMB.Unir01
            Case enumUnionPMB.Unir12.ToString
                Return enumUnionPMB.Unir12
            Case enumUnionPMB.Unir012.ToString
                Return enumUnionPMB.Unir012
            Case Else
                Return Nothing
        End Select
    End Function
    Public Shared Function toEnumEstadosCabezalRobot(valor As String) As enumEstadosCabezalRobot
        Select Case valor
            Case enumEstadosCabezalRobot.Faulted.ToString
                Return enumEstadosCabezalRobot.Faulted
            Case enumEstadosCabezalRobot.Loading.ToString
                Return enumEstadosCabezalRobot.Loading
            Case enumEstadosCabezalRobot.NotReady
                Return enumEstadosCabezalRobot.NotReady
            Case enumEstadosCabezalRobot.Ready
                Return enumEstadosCabezalRobot.Ready
            Case Else
                Return Nothing
        End Select
    End Function

    Public Shared Function toEnumTipoElectronica(valor As String) As enumTipoElectronica
        valor = valor.ToUpper()
        Select Case valor
            Case enumTipoElectronica.GIS.ToString
                Return enumTipoElectronica.GIS
            Case enumTipoElectronica.ROBOT.ToString
                Return enumTipoElectronica.ROBOT
            Case Else
                Return Nothing
        End Select
    End Function
    Public Shared Function toEnumPosPrimerCabezal(valor As String) As enumPosPrimerCabezal
        valor = valor.ToUpper()
        Select Case valor
            Case enumPosPrimerCabezal.Derecha.ToString
                Return enumPosPrimerCabezal.Derecha
            Case enumPosPrimerCabezal.Izquierda.ToString
                Return enumPosPrimerCabezal.IZQUIERDA
            Case Else
                Return Nothing
        End Select
    End Function

    Public Shared Function calculateMask(sMask As String) As Integer
        Dim nMask As Integer = -1
        'Se calcula el número entero del binario resultante de la cadena.
        For i As Integer = 0 To sMask.Length - 1
            If sMask(i) = "1" Then
                Dim prueba As Integer = Math.Pow(2, 7)
                nMask += Math.Pow(2, (sMask.Length - (i + 1)))
            ElseIf sMask(i) = "0" Then
                nMask += 0
            Else
                Return -1
            End If
        Next
        Return nMask
    End Function

    Public Enum enumTipoBarra
        DECORACION
        ESPESORADA
    End Enum
    Public Enum enumTipoCabezal
        'VSA (10/04/2015): El enumerado de tipo de cabezal está replicado en el GUI para evitar problemas entre los distintos kernel. 
        '       Si se desea añadir/modificar/eliminar algún tipo de cabezal hay que revisar el mismo enumerado del GUI (CretaTypes) y del resto 
        '       de proyectos de Kernel de C3.
        X = 0
        T = 1
        D = 2
        XGS40 = 3
        S = 4
    End Enum
    Public Enum enumUnionPMB 'Indica como se deben unir las PMBs cuando hay 3 o 4
        Unir01 'Las PMBs se unen de dos en dos. Si hay tres, las dos primeras van juntas y la tercera sola
        Unir12 'La primera PMB va sola, y las siguientes dos o tres van juntas.
        Unir012 'Las tres PMBs van juntas, la cuarta, si existe, va sola.
    End Enum

    Public Enum enumTipoElectronica
        GIS
        ROBOT
    End Enum

    Public Enum enumNumeroLinea
        L1
        L2
    End Enum

    Public Enum enumEstadosCabezalRobot
        Loading
        Ready
        NotReady
        Faulted
    End Enum

    Public Enum enumPosPrimerCabezal
        DERECHA
        IZQUIERDA
    End Enum

End Class


