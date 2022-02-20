using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Notes_Controller.Views
{
    /// <summary>
    /// Interaction logic for DotRotateLoader.xaml
    /// </summary>
    public partial class DotRotateLoader : Window
    {
        bool stop;

        public string WindowText { get; set; }

        public new double FontSize { get; set; }

        public DotRotateLoader()
        {
            InitializeComponent();

            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Top = (screenHeight - this.Height) / 2;
            this.Left = (screenWidth - this.Width) / 2;
        }

        private void Init(double angle, double radial_offset)
        {
            double angleConst = radial_offset * 0;

            Point p1 = SetXY(angle, angleConst, 120);

            this.Dispatcher.Invoke(() =>
            {
                this.El1.RenderTransform = new TranslateTransform(p1.X, p1.Y);
            });

            angleConst = radial_offset * 1;

            Point p2 = SetXY(angle, angleConst, 120);

            this.Dispatcher.Invoke(() =>
            {
                this.El2.RenderTransform = new TranslateTransform(p2.X, p2.Y);
            });

            angleConst = radial_offset * 2;

            Point p3 = SetXY(angle, angleConst, 120);

            this.Dispatcher.Invoke(() =>
            {
                this.El3.RenderTransform = new TranslateTransform(p3.X, p3.Y);
            });

            angleConst = radial_offset * 3;

            Point p4 = SetXY(angle, angleConst, 120);

            this.Dispatcher.Invoke(() =>
            {
                this.El4.RenderTransform = new TranslateTransform(p4.X, p4.Y);
            });

            angleConst = radial_offset * 4;

            Point p5 = SetXY(angle, angleConst, 120);

            this.Dispatcher.Invoke(() =>
            {
                this.El5.RenderTransform = new TranslateTransform(p5.X, p5.Y);
            });
        }

        private Point SetXY(double angle, double angle_offset, double offset)
        {
            double x = Math.Cos(angle + angle_offset) * offset;

            double y = Math.Sin(angle + angle_offset) * offset;

            return new Point(x, y);
        }

        private void Rotate(double increment, int threadSleep)
        {
            double angle = 0;

            while (!stop)
            {
                Init(angle, 0.5);

                angle += increment;

                Thread.Sleep(threadSleep);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock.FontSize = FontSize;

            TextBlock.Text = WindowText;

            stop = false;

            Init(0, 0.5);

            Task.Run(() =>
            {
                Rotate(0.5, 200);
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            stop = true;
        }
    }
}

