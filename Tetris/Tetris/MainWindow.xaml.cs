using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetris
{
    

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer;
        Table myBoard;
        int GameSpeed = 1000;
        int SpeedStep = 100;
        public MainWindow()
        {
            InitializeComponent();
            
        }
        void MainWindow_Initialized(object sender, EventArgs e)
        {
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(GameTick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, GameSpeed);
            GameStart();
        }
        private void GameStart()
        {
            MainGrid.Children.Clear();
            myBoard = new Table(MainGrid);
            GameSpeed = 1000;
            Timer.Start();
            LvlText.Content = "Уровень: " + myBoard.LVL;
        }
        private void GamePause()
        {
            if (Timer.IsEnabled) Timer.Stop();
            else Timer.Start();
        }
        private void GameOver()
        {
            Timer.Stop();
            MessageBox.Show( 
                String.Format("Игра окончена.\nНабрано очков: {0}\nУдалено строк: {1}", myBoard.Score, myBoard.Lines),
                "Конец игры!", 
                MessageBoxButton.OK, MessageBoxImage.Asterisk);
            GameStart();
           
        }
        void GameTick(object sender, EventArgs e)
        {
            LevelProgressBar.Value = myBoard.GetLvlProc();
            
            Score.Content = "Score: " + myBoard.Score.ToString();
            Lines.Content = "Lines: " + myBoard.Lines.ToString();
            myBoard.CurTetraminoMovDown();
            if (myBoard.GameOver)
                GameOver();
            if (myBoard.LvlUp)
            {
                Timer.Interval = new TimeSpan(0, 0, 0, 0, GameSpeed - SpeedStep*myBoard.LVL);
                myBoard.LvlUp = false;
                LvlText.Content = "Level: " + myBoard.LVL;
            }
        }
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (Timer.IsEnabled && myBoard.GameOver)
                GameOver();
            switch (e.Key)
            {
                case Key.Left:
                    if (Timer.IsEnabled) myBoard.CurentTetraminoMoveLeft();
                    break;
                case Key.Right:
                    if (Timer.IsEnabled) myBoard.CurTetraminoMoveRight();
                    break;
                case Key.Down:
                    if (Timer.IsEnabled) myBoard.CurTetraminoMovDown();
                    break;
                case Key.Up:
                    if (Timer.IsEnabled) myBoard.CurTetraminoMoveRotate();
                    break;
                case Key.Enter:
                    GameStart();
                    break;
                case Key.Space:
                    GamePause();
                    break;
                default:
                    break;
            }
        }
    }
}
