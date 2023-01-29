#define HEZKY_VYPIS
using System;
using System.Collections;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Linq;

namespace FormsChess
{
    public partial class Form1
    {
        const int columnsOfArduinoDisplay = 16;
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string serialPortData = serialPort.ReadExisting();
            Thread.Sleep(5);
            string[] serialPortData_Lines = serialPortData.Split(serialPort.NewLine.ToCharArray());
            if(serialPortData.StartsWith("piece moved"))
            {
                string[] movedPieceIndexes = serialPortData_Lines[1].Split(' ');
                int index1 = int.Parse(movedPieceIndexes[0]);
                int index2 = int.Parse(movedPieceIndexes[1]);
                string simulatedButtonClickName_1 = $"button{index1 + 1}";
                string simulatedButtonClickName_2 = $"button{index2 + 1}";
                GetRowAndColumnFromButton(simulatedButtonClickName_1, out _, out int row_1, out int column_1);
                GetRowAndColumnFromButton(simulatedButtonClickName_2, out _, out int row_2, out int column_2);
                ExecuteMove(row_1, column_1, row_2, column_2);
            }
            else if(serialPortData.StartsWith("button pressed"))
            {
                switch(serialPortData_Lines[1])
                {
                    case "1":
                        if(hracNaRade != PlayerColor.WHITE) { return; }
                        if (gameState == GameState.CHECKMATE) { return; }
                        Invoke(new Action(() =>
                        {
                            Prohra();
                        }));
                        break;
                    case "2":
                        if (hracNaRade == PlayerColor.WHITE) { return; }
                        if (gameState == GameState.CHECKMATE) { return; }
                        Invoke(new Action(() =>
                        {
                            Prohra();
                        }));
                        break;
                    case "3":
                        if (gameState == GameState.CHECKMATE) { return; }
                        Invoke(new Action(() =>
                        {
                            NCBbutton_remiza_Click(false, new EventArgs());
                        }));
                        break;
                    case "4":
                        NCBbutton_Restart_Click(false, new EventArgs());
                        break;
                    case "5":
                        if (gameState == GameState.CHECKMATE) { return; }
                        NCBbutton_minimaxAlgoritmus_Click(false, new EventArgs());
                        break;
                }
            }
        }
        private void SendGameEnding(string message)
        {
            while(message.Length < columnsOfArduinoDisplay) { message += ' '; }
            if(message.Length > columnsOfArduinoDisplay) { message = message.Substring(0, columnsOfArduinoDisplay); }
            message = RemoveDiacriticsFromString(message);
            serialPort.Write(new byte[] { 0b0000_0100 }, 0, 1);
            serialPort.Write(message);
        }
        private string RemoveDiacriticsFromString(string message)
        {
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(message);
            return System.Text.Encoding.UTF8.GetString(tempBytes);
        }
        private void ExecuteMove(int row1, int column1, int row2, int column2)
        {
            ChessPiece tile = chessBoard[row1, column1];

            if(tile is null || tile?.playerColor != hracNaRade)
            {
                ProvestKliknuti(row2, column2, false);
                ProvestKliknuti(row1, column1, true);
            }
            else// if(tile2 is null || tile2?.playerColor != hracNaRade)
            {
                ProvestKliknuti(row1, column1, false);
                ProvestKliknuti(row2, column2, true);
            }
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
            BitArray bitArray = new BitArray(256);
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
#endif

#if !SwapEverySecondRow
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
            serialPort.Write(new byte[] { 0b0000_0010 }, 0, 1);
            string text = string.Empty;
            BitArray bitArray = GetChessBoardState();
            if (casomira == Casomira.VYCHOZI)
            {
                TimeSpan remainingTime = maximalniDelkaTahu - casNaTah;
                if(remainingTime > TimeSpan.Zero)
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
                if(casNaTah_bila > TimeSpan.Zero)
                {
                    text += casNaTah_bila.Minutes.ToString("00");
                    text += casNaTah_bila.Seconds.ToString("00");
                }
                else
                {
                    text += new string('0', 4);
                }
                if(casNaTah_cerna > TimeSpan.Zero)
                {
                    text += casNaTah_cerna.Minutes.ToString("00");
                    text += casNaTah_cerna.Seconds.ToString("00");
                }
                else
                {
                    text += new string('0', 4);
                }
            }
            text += hodnotaVeProspechBile >= 0 ? '-' : '+';
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
            else if(color == highlightColorCapture)
            {
                return 0b011;
            }
            return 255;
        }
    }
}