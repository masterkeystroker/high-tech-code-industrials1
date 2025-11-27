using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PLC;
using Creta.ModBus;

namespace PruebaLexium
{
    public partial class Form1 : Form
    {
        private clsOTB OTB;
        private clsLX32 LX;
        private clsMBMaestro MB;

        public Form1()
        {
            MB = new clsMBMaestro();
            MB.Velocidad = Creta.ModBus.Baudios.b38400;
            MB.StopBits = System.IO.Ports.StopBits.One;
            MB.Paridad = System.IO.Ports.Parity.Even;
            MB.Bits = 8;
            MB.Puerto = "COM1";
            MB.Conectar = true;

            LX = new clsLX32(MB, 1);
            OTB = new clsOTB(MB, 4);

            InitializeComponent();

            comboBox1.SelectedIndex = 0;

            //1-Positive Negative Home / 2-Negative Positive Home / 3-Positive Home / 4-Positive / 5-Negative Home / 6-Negative
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Positive Negative Home");
            comboBox2.Items.Add("Negative Positive Home");
            comboBox2.Items.Add("Positive Home");
            comboBox2.Items.Add("Positive");
            comboBox2.Items.Add("Negative Home");
            comboBox2.Items.Add("Negative");
            comboBox2.SelectedIndex = 0;

            //1-Direct Coupling / 2-Belt Axis / 3-Spindle Axis
            comboBox3.Items.Clear();
            comboBox3.Items.Add("Direct Coupling");
            comboBox3.Items.Add("Belt Axis");
            comboBox3.Items.Add("Spindle Axis");
            comboBox3.SelectedIndex = 2;

            //0-Terminate / 1-Activate EasyTuning / 2-Activate ComfortTuning
            comboBox4.Items.Clear();
            comboBox4.Items.Add("Activate EasyTuning");
            comboBox4.Items.Add("Activate ComfortTuning");
            comboBox3.SelectedIndex = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = LX.velocidadRapidaJog.ToString();
            textBox2.Text = LX.velocidadLentaJog.ToString();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Dispose();
            OTB.Dispose();
            LX.Dispose();
            MB.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LX.Scan();

            if (MB.Conectar) pictureBox1.BackColor = Color.Green; else pictureBox1.BackColor = Color.Red;
            if (LX.Inicializado) pictureBox2.BackColor = Color.Green; else pictureBox2.BackColor = Color.Red;
            if (LX.Referenciado) pictureBox3.BackColor = Color.Green; else pictureBox3.BackColor = Color.Red;
            if (LX.moviendoJog) button1.BackColor = Color.Green; else button1.BackColor = Color.Red;
            if (LX.direccionJog) button2.Text = "<< Direccion"; else button2.Text = "Direccion >>";
            if (LX.velocidadJog) button3.BackColor = Color.Red; else button3.BackColor = Color.Green;
            if (LX.CWHALT) button7.BackColor = Color.Green; else button7.BackColor = Color.Red;
            if (LX.CWStop) button8.BackColor = Color.Green; else button8.BackColor = Color.Red;
            if (LX.Alarma) pictureBox4.BackColor = Color.Red; else pictureBox4.BackColor = Color.Gray;
            if (OTB.CalidadOk) pictureBox5.BackColor = Color.Green; else pictureBox5.BackColor = Color.Red;
            
            
            //textBox4.Text = LX.Paso.ToString();
            textBox5.Text = LX.AT_progress.ToString();
            textBox12.Text = LX.PosicionActual.ToString();
            textBox13.Text = LX.Estado.ToString();
            textBox14.Text = LX.LastError.ToString();
            textBox15.Text = LX.ContTx.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LX.moviendoJog = !LX.moviendoJog;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            LX.direccionJog = !LX.direccionJog;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            LX.velocidadJog = !LX.velocidadJog;
        }
  
        private void button6_Click(object sender, EventArgs e)
        {
            LX.Posicionar(Convert.ToDouble(textBox7.Text.Replace(".", ",")), Convert.ToDouble(textBox6.Text.Replace(".", ",")));
        }
        private void button7_Click(object sender, EventArgs e)
        {
            LX.CWHALT = !LX.CWHALT;
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                LX.velocidadRapidaJog = Convert.ToDouble(textBox1.Text.Replace(".", ","));
                EP.SetError((Control)sender, "");
            }
            catch
            {
                e.Cancel = true;
                EP.SetError((Control)sender, "Error en el valor");
            }
        }
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                LX.velocidadLentaJog = Convert.ToDouble(textBox2.Text.Replace(".",","));
                EP.SetError((Control)sender, "");
            }
            catch
            {
                e.Cancel = true;
                EP.SetError((Control)sender, "Error en el valor");
            }
        }
        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                LX.HomingPosicion = Convert.ToDouble(textBox3.Text.Replace(".", ","));
                EP.SetError((Control)sender, "");
            }
            catch
            {
                e.Cancel = true;
                EP.SetError((Control)sender, "Error en el valor");
            }
        }
        private void textBox7_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //LX.PosVelocidad = Convert.ToDouble(textBox7.Text.Replace(".", ","));
                EP.SetError((Control)sender, "");
            }
            catch
            {
                e.Cancel = true;
                EP.SetError((Control)sender, "Error en el valor");
            }
        }
        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //LX.PosDestino = Convert.ToDouble(textBox6.Text.Replace(".", ","));
                EP.SetError((Control)sender, "");
            }
            catch
            {
                e.Cancel = true;
                EP.SetError((Control)sender, "Error en el valor");
            }
        }
        private void textBox8_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                LX.CTRL_GlobGain = Convert.ToSingle(textBox8.Text.Replace(".", ","));
                EP.SetError((Control)sender, "");
            }
            catch
            {
                e.Cancel = true;
                EP.SetError((Control)sender, "Error en el valor");
            }
        }
        private void textBox9_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                LX.AT_TolVelocidadAuto = Convert.ToInt32(textBox9.Text.Replace(".", ","));
                EP.SetError((Control)sender, "");
            }
            catch
            {
                e.Cancel = true;
                EP.SetError((Control)sender, "Error en el valor");
            }
        }
        private void textBox10_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                LX.AT_n_ref = Convert.ToInt32(textBox10.Text.Replace(".", ","));
                EP.SetError((Control)sender, "");
            }
            catch
            {
                e.Cancel = true;
                EP.SetError((Control)sender, "Error en el valor");
            }
        }
        private void textBox11_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                LX.AT_dis = Convert.ToInt32(textBox11.Text.Replace(".", ","));
                EP.SetError((Control)sender, "");
            }
            catch
            {
                e.Cancel = true;
                EP.SetError((Control)sender, "Error en el valor");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            LX.CWStop = !LX.CWStop;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            LX.Reset();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox5.SelectedIndex)
            {
                case 0:
                    LX.Modo = eEstadoLX32Deseado.Cero; // = eEstadoLX32Deseado.Cero;
                    break;
                case 1:
                    LX.Modo = eEstadoLX32Deseado.Jog;
                    break;
                case 2:
                    LX.Modo = eEstadoLX32Deseado.Posicionado;
                    break;
                case 3:
                    LX.Modo = eEstadoLX32Deseado.Autotune;
                    break;
                case 4:
                    LX.Modo = eEstadoLX32Deseado.Homing;
                    break;
            }


        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            LX.AT_start = comboBox4.SelectedIndex + 1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LX.AT_dir = comboBox2.SelectedIndex + 1;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            LX.AT_mechanical = comboBox3.SelectedIndex + 1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LX.HomingModo = comboBox1.SelectedIndex;
        }









    

    }
}
