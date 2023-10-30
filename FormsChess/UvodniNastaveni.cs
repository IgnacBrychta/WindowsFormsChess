using System;
using System.Windows.Forms;

namespace FormsChess
{
    public partial class Form1
    {
        public static bool prohoditBarvy = false;
        public static bool atomicMode = false;
        private string FENnotace_filePath = string.Empty;
        private Casomira casomira = Casomira.VYCHOZI;
        public static bool odlisnaBilaCernaTextura = false;
        public static string pawn_white_texturePath = string.Empty;
        public static string rook_white_texturePath = string.Empty;
        public static string knight_white_texturePath = string.Empty;
        public static string bishop_white_texturePath = string.Empty;
        public static string queen_white_texturePath = string.Empty;
        public static string king_white_texturePath = string.Empty;
        private bool ignorovatCasomiru = false;
        private int inteligenceAI = 20;
        private bool AI_konzole = false;
        private bool connectionToChessBoard = false;
        public string portName = "COMX";

        private void NastaveniSachu()
        {
            var nastaveni = new SettingsWindow();
            nastaveni.ShowDialog();
            FENnotace_filePath                   = nastaveni.FENnotace;
            atomicMode                           = nastaveni.atomicMode;
            prohoditBarvy                        = nastaveni.prohoditBarvy;
            odlisnaBilaCernaTextura              = nastaveni.odlisnaBilaCernaTextura;
            pawn_white_texturePath               = nastaveni.pawn_white_texturePath;
            rook_white_texturePath               = nastaveni.rook_white_texturePath;
            knight_white_texturePath             = nastaveni.knight_white_texturePath;
            bishop_white_texturePath             = nastaveni.bishop_white_texturePath;
            queen_white_texturePath              = nastaveni.queen_white_texturePath;
            king_white_texturePath               = nastaveni.king_white_texturePath;
            ignorovatCasomiru                    = nastaveni.ignorovatCasomiru;
            inteligenceAI                        = nastaveni.inteligenceAI;
            AI_konzole                           = nastaveni.AIkonzole;
            connectionToChessBoard               = nastaveni.connectionToChessBoard;
            zaXsekundZiskaProtihracJednuSekundu  = nastaveni.casovaDotace;
            portName                             = (string)nastaveni.comboBox_SerialPortName.SelectedItem;
            allowBeeping                         = nastaveni.allowBeeping;
            if (nastaveni.comboBox_casomira.SelectedIndex == 1)
            {
                casomira = Casomira.VZAJEMNA;
            }
            if(connectionToChessBoard)
            {
                serialPort.PortName = portName;
                serialPort.DataReceived += SerialPort_DataReceived;
                try
                {
                    serialPort.Open();
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        "Zařízení se neidentifikovalo jako kompatibilní s tímto programem",
                        "Neplatné spojení",
                        MessageBoxButtons.OK, MessageBoxIcon.Error
                        );
                    throw;
                }
                ArduinoSetup();
            }
        }
        private void ArduinoSetup()
        {
            serialPort.Write(new byte[] { 0b0000_0011 }, 0, 1);
            serialPort.Write(casomira == Casomira.VZAJEMNA ? "2" : "1");
        }
        private enum Casomira
        {
            VYCHOZI,
            VZAJEMNA
        }
    }
}