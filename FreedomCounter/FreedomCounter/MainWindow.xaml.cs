using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MouseButton = System.Windows.Input.MouseButton;

namespace FreedomCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer _timer;
        TimeSpan _time;
        private int count = 0;
        public MainWindow()
        {
            InitializeComponent();
            if (SettingsButton.Background.IsFrozen)
            {
                SettingsButton.Background = SettingsButton.Background.Clone();
            }
            SettingsButton.Background.Opacity = 0;

            if (ExitButton.Background.IsFrozen)
            {
                ExitButton.Background = ExitButton.Background.Clone();
            }
            ExitButton.Background.Opacity = 0;

            if (ContentGrid.Background.IsFrozen)
            {
                ContentGrid.Background = ContentGrid.Background.Clone();
            }
            ContentGrid.Background.Opacity = 0.01;
            _time = TimeSpan.FromSeconds(120);
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                timeLeft.Content = _time.ToString("c");
                freeTime.Content = _time.ToString("c");
                overtime.Content = _time.ToString("c");
                if (_time == TimeSpan.Zero) _timer.Stop();
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
            _timer.Start();

            toolBar.Visibility = Visibility.Collapsed;
            ContentWindow.MouseMove += Window_MouseMove;
            ContentWindow.MouseLeave += Window_MouseInOut;
            ContentWindow.MouseEnter += Window_MouseInOut;

        }

        private void Window_MouseInOut(object sender, MouseEventArgs e)
        {
            if (ContentWindow.IsMouseOver)
            {
                toolBar.Visibility = Visibility.Visible;
            }
            else
            {
                toolBar.Visibility = Visibility.Collapsed;
            }
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
