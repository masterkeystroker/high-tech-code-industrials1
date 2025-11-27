Imports System.Runtime.InteropServices
Imports System.Drawing

Public Module Fiery

    <DllImport("libWarhol64.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi, EntryPoint:="Unpack_XF_Package")> _
    Private Function Unpack_XF_Package64(ByVal Package_name As String, ByVal Output_file_name As String, ByVal Key As String, ByVal Key_length As Integer) As Integer
    End Function

    <DllImport("libWarhol64.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi, EntryPoint:="Get_Preview")> _
    Private Function Get_Preview64(ByVal Package_name As String, ByVal Preview_file_name As String, ByRef width As Integer, ByRef height As Integer, ByRef res_x As Double, ByRef res_y As Double, ByRef number_of_channels As Integer, ByRef halftone As Integer, ByRef rotation As Integer) As Integer
    End Function

    <DllImport("libWarhol32.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi, EntryPoint:="Unpack_XF_Package")> _
    Private Function Unpack_XF_Package32(ByVal Package_name As String, ByVal Output_file_name As String, ByVal Key As String, ByVal Key_length As Integer) As Integer
    End Function

    <DllImport("libWarhol32.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi, EntryPoint:="Get_Preview")> _
    Private Function Get_Preview32(ByVal Package_name As String, ByVal Preview_file_name As String, ByRef width As Integer, ByRef height As Integer, ByRef res_x As Double, ByRef res_y As Double, ByRef number_of_channels As Integer, ByRef halftone As Integer, ByRef rotation As Integer) As Integer
    End Function

    Public Function DesencriptarTiff(RutaOrigen As String, RutaDestino As String) As Boolean
        Try
            Dim iRes As Integer = 0

            If Environment.Is64BitProcess Then
                iRes = Unpack_XF_Package64(RutaOrigen, RutaDestino, "", 0)
            Else
                iRes = Unpack_XF_Package32(RutaOrigen, RutaDestino, "", 0)
            End If

            If iRes = 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GenerarPreview(RutaOrigen As String, RutaDestino As String, ByRef Width As Integer, ByRef Height As Integer, ByRef Channels As Integer, ByRef halftone As Integer, ByRef ResX As Integer, ByRef ResY As Integer) As Boolean
        Try
            Dim iRes As Integer = 0
            Dim dResX As Double = 1
            Dim dResY As Double = 1
            Dim iWidth As Integer = 1
            Dim Rotacion As Integer = 0
            Height = 1
            Channels = 1

            If Environment.Is64BitProcess Then
                iRes = Get_Preview64(RutaOrigen, RutaDestino, iWidth, Height, dResX, dResY, Channels, halftone, Rotacion)
            Else
                iRes = Get_Preview32(RutaOrigen, RutaDestino, iWidth, Height, dResX, dResY, Channels, halftone, Rotacion)
            End If

            'Porque la funcion devuelve el numero de bytes de ancho, no pixeles
            Width = CInt(iWidth / Channels)
            ResX = CInt(dResX)
            ResY = CInt(dResY)

            If iRes = 0 Then
                'Rota la imegen
                If Rotacion = 90 Then
                    Dim Img As Image = Bitmap.FromFile(RutaDestino)
                    Img.RotateFlip(RotateFlipType.Rotate90FlipNone)
                    Img.Save(RutaDestino)
                    Img.Dispose()

                ElseIf Rotacion = 180 Then
                    Dim Img As Image = Bitmap.FromFile(RutaDestino)
                    Img.RotateFlip(RotateFlipType.Rotate180FlipNone)
                    Img.Save(RutaDestino)
                    Img.Dispose()

                ElseIf Rotacion = 270 Then
                    Dim Img As Image = Bitmap.FromFile(RutaDestino)
                    Img.RotateFlip(RotateFlipType.Rotate270FlipNone)
                    Img.Save(RutaDestino)
                    Img.Dispose()

                End If

                Return True
            Else
                Width = 0
                Height = 0
                Channels = 0
                ResX = 0
                ResY = 0
                Return False
            End If
        Catch ex As Exception
            Width = 0
            Height = 0
            Channels = 0
            ResX = 0
            ResY = 0
            Return False
        End Try
    End Function







    <DllImport("libWarhol64.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi, EntryPoint:="Build_XF_Package")> _
    Public Function Build_XF_Package64(ByVal OriginalRPF As String, ByVal New_Package_Name As String, ByVal Tiff_file_name As String, ByVal Key As String, ByVal KeyLen As Integer) As Integer
    End Function

    <DllImport("libWarhol32.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi, EntryPoint:="Build_XF_Package")> _
    Public Function Build_XF_Package32(ByVal OriginalRPF As String, ByVal New_Package_Name As String, ByVal Tiff_file_name As String, ByVal Key As String, ByVal KeyLen As Integer) As Integer
    End Function

    Public Function EncriptarTiff(ByVal OriginalRPF As String, ByVal New_Package_Name As String, ByVal Tiff_file_name As String) As Boolean
        Try
            Dim sOrigin As String = OriginalRPF
            Dim sDestino As String = New_Package_Name
            Dim sTiff As String = Tiff_file_name
            Dim sKey As String = ""
            Dim iKey As Integer = 0
            Dim iRes As Integer = -1

            If Environment.Is64BitProcess Then
                iRes = Build_XF_Package64(sOrigin, sDestino, sTiff, sKey, iKey)
            Else
                iRes = Build_XF_Package32(sOrigin, sDestino, sTiff, sKey, iKey)
            End If

            If iRes = 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function


End Module
