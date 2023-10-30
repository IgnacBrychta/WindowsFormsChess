using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace FormsChess
{
    public partial class Form1 : Form
    {
        private void NCBbutton_exportNotation_Click(object sender, EventArgs e)
        {
            bool bylCasovacSpusten = timer1.Enabled;
            if (casomira != Casomira.VYCHOZI)
            {
                Casomira_Stop();
            }
            else
            {
                timer1.Stop();
            }
            NotationExportWindow format = new NotationExportWindow();
            DialogResult notaceResult = format.ShowDialog();
            if (notaceResult == DialogResult.OK)
            {
                ExportNotace_PGN();
            }
            if (notaceResult == DialogResult.Yes)
            {
                ExportNotace_PGN();
                ExportNotace_FEN();
            }
            else if(notaceResult == DialogResult.No)
            {
                ExportNotace_FEN();
            }

            
            if (casomira != Casomira.VYCHOZI)
            {
                Casomira_Start();
            }
            else
            {
                if (casomiraPermanentneZastavena && bylCasovacSpusten)
                {
                    timer1.Start();
                }
            }
        }
        private void ExportNotace_FEN()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = "export notace.fen",
                Filter = "Forsyth-Edwards Notation (*.fen)|*.fen",
                AddExtension = true,
                Title = "Zvolte lokaci a název výstupního souboru."
            };

            var result = sfd.ShowDialog();

            switch (result)
            {
                case DialogResult.OK:
                    if (UlozitFENnotaci(sfd.FileName))
                    {
                        MessageBox.Show("Export proběhl úspěšně.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (otevritSouborNotacePoUlozeni)
                        {
                            Process.Start(sfd.FileName);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Export se nezdařil.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case DialogResult.Cancel:
                    break;
            }
        }
        private void ExportNotace_PGN()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = "export notace.pgn",
                Filter = "Portable Game Notation (*.pgn)|*.pgn",
                AddExtension = true,
                Title = "Zvolte lokaci a název výstupního souboru."
            };

            var result = sfd.ShowDialog();

            switch (result)
            {
                case DialogResult.OK:
                    if (UlozitPGNnotaci(sfd.FileName))
                    {
                        MessageBox.Show("Export proběhl úspěšně.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (otevritSouborNotacePoUlozeni)
                        {
                            Process.Start(sfd.FileName);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Export se nezdařil.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case DialogResult.Cancel:
                    break;
            }
        }
        private bool UlozitFENnotaci(string directory)
        {
            try
            {
                FileStream fs = new FileStream(directory, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(ZiskatFenNotaci());
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        private bool UlozitPGNnotaci(string directory)
        {
            try
            {
                FileStream fs = new FileStream(directory, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine($"[Event \"Ignácovy šachy\"]");
                sw.WriteLine($"[Site \"Planeta Země\"]");
                sw.WriteLine($"[Date \"{DateTime.Now}\"]");
                sw.WriteLine($"[Round \"-1\"]");
                sw.WriteLine($"[White \"{ZjistitJmenoHrace(PlayerColor.WHITE)}\"]");
                sw.WriteLine($"[Black \"{ZjistitJmenoHrace(PlayerColor.BLACK)}\"]");
                sw.WriteLine($"[Result \"{vysledekHry}\"]");
                sw.WriteLine($"[WhiteElo \"-1\"]");
                sw.WriteLine($"[BlackElo \"-1\"]");
                sw.WriteLine($"[TimeControl \"{maximalniDelkaTahu.TotalSeconds}\"]");
                sw.WriteLine($"[Termination \"{zpusobUkonceniHry}\"]");
                sw.WriteLine();
                for (int i = 0; i < notaceHry.Count; i++)
                {
                    sw.Write((i + 1) + ". " + notaceHry[i] + " ");
                }
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        private string ZjistitJmenoHrace(PlayerColor playerColor)
        {
            NameInputWindow form2 = new NameInputWindow();
            form2.Text = $"Zvolte jméno hráče {playerColor}";
            form2.ShowDialog();
            return form2.jmeno;
        }
    }
}