using System.Drawing;
using System.Windows.Forms;

namespace FormsChess
{
    public partial class Form1 : Form
    {
        internal struct Point
        {
            internal int x;
            internal int y;
            internal Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        Point souradniceBilychVyhozenychFigurek = new Point(0, 40);
        Point souradniceCernychVyhozenychFigurek = new Point(0, 0);
        private void PridatVyhozenouFigurku(ChessPiece chessPiece)
        {
            PlayerColor barvaFigurky = chessPiece.playerColor;
            Image texture;
            if (barvaFigurky == PlayerColor.BLACK)
            {
                texture = ChessPiece.ScaleImage(chessPiece.texture_black, 35, 35, false, false);
            } else
            {
                texture = ChessPiece.ScaleImage(chessPiece.texture_white, 35, 35, false, false);
            }
            AktualizovatObrazekVyhozenychFigurek(texture, barvaFigurky);
        }
        private void AktualizovatObrazekVyhozenychFigurek(Image image, PlayerColor playerColor)
        {
            Bitmap textura = (Bitmap)image;
            Bitmap obrazek = (Bitmap)pictureBox_vyhozeneFigurky.Image;
            Point vychoziBod = playerColor == PlayerColor.WHITE ? souradniceBilychVyhozenychFigurek : souradniceCernychVyhozenychFigurek;
            for (int i = 0; i < 35; i++)
            {
                for (int j = 0; j < 35; j++)
                {
                    Color pixel = textura.GetPixel(i, j);
                    obrazek.SetPixel(vychoziBod.x + i, vychoziBod.y + j, pixel);
                }
            }
            if (playerColor == PlayerColor.WHITE)
            {
                souradniceBilychVyhozenychFigurek.x += 40;
            } 
            else
            {
                souradniceCernychVyhozenychFigurek.x += 40;
            }
            pictureBox_vyhozeneFigurky.Refresh();
        }
    }
}