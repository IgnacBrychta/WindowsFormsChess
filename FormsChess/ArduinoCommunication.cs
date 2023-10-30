#define HEZKY_VYPIS
#define _SERIAL_PORT_DEBUG
#define _DEBUG_SHOW_ARRAY

using System;
using System.Collections;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;
using System.Linq;

namespace FormsChess
{
    public partial class Form1
    {
        const int columnsOfArduinoDisplay = 16;
        const int numOfDiodes = 256;
        Point capturedFigWhitePos = new Point(-2, 8);
        Point capturedFigBlackPos = new Point( 2, 8);
        private void CheckIfReceivedMoveValid(int row, int column)
        {
            if(!availableMovesOnChessboard.Any(x => x.Key[0] == row && x.Key[1] == column))
            {
                SendInvalidMoveData(row, column);
                return;
            }
            ProvestKliknuti(row, column, true);
        }
        private void SendInvalidMoveData(int invalidRow, int invalidColumn)
        {
            BitArray bitArray = new BitArray(numOfDiodes);
            int bitArrayIndex = 0;
            // set chessboard red
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    bitArray[bitArrayIndex] = !(chessBoard[i, j] is null);
                    bitArrayIndex++;
                    byte colorComb = GetByteCombinationOfColor(sachovaciBarva);
                    for (int k = 0; k < 3; k++)
                    {
                        bool bit = GetBitFromByte(colorComb, k);
                        bitArray.Set(bitArrayIndex, bit);
                        bitArrayIndex++;
                    }
                }
            }
            // set invalid tile blank
            bitArrayIndex = invalidRow * ROWS + invalidColumn;
            for (int k = 0; k < 3; k++)
            {
                bool bit = GetBitFromByte(GetByteCombinationOfColor(chessBoardColor1), k);
                bitArray.Set(bitArrayIndex, bit);
                bitArrayIndex++;
            }
            serialPort.Write(new byte[] { 0b00000001 }, 0, 1);
            byte[] byteArray = BitArrayToByteArray(bitArray);
            serialPort.Write(byteArray, 0, 32);
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            return;
            Thread.Sleep(5);
            string serialPortData = serialPort.ReadExisting();
#if SERIAL_PORT_DEBUG
            MessageBox.Show(serialPortData, "Serial Port: received data");
#endif
            //Thread.Sleep(5);
            string[] serialPortData_Lines = serialPortData.Split(serialPort.NewLine.ToCharArray());
            if (serialPortData.StartsWith("piece moved"))
            {
                return;
                int index = int.Parse(serialPortData_Lines[1]);
                string simulatedButtonClickName = $"button{index + 1}";
                GetRowAndColumnFromButton(simulatedButtonClickName, out _, out int row, out int column);
                CheckIfReceivedMoveValid(row, column);
            }
            else if (serialPortData.StartsWith("button pressed"))
            {
                while(gameState == GameState.AI_THINKING) { Thread.Sleep(5); }

                switch (serialPortData_Lines[1])
                {
                    case "1":
                        if (hracNaRade != PlayerColor.WHITE) { return; }
                        else if (gameState == GameState.CHECKMATE) { return; }
                        Invoke(new Action(() =>
                        {
                            Prohra("Hráč se vzdal.", "Prohra");
                        }));
                        break;
                    case "2":
                        if (hracNaRade == PlayerColor.WHITE) { return; }
                        else if (gameState == GameState.CHECKMATE) { return; }
                        Invoke(new Action(() =>
                        {
                            Prohra("Hráč se vzdal.", "Prohra");
                        }));
                        break;
                    case "3":
                        if (gameState == GameState.CHECKMATE) { return; }
                        Invoke(new Action(() =>
                        {
                            NCBbutton_remiza_Click(this, new EventArgs());
                        }));
                        break;
                    case "4":
                        NCBbutton_Restart_Click(this, new EventArgs());
                        break;
                    case "5":
                        if (gameState == GameState.CHECKMATE) { return; }
                        NCBbutton_minimaxAlgoritmus_Click(this, new EventArgs());
                        break;
                    case "6":
                        if (gameState == GameState.CHECKMATE) { return; }
                        else if(!NCBbutton_vratitTah.Enabled) { return; }
                        VratitPohyb();
                        break;
                }
            }
        }
        private void SendGameEnding(string message)
        {
            while (message.Length < columnsOfArduinoDisplay) { message += ' '; }
            if (message.Length > columnsOfArduinoDisplay) { message = message.Substring(0, columnsOfArduinoDisplay); }
            message = RemoveDiacriticsFromString(message);
            serialPort.Write(new byte[] { 0b0000_0100 }, 0, 1);
            serialPort.Write(message);
        }
        private string RemoveDiacriticsFromString(string message)
        {
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(message);
            return System.Text.Encoding.UTF8.GetString(tempBytes);
        }
        private void SendChessBoardState()
        {
            if (!serialPort.IsOpen)
            {
                return;
            }
            serialPort.Write(new byte[] { 0b00000001 }, 0, 1);
            BitArray bitArray = GetChessBoardState();
            byte[] byteArray = BitArrayToByteArray(bitArray);
            serialPort.Write(byteArray, 0, 32);
        }
        private void SendCaptureFigCommand(int row, int column)
        {
            //SendChessBoardState();
            return;
            serialPort.Write(new byte[] { 0b0000_0101 }, 0, 1);
            char[] moveData = new char[4];
            moveData[0] = Convert.ToChar(row);
            moveData[1] = Convert.ToChar(column);

            PlayerColor capturedPieceColor = chessBoard[row, column].playerColor;
            if(capturedPieceColor == PlayerColor.WHITE)
            {
                if (capturedFigWhitePos.x == 7) 
                {
                    capturedFigWhitePos.x = 0;
                    capturedFigWhitePos.y++;
                }
                capturedFigWhitePos.x++;
                moveData[2] = Convert.ToChar(capturedFigWhitePos.x);
                moveData[3] = Convert.ToChar(capturedFigWhitePos.y);
            }
            else
            {
                if (capturedFigBlackPos.x == 7)
                {
                    capturedFigBlackPos.x = 0;
                    capturedFigBlackPos.y--;
                }
                capturedFigBlackPos.x++;
                moveData[2] = Convert.ToChar(capturedFigBlackPos.x);
                moveData[3] = Convert.ToChar(capturedFigBlackPos.y);
            }
            serialPort.Write(string.Join("", moveData));
        }
        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            for (int i = 0; i < 32; i++)
            {
                int bitIndex = i * 8;
                for (int j = 0; j < 4; j++)
                {

                    bool bit1 = bits.Get(bitIndex + j);
                    bool bit2 = bits.Get(bitIndex + 7 - j);
                    bits.Set(bitIndex + j, bit2);
                    bits.Set(bitIndex + 7 - j, bit1);
                }
            }
            bits.CopyTo(ret, 0);
            return ret;
        }
        private void VisualizeArray(BitArray bitArray)
        {

            string bits = "";
#if HEZKY_VYPIS
            for (int i = 0; i < bitArray.Length; i++)
            {
                if (i % 4 == 0)
                {
                    bits += ' ';
                }

                if (i % 32 == 0)
                {
                    bits += "\n";
                }
                bits += bitArray[i] ? '1' : '0';
            }
#else
            for (int i = 0; i < bitArray.Length; i++)
            {
                bits += bitArray[i] ? '1' : '0';
            }
#endif
            Clipboard.SetText(bits);
            MessageBox.Show(bits);
        }
        private BitArray GetChessBoardState()
        {
            BitArray bitArray = new BitArray(numOfDiodes);
            int bitArrayIndex = 0;

#if SwapEverySecondRow
            for (int i = 0; i < ROWS; i++)
            {
                bool swappedRow = false;
                for (int j = 0; j < COLUMNS; j++)
                {
                    if (bitArrayIndex % 32 == 0 && bitArrayIndex != 0)
                    {
                        if (swappedRow)
                        {

                        }
                        else if(i % 2 != 0)
                        {
                            bitArrayIndex += 28;
                            swappedRow = true;
                        }
                    }
                    
                    bitArray[bitArrayIndex] = !(chessBoard[i, j] is null);
                    bitArrayIndex++;
                    byte colorComb = GetByteCombinationOfColor(buttonGrid[i, j].BackColor);
                    for (int k = 0; k < 3; k++)
                    {
                        bool bit = GetBitFromByte(colorComb, k);
                        bitArray.Set(bitArrayIndex, bit);
                        bitArrayIndex++;
                    }
                    if (swappedRow)
                    {
                        bitArrayIndex -= 8;
                    }
                }

                if (i % 2 == 1)
                {
                    bitArrayIndex += 36;
                }
            }
#else
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {

                    bitArray[bitArrayIndex] = !(chessBoard[i, j] is null);
                    bitArrayIndex++;
                    byte colorComb = GetByteCombinationOfColor(buttonGrid[i, j].BackColor);
                    for (int k = 0; k < 3; k++)
                    {
                        bool bit = GetBitFromByte(colorComb, k);
                        bitArray.Set(bitArrayIndex, bit);
                        bitArrayIndex++;
                    }
                }
            }
