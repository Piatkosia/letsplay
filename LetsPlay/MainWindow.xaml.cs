using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;

namespace LetsPlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Game> Gry;
        public MainWindow()
        {
            InitializeComponent();
            
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2\\Applications\\Games",
                    "SELECT * FROM Game"); 
                Gry = new ObservableCollection<Game>();
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    
                    Gry.Add(new Game{gameName = queryObj["Name"].ToString(), pathFile = queryObj["GDFBinaryPath"].ToString() });
                }
                listaGier.DataContext = Gry;
            }
            catch (ManagementException e)
            {
                MessageBox.Show("Coś nie poszło: " + e.Message);
            }
            
        }
    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is Game)
            {
                Game giera = (Game)cmd.DataContext;
                System.Diagnostics.Process.Start(giera.pathFile);
                okienko.WindowState = WindowState.Minimized;
            }
        }
    }
}
