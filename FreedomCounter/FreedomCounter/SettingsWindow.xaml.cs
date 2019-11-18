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
        public SettingsWindow()
        {
            InitializeComponent();
            config = new SettingsConfig();
            config.GetSetting("Workday");
            config.GetSetting("Lunch");
            int workday = config.Workday;
            int lunch = config.Lunch;
            SettingsWindowObj.MouseMove += Window_MouseMove;
            this.workday.Text = workday.ToString();
            this.workday.TextChanged += Workday_Changed;
            this.lunch.Text = lunch.ToString();
            this.lunch.TextChanged += lunch_TextChanged;
            ExitButton.Click += ExitButton_Click;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Workday_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                config.Workday = Convert.ToInt32(workday.Text);
                workday.Background = Brushes.White;
            }
            catch (Exception)
            {
                workday.Background = Brushes.Red;
            }
        }
        private void lunch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                config.Lunch = Convert.ToInt32(lunch.Text);
                lunch.Background = Brushes.White;
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
    }

}
