using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace interfejs
{
    /// <summary>
    /// Interaction logic for AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Window
    {
        List<User> users;
        SqlConnection dbConnection;
        User selected;
        public AdminMenu(SqlConnection con) : base()
        {
            InitializeComponent();
            sliderEtykiet.IsEnabled = false;
            dbConnection = con;
            updateUsers();
        }

        private void addNewUserClick(object sender, RoutedEventArgs e)
        {
            var c = new newUser(dbConnection);
            c.Owner = this.Owner;
            c.ShowDialog();
            updateUsers();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dbConnection.Open();
                SqlCommand cmd;
                String query;
                foreach (var u in users)
                {
                    query = $"UPDATE UZYTKOWNIK SET ETYKIETA='{u.etykieta}' WHERE [ID_UZYTKOWNIKA] like '{u.id}'";
                    cmd = new SqlCommand(query, dbConnection);

                    cmd.ExecuteNonQuery();

                }
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbConnection.Close();
            }
            this.Close();
        }

        private void listaUzytkownikow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var u in users)
            {
                if (listaUzytkownikow.SelectedItem != null && u.login == this.listaUzytkownikow.SelectedItem.ToString())
                {
                    selected = u;
                    sliderEtykiet.IsEnabled = true;
                    sliderEtykiet.Value = selected.etykieta;
                }
            }
        }

        private void sliderEtykiet_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (listaUzytkownikow.SelectedItem != null)
                selected.etykieta = (int)(sliderEtykiet.Value);
        }

        private void usunBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String query = $"DELETE FROM UZYTKOWNIK WHERE [ID_UZYTKOWNIKA] like '{selected.id}'";
                SqlCommand cmd = new SqlCommand(query, dbConnection);
                dbConnection.Open();
                cmd.ExecuteNonQuery();
                dbConnection.Close();
                users.Remove(selected);
                var tmp = listaUzytkownikow.SelectedItem;
                listaUzytkownikow.Items.Remove(tmp);

                listaUzytkownikow.Text = "";

                selected = null;
                sliderEtykiet.IsEnabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbConnection.Close();
            }
        }

        private void anulujBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void updateUsers()
        {
            users = new List<User>();
            listaUzytkownikow.Items.Clear();
            try
            {
                //wiadomka
                String query = $"SELECT * FROM UZYTKOWNIK";
                //zbinduj skrypt sql z połączeniem do sqlcommanda
                SqlCommand cmd = new SqlCommand(query, dbConnection);
                //uchwyt na okno główne
                MainWindow mainWindow = Owner as MainWindow;
                //otwieramy połączenie z bazą danych
                dbConnection.Open();
                //wykonujemy skrypt na bazie danych
                cmd.ExecuteNonQuery();
                SqlDataReader reader;
                //baza danych zwróciła wynik zapytania i przerzucamy odpowiedź na reader'a
                reader = cmd.ExecuteReader();

                //póki mamy dane to je odczytuj (każde wywołanie read bieże kolejny wiersz)
                while (reader.Read())
                {
                    //tworzymy użytkowników na podstawie danych z bazy danych (reader.get[tutaj typ zmiennej (string int itp)]([tutaj podajemy z której kolumny odczytać dane])
                    var user = new CurrentUser(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6));
                    //dodajemy do listy użytkowników
                    users.Add(user);

                }
                //jak skończylyśmy to zamykamy reader'a 
                reader.Close();
                //i połączenie z bazą danych
                dbConnection.Close();

                //tutaj dodajemy wszystkich użytkowników to combo boxa (rozwijanej listy)()tzn nie userów a same loginy
                foreach (var u in users)
                {
                    this.listaUzytkownikow.Items.Add(u.login);
                }

            }
            catch (Exception ex)
            {
                //obsługa błedów
                MessageBox.Show(ex.Message);
                dbConnection.Close();
            }
        }
    }
}
