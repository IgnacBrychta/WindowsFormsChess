using System;
using System.Windows.Forms;

namespace FormsChess
{
    public partial class Form2 : Form
    {
        public string jmeno = string.Empty;
        bool eraseString = true;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            jmeno = eraseString ? string.Empty : jmeno;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            jmeno = textBox1.Text;
            eraseString = false;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            jmeno = textBox1.Text;
            if (e.KeyCode == Keys.Enter)
            {
                eraseString = false;
                this.Close();
            }
        }
    }
}