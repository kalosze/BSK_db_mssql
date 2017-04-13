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
    /// Interaction logic for tableControl.xaml
    /// </summary>
    public partial class tableControl : Window
    {
        List<string> tables;
        List<Etykieta> etykiety;
        SqlConnection con;
        Etykieta selected;
        public tableControl(List<string> tables, SqlConnection con)
        {
            InitializeComponent();
            this.tables = tables;
            this.con = con;
            updateTables();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd;
                String query;
                MainWindow mainWindow = Owner as MainWindow;
                foreach (var et in etykiety)
                {
                    query = $"UPDATE ETYKIETY SET ETYKIETA='{et.etykieta}' WHERE [ID_ETYKIETY] like '{et.id}'";
                    cmd = new SqlCommand(query, con);

                    cmd.ExecuteNonQuery();

                    mainWindow.tableAccesLvl[et.nazwaTabeli] = et.etykieta;
                }
               
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
            this.Close();
        }

        private void anulujBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void slider_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (wyborTabeli.SelectedItem != null)
                selected.etykieta = (int)(slider.Value);
        }

        private void wyborTabeli_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var u in etykiety)
            {
                if (wyborTabeli.SelectedItem != null && u.nazwaTabeli == this.wyborTabeli.SelectedItem.ToString())
                {
                    selected = u;
                    slider.IsEnabled = true;
                    slider.Value = selected.etykieta;
                }
            }
        }

        private void updateTables()
        {
            etykiety = new List<Etykieta>();
            wyborTabeli.Items.Clear();
            try
            {
                //wiadomka
                String query = $"SELECT * FROM ETYKIETY";
                //zbinduj skrypt sql z połączeniem do sqlcommanda
                SqlCommand cmd = new SqlCommand(query, con);
                //uchwyt na okno główne
                MainWindow mainWindow = Owner as MainWindow;
                //otwieramy połączenie z bazą danych
                con.Open();
                //wykonujemy skrypt na bazie danych
                cmd.ExecuteNonQuery();
                SqlDataReader reader;
                //baza danych zwróciła wynik zapytania i przerzucamy odpowiedź na reader'a
                reader = cmd.ExecuteReader();

                //póki mamy dane to je odczytuj (każde wywołanie read bieże kolejny wiersz)
                while (reader.Read())
                {
                    //tworzymy użytkowników na podstawie danych z bazy danych (reader.get[tutaj typ zmiennej (string int itp)]([tutaj podajemy z której kolumny odczytać dane])
                    var etykieta = new Etykieta(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                    //dodajemy do listy użytkowników
                    etykiety.Add(etykieta);

                }
                //jak skończylyśmy to zamykamy reader'a 
                reader.Close();
                //i połączenie z bazą danych
                con.Close();

                //tutaj dodajemy wszystkich użytkowników to combo boxa (rozwijanej listy)()tzn nie userów a same loginy
                foreach (var u in etykiety)
                {
                    this.wyborTabeli.Items.Add(u.nazwaTabeli);
                }

            }
            catch (Exception ex)
            {
                //obsługa błedów
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private class Etykieta
        {
            public int id { get; }
            public string nazwaTabeli { get; }
            public int etykieta { get; set; }
            public Etykieta(int id, string nazwa, int e)
            {
                this.id = id;
                this.nazwaTabeli = nazwa;
                this.etykieta = e;
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
    }


}
