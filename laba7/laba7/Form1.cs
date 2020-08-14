using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
260
60
100
100
100
200
160
300
300
300
340
160
180
120
160
180
220
260
220
200
300
200
280
140
240
160

+

120
140
240
100
160
240
280
260
80
300
*/

namespace laba7
{
    public partial class Form1 : Form
    {        
        Graphics drawArea;

        Point startPoint = new Point();

        List<Point> points = new List<Point>();
        List<Point> minimumConvexHull = new List<Point>();

        public Form1()
        {
            InitializeComponent();
            drawArea = pictureBox1.CreateGraphics();
        }

        public static List<Point> Swap(List<Point> list, int indexA, int indexB)
        {
            Point tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Initialize();
            FoundMinimumConvexHull();
        }        

        private int CheckSide(Point A, Point B, Point C)
        {
            // с какой стороны вектора АВ находится точка С
            return (B.X - A.X) * (C.Y - B.Y) - (B.Y - A.Y) * (C.X - B.X);            
        }

        private void FoundMinimumConvexHull()
        {
            //посортим наши точки, чтобы найти крайнюю левую
            points.Sort(delegate (Point A, Point B)
            {
                return A.X.CompareTo(B.X);
            });
            //нашли, записали как стартовую
            startPoint = points[0];
            //добавили в итоговый список
            minimumConvexHull.Add(points[0]);
            //отрисовали стартовую
            drawArea.DrawEllipse(new Pen(Color.Blue, 2), points[0].X, points[0].Y, 2, 2);
            //запоминаем текущую
            Point current = startPoint;
            //закидываем текущую в конец - как только добежим до неё - задача выполнена
            Swap(points, 0, points.Count() - 1);
            //
            int right = 0;

            //пока не дошли до начальной вершины (пока контур не замкнут)
            do
            {
                //начинаем с точки, которую ещё не обработали
                for (int i = right; i < points.Count(); i++)
                {
                    //ищем самую правую точку относительно текущей
                    int checkSide = CheckSide(current, points[i], points[right]);
                    if (checkSide < 0)
                    {
                        //меняем местами точки
                        points = Swap(points, right, i);
                        drawArea.DrawLine(new Pen(Color.Orange, 2),
                                   current.X,
                                   current.Y,
                                   points[right].X,
                                   points[right].Y);
                        System.Threading.Thread.Sleep(250);
                        drawArea.DrawLine(new Pen(Color.Black, 2),
                                   current.X,
                                   current.Y,
                                   points[right].X,
                                   points[right].Y);
                        drawArea.DrawEllipse(new Pen(Color.Fuchsia, 2), current.X, current.Y, 2, 2);
                        drawArea.DrawEllipse(new Pen(Color.Fuchsia, 2), points[right].X, points[right].Y, 2, 2);
                    }
                }
                //запоминаем новую точку
                current = points[right];
                drawArea.DrawEllipse(new Pen(Color.SkyBlue, 2), current.X, current.Y, 2, 2);
                //добавляем найденную точку в конечный список
                minimumConvexHull.Add(current);
                drawArea.DrawEllipse(new Pen(Color.Fuchsia, 2), current.X, current.Y, 2, 2);
                //
                right++;
                //рисуем ответ
                for (int i = 1; i < minimumConvexHull.Count(); i++)
                {
                    drawArea.DrawLine(new Pen(Color.Aqua, 2),
                                      minimumConvexHull[i - 1].X,
                                      minimumConvexHull[i - 1].Y,
                                      minimumConvexHull[i].X,
                                      minimumConvexHull[i].Y);
                    System.Threading.Thread.Sleep(150);
                }
            }
            while (current != startPoint);
            //замыкаем контур
            minimumConvexHull.Add(startPoint);            
        }

        private void Initialize()
        {
            drawArea.FillRectangle(new SolidBrush(Color.Black), 0, 0, pictureBox1.Width, pictureBox1.Height);
            
            points = new List<Point>();
            minimumConvexHull = new List<Point>();

            string[] lines_points = textBox1.Lines;
            for (int i = 0; i < lines_points.Length; i += 2)
            {
                Point current_point = new Point(int.Parse(lines_points[i]), int.Parse(lines_points[i + 1]));
                points.Add(current_point);

                drawArea.DrawEllipse(new Pen(Color.LimeGreen, 2), current_point.X, current_point.Y, 2, 2);
                System.Threading.Thread.Sleep(50);
            }
        }        
    }
}
