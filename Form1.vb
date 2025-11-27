
'Publicado en 
'http://srvofitec2/GeneradorLicencias/publish.htm



Public Class Form1

    Private nAleatorio As Integer = -1

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Dim U As libUsuarios.cUsuarios = Nothing

        Try
#If Not Debug Then
            U = New libUsuarios.cUsuarios

            Dim id As Integer = U.ShowLogin(Me)
            If id < 1 Then
                MsgBox("Identificación erronea")
                Me.Close()
            Else
                Dim Dep() As String = U.GetDepartamentos(id)
                Dim Enc As Boolean = False

                For Each S As String In Dep
                    If S = "GenLic" Then Enc = True
                Next

                If Not Enc Then
                    'Este usuario no pertenece a generadores de licencia
                    MsgBox("Este usuario no tiene derecho a usar el generador de licencias")
                    Me.Close()
                End If
            End If
#End If
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
            Me.Close()
        Finally
            If U IsNot Nothing Then U.Dispose()
        End Try
    End Sub


    Private Sub txtMaquina_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtMaquina.TextChanged
        Try
            'Interpreta la cadena de la máquina
            Dim _Key() As Byte = {66, 200, 125, 36, 78, 23, 26, 98, 152, 16, 215, 255, 180, 185, 46, 73, 66, 230, 25, 36, 70, 203, 26, 9, 52, 66, 15, 155, 10, 185, 46, 73}
            Dim _IV() As Byte = {66, 200, 25, 36, 78, 203, 26, 98, 152, 166, 215, 255, 1, 185, 46, 73}
            Dim Cadena As String = System.Text.Encoding.UTF8.GetString(UnZip(decrypt_AES(StringToByte(txtMaquina.Text), _Key, _IV)))
            Dim Lineas() As String = Cadena.Split(New String() {vbCrLf}, StringSplitOptions.RemoveEmptyEntries)

            Dim Ok As Boolean = True
            Dim Numero As Integer = -1
            Dim Matricula As String = ""
            Dim NumeroSerie As String = ""

            If Lineas.Length = 4 Then
                If Lineas(0) <> "GETLICENCIA_CC3" Then
                    Ok = False
                End If

                If Lineas(1).StartsWith("ALEATORIO:") Then
                    Numero = CInt(Lineas(1).Substring(10))
                Else
                    Ok = False
                End If

                If Lineas(2).StartsWith("MATRICULA:") Then
                    Matricula = Lineas(2).Substring(10)
                Else
                    Ok = False
                End If

                If Lineas(3).StartsWith("NUMEROSERIE:") Then
                    NumeroSerie = Lineas(3).Substring(12)
                Else
                    Ok = False
                End If
            Else
                'Error
                Ok = False
            End If

            If Ok Then
                'Codigo aceptado
                nAleatorio = Numero
                txtMatricula.Text = Matricula
                Label5.Text = NumeroSerie
            Else
                'Error
                txtMatricula.Text = ""
                nAleatorio = -1
            End If
        Catch ex As Exception
            txtMatricula.Text = ""
            nAleatorio = -1
        End Try

        'Recalcula la licencia
        txtTiempo_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            'Introduce la diferencia en dias en la casilla de texto
            Dim D1 As Date = New Date(DateTimePicker1.Value.Year, DateTimePicker1.Value.Month, DateTimePicker1.Value.Day)
            Dim D2 As Date = New Date(Now.Year, Now.Month, Now.Day)
            txtTiempo.Text = Fix(CType((D1 - D2), TimeSpan).TotalDays).ToString()
        Catch ex As Exception
            txtTiempo.Text = ""
        End Try
    End Sub

    Private Sub txtTiempo_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtTiempo.TextChanged, Fiery.CheckStateChanged
        Try
            Dim Dias As Integer = CInt(txtTiempo.Text)
            If Dias > 0 And nAleatorio >= 0 And txtMatricula.Text <> "" Then
                'Todo bien
                Dim Cadena As New System.Text.StringBuilder
                Cadena.AppendLine("SETLICENCIA_CC3")
                Cadena.AppendLine("ALEATORIO:" & nAleatorio.ToString)
                Cadena.AppendLine("MATRICULA:" & txtMatricula.Text)
                Cadena.AppendLine("NUMEROSERIE:" & Label5.Text)
                Cadena.AppendLine("TIEMPO:" & Dias.ToString)
                Cadena.AppendLine("FIERY:" & Fiery.Checked.ToString)

                Dim _Key() As Byte = {66, 200, 125, 36, 78, 23, 26, 98, 152, 16, 215, 255, 180, 185, 46, 73, 66, 230, 25, 36, 70, 203, 26, 9, 52, 66, 15, 155, 10, 185, 46, 73}
                Dim _IV() As Byte = {66, 200, 25, 36, 78, 203, 26, 98, 152, 166, 215, 255, 1, 185, 46, 73}
                txtLicencia.Text = ByteToString(encrypt_AES(Zip(System.Text.Encoding.UTF8.GetBytes(Cadena.ToString)), _Key, _IV))
            Else
                'Numero no válido
                txtLicencia.Text = ""
            End If
        Catch ex As Exception
            txtLicencia.Text = ""
        End Try
    End Sub
    Private Sub DateTimePicker1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles DateTimePicker1.ValueChanged
        Button1_Click(Nothing, Nothing)
    End Sub


