using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FreedomCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsWindow settingsWindow;
        private DatabaseHandler handler;
        private DispatcherTimer timer;
        private DateTime endTime;

        public MainWindow()
        {
            InitializeComponent();
            SettingsConfig config = new SettingsConfig();
            handler = new DatabaseHandler();
            endTime = GetEndDate(config);

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

            GetDispatcherTimer();

            toolBar.Visibility = Visibility.Collapsed;
            ContentWindow.MouseMove += Window_MouseMove;
            ContentWindow.MouseLeave += Window_MouseInOut;
            ContentWindow.MouseEnter += Window_MouseInOut;

            SettingsButton.Click += (sender, e) => Settings_Clicked(sender, e, config);
            ExitButton.Click += ExitButton_Click;

            config.ValueChanged += (sender, e) => UpdateDispatcherTimer(sender, e, config);
        }

        private void UpdateDispatcherTimer(object sender, EventArgs e, SettingsConfig config)
        {
            endTime = GetEndDate(config);
            freeTime.Content = endTime.ToString("HH:mm");
        }

        private void GetDispatcherTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdateTimer;
            timer.Start();

            freeTime.Content = endTime.ToString("HH:mm");
        }

        private TimeSpan GetTimeRemaining(DateTime endTime)
        {
            if (DateTime.Now < endTime)
            {
                overTime.Visibility = Visibility.Collapsed;
                overTime.Foreground = Brushes.White;
                timeLeft.Foreground = Brushes.White;
                return endTime - DateTime.Now;
            }
            overTime.Visibility = Visibility.Visible;
            overTime.Foreground = Brushes.Red;
            timeLeft.Foreground = Brushes.Red;
            return DateTime.Now - endTime;
        }

        private void UpdateTimer(object sender, EventArgs e)
        {
            var tempTimeRemaining = GetTimeRemaining(endTime);
            var timeRemaining = new TimeSpan(tempTimeRemaining.Hours, tempTimeRemaining.Minutes,
                tempTimeRemaining.Seconds);
            timeLeft.Content = timeRemaining.ToString();
        }

        private void Settings_Clicked(object sender, RoutedEventArgs e, SettingsConfig config)
        {
            if (settingsWindow == null || !settingsWindow.IsOpen)
            {
                settingsWindow = null;
                settingsWindow = new SettingsWindow(config);
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

        private DateTime GetEndDate(SettingsConfig config)
        {
            DateTime startTime = GetStartDate();
            return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute,
                       startTime.Second) + new TimeSpan(config.Workday, config.Lunch, 0);
            
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
