using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Minesweeper
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            levelComboBox.SelectedIndex = 0;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            switch (levelComboBox.SelectedIndex)
            {
                case 0:
                    {
                        Properties.Settings.Default.height = 9;
                        Properties.Settings.Default.width = 9;
                        Properties.Settings.Default.mines = 10;
                        this.Hide();
                        break;
                    }
                case 1:
                    {
                        Properties.Settings.Default.height = 16;
                        Properties.Settings.Default.width = 16;
                        Properties.Settings.Default.mines = 40;
                        this.Hide();
                        break;
                    }
                case 2:
                    {
                        Properties.Settings.Default.height = 16;
                        Properties.Settings.Default.width = 30;
                        Properties.Settings.Default.mines = 99;
                        this.Hide();
                        break;
                    }
                case 3:
                    {
                        if (rows.Text.Replace(" ", "") == String.Empty || columns.Text.Replace(" ", "") == String.Empty || mines.Text.Replace(" ", "") == String.Empty)
                            MessageBox.Show(this, "Необходимо заполнить все поля!");
                        else
                        {
                            int rowsCount = int.Parse(rows.Text);
                            int columnsCount = int.Parse(columns.Text);
                            int minesCount = int.Parse(mines.Text);

                            if ((rowsCount < 1 || rowsCount > 40) || (columnsCount < 1 || columnsCount > 40) || (minesCount < 1 || minesCount > rowsCount * columnsCount))
                                MessageBox.Show(this, "Максимальное количество строк 40\nМаксимальное количество столбцов\nКоличество мин не может превышать количество клеток на поле");
                            else
                            {
                                Properties.Settings.Default.height = int.Parse(rows.Text);
                                Properties.Settings.Default.width = int.Parse(columns.Text);
                                Properties.Settings.Default.mines = int.Parse(mines.Text);
                                this.Hide();
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private void levelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            customSettings.Visibility = Visibility.Hidden;
            switch (levelComboBox.SelectedIndex)
            {
                case 3:
                    {
                        customSettings.Visibility = Visibility.Visible;
                        break;
                    }
                default:
                    break;
            }
        }


        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text[0]))
                e.Handled = true;
        }
    }
}