#endif
#if DEBUG_SHOW_ARRAY
            VisualizeArray(bitArray);
#endif
            return bitArray;
        }
        private bool GetBitFromByte(byte b, int bitNumber)
        {
            return ((b >> bitNumber) & 1) == 1; //return (b & (1 << bitNumber)) != 0;
        }
        private void UpdateDisplays()
        {
            if(!serialPort.IsOpen)
            {
                MessageBox.Show("Došlo ke ztrátě spojení s reálnou šachovnicí.", "Chyba připojení", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            serialPort.Write(new byte[] { 0b0000_0010 }, 0, 1);
            string text = string.Empty;
            if (casomira == Casomira.VYCHOZI)
            {
                TimeSpan remainingTime = maximalniDelkaTahu - casNaTah;
                if (remainingTime > TimeSpan.Zero)
                {
                    text += remainingTime.Minutes.ToString("00");
                    text += remainingTime.Seconds.ToString("00");
                    text += new string(' ', 4);
                }
                else
                {
                    text += new string('0', 8);
                }
            }
            else
            {
                if (casNaTah_bila > TimeSpan.Zero)
                {
                    text += casNaTah_bila.Minutes.ToString("00");
                    text += casNaTah_bila.Seconds.ToString("00");
                }
                else
                {
                    text += new string('0', 4);
                }
                if (casNaTah_cerna > TimeSpan.Zero)
                {
                    text += casNaTah_cerna.Minutes.ToString("00");
                    text += casNaTah_cerna.Seconds.ToString("00");
                }
                else
                {
                    text += new string('0', 4);
                }
            }
            text += hodnotaVeProspechBile >= 0 ? '+' : '-';
            text += Math.Abs(hodnotaVeProspechBile).ToString("00");
            text += hracNaRade == PlayerColor.WHITE ? 'W' : 'B';
            serialPort.Write(text);
        }
        private byte GetByteCombinationOfColor(Color color)
        {
            if (color == chessBoardColor1)
            {
                return 0;
            }
            else if (color == chessBoardColor2)
            {
                return 0;
            }
            else if (color == highlightColor) // 4
            {
                return 0b100;
            }
            else if (color == sachovaciBarva) // 1
            {
                return 0b001;
            }
            else if (color == barvaZvyrazneniObetavychFigurek) // 5
            {
                return 0b101;
            }
            else if (color == pohybFigurky_pocatek) // 2
            {
                return 0b010;
            }
            else if (color == pohybFigurky_cil) // 3
            {
                return 0b110;
            }
            else if (color == highlightColorCapture)
            {
                return 0b011;
            }
            return 255;
        }
    }
}