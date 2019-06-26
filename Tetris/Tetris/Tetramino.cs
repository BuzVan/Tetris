using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Tetris
{
    public static class FigureParams
    {
        /// <summary>
        /// цвета каждой фигуры
        /// </summary>
        public static Dictionary<char, Brush> ColorsDic = new Dictionary<char, Brush>
        {
            ['I'] = Brushes.Cyan,
            ['J'] = Brushes.Blue,
            ['L'] = Brushes.Orange,
            ['O'] = Brushes.Yellow,
            ['S'] = Brushes.Green,
            ['Z'] = Brushes.Red,
            ['T'] = Brushes.Purple
        };
        /// <summary>
        /// занимаемые клетки каждой фигуры
        /// </summary>
        public static Dictionary<char, Point[]> CellsDic = new Dictionary<char, Point[]>
        {
            ['I'] = new Point[]
                    {
                    new Point(-1, 0),
                    new Point(0, 0),
                    new Point(1, 0),
                    new Point(2, 0)
                    },
            ['J'] = new Point[]
                    {
                        new Point(-1,0),
                        new Point(-1,1),
                        new Point(0,1),
                        new Point(1,1)
                    },
            ['L'] = new Point[]
                    {
                        new Point(1,0),
                        new Point(-1,1),
                        new Point(0,1),
                        new Point(1,1)
                    },
            ['O'] = new Point[]
                    {
                        new Point(0,0),
                        new Point(0,1),
                        new Point(1,0),
                        new Point(1,1)
                    },
            ['S'] = new Point[]
                    {
                        new Point(0,1),
                        new Point(-1,1),
                        new Point(0,0),
                        new Point(1,0)
                    },
            ['Z'] = new Point[]
                    {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,1),
                        new Point(1,1)
                    },
            ['T'] = new Point[]
                    {
                        new Point(0,1),
                        new Point(-1,1),
                        new Point(0,0),
                        new Point(1,1)
                    }
        };
        /// <summary>
        /// поменять цвет фигуры
        /// </summary>
        /// <param name="Figure">имя фигуры</param>
        /// <param name="NewColor">новый цвет фигуры</param>
        public static void SetColor(char Figure, Brush NewColor)
        {
            ColorsDic[Figure] = NewColor;
        }
    }
    public class Tetramino
    {
        /// <summary>
        /// имя фигуры
        /// </summary>
        public char NameFig
        {
            get;
            protected set;
        }
        private Point position;
        private Point[] cells;
        private Brush color;
        /// <summary>
        /// создать случайную фигуру тетрамино
        /// </summary>
        public Tetramino()
        {
            position = new Point(0, 0);
            SetRandomFigure();
        }
        /// <summary>
        /// создать фигуру тетрамино
        /// </summary>
        /// <param name="fig">первая буква имени тетрамино</param>
        public Tetramino(char fig)
        {
            position = new Point(0, 0);
            SetFig(fig);
        }
        /// <summary>
        /// цвет клетки фигуры тетрамино
        /// </summary>
        public Brush Color => color;
        /// <summary>
        /// позиция этой фигуры на поле
        /// </summary>
        public Point Position => position;
        /// <summary>
        /// клетки, которая занимает фигура тетрамино
        /// </summary>
        public Point[] Cells => cells;
        /// <summary>
        /// сместить тетрамино на 1 клетку влево
        /// </summary>
        public void MoveLeft()
        {
            position.X -= 1;
        }
        /// <summary>
        /// сместить тетрамино на 1 клетку вправо
        /// </summary>
        public void MoveRight()
        {
            position.X += 1;
        }
        /// <summary>
        /// сместить тетрамино на 1 клетку вниз
        /// </summary>
        public void MoveDown()
        {
            position.Y += 1;
        }
        /// <summary>
        /// поворот фигуры
        /// </summary>
        public void Turn()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                double x = cells[i].X;
                cells[i].X =
                cells[i].X = -cells[i].Y;
                cells[i].Y = x;
            }
        }
        private void SetRandomFigure()
        {
            Random rand = new Random();
            char fig = '1';
            switch (rand.Next() % FigureParams.CellsDic.Count)
            {
                case 0: // I
                    fig = 'I'; break;
                case 1: // J
                    fig = 'J'; break;
                case 2: // L
                    fig = 'L'; break;
                case 3: // O
                    fig = 'O'; break;
                case 4: // S
                    fig = 'S'; break;
                case 5: // Z
                    fig = 'Z'; break;
                case 6: // T
                    fig = 'T'; break;
                default: break;
            }
            color = FigureParams.ColorsDic[fig];
            cells = new Point[FigureParams.CellsDic[fig].Length];
            FigureParams.CellsDic[fig].ToArray().CopyTo(cells, 0);
            NameFig = fig;
        }
        private void SetFig(char f)
        {
            color = FigureParams.ColorsDic[f];
            cells = new Point[FigureParams.CellsDic[f].Length];
            FigureParams.CellsDic[f].ToArray().CopyTo(cells, 0);
            NameFig = f;
        }
    }
}
