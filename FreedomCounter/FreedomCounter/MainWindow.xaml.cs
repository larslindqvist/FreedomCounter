using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FreedomCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsConfig config;
        private Window settingsWindow;
        private int _CounterDown;
        private int _CounterUp;
        private DatabaseHandler handler;
        private DateTime startTime;
        private DateTime endTime;
       
        public MainWindow()
        {
            InitializeComponent();
            config = new SettingsConfig();
            handler = new DatabaseHandler();
            startTime = GetStartDate();
            endTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, startTime.Second) + new TimeSpan(config.Workday, config.Lunch, 0);
            Console.WriteLine(startTime);

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



            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                _CounterDown -= 1;
                timeLeft.Content = new TimeSpan(
                    endTime.Hour - DateTime.Now.Hour,
                    endTime.Minute - DateTime.Now.Minute,
                    endTime.Second + _CounterDown);
                freeTime.Content = (startTime + TimeSpan.FromHours(config.Workday)).ToShortTimeString();
                if (timeLeft.Content.Equals("08:00"))
                {
                    _CounterUp += 1;
                    overtime.Content = new TimeSpan(0, 0, _CounterUp);
                }
                else
                {
                    overtime.Content = "00:00";
                };
            }, this.Dispatcher);

            toolBar.Visibility = Visibility.Collapsed;
            ContentWindow.MouseMove += Window_MouseMove;
            ContentWindow.MouseLeave += Window_MouseInOut;
            ContentWindow.MouseEnter += Window_MouseInOut;

            SettingsButton.Click += Settings_Clicked;
            ExitButton.Click += ExitButton_Click;

        }

        private void Settings_Clicked(object sender, RoutedEventArgs e)
        {
            if (settingsWindow == null)
            {
                settingsWindow = new SettingsWindow(startTime, endTime);
                settingsWindow.Show();
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            handler.InsertTime("Stopped", DateTime.Now);
            this.Close();
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

        private DateTime GetStartDate()
        {
            DateTime startDate = DateTime.Now;
            if (!handler.IsFirstStartToday())
            {
                startDate = handler.GetFirstStartToday();
            }
            else
            {
                handler.InsertTime("First", startDate);
            }

            return startDate;
        }
    }
}
