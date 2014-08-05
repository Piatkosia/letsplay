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
        string[] tmplist;
        uint tmplistiterator = 0;
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
                    Gry.Add(new Game { gameName = queryObj["Name"].ToString(), pathFile = queryObj["GDFBinaryPath"].ToString(), InstallFolder = queryObj["GameInstallPath"].ToString() });
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
                MessageBox.Show(giera.pathFile);
                try
                {
                    System.Diagnostics.Process.Start(giera.pathFile);
                }
                catch (Exception)
                {
                    FindBinary(giera);
                }
                okienko.WindowState = WindowState.Minimized;
            }
        }

        private void FindBinary(Game giera)
        {
            if (tmplist == null)
            {
                
                var files = System.IO.Directory.GetFiles(giera.InstallFolder, "*.exe");
                tmplist = files;
            }
            if (tmplist[tmplistiterator].Contains("uninst"))
            { 
                tmplistiterator++; 
            }
            System.Diagnostics.Process.Start(tmplist[tmplistiterator]);
            tmplistiterator++;
        }
    }
}
