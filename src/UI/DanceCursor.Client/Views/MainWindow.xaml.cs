using System.Runtime.InteropServices;
using System.Windows;

namespace DanceCursor.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PeriodicTimer timer;


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public int X;
            public int Y;
        };

        private void SetPosition(int a, int b)
        {
            SetCursorPos(a, b);
        }

        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer = new(TimeSpan.FromMilliseconds(5000));
            bool isDown = true;

            Task statisticsUploader = PeriodicAsync(async () =>
            {
                try
                {
                    UploadStatisticsAsync(isDown);
                    isDown = isDown ? false : true;
                }
                catch (Exception ex)
                {
                    // Log the exception
                }
            }, TimeSpan.FromMilliseconds(5000));
        }

        public async Task PeriodicAsync(Func<Task> action, TimeSpan interval, CancellationToken cancellationToken = default)

        {
            using PeriodicTimer timer = new(interval);
            while (true)
            {
                await action();
                await timer.WaitForNextTickAsync(cancellationToken);
            }
        }

        public void UploadStatisticsAsync(bool isDown)
        {
            Point a = GetMousePosition();
            if (isDown)
            {
                SetPosition(Convert.ToInt32(a.X - 5), Convert.ToInt32(a.Y - 5));
            }
            else
            {
                SetPosition(Convert.ToInt32(a.X + 5), Convert.ToInt32(a.Y + 5));
            }
        }
    }
}