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
using System.Data.SqlClient;


namespace interfejs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Użytkownik aktualnie zalogowany
        public CurrentUser usr { get; set; }

        //Nasza baza danych
        String dbServer;
        SqlConnection dbConnection;
        List<Control> kontrolki;
        List<Control> kontrolkiAdmin;

        public MainWindow()
        {
            InitializeComponent();
            kontrolki = new List<Control>();
            kontrolkiAdmin = new List<Control>();
            kontrolki.Add(this.wybieranieTabeli);
            kontrolki.Add(this.szukajBtn);
            kontrolki.Add(this.szukajBtn);
            kontrolki.Add(this.dodawanieRekorduBtn);
            kontrolki.Add(this.usunBtn);

            kontrolkiAdmin.Add(this.zarzadzanieTabelamiBtn);
            kontrolkiAdmin.Add(this.zarzadzanieUzytkownikamiBtn);
            /*for(var i = 0; i <10;++i)
                dataGrid.Items.Add(new kruwa() { Kolumna1 = "dana 1", Kolumna2 = "dana 2", Kolumna3 = "dana 3" });*/
            dbServer = "server=MUNESH-PC;database=windowapp;UID=sa;password=123";
            String query = "select * from data";
            try
            {
                dbConnection = new SqlConnection(dbServer);
                
                //SqlCommand cmd = new SqlCommand(query, dbConnection);
                /*dbConnection.Open();
                dbConnection.Close();*/
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var b = new AddNewRecord();
            b.Show();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var a = new AdminMenu(dbConnection);
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
                if (usr != null)
                {
                    foreach(var o in kontrolki)
                    {
                        o.IsEnabled = true;
                    }
                    if(usr.stanowisko == "Administrator")
                    {
                        foreach (var o in kontrolkiAdmin)
                        {
                            o.IsEnabled = true;
                            o.Visibility = Visibility.Visible;
                        }
                    }
                    this.loginButton.Content = "Wyloguj";
                    this.ktoZalogowany.Content = $"Witaj: {usr.name} {usr.surname}";
                }
            }
            else
            {
                foreach (var o in kontrolki)
                {
                    o.IsEnabled = false;
                }
                if (usr.stanowisko == "Administrator")
                {
                    foreach (var o in kontrolkiAdmin)
                    {
                        o.IsEnabled = false;
                        o.Visibility = Visibility.Hidden;
                    }
                }
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