#Region "Encript"

    Private Shared Function encrypt_AES(ByVal Matriz() As Byte, ByVal Key() As Byte, ByVal IV() As Byte) As Byte()

        ' Check arguments.
        If Matriz Is Nothing OrElse Matriz.Length <= 0 Then Throw New ArgumentNullException("Cadena")
        If Key Is Nothing OrElse Key.Length <> 32 Then Throw New ArgumentNullException("Key")
        If IV Is Nothing OrElse IV.Length <> 16 Then Throw New ArgumentNullException("VI")

        ' Declare the streams used to encrypt to an in memory array of bytes.
        Dim msEncrypt As System.IO.MemoryStream = Nothing
        Dim csEncrypt As System.Security.Cryptography.CryptoStream = Nothing
        Dim swEncrypt As System.IO.BinaryWriter = Nothing

        ' Declare the RijndaelManaged object used to encrypt the data.
        Dim aesAlg As System.Security.Cryptography.RijndaelManaged = Nothing

        ' Declare the bytes used to hold the encrypted data.
        Dim encrypted As Byte() = Nothing

        Try
            ' Create a RijndaelManaged object with the specified key and IV.
            aesAlg = New System.Security.Cryptography.RijndaelManaged()
            aesAlg.Key = Key
            aesAlg.IV = IV

            ' Create a decrytor to perform the stream transform.
            Dim encryptor As System.Security.Cryptography.ICryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)

            ' Create the streams used for encryption.
            msEncrypt = New System.IO.MemoryStream()
            csEncrypt = New System.Security.Cryptography.CryptoStream(msEncrypt, encryptor, System.Security.Cryptography.CryptoStreamMode.Write)
            swEncrypt = New System.IO.BinaryWriter(csEncrypt)

            'Write all data to the stream.
            swEncrypt.Write(Matriz)

        Finally
            ' Clean things up.
            ' Close the streams.
            If Not (swEncrypt Is Nothing) Then swEncrypt.Close()
            If Not (csEncrypt Is Nothing) Then csEncrypt.Close()
            If Not (msEncrypt Is Nothing) Then msEncrypt.Close()

            ' Clear the RijndaelManaged object.
            If Not (aesAlg Is Nothing) Then aesAlg.Clear()
        End Try

        ' Return the encrypted bytes from the memory stream.
        Return msEncrypt.ToArray()

    End Function
    Private Shared Function decrypt_AES(ByVal Encripted() As Byte, ByVal Key() As Byte, ByVal IV() As Byte) As Byte()
        ' Check arguments.
        If Encripted Is Nothing OrElse Encripted.Length <= 0 Then Throw New ArgumentNullException("Encripted")
        If Key Is Nothing OrElse Key.Length <> 32 Then Throw New ArgumentNullException("Key")
        If IV Is Nothing OrElse IV.Length <> 16 Then Throw New ArgumentNullException("VI")

        ' TDeclare the streams used to decrypt to an in memory array of bytes.
        Dim msDecrypt As System.IO.MemoryStream = Nothing
        Dim csDecrypt As System.Security.Cryptography.CryptoStream = Nothing
        Dim srDecrypt As System.IO.BinaryReader = Nothing

        ' Declare the RijndaelManaged object used to decrypt the data.
        Dim aesAlg As System.Security.Cryptography.RijndaelManaged = Nothing

        Dim Lista As New List(Of Byte)
        Dim Buf(5000) As Byte

        Try
            ' Create a RijndaelManaged object with the specified key and IV.
            aesAlg = New System.Security.Cryptography.RijndaelManaged()
            aesAlg.Key = Key
            aesAlg.IV = IV

            ' Create a decrytor to perform the stream transform.
            Dim decryptor As System.Security.Cryptography.ICryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV)

            ' Create the streams used for decryption.
            msDecrypt = New System.IO.MemoryStream(Encripted)
            csDecrypt = New System.Security.Cryptography.CryptoStream(msDecrypt, decryptor, System.Security.Cryptography.CryptoStreamMode.Read)
            srDecrypt = New System.IO.BinaryReader(csDecrypt)

            Do
                Dim i As Integer = srDecrypt.Read(Buf, 0, Buf.Length)
                If i = 0 Then
                    Exit Do
                Else
                    For x As Integer = 0 To i - 1
                        Lista.Add(Buf(x))
                    Next
                End If
            Loop
            Return Lista.ToArray

        Catch ex As Exception
            Return Nothing
        Finally
            ' Clean things up.
            ' Close the streams.
            If Not (srDecrypt Is Nothing) Then srDecrypt.Close()
            If Not (csDecrypt Is Nothing) Then csDecrypt.Close()
            If Not (msDecrypt Is Nothing) Then msDecrypt.Close()

            ' Clear the RijndaelManaged object.
            If Not (aesAlg Is Nothing) Then aesAlg.Clear()
        End Try
    End Function

    Private Shared Function ByteToString(Datos() As Byte) As String
        Dim Cadena As New System.Text.StringBuilder
        For Each b As Byte In Datos
            Cadena.Append(b.ToString("X2"))
        Next
        Return Cadena.ToString()
    End Function
    Private Shared Function StringToByte(Datos As String) As Byte()
        Dim lst As New List(Of Byte)
        For i As Integer = 0 To Datos.Length - 2 Step 2
            lst.Add(Byte.Parse(Datos.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))
        Next
        Return lst.ToArray()
    End Function

    Private Shared Function Zip(value() As Byte) As Byte()
        'Prepare for compress
        Dim ms As New System.IO.MemoryStream()
        Dim sw As New System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress)

        'Compress
        sw.Write(value, 0, value.Length)
        sw.Close()

        'Transform byte[] zip data to string
        Dim byteArray() As Byte = ms.ToArray()
        ms.Close()
        sw.Dispose()
        ms.Dispose()
        Return byteArray
    End Function
    Private Shared Function UnZip(value() As Byte) As Byte()

        'Prepare for decompress
        Dim ms As New System.IO.MemoryStream(value)
        Dim sr As New System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress)

        'Reset variable to collect uncompressed result
        Dim Respuesta As New List(Of Byte)

        'Decompress
        While True
            Dim Datos(4095) As Byte
            Dim rByte As Integer = sr.Read(Datos, 0, Datos.Length)

            For i As Integer = 0 To rByte - 1
                Respuesta.Add(Datos(i))
            Next

            If rByte < Datos.Length Then Exit While
        End While

        sr.Close()
        ms.Close()
        sr.Dispose()
        ms.Dispose()
        Return Respuesta.ToArray()
    End Function

#End Region

End Class
