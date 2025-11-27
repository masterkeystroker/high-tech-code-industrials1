using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GuiPlcPlotter
{
    public partial class Input : Form
    {
        private Creta.libMbOpc.cTAD Tad;

        public Input(Creta.libMbOpc.cTAD TAD)
        {
            Tad = TAD;
            InitializeComponent();
        }

        private void Input_Load(object sender, EventArgs e)
        {
            textBox1.Text = Tad.Valor.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Tad.Valor = textBox1.Text;
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
