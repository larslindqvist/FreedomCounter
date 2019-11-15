using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            int workday = config.Workday;
            SettingsWindowObj.MouseMove += Window_MouseMove;
            this.workday.Text = workday.ToString();
            this.workday.TextChanged += Workday_Changed;
        }

        private void Workday_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                config.Workday = Convert.ToInt32(workday.Text);
                workday.Background = Brushes.White;
            }
            catch (Exception ex)
            {
                workday.Background = Brushes.Red;
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
