namespace Homework8b
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private Bitmap b;

        private Point minValue, maxValue, max2Value, maxSquaredValue;
        private PointF[] pointsArray;
        private List<int> variableX, variableY;
        private int numberOfSamples = 100;
        private int maxIntValue = 100;
        private int interval = 100;
        private int tickCount = 0;
        private Pen currentPen; 
        private Random r;

        private Rectangle leftPlotWindow, rightPlotWindow;

        public Form1()
        {
            InitializeComponent();
            r = new Random();
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.Clear(Color.White);
            pictureBox1.Image = b;
            timer1.Interval = interval;
            leftPlotWindow = new Rectangle(10, 10, pictureBox1.Width /2 - 20, pictureBox1.Height / 2 - 20);
            rightPlotWindow = new Rectangle(leftPlotWindow.X + leftPlotWindow.Width + 10, 10, pictureBox1.Width - leftPlotWindow.Width - 20, pictureBox1.Height - 20);
            minValue = new Point(0, 0);
            maxValue = new Point(numberOfSamples, numberOfSamples);
            max2Value = new Point(numberOfSamples, 2 * maxIntValue);
            maxSquaredValue = new Point(numberOfSamples, maxIntValue * maxIntValue);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            variableX = new List<int>();
            variableY = new List<int>();
            for (int i = 0; i < numberOfSamples; i++)
            {
                variableX.Add(r.Next(-maxIntValue, maxIntValue));
                variableY.Add(r.Next(-maxIntValue, maxIntValue));
            }
        }
        //Distribution X
        private void button1_Click(object sender, EventArgs e)
        {
            currentPen = new Pen(Brushes.Black, 3);
            pointsArray = new PointF[variableX.Count];
            var arrayX = variableX.ToArray();
            for (int i = 0; i < arrayX.Length; i++)
            {
                pointsArray[i] = fromRealToVirtual(new PointF(i, arrayX[i]), minValue, maxValue, leftPlotWindow);
            }
            tickCount = 0;
            timer1.Start();
        }
        //Chi-square
        private void button2_Click(object sender, EventArgs e)
        {
            currentPen = new Pen(Brushes.DarkRed, 3);
            pointsArray = new PointF[variableX.Count];
            var arrayX = variableX.ToArray();
            for (int i = 0; i < arrayX.Length; i++)
            {
                pointsArray[i] = fromRealToVirtual(new PointF(i, arrayX[i] * arrayX[i]), minValue, maxValue, rightPlotWindow);
            }
            tickCount = 0;
            timer1.Start();
        }
        //X/Y-square
        private void button3_Click(object sender, EventArgs e)
        {
            currentPen = new Pen(Brushes.DarkBlue, 3);
            pointsArray = new PointF[variableX.Count];
            var arrayX = variableX.ToArray();
            var arrayY = variableY.ToArray();
            for (int i = 0; i < arrayX.Length; i++)
            {
                pointsArray[i] = fromRealToVirtual(new PointF(i, arrayX[i] / (float)(arrayY[i] * arrayY[i])), minValue, maxValue, leftPlotWindow);
            }
            tickCount = 0;
            timer1.Start();
        }
        //X/Y
        private void button4_Click(object sender, EventArgs e)
        {
            currentPen = new Pen(Brushes.DarkGreen, 3);
            pointsArray = new PointF[variableX.Count];
            var arrayX = variableX.ToArray();
            var arrayY = variableY.ToArray();
            for (int i = 0; i < arrayX.Length; i++)
            {
                pointsArray[i] = fromRealToVirtual(new PointF(i, arrayX[i] / (float)arrayY[i]), minValue, maxValue, leftPlotWindow);
            }
            tickCount = 0;
            timer1.Start();
        }
        //X-square/Y-square
        private void button5_Click(object sender, EventArgs e)
        {
            currentPen = new Pen(Brushes.BlueViolet, 3);
            pointsArray = new PointF[variableX.Count];
            var arrayX = variableX.ToArray();
            var arrayY = variableY.ToArray();
            for (int i = 0; i < arrayX.Length; i++)
            {
                pointsArray[i] = fromRealToVirtual(new PointF(i, arrayX[i] * arrayX[i] / (float)(arrayY[i] * arrayY[i])), minValue, maxValue, rightPlotWindow);
            }
            tickCount = 0;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tickCount < pointsArray.Length-1)
            {
                try
                {
                    g.DrawLine(currentPen, pointsArray[tickCount], pointsArray[tickCount + 1]);
                } catch (OverflowException o)
                {
                    Console.WriteLine("OVerflow exeption on Drawline()");
                }
                tickCount++;
                pictureBox1.Image = b;
            } 
            else
            {
                timer1.Stop();
            }
            
        }

        private int virtualX(int x, int minX, int maxX, double W)
        {
            return (int)(W * (double)(x - minX) / (maxX - minX));
        }

        private int virtualY(int y, int minY, int maxY, double H)
        {
            return (int)(H - H * (double)(y - minY) / (maxY - minY));
        }

        private PointF fromRealToVirtual(PointF point, Point minValue, Point maxValue, Rectangle rect)
        {
            float newX = maxValue.X - minValue.X == 0 ? 0 : (rect.Left + rect.Width * (point.X - minValue.X) / (maxValue.X - minValue.X));
            float newY = maxValue.Y - minValue.Y == 0 ? 0 : (rect.Top + rect.Height - rect.Height * (point.Y - minValue.Y) / (maxValue.Y - minValue.Y));
            return new PointF(newX, newY);
        }
    }
}