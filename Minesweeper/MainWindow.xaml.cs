using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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

        SettingsWindow settingsW;

        Board boardView;

        System.Windows.Threading.DispatcherTimer timer;

        private DateTime TimerStart { get; set; }

        private void StartBoard()
        {
            UniGrid.IsEnabled = true; 

            firstClick = false;

            timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += new EventHandler(timerTick);
            time.Text = "0";

            boardView = new Board(Properties.Settings.Default.mines, Properties.Settings.Default.width, Properties.Settings.Default.height);

            bombsCount.Text = boardView.userBombsLeft.ToString();

            UniGrid.Rows = boardView.Height;
            UniGrid.Columns = boardView.Width;

            Image smile = new Image() { Source = new BitmapImage(new Uri(@"/Resources/smile.png", UriKind.Relative)), Margin = new Thickness(0) };
            mainButton.Content = smile;

            map = new Dictionary<Cell, Image>();
            map1 = new Dictionary<Image, Cell>();
            for (int i = 0; i < Properties.Settings.Default.height; i++)
            {
                for (int j = 0; j < Properties.Settings.Default.width; j++)
                {
                    Image cellimg = new Image() { Source = new BitmapImage(new Uri(@"/Resources/unopenedcell.png", UriKind.Relative)), Name = $"canvas{i}{j}", Margin = new Thickness(0.1) };

                    UniGrid.Children.Add(cellimg);
                    cellimg.MouseDown += Img_MouseDown;
                    cellimg.MouseUp += Img_MouseUp;
                    map.Add(boardView.GetBoard[i, j], cellimg);
                    map1.Add(cellimg, boardView.GetBoard[i, j]);
                }
            }
        }

        Dictionary<Cell, Image> map;

        Dictionary<Image, Cell> map1;

        private void timerTick(object sender, EventArgs e)
        {
            var currentTime = DateTime.Now - TimerStart;
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
            if (!firstClick)
            {
                firstClick = true;
                TimerStart = DateTime.Now;
                timer.Start();
            }
            Image img = sender as Image;
            if (imageCheck == img)
            {
                if (e.ChangedButton == MouseButton.Right)
                {
                    boardView.RightClick(map1[img]);
                    bombsCount.Text = boardView.userBombsLeft.ToString();
                }
                else
                {
                    boardView.LeftClick(map1[img]);
                    if (boardView.IsGameOver)
                        GameOver();
                }
                UpdateBoard();
            }
            if (boardView.isWin)
                Win();
        }

        private void Win()
        {
            Image winImage = new Image() { Source = new BitmapImage(new Uri(@"/Resources/win.jpg", UriKind.Relative)), Margin = new Thickness(0) };
            mainButton.Content = winImage;
            UniGrid.IsEnabled = false;
            timer.Stop();
        }

        private void GameOver()
        {
            boardView.GameOver();
            Image fail = new Image() { Source = new BitmapImage(new Uri(@"/Resources/sad.png", UriKind.Relative)), Margin = new Thickness(0) };
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

        private void UpdateBoard()
        {
            foreach (var kv in map)
            {
                kv.Value.Source = kv.Key.CellPicture;
            }
        }

        private void Smile_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            RefreshBoard();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            settingsW = new SettingsWindow();
            settingsW.Owner = this;
            timer.Stop();
            settingsW.ShowDialog();
            RefreshBoard();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
