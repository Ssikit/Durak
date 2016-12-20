using System;
using System.Collections.Generic;
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
                return (BitmapSource)(IsClick ? (IsMine ? new BitmapImage(new Uri(@"/Resources/mine.png", UriKind.Relative)) // (BitmapSource) - приведение к типу BitmapSource (для старой версии проги)
                    : new BitmapImage(new Uri(@"/Resources/cell" + this.nearMines + ".png", UriKind.Relative))) 
                    : (IsFlag ? new BitmapImage(new Uri(@"/Resources/flag.png", UriKind.Relative))
                    : new BitmapImage(new Uri(@"/Resources/unopenedcell.png", UriKind.Relative))));
            }
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
