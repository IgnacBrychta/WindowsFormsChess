using System;
using System.Windows.Forms;

namespace FormsChess
{
    public partial class ChoosePawnAlternativeWindow : Form
    {
        bool figurkaZvolena = false;
        public ChoosePawnAlternativeWindow()
        {
            InitializeComponent();
        }

        private void Form2_choosePawnAlternative_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !figurkaZvolena;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            figurkaZvolena = true;
        }
    }
}