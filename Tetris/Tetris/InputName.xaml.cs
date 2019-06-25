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

namespace Tetris
{
    /// <summary>
    /// Логика взаимодействия для InputName.xaml
    /// </summary>
    public partial class InputName : Window
    {
        int Score, Lines;
        bool PressEnter=false;
        public InputName(int Score, int Lines)
        {
            InitializeComponent();
            NameTB.Focus();
            NameTB.SelectAll();
            this.Score = Score;
            this.Lines = Lines;
        }

        private void NameTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && PressEnter)  button_Click(sender, e);
            if (e.Key == Key.Space) NameTB.Text.Remove(NameTB.Text.Length - 1, 1);
        }

        private void NameTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) PressEnter = true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            RecordsWindow.AddRecord(NameTB.Text, Lines, Score);
            Close();
        }
    }
}
