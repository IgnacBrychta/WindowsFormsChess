using System;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FormsChess
{
    public partial class Form_nastaveni : Form
    {
        public string FENnotace = string.Empty;
        public bool atomicMode = false;
        public bool prohoditBarvy = false;
        public bool connectionToChessBoard = false;
        public bool odlisnaBilaCernaTextura = false;
        public string pawn_white_texturePath = string.Empty;
        public string rook_white_texturePath = string.Empty;
        public string knight_white_texturePath = string.Empty;
        public string bishop_white_texturePath = string.Empty;
        public string queen_white_texturePath = string.Empty;
        public string king_white_texturePath = string.Empty;
        public bool ignorovatCasomiru = false;
        public int inteligenceAI = 20;
        public bool AIkonzole = false;
        public int casovaDotace = 3;
        readonly TimeSpan sekunda = new TimeSpan(0, 0, 1);
        public TimeSpan zvolenyCasNaTah = new TimeSpan(0, 2, 0);
        public bool allowBeeping = false;
        public Form_nastaveni()
        {
            InitializeComponent();
            comboBox_casomira.SelectedIndex = 0;
            string[] portNames = SerialPort.GetPortNames();
            for (int i = 0; i < portNames.Length; i++)
            {
                comboBox_SerialPortName.Items.Add(portNames[i]);
            }
        }
        private void button_NacistFENnotaci(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Forsyth-Edwards Notation (*.fen)|*.fen",
                Title = "Zvolte soubor FEN notace",
                Multiselect = false
            };
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                MessageBox.Show("Soubor úspěšně načten", "Úspěšně načteno", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FENnotace = openFileDialog.FileName;
                textBox1.Text = openFileDialog.FileName;
            }
            else
            {
                MessageBox.Show("Vložen neplatný soubor", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            atomicMode = !atomicMode;
        }
        private void radioButton_atomic_ne_Click(object sender, EventArgs e)
        {
            atomicMode = !atomicMode;
        }
        private void button_zvolitTextury_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\textures",
                IsFolderPicker = true,
                Title = "Zvolte složku s texturami černých figurek."
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (TryUpdateTextures(dialog.FileName, out int nacteneTextury))
                {
                    MessageBox.Show($"Načteno {nacteneTextury} z celkem {(odlisnaBilaCernaTextura ? 12 : 6)} textur.", "Načteno", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Zvolená složka neobsahuje jedinou texturu, zvoleny výchozí textury.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                textBox_slozkaTextur.Text = dialog.FileName;
            }
        }
        private bool TryUpdateTextures(string folder, out int nacteneTextury)
        {
            for (int i = 0; i <= 5; i++)
            {
                checkedListBox_nacteneTextury_figurky.SetItemChecked(i, false); // reset
            }
            string[] textury = Directory.GetFiles(folder);
            nacteneTextury = 0;
            for (int i = 0; i < textury.Length; i++)
            {
                string path = textury[i];
                if (path.Contains("bishop_chess.png"))
                {
                    Bishop.texturePath = path;
                    checkedListBox_nacteneTextury_figurky.SetItemChecked(3, true);
                    nacteneTextury++;
                } 
                else if (path.Contains("king_chess.png"))
                {
                    King.texturePath = path; 
                    checkedListBox_nacteneTextury_figurky.SetItemChecked(5, true);
                    nacteneTextury++;
                }
                else if (path.Contains("knight_chess.png"))
                {
                    Knight.texturePath = path;
                    checkedListBox_nacteneTextury_figurky.SetItemChecked(2, true);
                    nacteneTextury++;
                }
                else if (path.Contains("pawn_chess.png"))
                {
                    Pawn.texturePath = path;
                    checkedListBox_nacteneTextury_figurky.SetItemChecked(0, true);
                    nacteneTextury++;
                }
                else if (path.Contains("queen_chess.png"))
                {
                    Queen.texturePath = path;
                    checkedListBox_nacteneTextury_figurky.SetItemChecked(4, true);
                    nacteneTextury++;
                }
                else if (path.Contains("rook_chess.png"))
                {
                    Rook.texturePath = path;
                    checkedListBox_nacteneTextury_figurky.SetItemChecked(1, true);
                    nacteneTextury++;
                }
                else if (path.Contains("pawn_chess_white.png") && odlisnaBilaCernaTextura)
                {
                    pawn_white_texturePath = path;
                    nacteneTextury++;
                }
                else if (path.Contains("rook_chess_white.png") && odlisnaBilaCernaTextura)
                {
                    rook_white_texturePath = path;
                    nacteneTextury++;
                }
                else if (path.Contains("knight_chess_white.png") && odlisnaBilaCernaTextura)
                {
                    knight_white_texturePath = path;
                    nacteneTextury++;
                }
                else if (path.Contains("bishop_chess_white.png") && odlisnaBilaCernaTextura)
                {
                    bishop_white_texturePath = path;
                    nacteneTextury++;
                }
                else if (path.Contains("queen_chess_white.png") && odlisnaBilaCernaTextura)
                {
                    queen_white_texturePath = path;
                    nacteneTextury++;
                }
                else if (path.Contains("king_chess_white.png") && odlisnaBilaCernaTextura)
                {
                    king_white_texturePath = path;
                    nacteneTextury++;
                }
            }
            return nacteneTextury > 0; 
        }
        private void radioButton_prohoditBarvy_click(object sender, EventArgs e)
        {
            prohoditBarvy = !prohoditBarvy;
        }
        private void radioButton4_Click(object sender, EventArgs e)
        {
            odlisnaBilaCernaTextura = !odlisnaBilaCernaTextura;
        }
        private void radioButton6_Click(object sender, EventArgs e)
        {
            ignorovatCasomiru = !ignorovatCasomiru;
        }
        private void button_AI_plus_Click(object sender, EventArgs e)
        {
            if (inteligenceAI < 20)
            {
                inteligenceAI++;
            }
            textBox_inteligenceAI.Text = inteligenceAI.ToString() + "/20";
        }
        private void button_AI_minus_Click(object sender, EventArgs e)
        {
            if (inteligenceAI > 0)
            {
                inteligenceAI--;
            }
            textBox_inteligenceAI.Text = inteligenceAI.ToString() + "/20";
        }
        private void radioButton7_Click(object sender, EventArgs e)
        {
            AIkonzole = !AIkonzole;
        }
        private void radioButton9_Click(object sender, EventArgs e)
        {
            connectionToChessBoard = !connectionToChessBoard;
            comboBox_SerialPortName.Enabled = connectionToChessBoard;
        }
        private void Form_nastaveni_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(connectionToChessBoard && string.IsNullOrEmpty((string)comboBox_SerialPortName.SelectedItem))
            {
                MessageBox.Show("Není zvolen sériový port pro komunikaci s deskou", "Neplatný port", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            casovaDotace++;
            textBox2.Text = casovaDotace.ToString() + " s";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            casovaDotace--;
            if (casovaDotace <= 0) casovaDotace = 1;
            textBox2.Text = casovaDotace.ToString() + " s";
        }
        private void comboBox_casomira_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_casomira.SelectedIndex == 1)
            {
                button3.Enabled = true;
                button2.Enabled = true;
                textBox2.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
                button2.Enabled = false;
                textBox2.Enabled = false;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            zvolenyCasNaTah += sekunda;
            textBox3.Text = zvolenyCasNaTah.ToString();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (zvolenyCasNaTah.TotalSeconds > 1)
            {
                zvolenyCasNaTah -= sekunda;
            }
            textBox3.Text = zvolenyCasNaTah.ToString();
        }
        private void radioButton11_Click(object sender, EventArgs e)
        {
            allowBeeping = !allowBeeping;
        }
    }
}