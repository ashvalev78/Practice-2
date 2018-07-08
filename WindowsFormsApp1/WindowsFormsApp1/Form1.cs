using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Color rectFillColor = Color.Black, rectWrapColor = Color.Green, lineColor = Color.Red;
        int a = 100, cX, cY;
        int minSide = 5, maxSide = 20;
        int pbWidth, pbHeight, penBold;
        Pen graphPen = new Pen(Color.Red, 1);
        bool solidLine = true;
        Graphics graph2, gr;
        // Right lemiskante part
        List<Point> positiveRightArray = new List<Point>();
        List<Point> negativeRightArray = new List<Point>();
        //--------------------------------------------------------
        // Left lemiskante part
        List<Point> positiveLeftArray = new List<Point>();
        List<Point> negativeLeftArray = new List<Point>();
        //--------------------------------------------------------
        Bitmap myBitmap;


        private void Form1_Load(object sender, EventArgs e)
        {
            pbWidth = pictureBox1.Width;
            pbHeight = pictureBox1.Height;
            cX = pbWidth / 2;
            cY = pbHeight / 2;
            textBox1.Text = a.ToString();
            textBox2.Text = minSide.ToString();
            textBox3.Text = maxSide.ToString();
            graph2 = pictureBox2.CreateGraphics();
            graph2.DrawLine(graphPen, 0, pictureBox2.Height / 2, pictureBox2.Width, pictureBox2.Height / 2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (minSide > maxSide)
            {
                int t = minSide;
                minSide = maxSide;
                maxSide = minSide;
            }
            myBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gr = Graphics.FromImage(myBitmap);
            gr.Clear(Color.White);
            GetPoints();
            PaintGraphic();
        }

        private void makeRect(Point center, Graphics graph, int side, int i, Point[] array)
        {
            Brush rectBrush = new SolidBrush(rectFillColor);
            Brush clearBrush = new SolidBrush(Color.White);
            //graph.DrawRectangle(new Pen(Color.White), center.X, center.Y, side, side);
            //graph.FillRectangle(clearBrush, center.X, center.Y, side, side);
            graph.DrawRectangle(new Pen(rectWrapColor), center.X, center.Y, side, side);
            graph.FillRectangle(rectBrush, center.X, center.Y, side, side);
            graph.DrawEllipse(new Pen(rectWrapColor), center.X - a/2, center.Y - a/2, a, a);
            //System.Threading.Thread.Sleep(1000);
            
            Timer time = new Timer();
            time.Interval = 30;
            time.Tick += new EventHandler((o, ev) =>
            {
                graph.DrawRectangle(new Pen(Color.White), center.X, center.Y, side, side);
                graph.FillRectangle(clearBrush, center.X, center.Y, side, side);
                graph.DrawEllipse(new Pen(Color.White), center.X - a/2, center.Y - a/2, a, a);
                restoreLostGraph(graph, i - 1, array);
                time.Stop();
            });
            time.Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            a = Convert.ToInt32(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button2.BackColor = colorDialog1.Color;
                rectWrapColor = colorDialog1.Color;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (colorDialog2.ShowDialog() == DialogResult.OK)
            {
                button3.BackColor = colorDialog2.Color;
                rectFillColor = colorDialog2.Color;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (colorDialog3.ShowDialog() == DialogResult.OK)
            {
                button4.BackColor = colorDialog3.Color;
                lineColor = colorDialog3.Color;
                graphPen.Color = lineColor;
                graph2.Clear(Color.White);
                graph2.DrawLine(graphPen, 0, pictureBox2.Height / 2, pictureBox2.Width, pictureBox2.Height / 2);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
                minSide = 0;
            else
                if (Convert.ToInt32(textBox2.Text) >= 0)
                    minSide = Convert.ToInt32(textBox2.Text);
        }   

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
                maxSide = 0;
            else
                if (Convert.ToInt32(textBox3.Text) >= 0)
                    maxSide = Convert.ToInt32(textBox3.Text);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 1)
                solidLine = false;
            else solidLine = true;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            penBold = hScrollBar1.Value;
            graphPen.Width = penBold;
            graph2.Clear(Color.White);
            graph2.DrawLine(graphPen, 0, pictureBox2.Height / 2, pictureBox2.Width, pictureBox2.Height / 2);
        }

        private void restoreLostGraph(Graphics graph, int i, Point[] array)
        {
            graph.DrawLine(new Pen(Color.Black), new Point(pbWidth / 2, pbHeight), new Point(pbWidth / 2, 0));
            graph.DrawLine(new Pen(Color.Black), new Point(0, pbHeight / 2), new Point(pbWidth, pbHeight / 2));
            if (solidLine)
            {
                for (int k = 0; k < i && k < array.Length - 1; k++)
                    graph.DrawLine(graphPen, array[k], array[k + 1]);
            }
            else
            {
                for (int k = 0; k < i && k < array.Length - 1; k++)
                    if (k % 4 == 0)
                        graph.DrawLine(graphPen, array[k], array[k + 1]);
            }
        }

        private void delayedPaint(Point[] array, Graphics graph)
        {
            Timer time = new Timer();
            time.Interval = 40;
            int i = 0;
            bool minOrmax = true; // true - maximize, false - minimize
            int side = minSide;
            time.Tick += new EventHandler((o, ev) =>
            {
                if (solidLine)
                    graph.DrawLine(graphPen, array[i], array[i + 1]);
                else
                {
                    if (i % 4 == 0)
                        graph.DrawLine(graphPen, array[i], array[i + 1]);
                }
                if (side == maxSide)
                    minOrmax = false;
                else
                {
                    if (side == minSide)
                        minOrmax = true;
                }
                
                if (minOrmax)
                    side++;
                else side--;
                if (minSide == maxSide)
                    side = minSide;

                makeRect(array[i + 1], graph, side, i, array);
                //restoreLostGraph(graph, i - 1, array);
                i++;
                

                if (i >= array.Length - 1)
                {
                    Timer t = o as Timer;
                    t.Stop();
                }

            });
            time.Start();
        }

        private void PaintGraphic()
        {
            Graphics graph = pictureBox1.CreateGraphics();
            graph.Clear(Color.White);
            Point[] pointRightPositiveArray = positiveRightArray.ToArray();
            Point[] pointLeftPositiveArray = positiveLeftArray.ToArray();
            Point[] pointRightNegativeArray = negativeRightArray.ToArray();
            Point[] pointLeftNegativeArray = negativeLeftArray.ToArray();
            graph.DrawLine(new Pen(Color.Black), new Point(pbWidth / 2, pbHeight), new Point(pbWidth / 2, 0));
            graph.DrawLine(new Pen(Color.Black), new Point(0, pbHeight / 2), new Point(pbWidth, pbHeight / 2));
            List<Point> mainList = new List<Point>();
            mainList.AddRange(positiveRightArray);
            mainList.AddRange(negativeRightArray);
            mainList.AddRange(positiveLeftArray);
            mainList.AddRange(negativeLeftArray);
            Point[] mainArray = mainList.ToArray();
            //int k = mainArray.Length;
            //MessageBox.Show(k.ToString());
            delayedPaint(mainArray, graph);
            //delayedPaint(pointRightNegativeArray, graph);
            //delayedPaint(pointLeftPositiveArray, graph);
            //delayedPaint(pointLeftNegativeArray, graph);
        }

        public Form1()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            InitializeComponent();
        }

        private void addArrays(double x, double y)
        {
            positiveRightArray.Add(new Point(cX + (int)x, cY + (int)y));
            negativeRightArray.Add(new Point(cX + (int)x, cY - (int)y));
            positiveLeftArray.Add(new Point(cX - (int)x, cY + (int)y));
            negativeLeftArray.Add(new Point(cX - (int)x, cY - (int)y));
        }

        private void GetPoints()
        {
            // List<Point> newArray = new List<Point>(); // Declare the main array of all points

            // Declare the secondary arrays
            // Right lemiskante part
            positiveRightArray.Clear();
            negativeRightArray.Clear();
            //--------------------------------------------------------
            // Left lemiskante part
            positiveLeftArray.Clear();
            negativeLeftArray.Clear();
            //--------------------------------------------------------
            //string str = "";
            double y = 0, x = 0;
            positiveRightArray.Add(new Point(cX + (int)x, cY + (int)y));
            bool flag = false;
            do
            {
                x++;
                //str += x;
                //str += ","; 
                y = 0.70711 * Math.Sqrt(Math.Sqrt(Math.Pow(a, 2) * (Math.Pow(a, 2) + 8 * Math.Pow(x, 2))) - Math.Pow(a, 2) - 2 * Math.Pow(x, 2));
                //str += y;
                //str += "; ";
                if (!(y == 0 && x > 5) || !flag)
                {
                    addArrays(x, y);
                    flag = true;
                }
            } while (y > 0);
            addArrays(x, y);
            negativeRightArray.Reverse();
            negativeLeftArray.Reverse();
            negativeLeftArray.Add(new Point(cX, cY));
            //MessageBox.Show(str);
        }
    }
}
