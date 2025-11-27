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
    public partial class InputBit : Form
    {
        private Creta.libMbOpc.cTAD Tad;

        public InputBit(Creta.libMbOpc.cTAD TAD)
        {
            InitializeComponent();
            Tad = TAD;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (Tad.Calidad == Creta.libMbOpc.eCalidades.OK)
                {
                    Tad.Valor = !Convert.ToBoolean(Tad.Valor);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Calidad no es OK");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Tad.Calidad == Creta.libMbOpc.eCalidades.OK)
                {
                    Tad.Valor = false;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Calidad no es OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Tad.Calidad == Creta.libMbOpc.eCalidades.OK)
                {
                    Tad.Valor = 1;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Calidad no es OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
