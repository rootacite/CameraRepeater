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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ShinenginePlus;

using ShinenginePlus.DrawableControls;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System.Diagnostics;

namespace Direct2DBase_NET5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        static IntPtr WindowHandle = (IntPtr)0;
        BackGroundLayer BackGround = null;

        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = 1280;
            this.Height = 720;
            WindowHandle = new WindowInteropHelper(this).Handle;

            var DX = new Direct2DWindow(new System.Drawing.Size(1280, 720), WindowHandle, false);

            BackGround = new BackGroundLayer(new System.Drawing.Size(1280, 720), this, new RawRectangleF(0, 0, 1280, 720), DX.DC);
            Thread updateTimer = new Thread(() =>
            {
                while (true)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    BackGround.Update();

                    sw.Stop();


                    decimal time = sw.ElapsedTicks / (decimal)Stopwatch.Frequency * 1000;
                    decimal wait_time = 1000.0M / 60M - time;

                    if (wait_time < 0)
                    {
                        wait_time = 0;
                    }

                    Thread.Sleep((int)wait_time);
                }

            })
            { IsBackground = true };
            DX.DrawProc += (s) => 
            {
                s.Clear(new SharpDX.Mathematics.Interop.RawColor4(1, 1, 1, 1));

                BackGround.Render();

                return DrawResultW.Commit;
            };

            //Add initial controls

            DX.Run();
            updateTimer.Start();
        }
        
    }
}
