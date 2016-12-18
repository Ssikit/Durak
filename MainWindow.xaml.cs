using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartBoard();
        }

        Board boardView;

        System.Windows.Threading.DispatcherTimer timer;

        private DateTime TimerStart { get; set; }

        private void StartBoard()
        {
            UniGrid.IsEnabled = true;

            firstClick = true;

            timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += new EventHandler(timerTick);
            timer.Start();
            TimerStart = DateTime.Now;

            boardView = new Board(Properties.Settings.Default.mines, Properties.Settings.Default.width, Properties.Settings.Default.height);

            bombsCount.Text = boardView.bombsLeft.ToString();

            UniGrid.Rows = boardView.Height;
            UniGrid.Columns = boardView.Width;

            Image smile = new Image() { Source = getSource(Properties.Resources.smile), Margin = new Thickness(0) };
            mainButton.Content = smile;

            map = new Dictionary<Cell, Image>();
            map1 = new Dictionary<Image, Cell>();
            for (int i = 0; i < Properties.Settings.Default.height; i++)
            {
                for (int j = 0; j < Properties.Settings.Default.width; j++)
                {
                    Image cellimg = new Image() { Source = getSource(Properties.Resources.unopenedcell), Name = $"canvas{i}{j}", Margin = new Thickness(0.1) };

                    UniGrid.Children.Add(cellimg);
                    cellimg.MouseDown += Img_MouseDown;
                    cellimg.MouseUp += Img_MouseUp;
                    map.Add(boardView.GetBoard[i, j], cellimg);
                    map1.Add(cellimg, boardView.GetBoard[i, j]);
                }
            }

        }

        private System.Windows.Media.Imaging.BitmapSource getSource(System.Drawing.Bitmap br)
        {
            System.Windows.Media.Imaging.BitmapSource b =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                       br.GetHbitmap(),
                       IntPtr.Zero,
                       Int32Rect.Empty,
                       BitmapSizeOptions.FromEmptyOptions());
            return b;
        }

        Dictionary<Cell, Image> map;

        Dictionary<Image, Cell> map1;

        private void timerTick(object sender, EventArgs e)
        {
            var currentTime = DateTime.Now - this.TimerStart;
            if (currentTime.Minutes < 1)
                time.Text = currentTime.Seconds.ToString();
            else
                time.Text = currentTime.Minutes + ":" + currentTime.Seconds;

        }

        Image imageCheck;

        private bool firstClick;

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imageCheck = sender as Image;
        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (imageCheck == img)
            {

                System.Drawing.Bitmap bmp;
                if (e.ChangedButton == MouseButton.Right)
                {
                    boardView.RightClick(map1[img]);
                    //DrawCell(map1[img]);
                    bombsCount.Text = boardView.bombsLeft.ToString();
                }
                else
                {
                    //if (firstClick)
                    //{
                    //    boardView.FirstClick(map.FirstOrDefault(x => x.Value == img).Key);
                    //    firstClick = false;
                    //}
                    //else
                    //{
                    boardView.LeftClick(map1[img]);
                   // DrawCell(map1[img]);
                    if (boardView.IsGameOver)
                        GameOver();
                    //}
                }
                 refresh();
            }
        }

        private void GameOver()
        {
            boardView.GameOver();
            Image fail = new Image() { Source = getSource(Properties.Resources.sad), Margin = new Thickness(0) };
            mainButton.Content = fail;
            timer.Stop();
            UniGrid.IsEnabled = false;
        }

        private void ContentControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void RefreshBoard()
        {
            boardView.InitializeBoard(boardView.bombs, Properties.Settings.Default.width, Properties.Settings.Default.height);

            UniGrid.Children.Clear();
            StartBoard();
        }

        private void DrawCell(Cell cell)
        {
            map[cell].Source = cell.CellPicture;
        }

        private void refresh()
        {
            foreach (var kv in map)
            {
                kv.Value.Source = kv.Key.CellPicture;
            }
        }

        private void setBoard()
        {
            foreach (Cell cell in boardView.GetBoard)
            {
                if (cell.IsClick)
                {
                    //   Image img = UniGrid.Children[i] as Image;
                    // img.Source = new BitmapImage(new Uri(@"D:\Documents\Visual Studio 2015\Projects\Minesweeper\Minesweeper\bin\Debug\07.bmp"));
                }
                if (cell.IsMine)
                {
                    //      Image img = UniGrid.Children[i] as Image;//
                    //    img.Source = new BitmapImage(new Uri(@"D:\Documents\Visual Studio 2015\Projects\Minesweeper\Minesweeper\bin\Debug\07.bmp"));
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RefreshBoard();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.height = 20;
            Properties.Settings.Default.width = 20;
        }
    }
}
