using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Minesweeper
{
    class Cell
    {
        public int nearMines;

        public int x, y;

        public bool IsMine { get; set; }

        public bool IsFlag { get; set; }

        public bool IsClick { get; set; }

        public BitmapSource CellPicture
        {
            get
            {
                return (BitmapSource)(IsClick ? (IsMine ? getSource(Properties.Resources.mine) : getSource(Properties.Resources.ResourceManager.GetObject("cell" + this.nearMines))) : (IsFlag ? getSource(Properties.Resources.flag) : getSource(Properties.Resources.unopenedcell)));
            }
        }

        private BitmapSource getSource(object bb)
        {
            Bitmap br = bb as Bitmap;
            System.Windows.Media.Imaging.BitmapSource b =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                       br.GetHbitmap(),
                       IntPtr.Zero,
                       Int32Rect.Empty,
                       BitmapSizeOptions.FromEmptyOptions());
            return b;
        }

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            nearMines = 0;
            IsMine = false;
            IsClick = false;
        }
    }
}
