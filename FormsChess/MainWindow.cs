#define _Upozorneni
#define _ZastavitCasomiru
#define EnPassant
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsChess
{
    public partial class Form1 : Form
    {
        #region InitialSettings
        public const int ROWS = 8;
        public const int COLUMNS = 8;
        public ChessPiece[,] chessBoard = new ChessPiece[ROWS, COLUMNS];
        private Button[,] buttonGrid = new Button[ROWS, COLUMNS];
        PlayerColor hracNaRade = !prohoditBarvy ? PlayerColor.BLACK : PlayerColor.WHITE;
        internal static GameState gameState = GameState.SELECT_MOVING_PIECE;
        Dictionary<int[], Color> availableMovesOnChessboard = new Dictionary<int[], Color>();
        int selectedPieceRow = -1;
        int selectedPieceColumn = -1;
        bool jeKralHraceNaTahuSachovan = false;
        readonly Color chessBoardColor1 = Color.Bisque;
        readonly Color chessBoardColor2 = Color.SandyBrown;
        readonly Color highlightColor = Color.Aqua;
        readonly Color highlightColorCapture = Color.Crimson;
        readonly Color sachovaciBarva = Color.Red;
        readonly Color barvaZvyrazneniObetavychFigurek = Color.FromArgb(255, 212, 102, 201);
        readonly Color pohybFigurky_pocatek = Color.FromArgb(255, 134, 232, 26);
        readonly Color pohybFigurky_cil = Color.FromArgb(255, 59, 174, 26); 
        int[] souradnice_pohybFigurky_pocatek = new int[] { -1, -1 };
        int[] souradnice_pohybFigurky_cil = new int[] { -1, -1 };
        const int pocetTahuBezVyhozeniDoRemizy = 40; // 40 půltahů / 2 = 20 celotahů
        const string prefixTlacitekMimoSachovnici = "NCB";
        int pocetTahuOdVyhozeni = 0;
        Rook whiteLeftRook = null;
        Rook whiteRightRook = null;
        King whiteKing = null;
        Rook blackLeftRook = null;
        Rook blackRightRook = null;
        King blackKing = null;
        TimeSpan uplynulyCas = new TimeSpan();
        readonly TimeSpan sekunda = new TimeSpan(0, 0, 1);
        TimeSpan casNaTah = new TimeSpan();
        readonly TimeSpan maximalniDelkaTahu = new TimeSpan(0, 2, 0);
        TimeSpan casNaTah_bila = new TimeSpan();
        TimeSpan casNaTah_cerna = new TimeSpan();
        internal static int tah = -1;
        private int hodnotaVeProspechBile = 0;
        internal static List<string> notaceHry = new List<string>();
        string vysledekHry = "0-0";
        string zpusobUkonceniHry = "Zatím neukončeno";
        bool casomiraPermanentneZastavena = true;
        const bool otevritSouborNotacePoUlozeni = true;
        List<List<char>> figurkyNotaceHry = new List<List<char>>();
        int zaXsekundZiskaProtihracJednuSekundu = 3;
        bool casovac1_temp = false;
        bool casovac2_temp = false;
        Process chessEngine;
        string dostupneRosady = "";
        Color[,] puvodniBarvySachovnice = new Color[8, 8];
        internal TimeSpan casPremysleniAI = new TimeSpan();
        internal static char AI_volbaPromocePesaka = ' ';
        readonly Regex FEN_regex;
        readonly ConsoleWindow konzole;
        ChessPiece lastCapturedFig;
        bool lastCapturedFig_hasMoved;
        bool lastMovedFig_hasMoved;
        int lastScoreChange = 0;
        bool allowBeeping = false;
        const int defaultScreenWidth = 1288;
        static internal int imageSizeFactor;
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Beep(uint dwFreq, uint dwDuration);
        public Form1()
        {
            InitializeComponent();
            NastaveniSachu();
            imageSizeFactor = (int)Math.Round((double)Width / defaultScreenWidth);
            pictureBox_vyhozeneFigurky.Image = new Bitmap(600, 75);
            FEN_regex = new Regex(
                "\\s*^(((?:[rnbqkpRNBQKP1-8]+\\/){7})[rnbqkpRNBQKP1-8]+)\\s([b|w])\\s([K|Q|k|q|-]{1,4})\\s(-|[a-h][1-8])\\s(\\d+\\s\\d+)$"
                );
            if (string.IsNullOrEmpty(FENnotace_filePath))
            {
                ConfigButtons();

                tah = -1;
                hracNaRade = !prohoditBarvy ? PlayerColor.BLACK : PlayerColor.WHITE;
            }
            else
            {
                ConfigButtonsAndLoadFEN();
            }
            if (odlisnaBilaCernaTextura)
            {
                PrepsatJednuBarvuTextur();
            }
            UlozitBarvyPolicek();
            ZmenaHraceNaRade();
            NCBbutton_remiza.Image = ChessPiece.ScaleImage(Image.FromFile(@"..\..\textures\default\draw.png"), 70, 70, outline: false);
            NCBbutton_Restart.Image = ChessPiece.ScaleImage(Image.FromFile(@"..\..\textures\default\restart.png"), 40, 40, outline: false);
            NCBbutton_exportNotation.Image = ChessPiece.ScaleImage(Image.FromFile(@"..\..\textures\default\export.png"), 70, 70, outline: false);
            NCBbutton_giveUp_Black.Image = ChessPiece.ScaleImage(Image.FromFile(@"..\..\textures\default\give up.png"), 50, 70, outline: false);
            NCBbutton_giveUp_White.Image = ChessPiece.ScaleImage(Image.FromFile(@"..\..\textures\default\give up.png"), 50, 70, outline: false);
            NCBbutton_vratitTah.Image = ChessPiece.ScaleImage(Image.FromFile(@"..\..\textures\default\return.png"), 50, 50, outline: false);
            NCBbutton_minimaxAlgoritmus.BackgroundImage = ChessPiece.ScaleImage(Image.FromFile(@"..\..\textures\default\telephone.png"), 200 * imageSizeFactor, 70 * imageSizeFactor, outline: false);
            if (atomicMode)
            {
                pictureBox_atomic1.Image = ChessPiece.ScaleImage(Image.FromFile(@"..\..\textures\default\atomic.png"), 50, 50, outline: false);
                pictureBox_atomic2.Image = pictureBox_atomic1.Image;
                pictureBox_atomic1.Refresh();
                pictureBox_atomic2.Refresh();
            }
            if (casomira == Casomira.VYCHOZI)
            {
                label_vzajemnaCasomira_bila.Hide();
                label_vzajemnaCasomira_black.Hide();
            }
            else
            {
                label8.Hide();
                label7.Hide();
                label9.Hide();
                label_zbyvajiciCasNaTah.Hide();
                label_casomiraText.Hide();
                label_casomira.Hide();

                casNaTah_bila = new TimeSpan(0, 0, (int)maximalniDelkaTahu.TotalSeconds);
                casNaTah_cerna = new TimeSpan(0, 0, (int)maximalniDelkaTahu.TotalSeconds);
                label_vzajemnaCasomira_black.Text = casNaTah_cerna.ToString();
                label_vzajemnaCasomira_bila.Text = casNaTah_bila.ToString();
                timer_bila.Stop();
                timer_cerna.Stop();
            }
            if (prohoditBarvy)
            {
                (NCBbutton_dlouhaBilaRosada.Text, NCBbutton_dlouhaCernaRosada.Text) =
                    (NCBbutton_dlouhaCernaRosada.Text, NCBbutton_dlouhaBilaRosada.Text);
                (NCBbutton_kratkaBilaRosada.Text, NCBbutton_kratkaCernaRosada.Text) =
                    (NCBbutton_kratkaCernaRosada.Text, NCBbutton_kratkaBilaRosada.Text);
            }
            if (ignorovatCasomiru)
            {
                label_vzajemnaCasomira_bila.ForeColor = Color.Yellow;
                label_vzajemnaCasomira_black.ForeColor = Color.Yellow;
                label_zbyvajiciCasNaTah.ForeColor = Color.Yellow;
            }
            
            if (AI_konzole)
            {
                konzole = new ConsoleWindow();
                konzole.Show(this);
            }
            ChessEngineInit();
            ZakazatVratitTah();
        }
        private void UlozitBarvyPolicek()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    puvodniBarvySachovnice[i, j] = buttonGrid[i, j].BackColor;
                }
            }
        }
        private bool JeFENnotaceVeSpravnemFormatu(string FENnotace)
        {
            return FEN_regex.IsMatch(FENnotace);
        }
        private void ConfigButtonsAndLoadFEN()
        {
            
            FileStream fs = new FileStream(FENnotace_filePath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string rawFEN = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            
            if (!JeFENnotaceVeSpravnemFormatu(rawFEN))
            {
                MessageBox.Show("Soubor FEN notace není ve správném tvaru.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            string[] castiFENnotace = rawFEN.Split(' ');
            string polohyFigurek = castiFENnotace[0];
            int boardRow = 0;
            int boardColumn = -1;
            Dictionary<char, int> mnozstviBilychFigurek = new Dictionary<char, int>()
            {
                { 'p', 0 },
                { 'r', 0 },
                { 'n', 0 },
                { 'b', 0 },
                { 'q', 0 },
                { 'k', 0 }
            };
            Dictionary<char, int> mnozstviCernychFigurek = new Dictionary<char, int>()
            {
                { 'p', 0 },
                { 'r', 0 },
                { 'n', 0 },
                { 'b', 0 },
                { 'q', 0 },
                { 'k', 0 }
            };
            Dictionary<char, int> optimalniMnozstviFigurek = new Dictionary<char, int>()
            {
                { 'p', 8 },
                { 'r', 2 },
                { 'n', 2 },
                { 'b', 2 },
                { 'q', 1 },
                { 'k', 1 }
            };
            Dictionary<char, string> texturePaths = new Dictionary<char, string>()
            {
                { 'p', Pawn.texturePath },
                { 'r', Rook.texturePath },
                { 'n', Knight.texturePath },
                { 'b', Bishop.texturePath },
                { 'q', Queen.texturePath },
                { 'k', King.texturePath }
            };
            Dictionary<char, string> alternativeTexturePaths = new Dictionary<char, string>()
            {
                { 'p', pawn_white_texturePath },
                { 'r', rook_white_texturePath },
                { 'n', knight_white_texturePath },
                { 'b', bishop_white_texturePath },
                { 'q', queen_white_texturePath },
                { 'k', king_white_texturePath }
            };
            for (int i = 0; i < this.Controls.Count; i++)
            {
                var controlItem = this.Controls[i];
                if (controlItem is Button button)
                {
                    if (controlItem.Name.StartsWith(prefixTlacitekMimoSachovnici))
                    {
                        continue;
                    }
                    GetRowAndColumnFromButton(controlItem.Name, out int buttonNumber, out int row, out int column);
                    buttonGrid[row, column] = button;
                }
            }
            
            void AktualizovatPocetFigurek(PlayerColor barva, char typFigurky)
            {
                typFigurky = char.ToLower(typFigurky);
                if (barva == PlayerColor.WHITE)
                {
                    mnozstviBilychFigurek[typFigurky]++;
                }
                else
                {
                    mnozstviCernychFigurek[typFigurky]++;
                }
            }
            void PricistHodnotuKeSkore(char chessPiece, PlayerColor barva)
            {
                int pieceValue = 0;
                switch (chessPiece)
                {
                    case 'p':
                        pieceValue = 1;
                        break;
                    case 'r':
                        pieceValue = 5;
                        break;
                    case 'n':
                        pieceValue = 3;
                        break;
                    case 'b':
                        pieceValue = 3;
                        break;
                    case 'q':
                        pieceValue = 9;
                        break;
                    case 'k':
                        pieceValue = 999;
                        break;
                }
                hodnotaVeProspechBile += pieceValue * (barva == PlayerColor.WHITE ? 1 : -1);
            }
            void AktualizovatSkore_FEN()
            {
                foreach (var item in mnozstviBilychFigurek)
                {
                    if (item.Value != optimalniMnozstviFigurek[item.Key])
                    {
                        for (int i = item.Value; i < optimalniMnozstviFigurek[item.Key]; i++)
                        {
                            PricistHodnotuKeSkore(item.Key, PlayerColor.WHITE);
                            bool textureExists = !string.IsNullOrEmpty(alternativeTexturePaths[item.Key]);
                            string texturePath = textureExists ? alternativeTexturePaths[item.Key] : texturePaths[item.Key];
                            AktualizovatObrazekVyhozenychFigurek(
                                ChessPiece.ScaleImage(Image.FromFile(texturePath), 35, 35, true ^ textureExists, true),
                                PlayerColor.WHITE
                                );
                        }
                    }
                }
                foreach (var item in mnozstviCernychFigurek)
                {
                    if (item.Value != optimalniMnozstviFigurek[item.Key])
                    {
                        for (int i = item.Value; i < optimalniMnozstviFigurek[item.Key]; i++)
                        {
                            Image image = ChessPiece.ScaleImage(Image.FromFile(texturePaths[item.Key]), 35, 35, false, true);
                            PricistHodnotuKeSkore(item.Key, PlayerColor.BLACK);
                            AktualizovatObrazekVyhozenychFigurek(
                                image,
                                PlayerColor.BLACK
                                );
                        }
                    }
                }
            }
            void DovolitEnPassantFigurce(string souradnice)
            {
                string coordsHorizontal = "abcdefgh";
                string coordsVertical = "87654321";
                int columnTile = coordsHorizontal.IndexOf(souradnice[0]);
                int rowTile = coordsVertical.IndexOf(souradnice[1]);
                if (rowTile == 5)
                {
                    if (chessBoard[rowTile - 1, columnTile] is Pawn pawn)
                    {
                        pawn.dvojityPohybBehemTahu_cislo = tah;
                    }
                }
                else if (rowTile == 2)
                {
                    if (chessBoard[rowTile + 1, columnTile] is Pawn pawn)
                    {
                        pawn.dvojityPohybBehemTahu_cislo = tah;
                    }
                }
            }
            for (int i = 0; i < polohyFigurek.Length; i++)
            {
                boardColumn++;
                char znak = polohyFigurek[i];
                if (znak == '/')
                {
                    boardRow++;
                    boardColumn = -1;
                    continue;
                }
                PlayerColor barvaFigurky = char.IsLower(znak) ? PlayerColor.BLACK : PlayerColor.WHITE;
                ChessPiece boardFig = null;
                switch(char.ToLower(znak))
                {
                    case 'p':
                        boardFig = new Pawn(barvaFigurky, boardRow, boardColumn, -1);
                        break;
                    case 'r':
                        boardFig = new Rook(barvaFigurky, boardRow, boardColumn, -1);
                        if (boardRow == 0 && boardColumn == 0)
                        {
                            blackLeftRook = (Rook)boardFig;
                        }
                        else if (boardRow == 0 && boardColumn == 7)
                        {
                            blackRightRook = (Rook)boardFig;
                        }
                        else if (boardRow == 7 && boardColumn == 0)
                        {
                            whiteLeftRook = (Rook)boardFig;
                        }
                        else if (boardRow == 7 && boardColumn == 7)
                        {
                            whiteRightRook = (Rook)boardFig;
                        }
                        break;
                    case 'n':
                        boardFig = new Knight(barvaFigurky, boardRow, boardColumn, -1);
                        break;
                    case 'b':
                        boardFig = new Bishop(barvaFigurky, boardRow, boardColumn, -1);
                        break;
                    case 'k':
                        boardFig = new King(barvaFigurky, boardRow, boardColumn, -1);
                        if (barvaFigurky == PlayerColor.WHITE)
                        {
                            whiteKing = (King)boardFig;
                        }
                        else
                        {
                            blackKing = (King)boardFig;
                        }
                        break;
                    case 'q':
                        boardFig = new Queen(barvaFigurky, boardRow, boardColumn, -1);
                        break;
                    default:
                        boardColumn += (int)char.GetNumericValue(znak) - 1;
                        continue;
                }
                AktualizovatPocetFigurek(barvaFigurky, znak);
                chessBoard[boardRow, boardColumn] = boardFig;
                buttonGrid[boardRow, boardColumn].BackgroundImage = barvaFigurky == PlayerColor.WHITE ? 
                    boardFig.texture_white : boardFig.texture_black;
            }

            hracNaRade = castiFENnotace[1] != "w" ? PlayerColor.WHITE : PlayerColor.BLACK; // inverze kvuli ZmenaHraceNaRade()
            string enPassantTile = castiFENnotace[3];
            pocetTahuOdVyhozeni = int.Parse(castiFENnotace[4]);
            tah = int.Parse(castiFENnotace[5]); // - 1 ??

            if (enPassantTile != "-")
            {
                DovolitEnPassantFigurce(enPassantTile);
            }

            AktualizovatSkore_FEN();
            if (mnozstviBilychFigurek['k'] == 0 || mnozstviCernychFigurek['k'] == 0)
            {
                MessageBox.Show("Nenalezeni oba králové", "CHYBA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gameState = GameState.CHECKMATE;
                return;
            }
            if (blackLeftRook is null)
            {
                blackLeftRook = new Rook(0, -1, -1, -1);
                blackLeftRook.hasMoved = true;
            }
            if (blackRightRook is null)
            {
                blackRightRook = new Rook(0, -1, -1, -1);
                blackRightRook.hasMoved = true;
            }
            if (whiteLeftRook is null)
            {
                whiteLeftRook = new Rook(0, -1, -1, -1);
                whiteLeftRook.hasMoved = true;
            }
            if (whiteRightRook is null)
            {
                whiteRightRook = new Rook(0, -1, -1, -1);
                whiteRightRook.hasMoved = true;
            }
        }
        private void ChessEngineInit()
        {
            string chessEnginePath = @"..\..\chess engine\stockfish_15.1_win_x64_avx2\stockfish_15.1_win_x64_avx2\stockfish-windows-2022-x86-64-avx2.exe";
            ProcessStartInfo chessEngineStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,

                FileName = chessEnginePath,
                CreateNoWindow = true
            };
            chessEngine = new Process();
            chessEngine.StartInfo = chessEngineStartInfo;
            chessEngine.Start();
            chessEngine.OutputDataReceived += ChessEngine_OutputDataReceived;
            chessEngine.ErrorDataReceived += ChessEngine_ErrorDataReceived;
            chessEngine.BeginOutputReadLine();
            chessEngine.BeginErrorReadLine();
        }
        private void ChessEngine_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            MessageBox.Show(e.Data, "Výjimka z StockFish", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void ProvestPohyb(int sourceRow, int sourceColumn, int targetRow, int targetColumn)
        {
            ProvestKliknuti(sourceRow, sourceColumn, false);
            ProvestKliknuti(targetRow, targetColumn, true);
        }
        private void StockFish()
        {
            chessEngine.StandardInput.WriteLine("isready");
            chessEngine.StandardInput.WriteLine("ucinewgame");
            chessEngine.StandardInput.WriteLine("setoption name UCI_LimitStrength value true");
            chessEngine.StandardInput.WriteLine("setoption name UCI_Elo value " + ((int)(2850 * inteligenceAI * 0.05)));
            chessEngine.StandardInput.WriteLine($"position fen {ZiskatFenNotaci()}"); // NE position startpos moves
            chessEngine.StandardInput.WriteLine("go depth 10");
        }
        private void ExecuteMove(string move)
        {
            string coordsHorizontal = "abcdefgh";
            string coordsVertical = "87654321";
            int columnSource = coordsHorizontal.IndexOf(move[0]);
            int rowSource = coordsVertical.IndexOf(move[1]);
            int columnTarget = coordsHorizontal.IndexOf(move[2]);
            int rowTarget = coordsVertical.IndexOf(move[3]);
            if (move.Length == 5)
            {
                AI_volbaPromocePesaka = move[4];
            }
            ProvestPohyb(rowSource, columnSource, rowTarget, columnTarget);
            AI_volbaPromocePesaka = ' ';
            
        }
        private void ChessEngine_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (AI_konzole)
                {
                    konzole.richTextBox1.Text += Environment.NewLine + e.Data;
                }
                if (string.IsNullOrEmpty(e.Data))
                {
                    gameState = GameState.CHECKMATE;
                    return;
                }
                else if (e.Data.StartsWith("bestmove", StringComparison.OrdinalIgnoreCase))
                {
                    gameState = GameState.SELECT_MOVING_PIECE;
                    string move = string.Empty;
                    if (e.Data.Length <= 13)
                    {
                        move = e.Data.Substring(9, 4);
                    }
                    else
                    {
                        move = e.Data.Substring(9, 5);
                    }
                    ExecuteMove(move);
                }
            }));
        }
        private void Casomira_Start()
        {
            timer1.Start();
            if (!(timer_bila.Enabled || timer_cerna.Enabled))
            {
                if (casovac1_temp)
                {
                    timer_bila.Start();
                }
                if (casovac2_temp)
                {
                    timer_cerna.Start();
                }
                casovac1_temp = timer_bila.Enabled;
                casovac2_temp = timer_cerna.Enabled;
            }
        }
        private void Casomira_Stop()
        {
            timer1.Stop();
            if (timer_bila.Enabled || timer_cerna.Enabled)
            {
                casovac1_temp = timer_bila.Enabled;
                casovac2_temp = timer_cerna.Enabled;
                timer_bila.Stop();
                timer_cerna.Stop();
            }
        }
        private void Casomira_StartStop()
        {
            if (casomira != Casomira.VYCHOZI)
            {
                if (hracNaRade != PlayerColor.WHITE)
                {
                    timer_bila.Start();
                    timer_cerna.Stop();
                }
                else
                {
                    timer_cerna.Start();
                    timer_bila.Stop();
                }
            }
        }
        public void GetRowAndColumnFromButton(string buttonName, out int buttonNumber, out int row, out int column)
        {
            string str_buttonNumber = buttonName.Substring(6);
            buttonNumber = int.Parse(str_buttonNumber);
            row = buttonNumber / ROWS;
            column = buttonNumber % COLUMNS - 1;
            if (column < 0)
            {
                column = 7;
                row--;
            }
        }
        private void ConfigButtons()
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                var controlItem = this.Controls[i];
                if (controlItem is Button button)
                {
                    if (controlItem.Name.StartsWith(prefixTlacitekMimoSachovnici))
                    {
                        continue;
                    }
                    GetRowAndColumnFromButton(controlItem.Name, out int buttonNumber, out int row, out int column);
                    buttonGrid[row, column] = button;
                    if (buttonNumber > 8 && buttonNumber <= 16)
                    {
                        chessBoard[row, column] = new Pawn(PlayerColor.BLACK, row, column, buttonNumber);
                        controlItem.BackgroundImage = chessBoard[row, column].texture_black;
                    }
                    else if (buttonNumber > 48 && buttonNumber <= 56)
                    {
                        chessBoard[row, column] = new Pawn(PlayerColor.WHITE, row, column, buttonNumber);
                        controlItem.BackgroundImage = chessBoard[row, column].texture_white;
                    }
                    else if (buttonNumber == 1)
                    {
                        chessBoard[row, column] = new Rook(PlayerColor.BLACK, row, column, buttonNumber);
                        blackLeftRook = (Rook)chessBoard[row, column];
                        controlItem.BackgroundImage = chessBoard[row, column].texture_black;
                    }
                    else if (buttonNumber == 8)
                    {
                        chessBoard[row, column] = new Rook(PlayerColor.BLACK, row, column, buttonNumber);
                        blackRightRook = (Rook)chessBoard[row, column];
                        controlItem.BackgroundImage = chessBoard[row, column].texture_black;
                    }
                    else if (buttonNumber == 57)
                    {
                        chessBoard[row, column] = new Rook(PlayerColor.WHITE, row, column, buttonNumber);
                        whiteLeftRook = (Rook)chessBoard[row, column];
                        controlItem.BackgroundImage = chessBoard[row, column].texture_white;
                    }
                    else if (buttonNumber == 64)
                    {
                        chessBoard[row, column] = new Rook(PlayerColor.WHITE, row, column, buttonNumber);
                        whiteRightRook = (Rook)chessBoard[row, column];
                        controlItem.BackgroundImage = chessBoard[row, column].texture_white;
                    }
                    else if (buttonNumber == 2 || buttonNumber == 7)
                    {
                        chessBoard[row, column] = new Knight(PlayerColor.BLACK, row, column, buttonNumber);
                        controlItem.BackgroundImage = chessBoard[row, column].texture_black;
                    }
                    else if (buttonNumber == 58 || buttonNumber == 63)
                    {
                        chessBoard[row, column] = new Knight(PlayerColor.WHITE, row, column, buttonNumber);
                        controlItem.BackgroundImage = chessBoard[row, column].texture_white;
                    }
                    else if (buttonNumber == 3 || buttonNumber == 6)
                    {
                        chessBoard[row, column] = new Bishop(PlayerColor.BLACK, row, column, buttonNumber);
                        controlItem.BackgroundImage = chessBoard[row, column].texture_black;
                    }
                    else if (buttonNumber == 59 || buttonNumber == 62)
                    {
                        chessBoard[row, column] = new Bishop(PlayerColor.WHITE, row, column, buttonNumber);
                        controlItem.BackgroundImage = chessBoard[row, column].texture_white;
                    }
                    else if (buttonNumber == 4)
                    {
                        chessBoard[row, column] = new Queen(PlayerColor.BLACK, row, column, buttonNumber);
                        controlItem.BackgroundImage = chessBoard[row, column].texture_black;
                    }
                    else if (buttonNumber == 60)
                    {
                        chessBoard[row, column] = new Queen(PlayerColor.WHITE, row, column, buttonNumber);
                        controlItem.BackgroundImage = chessBoard[row, column].texture_white;
                    }
                    else if (buttonNumber == 5)
                    {
                        chessBoard[row, column] = new King(PlayerColor.BLACK, row, column, buttonNumber);
                        blackKing = (King)chessBoard[row, column];
                        controlItem.BackgroundImage = chessBoard[row, column].texture_black;
                    }
                    else if (buttonNumber == 61)
                    {
                        chessBoard[row, column] = new King(PlayerColor.WHITE, row, column, buttonNumber);
                        whiteKing = (King)chessBoard[row, column];
                        controlItem.BackgroundImage = chessBoard[row, column].texture_white;
                    }
                }
            }
        }
        private void HighlightPossibleMoves(int row, int column)
        {
            availableMovesOnChessboard.Clear();
            List<int[]> availableMoves = chessBoard[row, column].dostupnePohybyBehemSachu;
            if (availableMoves.Count == 0 ||
                !(hracNaRade == PlayerColor.WHITE ? whiteKing : blackKing).jeSachovan
                )
            {
                availableMoves = chessBoard[row, column].GetAvailableMoves(ref chessBoard, false);
                if (chessBoard[row, column] is King king)
                {
                    availableMoves.AddRange(king.rosadovePohyby);
                }
            }
#if EnPassant
            // en passant: přidat možnost (políčka)
            if (chessBoard[row, column] is Pawn pawn)
            {
                if (pawn.EnPassantCheck(ref chessBoard, out List<int[]> enPassantPohyby))
                {
                    availableMoves.AddRange(enPassantPohyby);
                }
            }
#endif
            ZamknoutFigurcePohyby(row, column); // locking
            if (atomicMode && jeKralHraceNaTahuSachovan)
            {
                ZamknoutFigurcePohyby_AtomicMode(row, column);
            }
            foreach (var item in availableMoves)
            {
                availableMovesOnChessboard.Add(item, buttonGrid[item[0], item[1]].BackColor);
                if (chessBoard[item[0], item[1]] is null)
                {
                    buttonGrid[item[0], item[1]].BackColor = highlightColor;
                }
                else
                {
                    buttonGrid[item[0], item[1]].BackColor = highlightColorCapture;
                }
            }
        }
        private void ResetHighlighting()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    buttonGrid[i, j].BackColor = puvodniBarvySachovnice[i, j];
                }
            }

            ZobrazitZvyrazneniPolicekPriSachu();
            
        }
        private PlayerColor ZiskatOpacnouBarvu()
        {
            return hracNaRade == PlayerColor.WHITE ? PlayerColor.BLACK : PlayerColor.WHITE;
        }
        private string HracNaRade_ToString()
        {
            return (hracNaRade == PlayerColor.WHITE) ^ prohoditBarvy ? "Bílá" : "Černá";
        }
        private King ZiskatKrale(PlayerColor playerColor)
        {
            return playerColor == PlayerColor.WHITE ? whiteKing : blackKing;
        }
        private void UmoznitVratitTah()
        {
            NCBbutton_vratitTah.Enabled = true;
            NCBbutton_vratitTah.BackColor = Color.Green;
        }
        private void ZakazatVratitTah()
        {
            NCBbutton_vratitTah.Enabled = false;
            NCBbutton_vratitTah.BackColor = Color.Red;
        }
        private void ZmenaHraceNaRade()
        {
            if (gameState == GameState.CHECKMATE) return;
            if(!atomicMode) UmoznitVratitTah();
            Casomira_StartStop();
            ResetovatZvyrazneniPolicekPriSachu();
            ResetHighlighting();
            HighlightLastMove();
            List<int[]> vsechnyPohyby = ChessPiece.GetAllPossibleMoves(ref chessBoard, hracNaRade, out King king, out List<ChessPiece> _, true);
            // crash
            if (king is null)
            {
                Casomira_Stop();
                gameState = GameState.CHECKMATE;
                if (atomicMode)
                {
                    this.Text = "Král odbouchnut";
                    MessageBox.Show($"Král odbouchnut, {HracNaRade_ToString()} vyhrává.",
                    "Král odbouchnut", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                this.Text = "CHYBA";
                MessageBox.Show("král byl zabit, to se stát nemá, v kódu je chyba, vyfoťte šachovnici a pošlete ji vývojáři",
                    "CHYBA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            jeKralHraceNaTahuSachovan = King.JeKralSachovan(ref vsechnyPohyby, king.row, king.column);
            _ = king.GetAvailableMoves(ref chessBoard, false);
            hracNaRade = ZiskatOpacnouBarvu();
            this.Text = $"Ignácovy šachy: Na řadě je {HracNaRade_ToString()}.";
            label_hracNaRade_text.Text = HracNaRade_ToString();
            NCBbuttonLabel_hracNaRade_zobrazeni.BackColor =
                (hracNaRade == PlayerColor.WHITE) ^ prohoditBarvy ? Color.White : Color.Black;
            tah++;
            //label2.Text = "tah: " + (tah/2 + 1).ToString();
            AktualizovatSkore();
            BarvyRosadovychTlacitek();
            casNaTah = new TimeSpan(0, 0, 0);
            AktualizovatNotaci_Sach_Sachmat("+");

            if (jeKralHraceNaTahuSachovan)
            {
                // omezí figurkám pohyby, které by krále nezachránily
                bool mohouFigurkyZachranitKrale = MohouFigurkyZachranitKrale(ref chessBoard);
                if (king.availableMoves.Count == 0 && !mohouFigurkyZachranitKrale)
                {
                    Checkmate(king.row, king.column);
                }
                else
                {
                    NastavitZvyrazneniPolicekPriSachu();
#if Upozorneni
                    MessageBox.Show($"{HracNaRade_ToString()} král je šachován", "Upozornění", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
                }

            }
            else if (pocetTahuOdVyhozeni >= pocetTahuBezVyhozeniDoRemizy)
            {
                Remiza($"Během {pocetTahuBezVyhozeniDoRemizy} tahů nedošlo k vyhození.");
            }
            else
            {
                _ = ZjistitJestliRemiza(ZiskatKrale(hracNaRade));
            }
            AktualizovatVypisNotace();
        }
        private void NastavitZvyrazneniPolicekPriSachu()
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    ChessPiece figurka = chessBoard[i, j];
                    if (figurka?.playerColor == hracNaRade && (figurka?.dostupnePohybyBehemSachu.Count != 0 || figurka is King))
                    {
                        figurka.zachovaniBarvyPole.color = buttonGrid[figurka.row, figurka.column].BackColor;
                        figurka.zachovaniBarvyPole.row = figurka.row;
                        figurka.zachovaniBarvyPole.column = figurka.column;
                        buttonGrid[figurka.row, figurka.column].BackColor = figurka is King ? sachovaciBarva : barvaZvyrazneniObetavychFigurek;
                    }
                }
            }
        }
        private void ZobrazitZvyrazneniPolicekPriSachu()
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    ChessPiece figurka = chessBoard[i, j];
                    if (!(figurka is null) && figurka?.zachovaniBarvyPole.row != -1)
                    {
                        buttonGrid[figurka.zachovaniBarvyPole.row, figurka.zachovaniBarvyPole.column].BackColor =
                            figurka is King ? sachovaciBarva : barvaZvyrazneniObetavychFigurek;
                    }
                }
            }
        }
        private void ResetovatZvyrazneniPolicekPriSachu()
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    ChessPiece figurka = chessBoard[i, j];
                    if (!(figurka is null) && figurka?.zachovaniBarvyPole.row != -1)
                    {
                        //buttonGrid[figurka.zachovaniBarvyPole.row, figurka.zachovaniBarvyPole.column].BackColor = figurka.zachovaniBarvyPole.color;
                        figurka.zachovaniBarvyPole.color = Color.Transparent;
                        figurka.zachovaniBarvyPole.row = -1;
                        figurka.zachovaniBarvyPole.column = -1;

                    }
                }
            }
        }
        private void AktualizovatSkore()
        {
            label_skore.Text = Math.Abs(hodnotaVeProspechBile).ToString();
            if (hodnotaVeProspechBile >= 0)
            {
                label_skore.ForeColor = Color.Black;
                label_skore.BackColor = Color.White;
            }
            else
            {
                label_skore.ForeColor = Color.White;
                label_skore.BackColor = Color.Black;
            }
            label_skore.Text = '+' + label_skore.Text;
        }
        private void PricistSkore(int row, int column)
        {
            int pieceValue = chessBoard[row, column].pieceValue;
            lastScoreChange = pieceValue *
                (chessBoard[row, column].playerColor == PlayerColor.BLACK ? 1 : -1);
            hodnotaVeProspechBile += lastScoreChange;
        }
        private bool ZjistitJestliRemiza(King king, bool suppress = false)
        {
            _ = king.GetAvailableMoves(ref chessBoard, false);
            if (
                king.zakazanePohyby.Count > 0 &&
                king.availableMoves.Count == 0 &&
                !king.jeSachovan &&
                ChessPiece.GetAllPossibleMoves(ref chessBoard, king.playerColor, out _, out _, true).Count == 0
                )
            {
                if (!suppress)
                {
                    Remiza(king.row, king.column);
                }
                return true;
            }
            return false;
        }
        private void Checkmate(int row, int column)
        {
            Casomira_Stop();
            vysledekHry = hracNaRade == PlayerColor.BLACK ? "1-0" : "0-1";
            AktualizovatNotaci_Sach_Sachmat("#");
            string barva = (chessBoard[row, column].playerColor == PlayerColor.BLACK) ^ prohoditBarvy ? "Černý" : "Bílý";
            if (connectionToChessBoard)
            {
                SendGameEnding(barva + " prohrává.");
            }
            MessageBox.Show(
                $"{barva} král JE šachován, nemá se kam jinam hnout a žádná figurka se za něj nemůže obětovat.",
                $"{barva} poražena."
                );
            this.Text = $"Ignácovy šachy: {barva} prohrává. | Readonly mode";
            gameState = GameState.CHECKMATE;
            ZakazatVratitTah();
        }
        private void Prohra(string text, string caption)
        {
            Casomira_Stop();
            if (connectionToChessBoard)
            {
                SendGameEnding(HracNaRade_ToString() + " prohrává.");
            }
            MessageBox.Show(text, caption);
            this.Text = $"Ignácovy šachy: {HracNaRade_ToString()} prohrává. | Readonly mode";
            gameState = GameState.CHECKMATE;
            vysledekHry = hracNaRade == PlayerColor.BLACK ? "1-0" : "0-1";
            if (notaceHry.Count == 0)
            {
                notaceHry.Add(' ' + vysledekHry);
            }
            else
            {
                notaceHry[notaceHry.Count - 1] += ' ' + vysledekHry;
            }
            AktualizovatVypisNotace();
            zpusobUkonceniHry = text;
            ZakazatVratitTah();
        }
        private void Prohra()
        {
            Casomira_Stop();
            if (connectionToChessBoard)
            {
                SendGameEnding(HracNaRade_ToString() + " prohrává.");
            }
            MessageBox.Show(Text, "Prohra");
            this.Text = $"Ignácovy šachy: {HracNaRade_ToString()} prohrává. | Readonly mode";
            gameState = GameState.CHECKMATE;
            vysledekHry = hracNaRade == PlayerColor.BLACK ? "1-0" : "0-1";
            if (notaceHry.Count == 0)
            {
                notaceHry.Add(' ' + vysledekHry);
            }
            else
            {
                notaceHry[notaceHry.Count - 1] += ' ' + vysledekHry;
            }
            AktualizovatVypisNotace();
            zpusobUkonceniHry = "Tlačítko";
            ZakazatVratitTah();
        }
        private void Remiza(int row, int column)
        {
            Casomira_Stop();
            MessageBox.Show($"{(chessBoard[row, column].playerColor == PlayerColor.BLACK ^ prohoditBarvy ? "Bílý" : "Černý")} král NENÍ šachován a hráč se nemá kam jinam hnout.", "Remíza");
            this.Text = "Ignácovy šachy: Remíza | Readonly mode";
            gameState = GameState.CHECKMATE;
            if (connectionToChessBoard)
            {
                SendGameEnding("Remíza.");
            }
            if (notaceHry.Count > 0)
            {
                notaceHry[notaceHry.Count - 1] += " 1/2 1/2";
            }
            else
            {
                notaceHry.Add(" 1/2 1/2");
            }
            AktualizovatVypisNotace();
            zpusobUkonceniHry = (hracNaRade != PlayerColor.WHITE ^ prohoditBarvy ? whiteKing : blackKing).ToString() + " není šachován a hráč se nemá kam jinam hnout.";
            ZakazatVratitTah();
        }
        private void Remiza(string textOverride)
        {
            Casomira_Stop();
            MessageBox.Show(textOverride, "Remíza");
            this.Text = "Ignácovy šachy: Remíza. | Readonly mode";
            gameState = GameState.CHECKMATE;
            if (connectionToChessBoard)
            {
                SendGameEnding("Remíza.");
            }
            if (notaceHry.Count == 0)
            {
                notaceHry.Add("1/2 1/2");
            }
            else
            {
                notaceHry[notaceHry.Count - 1] += " 1/2 1/2";
            }
            AktualizovatVypisNotace();
            zpusobUkonceniHry = textOverride;
            ZakazatVratitTah();
        }
        #region Notace
        private string ZiskatFenNotaci()
        {
            string notace = "";
            for (int i = 0; i < 8; i++)
            {
                int j = 0;
                ChessPiece chessPiece = null;
                int emptyTiles = 0;
                bool chessPieceWasNull = false;
                for (; j < 8; j++)
                {
                    chessPiece = chessBoard[i, j];
                    if (chessPiece == null)
                    {
                        chessPieceWasNull = true;
                        emptyTiles++;
                    }
                    else
                    {
                        if (chessPieceWasNull)
                        {
                            notace += emptyTiles.ToString();
                            emptyTiles = 0;
                            chessPieceWasNull = false;
                        }
                        
                        if (chessPiece is Pawn)
                        {
                            notace += chessPiece.playerColor == PlayerColor.WHITE ? 'P' : 'p';
                        }
                        else if (chessPiece is Rook)
                        {
                            notace += chessPiece.playerColor == PlayerColor.WHITE ? 'R' : 'r';
                        }
                        else if (chessPiece is Knight)
                        {
                            notace += chessPiece.playerColor == PlayerColor.WHITE ? 'N' : 'n';
                        }
                        else if (chessPiece is Bishop)
                        {
                            notace += chessPiece.playerColor == PlayerColor.WHITE ? 'B' : 'b';
                        }
                        else if (chessPiece is Queen)
                        {
                            notace += chessPiece.playerColor == PlayerColor.WHITE ? 'Q' : 'q';
                        }
                        else if (chessPiece is King)
                        {
                            notace += chessPiece.playerColor == PlayerColor.WHITE ? 'K' : 'k';
                        }
                    }
                   
                }
                if (chessPiece is null)
                {
                    notace += emptyTiles.ToString();
                }
                notace += '/';
            }
            notace = notace.Substring(0, notace.Length - 1) + ' ' + (hracNaRade == PlayerColor.WHITE ? 'w' : 'b'); // pohyby
            BarvyRosadovychTlacitek(FENexport: true);
            notace += ' ' + dostupneRosady; // rošády
            notace += MuzeNekteraFigurkaEnPassant_FEN();
            notace += (pocetTahuOdVyhozeni / 2).ToString() + ' '; // Počet tahů od posledního vyhození // správně?
            notace += (tah / 2 + 1).ToString(); // aktuální tah
            return notace;
        }
        private string MuzeNekteraFigurkaEnPassant_FEN()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ChessPiece chessPiece = chessBoard[i, j];
                    if (chessPiece is Pawn pawn)
                    {
                        if (pawn.playerColor != hracNaRade &&
                            tah - pawn.dvojityPohybBehemTahu_cislo == 1)
                        {
                            return ' ' + TranslateCoordinates(pawn.row + (pawn.playerColor == PlayerColor.WHITE ? 1 : -1), pawn.column) + ' ';
                        }
                    }
                }
            }
            return " - ";
        }
        private void AktualizovatNotaci_Sach_Sachmat(string znak)
        {
            if (notaceHry.Count < 2)
            {
                return;
            }
            // v době vytváření notace pohybu ještě není známo, jestli pohyb způsobí šach
            string sach = jeKralHraceNaTahuSachovan ? znak : string.Empty;

            if (notaceHry[notaceHry.Count - 1].Contains("+") && znak.Contains("#"))
            {
                notaceHry[notaceHry.Count - 1] = notaceHry[notaceHry.Count - 1].Substring(0, notaceHry[notaceHry.Count - 1].Length - 1) + sach; // replace + -> #
                zpusobUkonceniHry = (hracNaRade != PlayerColor.WHITE ? whiteKing : blackKing).ToString() + " je šachován, nemá se kam hnout a nemá jej kdo zachránit.";
                return;
            }
            notaceHry[notaceHry.Count - 1] += sach;

        }
        private void AktualizovatVypisNotace()
        {
            richTextBox_notace.Text = string.Empty;
            for (int i = notaceHry.Count - 1; i >= 0; i--)
            {
                int mezeraLokace = notaceHry[i].IndexOf(' ');
                if (mezeraLokace < 0 || notaceHry[i].Contains("-") || notaceHry[i].Contains("/"))
                {
                    richTextBox_notace.Text += (i + 1) + ". " + (figurkyNotaceHry.Count != 0 ? figurkyNotaceHry[i][0] : ' ') + notaceHry[i] + Environment.NewLine;
                }
                else
                {
                    string[] leftRight = notaceHry[i].Split(' ');
                    string left = leftRight[0];
                    string right = leftRight[1];
                    richTextBox_notace.Text += (i + 1) + ". " + figurkyNotaceHry[i][0] + left + ' ' + figurkyNotaceHry[i][1] + right + Environment.NewLine;
                }

            }
        }
        private void AktualizovatNotaci(ChessPiece figurka1, ChessPiece figurka2)
        {
            string aktualniPohyb = Case_capture(figurka1, figurka2);

            if (tah % 2 == 0)
            {
                notaceHry.Add(aktualniPohyb);
                figurkyNotaceHry.Add(new List<char> { ZiskatZnakFigurky(figurka1) });
            }
            else
            {
                notaceHry[notaceHry.Count - 1] += ' ' + aktualniPohyb;
                figurkyNotaceHry[notaceHry.Count - 1].Add(ZiskatZnakFigurky(figurka1));
            }
        }
        private char ZiskatZnakFigurky(ChessPiece figurka)
        {
            if ((figurka.playerColor == PlayerColor.WHITE) ^ prohoditBarvy)
            {
                if (figurka is Pawn)
                {
                    return (char)WhitePiecesChars.PAWN;
                }
                else if (figurka is Rook)
                {
                    return (char)WhitePiecesChars.ROOK;
                }
                else if (figurka is Knight)
                {
                    return (char)WhitePiecesChars.KNIGHT;
                }
                else if (figurka is Bishop)
                {
                    return (char)WhitePiecesChars.BISHOP;
                }
                else if (figurka is Queen)
                {
                    return (char)WhitePiecesChars.QUEEN;
                }
                else
                {
                    return (char)WhitePiecesChars.KING;
                }
            }
            else
            {
                if (figurka is Pawn)
                {
                    return (char)BlackPiecesChars.PAWN;
                }
                else if (figurka is Rook)
                {
                    return (char)BlackPiecesChars.ROOK;
                }
                else if (figurka is Knight)
                {
                    return (char)BlackPiecesChars.KNIGHT;
                }
                else if (figurka is Bishop)
                {
                    return (char)BlackPiecesChars.BISHOP;
                }
                else if (figurka is Queen)
                {
                    return (char)BlackPiecesChars.QUEEN;
                }
                else
                {
                    return (char)BlackPiecesChars.KING;
                }
            }
        }
        private void AktualizovatNotaci(int tilesRookMoved)
        {
            string aktualniPohyb = tilesRookMoved == 2 ? "0-0" : "0-0-0";
            if (tah % 2 == 0)
            {
                notaceHry.Add(aktualniPohyb);
                figurkyNotaceHry.Add(new List<char> { (char)WhitePiecesChars.KING });
            }
            else
            {
                notaceHry[notaceHry.Count - 1] += ' ' + aktualniPohyb;
                figurkyNotaceHry[notaceHry.Count - 1].Add((char)BlackPiecesChars.KING);
            }
        }
        private string Case_capture(ChessPiece figurka1, ChessPiece figurka2)
        {
            // musí být uveden sloupec pěšce?
            bool horizontalDuplicacy = IsSameKindOfPieceOnRow(figurka1, figurka2.row, figurka2.column);
            bool verticalDuplicacy = IsSameKindOfPieceOnColumn(figurka1, figurka2.row, figurka2.column);
            if (horizontalDuplicacy)
            {
                return figurka1.notationLetter + TranslateCoordinates(-1, figurka1.column) + "x" + TranslateCoordinates(figurka2.row, figurka2.column);
            }
            else if (verticalDuplicacy)
            {
                return figurka1.notationLetter + TranslateCoordinates(figurka1.row, -1) + "x" + TranslateCoordinates(figurka2.row, figurka2.column);
            }
            else
            {
                return figurka1.notationLetter + "x" + TranslateCoordinates(figurka2.row, figurka2.column);
            }
        }
        private string Case_move(ChessPiece figurka, int newRow, int newColumn)
        {
            bool horizontalDuplicacy = IsSameKindOfPieceOnRow(figurka, newRow, newColumn);
            bool verticalDuplicacy = IsSameKindOfPieceOnColumn(figurka, newRow, newColumn);
            bool twoKnights = CanTwoKnightsJumpToSameTile(newRow, newColumn, figurka.playerColor);

            if (twoKnights)
            {
                return figurka.notationLetter + TranslateCoordinates(-1, figurka.column) + TranslateCoordinates(newRow, newColumn);
            }
            else if (!horizontalDuplicacy && !verticalDuplicacy)
            {
                return figurka.notationLetter + TranslateCoordinates(newRow, newColumn);
            }
            else if (!horizontalDuplicacy && verticalDuplicacy)
            {
                return figurka.notationLetter + TranslateCoordinates(figurka.row, -1) + TranslateCoordinates(newRow, newColumn);
            }
            else // else if (horizontalDuplicacy && !verticalDuplicacy)
            {
                return figurka.notationLetter + TranslateCoordinates(-1, figurka.column) + TranslateCoordinates(newRow, newColumn);
            }
        }
        private void AktualizovatNotaci(ChessPiece figurka1, int newRow, int newColumn)
        {
            string aktualniPohyb = Case_move(figurka1, newRow, newColumn);

            if (tah % 2 == 0)
            {
                notaceHry.Add(aktualniPohyb);
                figurkyNotaceHry.Add(new List<char> { ZiskatZnakFigurky(figurka1) });
            }
            else
            {
                if (notaceHry.Count == 0)
                {
                    notaceHry.Add("null");
                    figurkyNotaceHry.Add(new List<char> { '?' });
                }
                notaceHry[notaceHry.Count - 1] += ' ' + aktualniPohyb;
                figurkyNotaceHry[notaceHry.Count - 1].Add(ZiskatZnakFigurky(figurka1));
            }
        }
        private bool IsSameKindOfPieceOnRow(ChessPiece chessPiece, int newRow, int newColumn)
        {
            int count = 0;
            for (int j = 0; j < 8; j++)
            {
                if (chessBoard[chessPiece.row, j]?.GetType() == chessPiece.GetType() && chessBoard[chessPiece.row, j]?.playerColor == chessPiece.playerColor)
                {
                    if (newRow == -1)
                    {
                        count++;
                    }
                    else if ((bool)chessBoard[chessPiece.row, j]?.GetAvailableMoves(ref chessBoard, false).Any(x => x[0] == newRow && x[1] == newColumn)) // figurka se počítá pouze tehdy, pokud může udělat pohyb [newRow; newColumn]
                    {
                        count++;
                    }
                }
            }
            return count == 2;
        }
        private bool IsSameKindOfPieceOnColumn(ChessPiece chessPiece, int newRow, int newColumn)
        {
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                if (chessBoard[i, chessPiece.column]?.GetType() == chessPiece.GetType() && chessBoard[i, chessPiece.column]?.playerColor == chessPiece.playerColor)
                {
                    if (newRow == -1)
                    {
                        count++;
                    }
                    else if ((bool)chessBoard[i, chessPiece.column]?.GetAvailableMoves(ref chessBoard, false).Any(x => x[0] == newRow && x[1] == newColumn))
                    {
                        count++;
                    }
                }
            }
            return count == 2;
        }
        private bool CanTwoKnightsJumpToSameTile(int row, int column, PlayerColor color)
        {
            int[,] pohyby = new int[,]
            {
                { -2, -1 },
                { -2, 1 },
                { 2, -1 },
                { 2, 1 },
                { -1, 2 },
                { 1, 2 },
                { -1, -2 },
                { 1, -2 }
            };
            int count = 0;
            for (int i = 0; i < pohyby.GetLength(0); i++)
            {
                int jumpRow = row + pohyby[i, 0];
                int jumpColumn = column + pohyby[i, 1];
                if (jumpRow >= 0 && jumpRow <= 7 && jumpColumn >= 0 && jumpColumn <= 7)
                {
                    if (
                        chessBoard[jumpRow, jumpColumn] is Knight knight && knight.playerColor == color &&
                        knight.dostupnePohybyBehemSachu.Any(x => x[0] == row && x[1] == column)
                        )
                    {
                        count++;
                    }
                }
            }
            return count == 2;
        }
        #endregion
        public static string TranslateCoordinates(int row, int column)
        {
            char[] coordsHorizontal = "abcdefgh".ToCharArray();
            char[] coordsVertical = "87654321".ToCharArray();
            return (column >= 0 ? new string(coordsHorizontal[column], 1) : string.Empty) +
                (row >= 0 ? new string(coordsVertical[row], 1) : string.Empty);
        }
        private void HighlightLastMove()
        {
            if (souradnice_pohybFigurky_pocatek[0] == -1 || souradnice_pohybFigurky_cil[0] == -1)
            {
                return;
            }
            buttonGrid[souradnice_pohybFigurky_pocatek[0], souradnice_pohybFigurky_pocatek[1]]
                .BackColor = pohybFigurky_pocatek;
            buttonGrid[souradnice_pohybFigurky_cil[0], souradnice_pohybFigurky_cil[1]]
                .BackColor = pohybFigurky_cil;
        }
        private void ProvestKliknuti(int row, int column, bool poslatZmenu = true)
        {
            ClickOnChessboard(row, column);
            if(connectionToChessBoard && poslatZmenu)
            {
                SendChessBoardState();
            }
        }
        private async Task PlaySoundOnBackground()
        {
            await Task.Run(() =>
            {
                Beep(9000, 500);
            });
        }
        private void ClickOnChessboard(int row, int column)
        {
            bool figurkaVyhozena = false;

            if (gameState == GameState.SELECT_MOVING_PIECE)
            {
                // Souřadnice původní zvolené figurky (source) jsou neplatné && kliknutí není ve sféře vlivu právě zvolené figurky (target)
                if (selectedPieceRow == -1 && selectedPieceColumn == -1 || !availableMovesOnChessboard.Any(x => x.Key[0] == row && x.Key[1] == column))
                {
                    if (chessBoard[row, column] is null)
                    {
                        return;
                    }
                    if (chessBoard[row, column].playerColor != hracNaRade)
                    {
                        return;
                    }
                    if (jeKralHraceNaTahuSachovan)
                    {
                        if (chessBoard[row, column]?.dostupnePohybyBehemSachu.Count == 0 && chessBoard[row, column].GetType() != typeof(King))
                        {
                            return;
                        }
                    }
                    ResetHighlighting();
                    HighlightLastMove();
                    HighlightPossibleMoves(row, column);
                    selectedPieceColumn = column; // target = source
                    selectedPieceRow = row;
                    gameState = GameState.SELECT_TARGET_TILE;
                }
            }
            else if (gameState == GameState.SELECT_TARGET_TILE)
            {
                if (selectedPieceRow == row && selectedPieceColumn == column) // cancel selection
                {
                    ResetHighlighting();
                    HighlightLastMove();
                    selectedPieceRow = -1;
                    selectedPieceColumn = -1;
                    gameState = GameState.SELECT_MOVING_PIECE;
                    return;
                }
                bool boardChanged = false;
                foreach (var item in availableMovesOnChessboard)
                {
                    if (item.Key[0] == row && item.Key[1] == column) // Valid target
                    {
                        if (allowBeeping) _ = PlaySoundOnBackground();

                        ChessPiece figurka_RowColumn = chessBoard[row, column];
                        ChessPiece figurka_selectedRowColumn = chessBoard[selectedPieceRow, selectedPieceColumn];
                        if (!(figurka_RowColumn is null))
                        {
                            pocetTahuOdVyhozeni = 0;
                            PricistSkore(row, column);
                            if (connectionToChessBoard) SendCaptureFigCommand(row, column);
                            PridatVyhozenouFigurku(figurka_RowColumn);
                            AktualizovatNotaci(figurka_selectedRowColumn, figurka_RowColumn);
                            figurkaVyhozena = true;
                            lastCapturedFig = figurka_RowColumn;
                            if (lastCapturedFig is Pawn p) lastCapturedFig_hasMoved = p.hasMoved;
                            else if (lastCapturedFig is Rook r) lastCapturedFig_hasMoved = r.hasMoved;
                            else if (lastCapturedFig is King k) lastCapturedFig_hasMoved = k.hasMoved;
                        }
                        else
                        {
                            lastCapturedFig = null;
                            if (figurka_selectedRowColumn is King castlingKing)
                            {
                                if (castlingKing.rosadovePohyby.Any(x => x[0] == row && x[1] == column)) // rošáda přes klik na šachovnici
                                {
                                    if (column == 2)
                                    {
                                        ProvestDlouhouRosadu(castlingKing, hracNaRade == PlayerColor.WHITE ? whiteLeftRook : blackLeftRook); // věž pro rošádu
                                    }
                                    else // column = 6
                                    {
                                        ProvestKratkouRosadu(castlingKing, hracNaRade == PlayerColor.WHITE ? whiteRightRook : blackRightRook);
                                    }
                                    HighlightLastMove();
                                    ZakazatVratitTah();
                                    return; // metody pro provedení rošády se postarají o vše potřebné
                                }
                            }
                            pocetTahuOdVyhozeni++;
                            AktualizovatNotaci(figurka_selectedRowColumn, row, column); // temp
                        }
                        if (figurka_selectedRowColumn is Pawn p_) lastMovedFig_hasMoved = p_.hasMoved;
                        else if (figurka_selectedRowColumn is Rook r_) lastMovedFig_hasMoved = r_.hasMoved;
                        else if (figurka_selectedRowColumn is King k_) lastMovedFig_hasMoved = k_.hasMoved;

                        if (figurka_selectedRowColumn is Pawn) pocetTahuOdVyhozeni = 0;
                        if (figurka_RowColumn is Rook rook) rook.hasMoved = true;
                        chessBoard[row, column] = chessBoard[selectedPieceRow, selectedPieceColumn]; // swap piece reference on grid
                        chessBoard[selectedPieceRow, selectedPieceColumn] = null; // delete piece
                        chessBoard[row, column].MovePiece(row, column, ref chessBoard, ref buttonGrid); // move coords in piece reference

                        buttonGrid[row, column].BackgroundImage = chessBoard[row, column].playerColor == PlayerColor.WHITE ? // change background image
                            chessBoard[row, column].texture_white : chessBoard[row, column].texture_black;
                        buttonGrid[selectedPieceRow, selectedPieceColumn].BackgroundImage = null; // reset background image
                        boardChanged = true;

#if EnPassant
                        // en passant: provést
                        if (chessBoard[row, column] is Pawn pawn)
                        {
                            if (pawn.enPassantPohyby.Count != 0 && pawn.enPassantPohyby[0][0] == row && pawn.enPassantPohyby[0][1] == column)
                            {
                                // řádek ze zvolení source, nikde jinde se selectedPieceRow a column nepotkají
                                PricistSkore(selectedPieceRow, column);
                                if (connectionToChessBoard) SendCaptureFigCommand(selectedPieceRow, column);
                                PridatVyhozenouFigurku(chessBoard[selectedPieceRow, column]);
                                notaceHry.RemoveAt(notaceHry.Count - 1);
                                AktualizovatNotaci(chessBoard[selectedPieceRow, column], pawn);
                                lastCapturedFig = chessBoard[selectedPieceRow, column];
                                lastCapturedFig_hasMoved = ((Pawn)chessBoard[selectedPieceRow, column]).hasMoved;
                                chessBoard[selectedPieceRow, column] = null;
                                buttonGrid[selectedPieceRow, column].BackgroundImage = null;
                                pocetTahuOdVyhozeni = 0;
                                break;
                            }
                            
                        }
#endif

#if !ZastavitCasomiru
                        if (!timer1.Enabled) // start clock if not started
                        {
                            Casomira_Start();
                            casomiraPermanentneZastavena = false;
                        }
#endif
                        break;
                    }
                }

                ResetHighlighting();

                if (jeKralHraceNaTahuSachovan && !boardChanged) // nepovolit pohnutí s jinou figurkou než s králem, když je král šachován
                {
                    gameState = GameState.SELECT_MOVING_PIECE;
                    HighlightLastMove();
                    return;
                }
                if (!boardChanged)
                {

                    HighlightLastMove();
                    if (chessBoard[row, column] is null) // Pokud je zvolena figurka a poté je zvoleno prázdné pole, resetovat výběr
                    {

                        selectedPieceRow = -1;
                        selectedPieceColumn = -1;
                        gameState = GameState.SELECT_MOVING_PIECE;
                        return;
                    }
                    // vrátit pokud není nová zvolená figurka stejné barvy jako barva hráče
                    if (chessBoard[row, column].playerColor != hracNaRade)
                    {
                        return;
                    }
                    HighlightPossibleMoves(row, column);
                    selectedPieceRow = row;
                    selectedPieceColumn = column;

                }
                else
                {
                    if (atomicMode && figurkaVyhozena)
                    {
                        for (int i = row - 1; i <= row + 1; i++)
                        {
                            for (int j = column - 1; j <= column + 1; j++)
                            {
                                if (i >= 0 && i <= 7 && j >= 0 && j <= 7)
                                {
                                    ChessPiece atomicPiece = chessBoard[i, j];
                                    if (!(atomicPiece is null) && !(atomicPiece is Pawn))
                                    {
                                        PridatVyhozenouFigurku(atomicPiece);
                                        PricistSkore(i, j);
                                        atomicPiece = null;
                                        chessBoard[i, j] = null;
                                        buttonGrid[i, j].BackgroundImage = null;
                                    }
                                }

                            }
                        }
                    }
                    // šachovnice se změnila, reset výběru
                    souradnice_pohybFigurky_pocatek[0] = selectedPieceRow;
                    souradnice_pohybFigurky_pocatek[1] = selectedPieceColumn;
                    souradnice_pohybFigurky_cil[0] = row;
                    souradnice_pohybFigurky_cil[1] = column;
                    HighlightLastMove();

                    selectedPieceRow = -1;
                    selectedPieceColumn = -1;
                    if (gameState != GameState.CHECKMATE)
                    {
                        gameState = GameState.SELECT_MOVING_PIECE;
                    }

                    ZmenaHraceNaRade();
                }
            }
        }
        private void chessBoardButton_click(object sender, EventArgs e)
        {
            Button senderButton = (Button)sender;
            GetRowAndColumnFromButton(senderButton.Name, out _, out int row, out int column);
            ProvestKliknuti(row, column);
        }
        private void ZamknoutFigurcePohyby(int pieceRow, int pieceColumn)
        {
            if (chessBoard[pieceRow, pieceColumn] is null)
            {
                return;
            }
            if (chessBoard[pieceRow, pieceColumn].playerColor != hracNaRade)
            {
                return;
            }
            bool pohybByZabilKrale = false;
            List<int[]> movesOfPiece = chessBoard[pieceRow, pieceColumn].dostupnePohybyBehemSachu;
            bool isKingTile = false;
            if (chessBoard[pieceRow, pieceColumn] is King)
            {
                isKingTile = true;
            }
            for (int i = 0; i < movesOfPiece.Count; i++)
            {
                var item = movesOfPiece[i];
                // swap
                ChessPiece buffer = chessBoard[item[0], item[1]];
                chessBoard[item[0], item[1]] = chessBoard[pieceRow, pieceColumn];
                chessBoard[pieceRow, pieceColumn] = null;

                List<int[]> vsechnyPohyby = ChessPiece.GetAllPossibleMoves(ref chessBoard, ZiskatOpacnouBarvu(), out King king, out List<ChessPiece> _, allTiles: false);
                if (isKingTile)
                {
                    pohybByZabilKrale = King.JeKralSachovan(ref vsechnyPohyby, item[0], item[1]);
                }
                else
                {
                    pohybByZabilKrale = King.JeKralSachovan(ref vsechnyPohyby, king.row, king.column);
                }


                // swap back
                chessBoard[pieceRow, pieceColumn] = chessBoard[item[0], item[1]];
                chessBoard[item[0], item[1]] = buffer;

                if (pohybByZabilKrale)
                {
                    movesOfPiece.RemoveAt(i);
                    i--; // při smazání pohybu nutno změnit index
                }
            }
        }
        private void ZamknoutFigurcePohyby_AtomicMode(int pieceRow, int pieceColumn)
        {
            ChessPiece chessPiece = chessBoard[pieceRow, pieceColumn];
            for (int h = 0; h < chessPiece.dostupnePohybyBehemSachu.Count; h++)
            {
                int row = chessPiece.dostupnePohybyBehemSachu[0][0];
                int column = chessPiece.dostupnePohybyBehemSachu[0][1];
                for (int i = row - 1; i <= row + 1; i++)
                {
                    for (int j = column - 1; j <= column + 1; j++)
                    {
                        if (i >= 0 && i <= 7 && j >= 0 && j <= 7)
                        {
                            ChessPiece atomicPiece = chessBoard[i, j];
                            if (atomicPiece is King king && king.playerColor == chessPiece.playerColor)
                            {
                                chessPiece.dostupnePohybyBehemSachu.RemoveAt(h);
                                h--;
                            }
                        }

                    }
                }
            }
        }
        private bool MohouFigurkyZachranitKrale(ref ChessPiece[,] chessBoard)
        {
            // aktualizace pohybů figurek
            _ = ChessPiece.GetAllPossibleMoves(ref chessBoard, hracNaRade, out King _, out List<ChessPiece> _, allTiles: false);
            bool existujeObetavaFigurka = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bool obetavaFigurka = MuzeFigurkaZachranitKrale(i, j, ref chessBoard);
                    if (obetavaFigurka)
                    {
                        existujeObetavaFigurka = true;
                    }
                }
            }
            return existujeObetavaFigurka;
        }
        private bool MuzeFigurkaZachranitKrale(int pieceRow, int pieceColumn, ref ChessPiece[,] chessBoard)
        {
            if (chessBoard[pieceRow, pieceColumn] is null)
            {
                return false;
            }
            if (chessBoard[pieceRow, pieceColumn].playerColor != hracNaRade)
            {
                return false;
            }
            if (chessBoard[pieceRow, pieceColumn] is King && chessBoard[pieceRow, pieceColumn].playerColor == hracNaRade)
            {
                return false;
            }

            List<int[]> movesOfPiece = chessBoard[pieceRow, pieceColumn].dostupnePohybyBehemSachu;
            for (int i = 0; i < movesOfPiece.Count; i++)
            {
                var item = movesOfPiece[i];
                // swap
                ChessPiece buffer = chessBoard[item[0], item[1]];
                chessBoard[item[0], item[1]] = chessBoard[pieceRow, pieceColumn];
                chessBoard[pieceRow, pieceColumn] = null;

                List<int[]> vsechnyPohyby = ChessPiece.GetAllPossibleMoves(ref chessBoard, ZiskatOpacnouBarvu(), out King king, out List<ChessPiece> _, allTiles: false);
                bool pohybNezachraniKrale = King.JeKralSachovan(ref vsechnyPohyby, king.row, king.column);

                // swap back
                chessBoard[pieceRow, pieceColumn] = chessBoard[item[0], item[1]];
                chessBoard[item[0], item[1]] = buffer;

                if (pohybNezachraniKrale)
                {
                    movesOfPiece.RemoveAt(i);
                    i--; // při smazání pohybu nutno změnit index
                }
            }
            return movesOfPiece.Count > 0;
        }
        private void BarvyRosadovychTlacitek(bool FENexport = false)
        {
            dostupneRosady = string.Empty;

            if (JeDostupnaRosada(7, 5, 7, whiteRightRook, whiteKing, FENexport) &&
                !King.JeKralSachovan(ref whiteKing.moznePohybyVsechFigurek, whiteKing.row, whiteKing.column + 1) &&
                !King.JeKralSachovan(ref whiteKing.moznePohybyVsechFigurek, whiteKing.row, whiteKing.column + 2)
               )
            {
                if (FENexport)
                {
                    dostupneRosady += 'K';
                }
                else
                {

                    NCBbutton_kratkaBilaRosada.BackColor = Color.Green;
                    NCBbutton_kratkaBilaRosada.Enabled = true;
                    whiteKing.PridatRosadovePohyby(new int[] { 7, 6 });
                }
            }
            else
            {
                NCBbutton_kratkaBilaRosada.BackColor = Color.Beige;
                NCBbutton_kratkaBilaRosada.Enabled = false;
                whiteKing.SmazatRosadovePohyby();
            }


            if (JeDostupnaRosada(7, 1, 4, whiteLeftRook, whiteKing, FENexport) &&
                !King.JeKralSachovan(ref whiteKing.moznePohybyVsechFigurek, whiteKing.row, whiteKing.column - 1) &&
                !King.JeKralSachovan(ref whiteKing.moznePohybyVsechFigurek, whiteKing.row, whiteKing.column - 2)
                )
            {
                if (FENexport)
                {
                    dostupneRosady += 'Q';
                }
                else
                {
                    NCBbutton_dlouhaBilaRosada.BackColor = Color.Green;
                    NCBbutton_dlouhaBilaRosada.Enabled = true;
                    whiteKing.PridatRosadovePohyby(new int[] { 7, 2 });
                }
            }
            else
            {
                NCBbutton_dlouhaBilaRosada.BackColor = Color.Beige;
                NCBbutton_dlouhaBilaRosada.Enabled = false;
            }

            if (JeDostupnaRosada(0, 5, 7, blackRightRook, blackKing, FENexport) &&
                !King.JeKralSachovan(ref blackKing.moznePohybyVsechFigurek, blackKing.row, blackKing.column + 1) &&
                !King.JeKralSachovan(ref blackKing.moznePohybyVsechFigurek, blackKing.row, blackKing.column + 2)
               )
            {
                if (FENexport)
                {
                    dostupneRosady += 'k';
                }
                else
                {
                    NCBbutton_kratkaCernaRosada.BackColor = Color.Green;
                    NCBbutton_kratkaCernaRosada.Enabled = true;
                    blackKing.PridatRosadovePohyby(new int[] { 0, 6 });
                }
            }
            else
            {
                NCBbutton_kratkaCernaRosada.BackColor = Color.Beige;
                NCBbutton_kratkaCernaRosada.Enabled = false;
                blackKing.SmazatRosadovePohyby();
            }


            if (JeDostupnaRosada(0, 1, 4, blackLeftRook, blackKing, FENexport) &&
                !King.JeKralSachovan(ref blackKing.moznePohybyVsechFigurek, blackKing.row, blackKing.column - 1) &&
                !King.JeKralSachovan(ref blackKing.moznePohybyVsechFigurek, blackKing.row, blackKing.column - 2)
               )
            {
                if (FENexport)
                {
                    dostupneRosady += 'q';
                }
                else
                {
                    NCBbutton_dlouhaCernaRosada.BackColor = Color.Green;
                    NCBbutton_dlouhaCernaRosada.Enabled = true;
                    blackKing.PridatRosadovePohyby(new int[] { 0, 2 });
                }
            }
            else
            {
                NCBbutton_dlouhaCernaRosada.BackColor = Color.Beige;
                NCBbutton_dlouhaCernaRosada.Enabled = false;
            }

            if (string.IsNullOrEmpty(dostupneRosady))
            {
                dostupneRosady = "-";
            }
        }
        private bool JeDostupnaRosada(int row, int columnStart, int columnEnd, Rook rook, King king, bool FENexport = false)
        {
            if (!FENexport)
            {
                for (int i = columnStart; i < columnEnd; i++)
                {
                    if (!(chessBoard[row, i] is null))
                    {
                        return false; // volný prostor pro rošádu
                    }
                }
            }
            
            if (
                !(bool)rook?.hasMoved && !(bool)king?.hasMoved &&
                 !(bool)king?.jeSachovan
                 )
            {
                if (king?.playerColor == hracNaRade || FENexport)
                {
                    return true;
                }
            }
            return false;
        }
        private void ProvestKratkouRosadu(King king, Rook rook)
        {
            souradnice_pohybFigurky_pocatek[0] = king.row;
            souradnice_pohybFigurky_pocatek[1] = king.column;
            king.MovePiece(king.row, 6);
            souradnice_pohybFigurky_cil[0] = king.row;
            souradnice_pohybFigurky_cil[1] = king.column;
            chessBoard[king.row, 6] = king;
            chessBoard[king.row, 4] = null;
            buttonGrid[king.row, 6].BackgroundImage = king.playerColor == PlayerColor.WHITE ?
                king.texture_white : king.texture_black;
            buttonGrid[king.row, 4].BackgroundImage = null;

            rook.MovePiece(rook.row, 5);
            chessBoard[rook.row, 5] = rook;
            chessBoard[rook.row, 7] = null;
            buttonGrid[rook.row, 5].BackgroundImage = rook.playerColor == PlayerColor.WHITE ?
                rook.texture_white : rook.texture_black;
            buttonGrid[rook.row, 7].BackgroundImage = null;
            king.SmazatRosadovePohyby();
            ResetHighlighting();
            AktualizovatNotaci(2);
            ZmenaHraceNaRade();
            ZakazatVratitTah();
        }
        private void ProvestDlouhouRosadu(King king, Rook rook)
        {
            souradnice_pohybFigurky_pocatek[0] = king.row;
            souradnice_pohybFigurky_pocatek[1] = king.column;
            king.MovePiece(king.row, 2);
            souradnice_pohybFigurky_cil[0] = king.row;
            souradnice_pohybFigurky_cil[1] = king.column;
            chessBoard[king.row, 2] = king;
            chessBoard[king.row, 4] = null;
            buttonGrid[king.row, 2].BackgroundImage = king.playerColor == PlayerColor.WHITE ?
                king.texture_white : king.texture_black;
            buttonGrid[king.row, 4].BackgroundImage = null;

            rook.MovePiece(rook.row, 3);
            chessBoard[rook.row, 3] = rook;
            chessBoard[rook.row, 0] = null;
            buttonGrid[rook.row, 3].BackgroundImage = rook.playerColor == PlayerColor.WHITE ?
                rook.texture_white : rook.texture_black;
            buttonGrid[rook.row, 0].BackgroundImage = null;
            king.SmazatRosadovePohyby();
            ResetHighlighting();
            AktualizovatNotaci(3);
            ZmenaHraceNaRade();
            ZakazatVratitTah();
        }
        private void NCBbutton_kratkaBilaRosada_Click(object sender, EventArgs e)
        {
            ProvestKratkouRosadu(whiteKing, whiteRightRook);
        }
        private void NCBbutton_dlouhaBilaRosada_Click(object sender, EventArgs e)
        {
            ProvestDlouhouRosadu(whiteKing, whiteLeftRook);
        }
        private void NCBbutton_dlouhaCernaRosada_Click(object sender, EventArgs e)
        {
            ProvestDlouhouRosadu(blackKing, blackLeftRook);
        }
        private void NCBbutton_kratkaCernaRosada_Click(object sender, EventArgs e)
        {
            ProvestKratkouRosadu(blackKing, blackRightRook);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                uplynulyCas += sekunda;
                casNaTah += sekunda;
                label_casomira.Text = uplynulyCas.ToString();
                label_zbyvajiciCasNaTah.Text = (maximalniDelkaTahu - casNaTah).ToString();
                if(connectionToChessBoard)
                {
                    UpdateDisplays();
                }
                if (casNaTah == maximalniDelkaTahu)
                {
                    if (ignorovatCasomiru)
                    {
                        return;
                    }
                    Prohra($"Čas vypršel. {(prohoditBarvy ? ZiskatOpacnouBarvu(): hracNaRade)} prohrává.", "Čas vypršel.");
                }
            }));
        }
        private void NCBbutton_Restart_Click(object sender, EventArgs e)
        {
            Casomira_Stop();
            DialogResult result = MessageBox.Show("Opravdu chcete aplikaci restartovat?", "Restart", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                Application.Restart();
                try
                {
                    Environment.Exit(0);
                }
                catch (Exception) { }
            }
            if (gameState != GameState.CHECKMATE)
            {
                Casomira_Start();
            }
        }
        private void NCBbutton_giveUp_White_Click(object sender, EventArgs e)
        {
            if (hracNaRade == PlayerColor.WHITE && gameState != GameState.CHECKMATE)
            {
                Casomira_Stop();
                DialogResult result = MessageBox.Show("Opravdu to chcete vzdát?", "Vzdát?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                Casomira_Start();
                if (result == DialogResult.OK)
                {
                    Prohra(HracNaRade_ToString() + " se vzdal.", "Prohra");
                }
            }
        }
        private void NCBbutton_giveUp_Black_Click(object sender, EventArgs e)
        {
            if (hracNaRade == PlayerColor.BLACK && gameState != GameState.CHECKMATE)
            {
                Casomira_Stop();
                DialogResult result = MessageBox.Show("Opravdu to chcete vzdát?", "Vzdát?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                Casomira_Start();
                if (result == DialogResult.OK)
                {
                    Prohra(HracNaRade_ToString() + " se vzdal.", "Prohra");
                }
            }
        }
        private void TimerBila_Tick(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                casNaTah_bila -= sekunda;
                if (casNaTah_bila.TotalSeconds % zaXsekundZiskaProtihracJednuSekundu == 0)
                {
                    casNaTah_cerna += sekunda;
                    label_vzajemnaCasomira_black.Text = casNaTah_cerna.ToString();
                }
                label_casomira.Text = uplynulyCas.ToString();
                label_vzajemnaCasomira_bila.Text = casNaTah_bila.ToString();
                //if (connectionToChessBoard) // the other timer is responsible for this
                //{
                //    UpdateDisplays();
                //}
                if (casNaTah_bila.TotalSeconds == 0)
                {
                    if (ignorovatCasomiru)
                    {
                        return;
                    }
                    Prohra($"Čas vypršel. {(prohoditBarvy ? ZiskatOpacnouBarvu() : hracNaRade)} prohrává.", "Čas vypršel.");
                }
            }));
        }
        private void TimerCerna_Tick(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                casNaTah_cerna -= sekunda;
                if (casNaTah_cerna.TotalSeconds % zaXsekundZiskaProtihracJednuSekundu == 0)
                {
                    casNaTah_bila += sekunda;
                    label_vzajemnaCasomira_bila.Text = casNaTah_bila.ToString();
                }
                label_vzajemnaCasomira_black.Text = casNaTah_cerna.ToString();
                if (connectionToChessBoard)
                {
                    UpdateDisplays();
                }
                if (casNaTah_cerna.TotalSeconds == 0)
                {
                    if (ignorovatCasomiru)
                    {
                        return;
                    }
                    Prohra($"Čas vypršel. {(prohoditBarvy ? ZiskatOpacnouBarvu() : hracNaRade)} prohrává.", "Čas vypršel.");
                }
            }));
        }
        private void NCBbutton_remiza_Click(object sender, EventArgs e)
        {
            if (gameState != GameState.CHECKMATE)
            {
                Casomira_Stop();
                DialogResult result = MessageBox.Show("Opravdu jste se dohodli na remíze?", "Remíza?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                Casomira_Start();
                if (result == DialogResult.OK)
                {
                    Remiza("Hráči se dohodli na remíze.");
                }
            }
        }
        private void PrepsatJednuBarvuTextur()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ChessPiece chessPiece = chessBoard[i, j];
                    if (chessPiece is Pawn && !string.IsNullOrEmpty(pawn_white_texturePath))
                    {
                        AktualizovatTexturu(chessPiece, pawn_white_texturePath);
                    }
                    else if (chessPiece is Rook && !string.IsNullOrEmpty(rook_white_texturePath))
                    {
                        AktualizovatTexturu(chessPiece, rook_white_texturePath);
                    }
                    else if (chessPiece is Knight && !string.IsNullOrEmpty(knight_white_texturePath))
                    {
                        AktualizovatTexturu(chessPiece, knight_white_texturePath);
                    }
                    else if (chessPiece is Bishop && !string.IsNullOrEmpty(bishop_white_texturePath))
                    {
                        AktualizovatTexturu(chessPiece, bishop_white_texturePath);
                    }
                    else if (chessPiece is Queen && !string.IsNullOrEmpty(queen_white_texturePath))
                    {
                        AktualizovatTexturu(chessPiece, queen_white_texturePath);
                    }
                    else if (chessPiece is King && !string.IsNullOrEmpty(king_white_texturePath))
                    {
                        AktualizovatTexturu(chessPiece, king_white_texturePath);
                    }
                }
            }
        }
        private void AktualizovatTexturu(ChessPiece chessPiece, string texturePath)
        {
            chessPiece.texture_white = ChessPiece.ScaleImage(Image.FromFile(texturePath), ChessPiece.rescaledImageSize, ChessPiece.rescaledImageSize);
            buttonGrid[chessPiece.row, chessPiece.column].BackgroundImage = 
                chessPiece.playerColor == PlayerColor.WHITE ^ prohoditBarvy ? 
                chessPiece.texture_white : chessPiece.texture_black;
        }
        private void NCBbutton_minimaxAlgoritmus_Click(object sender, EventArgs e)
        {
            if (gameState != GameState.AI_THINKING && gameState != GameState.CHECKMATE)
            {
                selectedPieceRow = -1;
                selectedPieceColumn = -1;
                gameState = GameState.AI_THINKING;
                StockFish();
            }
        }
        private void VratitPohyb()
        {
            pocetTahuOdVyhozeni -= 2;
            tah -= 2;
            hodnotaVeProspechBile += lastScoreChange * -1;

            ChessPiece chessPiece = chessBoard[souradnice_pohybFigurky_cil[0], souradnice_pohybFigurky_cil[1]];
            int GetEnPassantOffset() // if en passant, offset row
            {
                if (!(lastCapturedFig is Pawn pawn)) return 0;
                if (tah - pawn.dvojityPohybBehemTahu_cislo == 0 && (chessPiece.row == 2 || chessPiece.row == 5)) return pawn.row == 3 ? 1 : -1;
                return 0;
            }
            int enpassantOffset = GetEnPassantOffset();
            ZmenaHraceNaRade(); // change current player back
            chessPiece.MovePiece(souradnice_pohybFigurky_pocatek[0], souradnice_pohybFigurky_pocatek[1]); // move back
            buttonGrid[souradnice_pohybFigurky_cil[0] + enpassantOffset, souradnice_pohybFigurky_cil[1]].BackgroundImage =              // show changes
                lastCapturedFig?.playerColor == PlayerColor.WHITE ? lastCapturedFig.texture_white : lastCapturedFig?.texture_black;
            chessBoard[souradnice_pohybFigurky_cil[0] + enpassantOffset, souradnice_pohybFigurky_cil[1]] = lastCapturedFig ?? null;

            if (enpassantOffset != 0)
            {
                buttonGrid[souradnice_pohybFigurky_cil[0], souradnice_pohybFigurky_cil[1]].BackgroundImage = null;
                chessBoard[souradnice_pohybFigurky_cil[0], souradnice_pohybFigurky_cil[1]] = null;
            }

            if (lastCapturedFig is Pawn p) p.hasMoved = lastCapturedFig_hasMoved;
            else if (lastCapturedFig is Rook r) r.hasMoved = lastCapturedFig_hasMoved;
            else if (lastCapturedFig is King k) k.hasMoved = lastCapturedFig_hasMoved;

            buttonGrid[souradnice_pohybFigurky_pocatek[0], souradnice_pohybFigurky_pocatek[1]].BackgroundImage =
                chessPiece.playerColor == PlayerColor.WHITE ? chessPiece.texture_white : chessPiece.texture_black;
            chessBoard[souradnice_pohybFigurky_pocatek[0], souradnice_pohybFigurky_pocatek[1]] = chessPiece;

            if (chessPiece is Pawn p_) p_.hasMoved = lastMovedFig_hasMoved;
            else if (chessPiece is Rook r_) r_.hasMoved = lastMovedFig_hasMoved;
            else if (chessPiece is King k_) k_.hasMoved = lastMovedFig_hasMoved;

            gameState = GameState.SELECT_MOVING_PIECE;
            ZakazatVratitTah();
            ResetHighlighting();
            BarvyRosadovychTlacitek();

            RemoveLastFigFromPicture();
        }
        private void RemoveLastFigFromPicture()
        {
            if (lastCapturedFig is null) return;
            Bitmap obrazek = (Bitmap)pictureBox_vyhozeneFigurky.Image;
            if (lastCapturedFig.playerColor == PlayerColor.WHITE)
            {
                souradniceBilychVyhozenychFigurek.x -= 40;
                for (int i = 0; i < 35; i++)
                {
                    for (int j = 0; j < 35; j++)
                    {
                        obrazek.SetPixel(souradniceBilychVyhozenychFigurek.x + i, souradniceBilychVyhozenychFigurek.y + j, Color.Transparent);
                    }
                }
            }
            else
            {
                souradniceCernychVyhozenychFigurek.x -= 40;
                for (int i = 0; i < 35; i++)
                {
                    for (int j = 0; j < 35; j++)
                    {
                        obrazek.SetPixel(souradniceCernychVyhozenychFigurek.x + i, souradniceCernychVyhozenychFigurek.y + j, Color.Transparent);
                    }
                }
            }
            pictureBox_vyhozeneFigurky.Refresh();
        }
        private void NCBbutton_vratitTah_Click(object sender, EventArgs e)
        {
            if (notaceHry.Count == 0)
            {
                return;
            }
            VratitPohyb();
            if (tah % 2 == 0)
            {
                notaceHry.RemoveAt(notaceHry.Count - 1);
            }
            else
            {
                string posledniTah = notaceHry[notaceHry.Count - 1].Split(' ')[0];
                notaceHry[notaceHry.Count - 1] = posledniTah;
            }
            AktualizovatVypisNotace();
        }
    }
    internal struct ZachovaniBarvyPole
    {
        internal Color color;
        internal int row;
        internal int column;
        public ZachovaniBarvyPole(Color color, int row, int column)
        {
            this.color = color;
            this.row = row;
            this.column = column;
        }
    }
    public abstract class ChessPiece
    {
        internal const int outlineWidth = 6; // outlineWidth = circle diameter
        internal static Color outlineColor = Color.FromArgb(128, Color.Black);
        internal static string directory = Directory.GetCurrentDirectory();
        protected const int imageSize = 70;
        internal static int rescaledImageSize = imageSize * Form1.imageSizeFactor;
        internal PlayerColor playerColor;
        internal abstract int pieceValue { get; }
        internal abstract Image texture_black { get; set; }
        internal abstract Image texture_white { get; set; }
        internal abstract string notationLetter { get; }
        internal int row { get; set; }
        internal int column { get; set; }
        internal int lastRow = -1;
        internal int lastColumn = -1;
        internal int buttonNumber { get; set; }
        internal ZachovaniBarvyPole zachovaniBarvyPole = new ZachovaniBarvyPole(Color.Transparent, -1, -1);
        public virtual void MovePiece(int newRow, int newColumn)
        {
            lastRow = row; lastColumn = column; row = newRow; column = newColumn;
        }
        public virtual void MovePiece(int newRow, int newColumn, ref ChessPiece[,] chessBoard, ref Button[,] buttonGrid)
        {
            lastRow = row; lastColumn = column; row = newRow; column = newColumn;
        }
        public bool IsAtLeftEdge() => column == 0;
        public bool IsAtRightEdge() => column == Form1.ROWS - 1;
        internal List<int[]> dostupnePohybyBehemSachu = new List<int[]>();
        public abstract List<int[]> GetAvailableMoves(ref ChessPiece[,] chessBoard, bool allTiles);
        internal static List<int[]> GetAllPossibleMoves(ref ChessPiece[,] chessBoard, PlayerColor hracNaRade, out King king, out List<ChessPiece> figs, bool allTiles = false)
        {
            figs = new List<ChessPiece>();
            List<int[]> allAvailableMovesOfAllPiecesOfCurrentPlayer = new List<int[]>();
            king = null;
            for (int i = 0; i < chessBoard.GetLength(0); i++)
            {
                for (int j = 0; j < chessBoard.GetLength(1); j++)
                {
                    bool isKingTile = chessBoard[i, j]?.GetType() == typeof(King);
                    if (chessBoard[i, j]?.playerColor == hracNaRade && !isKingTile)
                    {
                        List<int[]> availableMovesOfPiece = chessBoard[i, j].GetAvailableMoves(ref chessBoard, allTiles);
                        allAvailableMovesOfAllPiecesOfCurrentPlayer = allAvailableMovesOfAllPiecesOfCurrentPlayer.Concat(availableMovesOfPiece).ToList();
                        figs.Add(chessBoard[i, j]);
                    }
                    if (isKingTile && chessBoard[i, j]?.playerColor != hracNaRade) // kral opacne barvy
                    {
                        king = (King)chessBoard[i, j];
                        figs.Add(chessBoard[i, j]);
                    }
                }
            }
            return allAvailableMovesOfAllPiecesOfCurrentPlayer;
        }
        internal static Image ScaleImage(Image image, int maxWidth, int maxHeight, bool invertColors = false, bool outline = true)
        {
            double ratioX = (double)maxWidth / image.Width;
            double ratioY = (double)maxHeight / image.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            Bitmap newImage = new Bitmap(maxWidth, maxHeight);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                // Calculate x and y which center the image
                int y = (maxHeight / 2) - newHeight / 2;
                int x = (maxWidth / 2) - newWidth / 2;

                // Draw image on x and y with newWidth and newHeight
                graphics.DrawImage(image, x, y, newWidth, newHeight);
                graphics.Dispose();
            }

            if (invertColors && !Form1.odlisnaBilaCernaTextura)
            {
                for (int y = 0; y <= newImage.Height - 1; y++)
                {
                    for (int x = 0; x <= newImage.Width - 1; x++)
                    {
                        Color inv = newImage.GetPixel(x, y);
                        if (inv.A == 0)
                        {
                            continue;
                        }
                        inv = Color.FromArgb(255, 255, 255, 255);
                        newImage.SetPixel(x, y, inv);
                    }
                }
            }
            if (outline)
            {
                newImage = DrawOutlineAroundBitmap(newImage);
            }
            return newImage;
        }
        private static Bitmap DrawOutlineAroundBitmap(Bitmap original)
        {
            Bitmap copyOfOriginal = (Bitmap)original.Clone();
            Color lastPixel = Color.FromArgb(0, 0, 0, 0);
            for (int i = 0; i < original.Width; i++) // vertical
            {
                for (int j = 0; j < original.Height; j++)
                {
                    Color currentPixel = original.GetPixel(i, j);

                    if (lastPixel.A != currentPixel.A) // potkání se s tělem (přechod transparentní -> netransparentní
                    {
                        DrawCircleOutline(copyOfOriginal, i, j);
                    }

                    lastPixel = currentPixel;
                }
            }
            for (int i = 0; i < original.Width; i++) // horizontal
            {
                for (int j = 0; j < original.Height; j++)
                {
                    Color currentPixel = original.GetPixel(j, i);

                    if (lastPixel.A != currentPixel.A)
                    {
                        DrawCircleOutline(copyOfOriginal, j, i);
                    }

                    lastPixel = currentPixel;
                }
            }
            original = copyOfOriginal;
            return original;
        }
        private static void DrawCircleOutline(Bitmap bitmap, int x, int y)
        {
            // čtverec kružnici opsaný
            for (int i = x - outlineWidth / 2; i <= x + outlineWidth / 2; i++)
            {
                for (int j = y - outlineWidth / 2; j <= y + outlineWidth / 2; j++)
                {
                    if (i >= bitmap.Width || j >= bitmap.Height || i < 0 || j < 0)
                    {
                        continue; // Jsou souřadnice bodu platné?
                    }
                    if (IsPointInsideCircle(x, y, i, j))
                    {
                        if (bitmap.GetPixel(i, j).A == 0) // je pixel průhledný?
                        {
                            bitmap.SetPixel(i, j, outlineColor);
                        }
                    }
                }
            }
        }
        private static bool IsPointInsideCircle(int centerX, int centerY, int pointX, int pointY)
        {
            return Math.Pow(pointX - centerX, 2) + Math.Pow(pointY - centerY, 2) < outlineWidth;
        }
        public override string ToString()
        {
            return playerColor + " " + this.GetType().ToString() + " at " + Form1.TranslateCoordinates(row, column) + " started at btn #" + buttonNumber;
        }
    }
    internal class Pawn : ChessPiece
    {
        internal static string texturePath = "..\\..\\textures\\default\\pawn_chess.png";
        internal override Image texture_black { get; set; }
        internal override Image texture_white { get; set; }
        internal bool hasMoved = false;
        internal override int pieceValue { get => 1; }
        internal override string notationLetter { get => string.Empty; }
#if EnPassant
        internal int dvojityPohybBehemTahu_cislo = -2;
#endif
        internal List<int[]> enPassantPohyby = new List<int[]>();
        public Pawn(PlayerColor playerColor, int x, int y, int buttonNumber)
        {
            Image image = Image.FromFile(texturePath);
            if (playerColor == PlayerColor.WHITE)
            {
                texture_white = ScaleImage(image, rescaledImageSize, rescaledImageSize, true ^ Form1.prohoditBarvy);
            }
            else
            {
                texture_black = ScaleImage(image, rescaledImageSize, rescaledImageSize, false ^ Form1.prohoditBarvy);
            }
            image.Dispose();
            this.playerColor = playerColor;
            this.row = x;
            this.column = y;
            this.buttonNumber = buttonNumber;
        }
        public override List<int[]> GetAvailableMoves(ref ChessPiece[,] chessBoard, bool allTiles = false)
        {
            List<int[]> availableMoves = new List<int[]>();
            if (playerColor == PlayerColor.WHITE)
            {
                if (chessBoard[row - 1, column] is null && !allTiles) // IsAtTop // pěšec nemůže vyhazovat před sebou
                {
                    availableMoves.Add(new int[] { row - 1, column });

                    if (!hasMoved && chessBoard[row - 2, column] is null && !allTiles)
                    {
                        availableMoves.Add(new int[] { row - 2, column });
                    }
                }
                if (!IsAtLeftEdge())
                {
                    if (!(chessBoard[row - 1, column - 1] is null) || allTiles) // pěšec může jedině vyhazovat v úlopříčce
                    {
                        if (chessBoard[row - 1, column - 1]?.playerColor != playerColor || allTiles)// || (chessBoard[row - 1, column - 1] is null))
                        {
                            availableMoves.Add(new int[] { row - 1, column - 1 });
                        }
                    }


                }
                if (!IsAtRightEdge())
                {
                    if (!(chessBoard[row - 1, column + 1] is null) || allTiles)
                    {
                        if (chessBoard[row - 1, column + 1]?.playerColor != playerColor || allTiles)// || (chessBoard[row - 1, column - 1] is null))
                        {
                            availableMoves.Add(new int[] { row - 1, column + 1 });
                        }
                    }
                }
            }
            else
            {
                if (chessBoard[row + 1, column] is null && !allTiles) // IsAtBottom
                {
                    availableMoves.Add(new int[] { row + 1, column });

                    if (!hasMoved && chessBoard[row + 2, column] is null && !allTiles)
                    {
                        availableMoves.Add(new int[] { row + 2, column });
                    }
                }
                if (!IsAtRightEdge()) // 
                {
                    if (!(chessBoard[row + 1, column + 1] is null) || allTiles)
                    {
                        if (chessBoard[row + 1, column + 1]?.playerColor != playerColor || allTiles)// || (chessBoard[row - 1, column - 1] is null))
                        {
                            availableMoves.Add(new int[] { row + 1, column + 1 });
                        }
                    }
                }
                if (!IsAtLeftEdge())
                {
                    if (!(chessBoard[row + 1, column - 1] is null) || allTiles)
                    {
                        if (chessBoard[row + 1, column - 1]?.playerColor != playerColor || allTiles)// || (chessBoard[row - 1, column - 1] is null))
                        {
                            availableMoves.Add(new int[] { row + 1, column - 1 });
                        }
                    }
                }
            }
            dostupnePohybyBehemSachu = availableMoves;
            return availableMoves;
        }
        internal bool EnPassantCheck(ref ChessPiece[,] chessBoard, out List<int[]> enPassantPohyby)
        {
            // en passant: jestli je proveditelný
            this.enPassantPohyby.Clear();
            enPassantPohyby = this.enPassantPohyby;
            if (column - 1 >= 0)
            {
                if (chessBoard[row, column - 1] is Pawn pawn)
                {
                    if (this.playerColor != pawn.playerColor &&
                        Form1.tah - pawn.dvojityPohybBehemTahu_cislo == 1)
                    {
                        enPassantPohyby.Add(new int[] {
                            this.playerColor == PlayerColor.BLACK ? row + 1 : row - 1,
                            column - 1 }
                        );
                    }
                }
            }

            if (column + 1 < 8)
            {
                if (chessBoard[row, column + 1] is Pawn pawn)
                {
                    if (this.playerColor != pawn.playerColor &&
                        Form1.tah - pawn.dvojityPohybBehemTahu_cislo == 1)
                    {
                        enPassantPohyby.Add(new int[] {
                            this.playerColor == PlayerColor.BLACK ? row + 1 : row - 1,
                            column + 1 });
                    }
                }
            }

            return enPassantPohyby.Count > 0;
        }
        private static NahradyZaPesaka TazatSeNeZmenuPesaka()
        {
            
            if (char.IsLetter(Form1.AI_volbaPromocePesaka))
            {
                switch (Form1.AI_volbaPromocePesaka)
                {
                    case 'q':
                        return NahradyZaPesaka.QUEEN;
                    case 'r':
                        return NahradyZaPesaka.ROOK;
                    case 'n':
                        return NahradyZaPesaka.KNIGHT;
                    case 'b':
                        return NahradyZaPesaka.BISHOP;
                }
            }
            Form form2 = new ChoosePawnAlternativeWindow();
            DialogResult zvolenaFigurka = form2.ShowDialog();
            switch (zvolenaFigurka)
            {
                case DialogResult.OK:
                    return NahradyZaPesaka.QUEEN;
                case DialogResult.Yes:
                    return NahradyZaPesaka.KNIGHT;
                case DialogResult.No:
                    return NahradyZaPesaka.ROOK;
                case DialogResult.Ignore:
                    return NahradyZaPesaka.BISHOP;
            }
            return NahradyZaPesaka.QUEEN;
        }
        private void NahradaZaPesaka(int newRow, int newColumn, ref ChessPiece[,] chessBoard, ref Button[,] buttonGrid)
        {
            NahradyZaPesaka nahradaZaPesaka = TazatSeNeZmenuPesaka();
            string texture = string.Empty;
            switch (nahradaZaPesaka)
            {
                case NahradyZaPesaka.QUEEN:
                    chessBoard[row, column] = new Queen(playerColor, newRow, newColumn, buttonNumber);
                    texture = Form1.queen_white_texturePath;
                    ChangeNotation("Q");
                    break;
                case NahradyZaPesaka.ROOK:
                    chessBoard[row, column] = new Rook(playerColor, newRow, newColumn, buttonNumber);
                    texture = Form1.rook_white_texturePath;
                    ChangeNotation("R");
                    break;
                case NahradyZaPesaka.BISHOP:
                    chessBoard[row, column] = new Bishop(playerColor, newRow, newColumn, buttonNumber);
                    texture = Form1.bishop_white_texturePath;
                    ChangeNotation("B");
                    break;
                case NahradyZaPesaka.KNIGHT:
                    chessBoard[row, column] = new Knight(playerColor, newRow, newColumn, buttonNumber);
                    texture = Form1.knight_white_texturePath;
                    ChangeNotation("N");
                    break;
            }
            buttonGrid[row, column].BackgroundImage = (chessBoard[row, column].playerColor == PlayerColor.WHITE ?
                (!string.IsNullOrEmpty(texture) ? ChessPiece.ScaleImage(Image.FromFile(texture), 70, 70) : chessBoard[row, column].texture_white) : chessBoard[row, column].texture_black);
        }
        private void ChangeNotation(string change)
        {
            Form1.notaceHry[Form1.notaceHry.Count - 1] += '=' + change;
        }
        public override void MovePiece(int newRow, int newColumn, ref ChessPiece[,] chessBoard, ref Button[,] buttonGrid)
        {
#if EnPassant
            // en passant: zapamatování tahu dvojitého pohybu
            if (!hasMoved && (newRow == 3 || newRow == 4))//(Math.Abs(newRow - row) == 2)
            {
                dvojityPohybBehemTahu_cislo = Form1.tah;
            }
#endif
            hasMoved = true;
            base.MovePiece(newRow, newColumn);
            //row = newRow;
            //column = newColumn;
            if (newRow == 0 && playerColor == PlayerColor.WHITE)
            {
                NahradaZaPesaka(newRow, newColumn, ref chessBoard, ref buttonGrid);
            }
            else if (newRow == Form1.ROWS - 1 && playerColor == PlayerColor.BLACK)
            {
                NahradaZaPesaka(newRow, newColumn, ref chessBoard, ref buttonGrid);
            }
        }
    }
    internal class Rook : ChessPiece
    {
        internal static string texturePath = "..\\..\\textures\\default\\rook_chess.png";
        internal override Image texture_black { get; set; }
        internal override Image texture_white { get; set; }
        internal bool hasMoved = false;
        internal override int pieceValue { get => 5; }
        internal override string notationLetter { get => "R"; }
        public Rook(PlayerColor playerColor, int x, int y, int buttonNumber)
        {
            Image image = Image.FromFile(texturePath);
            if (playerColor == PlayerColor.WHITE)
            {
                texture_white = ScaleImage(image, rescaledImageSize, rescaledImageSize, true ^ Form1.prohoditBarvy);
            }
            else
            {
                texture_black = ScaleImage(image, rescaledImageSize, rescaledImageSize, false ^ Form1.prohoditBarvy);
            }
            image.Dispose();
            this.playerColor = playerColor;
            this.row = x;
            this.column = y;
            this.buttonNumber = buttonNumber;
        }
        public override List<int[]> GetAvailableMoves(ref ChessPiece[,] chessBoard, bool allTiles)
        {
            List<int[]> availableMoves = new List<int[]>();
            // Position to bottom
            bool breakNextTile = false;
            for (int i = row + 1; i < 8; i++)
            {
                if (chessBoard[i, column] is null)
                {
                    availableMoves.Add(new int[] { i, column });
                    if (breakNextTile)
                    {
                        break;
                    }
                }
                else if (!(chessBoard[i, column] is null))
                {
                    if (chessBoard[i, column].playerColor != playerColor || allTiles)
                    {
                        availableMoves.Add(new int[] { i, column });
                    }
                    if (allTiles && chessBoard[i, column].GetType() == typeof(King) && chessBoard[i, column]?.playerColor != playerColor)
                    {
                        breakNextTile = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Position to top
            breakNextTile = false;
            for (int i = row - 1; i >= 0; i--)
            {
                if (chessBoard[i, column] is null)
                {
                    availableMoves.Add(new int[] { i, column });
                    if (breakNextTile)
                    {
                        break;
                    }
                }
                else if (!(chessBoard[i, column] is null))
                {
                    if (chessBoard[i, column].playerColor != playerColor || allTiles)
                    {
                        availableMoves.Add(new int[] { i, column });
                    }
                    if (allTiles && chessBoard[i, column].GetType() == typeof(King) && chessBoard[i, column]?.playerColor != playerColor)
                    {
                        breakNextTile = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Position to right
            breakNextTile = false;
            for (int i = column + 1; i < 8; i++)
            {
                if (chessBoard[row, i] is null)
                {
                    availableMoves.Add(new int[] { row, i });
                    if (breakNextTile)
                    {
                        break;
                    }
                }
                else if (!(chessBoard[row, i] is null))
                {
                    if (chessBoard[row, i].playerColor != playerColor || allTiles)
                    {
                        availableMoves.Add(new int[] { row, i });
                    }
                    if (allTiles && chessBoard[row, i].GetType() == typeof(King) && chessBoard[i, column]?.playerColor != playerColor)
                    {
                        breakNextTile = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Position to left
            breakNextTile = false;
            for (int i = column - 1; i >= 0; i--)
            {
                if (chessBoard[row, i] is null)
                {
                    availableMoves.Add(new int[] { row, i });
                    if (breakNextTile)
                    {
                        break;
                    }
                }
                else if (!(chessBoard[row, i] is null))
                {
                    if (chessBoard[row, i].playerColor != playerColor || allTiles)
                    {
                        availableMoves.Add(new int[] { row, i });
                    }
                    if (allTiles && chessBoard[row, i].GetType() == typeof(King) && chessBoard[i, column]?.playerColor != playerColor)
                    {
                        breakNextTile = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            dostupnePohybyBehemSachu = availableMoves;
            return availableMoves;
        }
        public override void MovePiece(int newRow, int newColumn)
        {
            base.MovePiece(newRow, newColumn);
            hasMoved = true;
        }
        public override void MovePiece(int newRow, int newColumn, ref ChessPiece[,] chessBoard, ref Button[,] buttonGrid)
        {
            base.MovePiece(newRow, newColumn, ref chessBoard, ref buttonGrid);
            hasMoved = true;
        }
    }
    internal class Knight : ChessPiece
    {
        internal static string texturePath = "..\\..\\textures\\default\\knight_chess.png";
        internal override Image texture_black { get; set; }
        internal override Image texture_white { get; set; }
        internal override int pieceValue { get => 3; }
        internal override string notationLetter { get => "N"; }
        public Knight(PlayerColor playerColor, int x, int y, int buttonNumber)
        {
            Image image = Image.FromFile(texturePath);
            if (playerColor == PlayerColor.WHITE)
            {
                texture_white = ScaleImage(image, rescaledImageSize, rescaledImageSize, true ^ Form1.prohoditBarvy);
            }
            else
            {
                texture_black = ScaleImage(image, rescaledImageSize, rescaledImageSize, false ^ Form1.prohoditBarvy);
            }
            image.Dispose();
            this.playerColor = playerColor;
            this.row = x;
            this.column = y;
            this.buttonNumber = buttonNumber;
        }
        public override List<int[]> GetAvailableMoves(ref ChessPiece[,] chessBoard, bool allTiles)
        {
            List<int[]> availableMoves = new List<int[]>();
            int[,] jumps = new int[,]
            {
                { -2, 1},
                { -2, -1},
                { 1, 2},
                { -1, 2},
                { 2, 1},
                { 2, -1},
                { 1, -2},
                { -1, -2},
            };
            for (int i = 0; i < jumps.GetLength(0); i++)
            {
                int jumpRow = row + jumps[i, 0];
                int jumpColumn = column + jumps[i, 1];
                if (jumpRow >= 0 && jumpRow <= 7 && jumpColumn >= 0 && jumpColumn <= 7) // platne souradnice
                {
                    if (chessBoard[jumpRow, jumpColumn] is null)
                    {
                        availableMoves.Add(new int[] { jumpRow, jumpColumn });
                    }
                    else if (chessBoard[jumpRow, jumpColumn].playerColor != playerColor || allTiles)
                    {
                        availableMoves.Add(new int[] { jumpRow, jumpColumn });
                    }
                }
            }
            dostupnePohybyBehemSachu = availableMoves;
            return availableMoves;
        }
    }
    internal class Bishop : ChessPiece
    {
        internal static string texturePath = "..\\..\\textures\\default\\bishop_chess.png";
        internal override Image texture_black { get; set; }
        internal override Image texture_white { get; set; }
        internal override int pieceValue { get => 3; }
        internal override string notationLetter { get => "B"; }
        public Bishop(PlayerColor playerColor, int x, int y, int buttonNumber)
        {
            Image image = Image.FromFile(texturePath);
            if (playerColor == PlayerColor.WHITE)
            {
                texture_white = ScaleImage(image, rescaledImageSize, rescaledImageSize, true ^ Form1.prohoditBarvy);
            }
            else
            {
                texture_black = ScaleImage(image, rescaledImageSize, rescaledImageSize, false ^ Form1.prohoditBarvy);
            }
            image.Dispose(); this.playerColor = playerColor;
            this.row = x;
            this.column = y;
            this.buttonNumber = buttonNumber;
        }
        public override List<int[]> GetAvailableMoves(ref ChessPiece[,] chessBoard, bool allTiles)
        {
            List<int[]> availableMoves = new List<int[]>();
            int[,] jumps = new int[,]
            {
                { 1, 1 },
                { -1, 1 },
                { -1, -1 },
                { 1, -1 }
            };
            // NE NW SE SW
            bool breakNextTile = false;
            for (int i = 0; i < jumps.GetLength(0); i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    int jumpRow = row + jumps[i, 0] * j;
                    int jumpColumn = column + jumps[i, 1] * j;
                    if (jumpRow >= 0 && jumpRow <= 7 && jumpColumn >= 0 && jumpColumn <= 7) // platne souradnice
                    {
                        if (chessBoard[jumpRow, jumpColumn] is null)
                        {
                            availableMoves.Add(new int[] { jumpRow, jumpColumn });
                            if (breakNextTile)
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (chessBoard[jumpRow, jumpColumn].playerColor != playerColor || allTiles)
                            {
                                availableMoves.Add(new int[] { jumpRow, jumpColumn });
                            }
                            if (allTiles && chessBoard[jumpRow, jumpColumn].GetType() == typeof(King) && chessBoard[jumpRow, jumpColumn]?.playerColor != playerColor)
                            {
                                breakNextTile = true;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            dostupnePohybyBehemSachu = availableMoves;
            return availableMoves;
        }
    }
    internal class Queen : ChessPiece
    {
        internal static string texturePath = "..\\..\\textures\\default\\queen_chess.png";
        internal override Image texture_black { get; set; }
        internal override Image texture_white { get; set; }
        internal override int pieceValue { get => 9; }
        internal override string notationLetter { get => "Q"; }
        public Queen(PlayerColor playerColor, int x, int y, int buttonNumber)
        {
            Image image = Image.FromFile(texturePath);
            if (playerColor == PlayerColor.WHITE)
            {
                texture_white = ScaleImage(image, rescaledImageSize, rescaledImageSize, true ^ Form1.prohoditBarvy);
            }
            else
            {
                texture_black = ScaleImage(image, rescaledImageSize, rescaledImageSize, false ^ Form1.prohoditBarvy);
            }
            image.Dispose(); this.playerColor = playerColor;
            this.row = x;
            this.column = y;
            this.buttonNumber = buttonNumber;
        }
        public override List<int[]> GetAvailableMoves(ref ChessPiece[,] chessBoard, bool allTiles)
        {
            List<int[]> availableMoves = new List<int[]>();
            bool breakNextTile = false;
            for (int i = row + 1; i < 8; i++)
            {
                if (chessBoard[i, column] is null)
                {
                    availableMoves.Add(new int[] { i, column });
                    if (breakNextTile)
                    {
                        break;
                    }
                }
                else if (!(chessBoard[i, column] is null))
                {
                    if (chessBoard[i, column].playerColor != playerColor || allTiles)
                    {
                        availableMoves.Add(new int[] { i, column });
                    }
                    if (allTiles && chessBoard[i, column].GetType() == typeof(King) && chessBoard[i, column]?.playerColor != playerColor)
                    {
                        breakNextTile = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Position to top
            breakNextTile = false;
            for (int i = row - 1; i >= 0; i--)
            {
                if (chessBoard[i, column] is null)
                {
                    availableMoves.Add(new int[] { i, column });
                    if (breakNextTile)
                    {
                        break;
                    }
                }
                else if (!(chessBoard[i, column] is null))
                {
                    if (chessBoard[i, column].playerColor != playerColor || allTiles)
                    {
                        availableMoves.Add(new int[] { i, column });
                    }
                    if (allTiles && chessBoard[i, column].GetType() == typeof(King) && chessBoard[i, column]?.playerColor != playerColor)
                    {
                        breakNextTile = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Position to right
            breakNextTile = false;
            for (int i = column + 1; i < 8; i++)
            {
                if (chessBoard[row, i] is null)
                {
                    availableMoves.Add(new int[] { row, i });
                    if (breakNextTile)
                    {
                        break;
                    }
                }
                else if (!(chessBoard[row, i] is null))
                {
                    if (chessBoard[row, i].playerColor != playerColor || allTiles)
                    {
                        availableMoves.Add(new int[] { row, i });
                    }
                    if (allTiles && chessBoard[row, i].GetType() == typeof(King) && chessBoard[i, column]?.playerColor != playerColor)
                    {
                        breakNextTile = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Position to left
            breakNextTile = false;
            for (int i = column - 1; i >= 0; i--)
            {
                if (chessBoard[row, i] is null)
                {
                    availableMoves.Add(new int[] { row, i });
                    if (breakNextTile)
                    {
                        break;
                    }
                }
                else if (!(chessBoard[row, i] is null))
                {
                    if (chessBoard[row, i].playerColor != playerColor || allTiles)
                    {
                        availableMoves.Add(new int[] { row, i });
                    }
                    if (allTiles && chessBoard[row, i].GetType() == typeof(King) && chessBoard[i, column]?.playerColor != playerColor)
                    {
                        breakNextTile = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            int[,] jumpsBishop = new int[,]
            {
                { 1, 1 },
                { -1, 1 },
                { -1, -1 },
                { 1, -1 }
            };
            breakNextTile = false;
            for (int i = 0; i < jumpsBishop.GetLength(0); i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    int jumpRow = row + jumpsBishop[i, 0] * j;
                    int jumpColumn = column + jumpsBishop[i, 1] * j;
                    if (jumpRow >= 0 && jumpRow <= 7 && jumpColumn >= 0 && jumpColumn <= 7) // platne souradnice
                    {
                        if (chessBoard[jumpRow, jumpColumn] is null)
                        {
                            availableMoves.Add(new int[] { jumpRow, jumpColumn });
                            if (breakNextTile)
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (chessBoard[jumpRow, jumpColumn].playerColor != playerColor || allTiles)
                            {
                                availableMoves.Add(new int[] { jumpRow, jumpColumn });
                            }
                            if (allTiles && chessBoard[jumpRow, jumpColumn].GetType() == typeof(King) && chessBoard[jumpRow, jumpColumn]?.playerColor != playerColor)
                            {
                                breakNextTile = true;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            dostupnePohybyBehemSachu = availableMoves;
            return availableMoves;
        }
    }
    internal class King : ChessPiece
    {
        internal static string texturePath = "..\\..\\textures\\default\\king_chess.png";
        internal override Image texture_black { get; set; }
        internal override Image texture_white { get; set; }
        internal bool jeSachovan = false;
        internal List<int[]> moznePohybyVsechFigurek = new List<int[]>(); // maximalni počet polí, kam král nesmí vstoupit
        internal static CoordinateComparer comparer = new CoordinateComparer();
        internal List<int[]> availableMoves = new List<int[]>();
        internal List<int[]> zakazanePohyby = new List<int[]>();
        internal bool hasMoved = false;
        internal override int pieceValue { get => 999; }
        internal override string notationLetter { get => "K"; }
        internal List<int[]> rosadovePohyby = new List<int[]>();
        public King(PlayerColor playerColor, int x, int y, int buttonNumber)
        {
            Image image = Image.FromFile(texturePath);
            if (playerColor == PlayerColor.WHITE)
            {
                texture_white = ScaleImage(image, rescaledImageSize, rescaledImageSize, true ^ Form1.prohoditBarvy);
            }
            else
            {
                texture_black = ScaleImage(image, rescaledImageSize, rescaledImageSize, false ^ Form1.prohoditBarvy);
            }
            image.Dispose(); this.playerColor = playerColor;
            this.row = x;
            this.column = y;
            this.buttonNumber = buttonNumber;
        }
        internal void PridatRosadovePohyby(int[] pohyb)
        {
            rosadovePohyby.Add(pohyb);
            dostupnePohybyBehemSachu.Add(pohyb);
            
        }
        internal void SmazatRosadovePohyby()
        {
            rosadovePohyby.Clear();
        }
        private static List<int[]> ZiskatSpolecnePohyby(ref List<int[]> pohyby1, ref List<int[]> pohyby2)
        {
            List<int[]> spolecnePohyby = pohyby1.Intersect(pohyby2, comparer).ToList();
            return spolecnePohyby;
        }
        private static List<int[]> ZiskatRozdilnePohyby(ref List<int[]> pohyby1, ref List<int[]> pohyby2)
        {
            List<int[]> spolecnePohyby = new List<int[]>();
            foreach (var item in pohyby1)
            {
                if (!pohyby2.Contains(item, comparer))
                {
                    spolecnePohyby.Add(item);
                }
            }
            return spolecnePohyby;
        }
        internal static bool JeKralSachovan(ref List<int[]> pohyby, int rowOfKing, int columnOfKing)
        {
            return pohyby.Any(x => x[0] == rowOfKing && x[1] == columnOfKing);
        }
        public override List<int[]> GetAvailableMoves(ref ChessPiece[,] chessBoard, bool _)
        {
            availableMoves.Clear();
            zakazanePohyby.Clear();
            int[,] jumps = new int[,]
            {
                { 1, 1 },
                { 1, -1 },
                { -1, 1 },
                { -1, -1 },
                { 0, 1},
                { 0, -1 },
                { 1, 0 },
                { -1, 0 }
            };

            for (int i = 0; i < jumps.GetLength(0); i++)
            {
                int jumpRow = row + jumps[i, 0];
                int jumpColumn = column + jumps[i, 1];
                if (jumpRow >= 0 && jumpRow <= 7 && jumpColumn >= 0 && jumpColumn <= 7)
                {
                    if (chessBoard[jumpRow, jumpColumn] is null)
                    {
                        availableMoves.Add(new int[] { jumpRow, jumpColumn });
                    }
                    else
                    {
                        if (chessBoard[jumpRow, jumpColumn].playerColor != playerColor && !Form1.atomicMode)
                        {
                            availableMoves.Add(new int[] { jumpRow, jumpColumn });
                        }
                    }
                }
            }
            PlayerColor opacnaBarva = playerColor == PlayerColor.WHITE ? PlayerColor.BLACK : PlayerColor.WHITE;
            moznePohybyVsechFigurek = GetAllPossibleMoves(ref chessBoard, opacnaBarva, out King _, out List<ChessPiece> _, true);
            jeSachovan = JeKralSachovan(ref moznePohybyVsechFigurek, row, column);


            zakazanePohyby = ZiskatSpolecnePohyby(ref availableMoves, ref moznePohybyVsechFigurek);
            zakazanePohyby.AddRange(RemoveOppositeKingTiles(ref chessBoard));
            availableMoves = ZiskatRozdilnePohyby(ref availableMoves, ref zakazanePohyby);
            dostupnePohybyBehemSachu = availableMoves;
            return availableMoves;
        }
        private List<int[]> RemoveOppositeKingTiles(ref ChessPiece[,] chessBoard)
        {
            King oppositeKing = null;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (chessBoard[i, j] is King king && chessBoard[i, j]?.playerColor != this.playerColor)
                    {
                        oppositeKing = king;
                        break;
                    }
                }
            }
            List<int[]> oppositeKingMoves = new List<int[]>();
            for (int i = oppositeKing.row - 1; i <= oppositeKing.row + 1; i++)
            {
                for (int j = oppositeKing.column - 1; j <= oppositeKing.column + 1; j++)
                {
                    oppositeKingMoves.Add(new int[] { i, j });
                }
            }
            return oppositeKingMoves;
        }
        public override void MovePiece(int newRow, int newColumn)
        {
            base.MovePiece(newRow, newColumn);
            hasMoved = true;
        }
        public override void MovePiece(int newRow, int newColumn, ref ChessPiece[,] chessBoard, ref Button[,] buttonGrid)
        {
            base.MovePiece(newRow, newColumn, ref chessBoard, ref buttonGrid);
            hasMoved = true;
        }
    }
    public class CoordinateComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] coords1, int[] coords2)
        {
            return coords1[0] == coords2[0] && coords1[1] == coords2[1];
        }
        public int GetHashCode(int[] obj)
        {
            return -1;
        }
    }
    public enum PlayerColor
    {
        WHITE,
        BLACK
    }
    public enum GameState
    {
        SELECT_MOVING_PIECE,
        SELECT_TARGET_TILE,
        CHECKMATE,
        AI_THINKING
    }
    public enum NahradyZaPesaka
    {
        QUEEN,
        ROOK,
        BISHOP,
        KNIGHT
    }
    public enum WhitePiecesChars
    {
        PAWN = 0x2659,
        ROOK = 0x2656,
        KNIGHT = 0x2658,
        BISHOP = 0x2657,
        QUEEN = 0x2655,
        KING = 0x2654
    }
    public enum BlackPiecesChars
    {
        PAWN = 0x265f,
        ROOK = 0x265c,
        KNIGHT = 0x265e,
        BISHOP = 0x265d,
        QUEEN = 0x265b,
        KING = 0x265a
    }
}