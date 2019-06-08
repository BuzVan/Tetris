using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    public class Table
    {
        private int rows;
        private int columns;
        private int score;
        private int linesAssembled;
        private int lvl;
        private int StepLvl = 3;
        private int thisLevelLinesAssembled;
        private bool gameOver = false;
        public bool LvlUp;
        private Tetramino currentTetramino;
        private Rectangle[,] Field;
        static private Brush fieldBrush = Brushes.White;
        static private Brush granBrush = Brushes.Silver;
        public int GetLvlProc()
        {
            if (linesAssembled == 0 || thisLevelLinesAssembled == StepLvl) return 0;
            return 100 * thisLevelLinesAssembled/ StepLvl;
        }

        public Table(Grid TetrisGrid)
        {
            LvlUp = false;
            rows = TetrisGrid.RowDefinitions.Count;
            columns = TetrisGrid.ColumnDefinitions.Count;
            score = 0;
            linesAssembled = 0;
            lvl = 1;
            thisLevelLinesAssembled = 0;
            Field = new Rectangle[columns, rows];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Field[x, y] = new System.Windows.Shapes.Rectangle();
                    Field[x, y].Fill = fieldBrush;
                    Field[x, y].Stroke = granBrush;
                    Field[x, y].StrokeThickness = 1;
                    Grid.SetColumn(Field[x, y], x);
                    Grid.SetRow(Field[x, y], y);
                    TetrisGrid.Children.Add(Field[x, y]);
                }
            }
            currentTetramino = new Tetramino();
            CurrentTetraminoDraw();
        }
        public int Score => score;
        public int Lines => linesAssembled;
        public int LVL => lvl;
        public bool GameOver => gameOver;
        private void CurrentTetraminoDraw()
        {
            Point pos = currentTetramino.Position;
            Point[] figure = currentTetramino.Figure;
            Brush color = currentTetramino.Color;
            foreach (Point P in figure)
            {
                Field[(int)(P.X + pos.X) + ((columns / 2) - 1),
                    (int)(P.Y + pos.Y)].Fill = color;
            }
        }
        private void CurrentTetraminoErase()
        {
            Point pos = currentTetramino.Position;
            Point[] figure = currentTetramino.Figure;
            foreach (Point S in figure)
            {
                Field[(int)(S.X + pos.X) + ((columns / 2) - 1),
                    (int)(S.Y + pos.Y)].Fill = fieldBrush;
            }
        }
        private void CheckGameOver()
        {
            for (int x = 0; x < columns; x++)
                if (Field[x, 0].Fill != fieldBrush)
                    gameOver = true;
        }
        private void CheckRows()
        {
            bool full;
            for (int y = 0; y < rows; y++)
            {
                full = true;
                for (int x = 0; x < columns; x++)
                {
                    if (Field[x, y].Fill == fieldBrush)
                    {
                        full = false;
                        break;
                    }
                }
                if (full)
                {
                    RemoveRow(y);
                    score += 100;
                    linesAssembled += 1;
                    thisLevelLinesAssembled += 1;
                    CheckLvlUp();
                }
            }

        }
        private void CheckLvlUp()
        {
            if (thisLevelLinesAssembled == StepLvl)
            {
                lvl++;
                thisLevelLinesAssembled = 0;
                StepLvl++;
                LvlUp = true;
            }
        }
        private void RemoveRow(int row)
        {
            for (int y = row; y > 0; y--)
            {
                for (int x = 0; x < columns; x++)
                {
                    Field[x, y].Fill = Field[x, y - 1].Fill;
                }
            }
        }
        public void CurentTetraminoMoveLeft()
        {
            Point pos = currentTetramino.Position;
            Point[] figure = currentTetramino.Figure;
            bool canMove = true;
            CurrentTetraminoErase();
            foreach (Point P in figure)
            {
                // не можем сдвинуть влево - у левой границы поля
                if (((int)(P.X + pos.X) + ((columns / 2) - 1) - 1) < 0)
                {
                    canMove = false;
                }
                // не можем влево - слева стоит фигура
                else if (Field[((int)(P.X + pos.X) + ((columns / 2) - 1) - 1),
                    (int)(P.Y + pos.Y)].Fill != fieldBrush)
                {
                    canMove = false;
                }
            }
            if (canMove)
                currentTetramino.MoveLeft();
            CurrentTetraminoDraw();
        }
        public void CurTetraminoMoveRight()
        {
            Point pos = currentTetramino.Position;
            Point[] figure = currentTetramino.Figure;
            CurrentTetraminoErase();
            bool canMove = true;
            foreach (Point S in figure)
            {
                if (((int)(S.X + pos.X) + ((columns / 2) - 1) + 1) >= columns)
                {
                    canMove = false;
                }
                else if (Field[((int)(S.X + pos.X) + ((columns / 2) - 1) + 1),
                    (int)(S.Y + pos.Y)].Fill != fieldBrush)
                {
                    canMove = false;
                }
            }
            if (canMove)
                currentTetramino.MoveRight();
            CurrentTetraminoDraw();
        }
        public void CurTetraminoMovDown()
        {
            Point pos = currentTetramino.Position;
            Point[] figure = currentTetramino.Figure;
            bool canMove = true;
            CurrentTetraminoErase();
            foreach (Point S in figure)
            {
                if (((int)(S.Y + pos.Y) + 1) >= rows)
                {
                    canMove = false;
                }
                else if (Field[((int)(S.X + pos.X) + ((columns / 2) - 1)),
                    (int)(S.Y + pos.Y) + 1].Fill != fieldBrush)
                {
                    canMove = false;
                }
            }
            if (canMove)
            {
                currentTetramino.MoveDown();
                CurrentTetraminoDraw();
            }
            else
            {
                CurrentTetraminoDraw();
                CheckRows();
                CheckGameOver();
                currentTetramino = new Tetramino();
            }
        }
        public void CurTetraminoMoveRotate()
        {
            Point pos = currentTetramino.Position;
            Point[] S = new Point[4];
            Point[] figure = currentTetramino.Figure;
            bool canMove = true;
            figure.CopyTo(S, 0);
            CurrentTetraminoErase();
            for (int i = 0; i < S.Length; i++)
            {
                double x = S[i].X;
                S[i].X = -S[i].Y;
                S[i].Y = x;

                if (((int)((S[i].Y + pos.Y) + 1)) <= 0) // сверху - стена
                {
                    canMove = false;
                }
                else if (((int)((S[i].Y + pos.Y) + 1)) >= rows) // снизу - стена
                {
                    canMove = false;
                }
                else if (((int)(S[i].X + pos.X) + ((columns / 2) - 1) + 1) <= 0) //слева - стена
                {
                    canMove = false;
                }
                else if (((int)(S[i].X + pos.X) + ((columns / 2) - 1) + 1) > columns) // справа - стена
                {
                    canMove = false;
                }
                else if (Field[(int)(S[i].X + pos.X) + ((columns / 2) - 1),
                    (int)(S[i].Y + pos.Y)].Fill != fieldBrush)
                {

                    canMove = false;
                }
            }
            if (canMove)
                currentTetramino.Turn();
            CurrentTetraminoDraw();
        }
    }
}
