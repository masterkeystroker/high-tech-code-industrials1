<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.txtMaquina = New System.Windows.Forms.TextBox()
        Me.txtLicencia = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtMatricula = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtTiempo = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Fiery = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'txtMaquina
        '
        Me.txtMaquina.Location = New System.Drawing.Point(12, 25)
        Me.txtMaquina.Multiline = True
        Me.txtMaquina.Name = "txtMaquina"
        Me.txtMaquina.Size = New System.Drawing.Size(599, 116)
        Me.txtMaquina.TabIndex = 0
        '
        'txtLicencia
        '
        Me.txtLicencia.BackColor = System.Drawing.Color.White
        Me.txtLicencia.Location = New System.Drawing.Point(12, 228)
        Me.txtLicencia.Multiline = True
        Me.txtLicencia.Name = "txtLicencia"
        Me.txtLicencia.ReadOnly = True
        Me.txtLicencia.Size = New System.Drawing.Size(599, 116)
        Me.txtLicencia.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(163, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Código generado por la máquina:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 212)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(192, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Código con la licencia para la máquina:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 150)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Matrícula:"
        '
        'txtMatricula
        '
        Me.txtMatricula.BackColor = System.Drawing.Color.White
        Me.txtMatricula.Location = New System.Drawing.Point(75, 147)
        Me.txtMatricula.Name = "txtMatricula"
        Me.txtMatricula.ReadOnly = True
        Me.txtMatricula.Size = New System.Drawing.Size(100, 20)
        Me.txtMatricula.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.txtMatricula, "Matrícula de la máquina")
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(14, 176)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Tiempo:"
        '
        'txtTiempo
        '
        Me.txtTiempo.Location = New System.Drawing.Point(75, 173)
        Me.txtTiempo.Name = "txtTiempo"
        Me.txtTiempo.Size = New System.Drawing.Size(100, 20)
        Me.txtTiempo.TabIndex = 7
        Me.txtTiempo.Text = "30"
        Me.ToolTip1.SetToolTip(Me.txtTiempo, "Tiempo, en días, de duración de la licencia desde que se instale en máquina")
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Location = New System.Drawing.Point(221, 173)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker1.TabIndex = 8
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(181, 173)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(34, 20)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "<="
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(178, 150)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(69, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Numero serie"
        '
        'Fiery
        '
        Me.Fiery.AutoSize = True
        Me.Fiery.Location = New System.Drawing.Point(356, 150)
        Me.Fiery.Name = "Fiery"
        Me.Fiery.Size = New System.Drawing.Size(100, 17)
        Me.Fiery.TabIndex = 11
        Me.Fiery.Text = "Fiery Disponible"
        Me.Fiery.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(623, 353)
        Me.Controls.Add(Me.Fiery)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.txtTiempo)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtMatricula)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtLicencia)
        Me.Controls.Add(Me.txtMaquina)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Generador licencias CC3"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtMaquina As System.Windows.Forms.TextBox
    Friend WithEvents txtLicencia As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtMatricula As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtTiempo As System.Windows.Forms.TextBox
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Fiery As System.Windows.Forms.CheckBox

End Class
