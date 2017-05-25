using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace interfejs
{
    /// <summary>
    /// Interaction logic for AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Window
    {
        List<User> users;
        public SqlConnection dbConnection { get; }
        User selected;
        public AdminMenu(SqlConnection con)
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

        //jesli zakończymy pracę w tym okienku, to po kliknięciu OK nasze zmiany zostają wysłane do bazy danych (robim update) 
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

        //gdy wybierzemy z listy innego użytkownika
        private void listaUzytkownikow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Owner as MainWindow).usr.etykieta < 0) && showPass.Visibility == Visibility.Hidden)
                showPass.Visibility = Visibility.Visible;

            foreach (var u in users)
            {
                if (listaUzytkownikow.SelectedItem != null && u.login == this.listaUzytkownikow.SelectedItem.ToString())
                {
                    usunBtn.IsEnabled = true;
                    passReset.IsEnabled = true;
                    showPass.IsEnabled = true;
                    selected = u;
                    sliderEtykiet.IsEnabled = true;
                    sliderEtykiet.Value = selected.etykieta;
                }
            }
        }

        //jeśli zmienimy slider do sterowania etykietami
        private void sliderEtykiet_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (listaUzytkownikow.SelectedItem != null)
                selected.etykieta = (int)(sliderEtykiet.Value);
        }

        //jeżeli chcemy usunąć istniejącego użytkownika :(
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

                MainWindow mainWindow = Owner as MainWindow;
                if (selected.id == mainWindow.usr.id)
                {
                    mainWindow.usr = null;
                    dbConnection.Close();
                    this.Close();
                }
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                okBtn_Click(this, e);
            }
            else if (e.Key == Key.Escape)
            {
                e.Handled = true;
                anulujBtn_Click(this, e);
            }

        }

        private void passReset_Click(object sender, RoutedEventArgs e)
        {
            var name = listaUzytkownikow.SelectedItem;
            if (name != null)
            {
                var resetWindow = new PassReset(selected, dbConnection);
                resetWindow.Owner = this;
                resetWindow.ShowDialog();
            }
        }

        private void showPass_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Encryptor.Decrypt(selected.pass, selected.login));
        }
    }
}
