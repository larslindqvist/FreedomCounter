using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FreedomCounter
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private SettingsConfig config;
        private DateTime startTime;
        private DateTime endTime;
        public SettingsWindow(SettingsConfig config)
        {
            InitializeComponent();
            this.config = config;
            config.GetSetting("Workday");
            config.GetSetting("Lunch");
            int workday = config.Workday;
            int lunch = config.Lunch;
            SettingsWindowObj.MouseMove += Window_MouseMove;
            this.workday.Text = workday.ToString();
            this.workday.TextChanged += Workday_Changed;
            this.lunch.Text = lunch.ToString();
            this.lunch.TextChanged += Lunch_TextChanged;
            ExitButton.Click += ExitButton_Click;
            IsOpen = true;
        }

        public bool IsOpen { get; set; }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
            this.Close();
        }
        private void Workday_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                config.Workday = Convert.ToInt32(workday.Text);
                workday.Background = Brushes.White;
                SetEndTime(startTime, endTime);
            }
            catch (Exception)
            {
                workday.Background = Brushes.Red;
            }
        }
        private void Lunch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                config.Lunch = Convert.ToInt32(lunch.Text);
                lunch.Background = Brushes.White;
                SetEndTime(startTime, endTime);
            }
            catch (Exception)
            {
                lunch.Background = Brushes.Red;
            }

        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void SetEndTime(DateTime startTime, DateTime endTime)
        {
            this.endTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, startTime.Second) + new TimeSpan(config.Workday, config.Lunch, 0);
        }
    }

}
