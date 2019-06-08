using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    public class Table
    {
        class GridField
        {
            public int rows;
            public int columns;
            public Rectangle[,] Field;
            public GridField(Grid grid)
            {
                rows = grid.RowDefinitions.Count;
                columns = grid.ColumnDefinitions.Count;
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
                        grid.Children.Add(Field[x, y]);
                    }
                }
            }
            
    }

        private int score;
        private int linesAssembled;
        private int lvl;
        public int StepLvl = 3;
        private int thisLevelLinesAssembled;
        private bool gameOver = false;
        public bool LvlUp;
        private Tetramino currentTetramino;
        static private Brush fieldBrush = Brushes.Black;
        static private Brush granBrush = Brushes.White;
        private GridField TetrisField;
        private GridField NextFigGrid;
        private Tetramino nextTetramino;
        public int GetLvlProc()
        {
            if (linesAssembled == 0 || thisLevelLinesAssembled == StepLvl) return 0;
            return 100 * thisLevelLinesAssembled/ StepLvl;
        }

        public Table(Grid TetrisGrid, Grid NextFigGrid)
        {
            TetrisField = new GridField(TetrisGrid);
            this.NextFigGrid = new GridField(NextFigGrid);
            LvlUp = false;
            score = 0;
            linesAssembled = 0;
            lvl = 1;
            thisLevelLinesAssembled = 0;
            
            currentTetramino = new Tetramino();
            TetraminoDraw(TetrisField, currentTetramino);
            nextTetramino = new Tetramino();
            TetraminoDraw(this.NextFigGrid, nextTetramino);
        }
        public int Score => score;
        public int Lines => linesAssembled;
        public int LVL => lvl;
        public bool GameOver => gameOver;
        private void TetraminoDraw(GridField grid, Tetramino tetramino)
        {
            Point pos = tetramino.Position;
            Point[] figure = tetramino.Cells;
            Brush color = tetramino.Color;
            foreach (Point P in figure)
            {
                grid.Field[(int)(P.X + pos.X) + ((grid.columns / 2) - 1),
                    (int)(P.Y + pos.Y)].Fill = color;
            }
        }
        private void TetraminoErase(GridField grid, Tetramino tetramino)
        {
            Point pos = tetramino.Position;
            Point[] figure = tetramino.Cells;
            foreach (Point S in figure)
            {
                grid.Field[(int)(S.X + pos.X) + ((grid.columns / 2) - 1),
                    (int)(S.Y + pos.Y)].Fill = fieldBrush;
            }
        }
        private void CheckGameOver()
        {
            for (int x = 0; x < TetrisField.columns; x++)
                if (TetrisField.Field[x, 0].Fill != fieldBrush)
                    gameOver = true;
        }
        private void CheckRows()
        {
            bool full;
            int kol = 0;
            for (int y = 0; y < TetrisField.rows; y++)
            {
                full = true;
                for (int x = 0; x < TetrisField.columns; x++)
                {
                    if (TetrisField.Field[x, y].Fill == fieldBrush)
                    {
                        full = false;
                        break;
                    }
                }
                if (full)
                {
                    kol++;
                    RemoveRow(y);
                    score += 100;
                    linesAssembled += 1;
                    thisLevelLinesAssembled += 1;
                    
                }
            }

            if (kol > 0)
            {
                score += (kol - 1) * 100;//бонус за несколько подряд удалённых линий
                CheckLvlUp();
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
                for (int x = 0; x < TetrisField.columns; x++)
                {
                    TetrisField.Field[x, y].Fill = TetrisField.Field[x, y - 1].Fill;
                }
            }
        }
        public void CurentTetraminoMoveLeft()
        {
            Point pos = currentTetramino.Position;
            Point[] figure = currentTetramino.Cells;
            bool canMove = true;
            TetraminoErase(TetrisField, currentTetramino);
            foreach (Point P in figure)
            {
                // не можем сдвинуть влево - у левой границы поля
                if (((int)(P.X + pos.X) + ((TetrisField.columns / 2) - 1) - 1) < 0)
                {
                    canMove = false;
                }
                // не можем влево - слева стоит фигура
                else if (TetrisField.Field[((int)(P.X + pos.X) + ((TetrisField.columns / 2) - 1) - 1),
                    (int)(P.Y + pos.Y)].Fill != fieldBrush)
                {
                    canMove = false;
                }
            }
            if (canMove)
                currentTetramino.MoveLeft();
            TetraminoDraw(TetrisField, currentTetramino);
        }
        public void CurTetraminoMoveRight()
        {
            Point pos = currentTetramino.Position;
            Point[] figure = currentTetramino.Cells;
            TetraminoErase(TetrisField, currentTetramino);
            bool canMove = true;
            foreach (Point S in figure)
            {
                if (((int)(S.X + pos.X) + ((TetrisField.columns / 2) - 1) + 1) >= TetrisField.columns)
                {
                    canMove = false;
                }
                else if (TetrisField.Field[((int)(S.X + pos.X) + ((TetrisField.columns / 2) - 1) + 1),
                    (int)(S.Y + pos.Y)].Fill != fieldBrush)
                {
                    canMove = false;
                }
            }
            if (canMove)
                currentTetramino.MoveRight();
            TetraminoDraw(TetrisField, currentTetramino);
        }
        public void CurTetraminoMovDown(bool CoolGame=false)
        {
            // бонус за быструю игру (нажата кнопка вниз)
            if (CoolGame) score += 1*lvl;
            Point pos = currentTetramino.Position;
            Point[] figure = currentTetramino.Cells;
            bool canMove = true;
            TetraminoErase(TetrisField, currentTetramino);
            foreach (Point S in figure)
            {
                if (((int)(S.Y + pos.Y) + 1) >= TetrisField.rows)
                {
                    canMove = false;
                }
                else if (TetrisField.Field[((int)(S.X + pos.X) + ((TetrisField.columns / 2) - 1)),
                    (int)(S.Y + pos.Y) + 1].Fill != fieldBrush)
                {
                    canMove = false;
                }
            }
            if (canMove)
            {
                currentTetramino.MoveDown();
                TetraminoDraw(TetrisField, currentTetramino);
            }
            else
            {
                TetraminoDraw(TetrisField, currentTetramino);
                CheckRows();
                CheckGameOver();
                currentTetramino = nextTetramino;
                TetraminoErase(NextFigGrid, nextTetramino);
                nextTetramino = new Tetramino();
                TetraminoDraw(NextFigGrid, nextTetramino);
                score += LVL * 10; //бонус за новую фигуру
            }
        }
        public void CurTetraminoMoveRotate()
        {
            Point pos = currentTetramino.Position;
            Point[] S = new Point[4];
            Point[] figure = currentTetramino.Cells;
            bool canMove = true;
            figure.CopyTo(S, 0);
            TetraminoErase(TetrisField, currentTetramino);
            for (int i = 0; i < S.Length; i++)
            {
                double x = S[i].X;
                S[i].X = -S[i].Y;
                S[i].Y = x;

                if (((int)((S[i].Y + pos.Y) + 1)) <= 0) // сверху - стена
                {
                    canMove = false;
                }
                else if (((int)((S[i].Y + pos.Y) + 1)) >= TetrisField.rows) // снизу - стена
                {
                    canMove = false;
                }
                else if (((int)(S[i].X + pos.X) + ((TetrisField.columns / 2) - 1) + 1) <= 0) //слева - стена
                {
                    canMove = false;
                }
                else if (((int)(S[i].X + pos.X) + ((TetrisField.columns / 2) - 1) + 1) > TetrisField.columns) // справа - стена
                {
                    canMove = false;
                }
                else if (TetrisField.Field[(int)(S[i].X + pos.X) + ((TetrisField.columns / 2) - 1),
                    (int)(S[i].Y + pos.Y)].Fill != fieldBrush)
                {

                    canMove = false;
                }
            }
            if (canMove)
                currentTetramino.Turn();
            TetraminoDraw(TetrisField, currentTetramino);
            
        }
    }
}
