using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        DispatcherTimer _timer;
        TimeSpan _time;
        private Window settingsWindow;
        private int _Counter;

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



             DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
             {
                 GetWorkdayValue();
                 _Counter += 1;
                 timeLeft.Content = TimeSpan.FromSeconds(_Counter).ToString();
                 freeTime.Content = (DateTime.Now + TimeSpan.FromHours(config.Workday)).ToShortTimeString();
                 overtime.Content = "00:00";
             }, this.Dispatcher);


            toolBar.Visibility = Visibility.Collapsed;
            ContentWindow.MouseMove += Window_MouseMove;
            ContentWindow.MouseLeave += Window_MouseInOut;
            ContentWindow.MouseEnter += Window_MouseInOut;

            SettingsButton.Click += Settings_Clicked;

        }

        private void GetWorkdayValue()
        {
            config = new SettingsConfig();
            config.GetSetting("Workday");
        }

        private void Settings_Clicked(object sender, RoutedEventArgs e)
        {
            if (settingsWindow == null)
            {
                settingsWindow = new SettingsWindow();
                settingsWindow.Show();
            }
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
