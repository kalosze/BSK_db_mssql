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
using System.Data;

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
        DataTable tablesFromDataBase;
        Dictionary<string, DataTable> tables;
        Dictionary<string, List<string>> tableColumns;
        Dictionary<string, List<DataRow>> tableColumnsMeta;


        public MainWindow()
        {
            InitializeComponent();

            //tutaj do listy dodajem kontrolki (po to by łatwiej je było włączać i wyłączać przy zalogowaniu/wylogowaniu)
            kontrolki = new List<Control>();
            kontrolkiAdmin = new List<Control>();
            kontrolki.Add(this.wybieranieTabeli);
            kontrolki.Add(this.szukajBtn);
            kontrolki.Add(this.szukajBtn);
            kontrolki.Add(this.dodawanieRekorduBtn);
            kontrolki.Add(this.usunBtn);


            //j.w. tylko że dla kontrolek administratora
            kontrolkiAdmin.Add(this.zarzadzanieTabelamiBtn);
            kontrolkiAdmin.Add(this.zarzadzanieUzytkownikamiBtn);

            //inicjalizujemy słowniki dla danych o tablicach w bazie danych
            tables = new Dictionary<string, DataTable>();
            tableColumns = new Dictionary<string, List<string>>();
            tableColumnsMeta = new Dictionary<string, List<DataRow>>();

            //dane do połączenia z bazą danych
            dbServer = "server=MICHAL-PC;database=BSK;Trusted_Connection=true";

            try
            {
                dbConnection = new SqlConnection(dbServer);

                //SqlCommand cmd = new SqlCommand(query, dbConnection);
                dbConnection.Open();

                //pobieramy metadane tabel z bazy danych
                tablesFromDataBase = dbConnection.GetSchema("Tables");

                //Wyciągamy tablice i nazwy kolumn w tablicach z bazy danych
                foreach (DataRow row in tablesFromDataBase.Rows)
                {

                    string tablename = (string)row[2];
                    //wybieramy z bazy danych dane o konkretnej kolumnie
                    string[] restrictions = new string[4];
                    //bo to jakoś wygląda tak że tam w [0] jest adres bazy danych, [1] nazwa bazy danych i [2] nazwa artefaktu
                    restrictions[2] = tablename;
                    //wysyłamy do bazy danych zapytanie o kolumny z tej tablicy (restrictions) 
                    var column = dbConnection.GetSchema("Columns", restrictions);
                    //dodajemy do słownika po nazwach metadane o tablicach
                    tables.Add(tablename, column);
                    List<string> lista = new List<string>();
                    List<DataRow> rowList = new List<DataRow>();
                    foreach (DataRow c in column.Rows)
                    {
                        //c.ColumnName
                        //przechodzimy po uzyskanych wynikach (meta danych kolumn z bazy danych) i wyciągamy nazwę z tego (która znajduje się na 3 pozycji [3]
                        lista.Add((string)c.ItemArray[3]);
                        rowList.Add(c);
                    }
                    //dodajemy naszą listę
                    tableColumns.Add(tablename, lista);
                    tableColumnsMeta.Add(tablename, rowList);
                    //MessageBox.Show(column.Rows[0].ToString());
                }
                //po tych operacjach w słowniku tables mamy metadane o tablicach połączone z ich nazwami
                //a w tableColumns mamy nazwy kolumn w poszczególnych tablicach (gdzie key to nazwa tablicy a value to lista z nazwami kolumn)
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
                    foreach (var o in kontrolki)
                    {
                        o.IsEnabled = true;
                    }
                    if (usr.etykieta == 0)
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
                    //var tables = Enum.GetNames(typeof(TabeleEnum));
                    foreach (string tableName in tables.Keys)
                    {
                        wybieranieTabeli.Items.Add(tableName);
                        //MessageBox.Show(tablename);
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
                dodawanieRekorduBtn.IsEnabled = false;
                usunBtn.IsEnabled = false;
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
            b.Owner = this;
            b.ShowDialog();
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

        //ustawienia tabel
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            var b = new tableControl();
            b.Show();
        }


        //jak wybierzemy tabele z combo boxa
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
