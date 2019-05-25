using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Tetris
{
    public class Tetramino
    {
        private Point position;
        private Point[] figure;
        private Brush color;
        private bool IsTurning;
        public Tetramino()
        {
            position = new Point(0, 0);
            color = Brushes.Transparent;
            figure = SetRandomFigure();
        }
        public Brush Color => color;
        public Point Position => position;
        public Point[] Figure => figure;
        public void MoveLeft()
        {
            position.X -= 1;
        }
        public void MoveRight()
        {
            position.X += 1;
        }
        public void MoveDown()
        {
            position.Y += 1;
        }
        public void Turn()
        {
            if (IsTurning)
            {
                for (int i = 0; i < figure.Length; i++)
                {
                    double x = figure[i].X;
                    figure[i].X = -figure[i].Y;
                    figure[i].Y = x;
                }
            }
        }
        private Point[] SetRandomFigure()
        {
            Random rand = new Random();
            switch (rand.Next() % 7)
            {
                case 0: // I
                    IsTurning = true;
                    color = Brushes.Cyan;
                    return new Point[]
                    {
                        new Point(-1,0),
                        new Point(0,0),
                        new Point(1,0),
                        new Point(2,0),
                    }; //0000
                case 1: // J
                    IsTurning = true;
                    color = Brushes.Blue;
                    return new Point[]
                    {
                        new Point(-1,0),
                        new Point(-1,1),
                        new Point(0,1),
                        new Point(1,1),
                    };
                case 2: // L
                    IsTurning = true;
                    color = Brushes.Orange;
                    return new Point[]
                    {
                        new Point(1,0),
                        new Point(-1,1),
                        new Point(0,1),
                        new Point(1,1),
                    };
                case 3: // O
                    IsTurning = false;
                    color = Brushes.Yellow;
                    return new Point[]
                    {
                        new Point(0,0),
                        new Point(0,1),
                        new Point(1,0),
                        new Point(1,1),
                    };
                case 4: // S
                    IsTurning = true;
                    color = Brushes.Green;
                    return new Point[]
                    {
                        new Point(0,1),
                        new Point(-1,1),
                        new Point(0,0),
                        new Point(1,0),
                    };
                case 5: // Z
                    IsTurning = true;
                    color = Brushes.Red;
                    return new Point[]
                    {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,1),
                        new Point(1,1),
                    };
                case 6: // T
                    IsTurning = true;
                    color = Brushes.Purple;
                    return new Point[]
                    {
                        new Point(0,1),
                        new Point(-1,1),
                        new Point(0,0),
                        new Point(1,1),
                    };
                default:
                    return null;
            }
        }
    }
}
