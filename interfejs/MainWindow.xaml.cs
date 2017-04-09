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

namespace interfejs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CurrentUser usr { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            /*for(var i = 0; i <10;++i)
                dataGrid.Items.Add(new kruwa() { Kolumna1 = "dana 1", Kolumna2 = "dana 2", Kolumna3 = "dana 3" });*/
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var b = new AddNewRecord();
            b.Show();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var a = new AdminMenu();
            a.Show();
        }

        private void loginClick(object sender, RoutedEventArgs e)
        {
            if (usr == null)
            {
                var b = new Logowanie();
                b.Owner = this;
                //b.usr = usr;
                b.ShowDialog();
                this.loginButton.Content = "Wyloguj";
                this.ktoZalogowany.Content = $"Witaj: {usr.name} {usr.surname}";
            }
            else
            {
                usr = null;
                this.loginButton.Content = "Zaloguj";
                this.ktoZalogowany.Content = "";
            }
            //this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var b = new Search();
            b.Show();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            var b = new tableControl();
            b.Show();
        }
    }

    /*public class kruwa
    {
        public string Kolumna1 { get; set; }
        public string Kolumna2 { get; set; }
        public string Kolumna3 { get; set; }
    }*/


}
