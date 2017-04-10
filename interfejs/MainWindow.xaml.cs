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
using static interfejs.Database;


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
            dbServer = "server=MICHAL-PC;database=BSK;Trusted_Connection=true";
           // String query = "select * from data";
            try
            {
                dbConnection = new SqlConnection(dbServer);
                
                //SqlCommand cmd = new SqlCommand(query, dbConnection);
                dbConnection.Open();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                dbConnection.Close();
                this.Close();
            }
        }

        //Dodawanie nowych rekordów
        private void button_Click(object sender, RoutedEventArgs e)
        {
            var b = new AddNewRecord(wybieranieTabeli.SelectedItem.ToString());
            b.ShowDialog();
            dataGrid.Items.Refresh();
        }

        //guzik zarządzania użytkownikai
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var a = new AdminMenu(dbConnection);
            a.Owner = this;
            a.Show();
        }

        //Guzik logowania
        private void loginClick(object sender, RoutedEventArgs e)
        {
            if (usr == null)
            {
                var b = new Logowanie(dbConnection);
                b.Owner = this;
                //b.usr = usr;
                b.ShowDialog();
                if (usr != null)
                {
                    foreach(var o in kontrolki)
                    {
                        o.IsEnabled = true;
                    }
                    if(usr.etykieta == 0)
                    {
                        foreach (var o in kontrolkiAdmin)
                        {
                            o.IsEnabled = true;
                            o.Visibility = Visibility.Visible;
                        }
                    }
                    this.loginButton.Content = "Wyloguj";
                    this.ktoZalogowany.Content = $"Witaj: {usr.name} {usr.surname}";
                    dodawanieRekorduBtn.IsEnabled = false;
                    usunBtn.IsEnabled = false;

                    /* dropdown lista tabel z fejkowej bazy danych */
                    var tables = Enum.GetNames(typeof(TabeleEnum));
                    foreach (var table in tables)
                    {
                        wybieranieTabeli.Items.Add(table);
                    }
                }
            }
            else
            {
                foreach (var o in kontrolki)
                {
                    o.IsEnabled = false;
                }
                if (usr.etykieta == 0)
                {
                    foreach (var o in kontrolkiAdmin)
                    {
                        o.IsEnabled = false;
                        o.Visibility = Visibility.Hidden;
                    }
                }
                dataGrid.ItemsSource = null;
                wybieranieTabeli.Items.Clear();

                usr = null;
                this.loginButton.Content = "Zaloguj";
                this.ktoZalogowany.Content = "";
            }
            //this.Close();
        }

        //Guzik szukania rekordów
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var b = new Search();
            b.Show();
        }

        //usuwanie rekordów
        private void usunClick(object sender, RoutedEventArgs e)
        {
            var selected = dataGrid.SelectedItem;
            var tables = Enum.GetNames(typeof(TabeleEnum));
            foreach (var table in tables)
            {
                if (wybieranieTabeli.SelectedItem.Equals(table))
                {
                    Database.tabele[(int)Enum.Parse(typeof(TabeleEnum), table)].Item2.Remove(selected);
                    dataGrid.Items.Refresh();
                    return;
                }
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            var b = new tableControl();
            b.Show();
        }

         private void ComboBox_ChangeTable(object sender, SelectionChangedEventArgs e)
        {
            dodawanieRekorduBtn.IsEnabled = true;
            usunBtn.IsEnabled = true;
            if (e.AddedItems.Count == 0) return;
            var tables = Enum.GetNames(typeof(TabeleEnum));
            foreach (var table in tables)
            {
                if (wybieranieTabeli.SelectedItem.Equals(table))
                {
                    dataGrid.Columns.Clear();
                    dataGrid.ItemsSource = Database.tabele[(int)Enum.Parse(typeof(TabeleEnum), table)].Item2;

                    return;
                }
            }
        }
    }

}
