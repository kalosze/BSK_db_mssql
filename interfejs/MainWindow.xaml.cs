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
        List<Control> kontrolkiEdycji;
        List<Control> kontrolkiWyswietlania;
        List<Control> kontrolkiAdmin;
        DataTable tablesFromDataBase;
        List<string> tableNames;
        Dictionary<string, DataTable> tables;
        Dictionary<string, List<string>> tableColumns;
        Dictionary<string, List<DataRow>> tableColumnsMeta;
        Dictionary<string, Dictionary<string, string>> tableColumnsType;
        Dictionary<string, int> tableAccesLvl;


        public MainWindow()
        {
            InitializeComponent();

            //tutaj do listy dodajem kontrolki (po to by łatwiej je było włączać i wyłączać przy zalogowaniu/wylogowaniu)
            kontrolki = new List<Control>();
            kontrolkiEdycji = new List<Control>();
            kontrolkiWyswietlania = new List<Control>();
            kontrolkiAdmin = new List<Control>();
            kontrolki.Add(this.wybieranieTabeli);
            kontrolki.Add(this.szukajBtn);
            kontrolki.Add(this.dodawanieRekorduBtn);
            kontrolki.Add(this.usunBtn);

            //kontrolkiBezWybierania.Add(this.wybieranieTabeli);

            kontrolkiEdycji.Add(this.dodawanieRekorduBtn);
            kontrolkiEdycji.Add(this.usunBtn);

            //kontrolkiWyswietlania.Add(this.wybieranieTabeli);
            kontrolkiWyswietlania.Add(this.szukajBtn);


            //j.w. tylko że dla kontrolek administratora
            kontrolkiAdmin.Add(this.zarzadzanieTabelamiBtn);
            kontrolkiAdmin.Add(this.zarzadzanieUzytkownikamiBtn);

            //inicjalizujemy słowniki dla danych o tablicach w bazie danych
            tableNames = new List<string>();
            tables = new Dictionary<string, DataTable>();
            tableColumns = new Dictionary<string, List<string>>();
            tableColumnsMeta = new Dictionary<string, List<DataRow>>();
            tableColumnsType = new Dictionary<string, Dictionary<string, string>>();
            tableAccesLvl = new Dictionary<string, int>();

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
                    tableNames.Add(tablename);
                    tables.Add(tablename, column);
                    List<string> lista = new List<string>();
                    List<DataRow> rowList = new List<DataRow>();

                    Dictionary<string, string> typy = new Dictionary<string, string>();
                    foreach (DataRow c in column.Rows)
                    {
                        //c.ColumnName
                        //przechodzimy po uzyskanych wynikach (meta danych kolumn z bazy danych) i wyciągamy nazwę z tego (która znajduje się na 3 pozycji [3]  - [7] to typ zmiennej
                        lista.Add((string)c.ItemArray[3]);
                        rowList.Add(c);
                        typy.Add((string)c.ItemArray[3], (string)c.ItemArray[7]);
                    }
                    //dodajemy naszą listę
                    tableColumns.Add(tablename, lista);
                    tableColumnsMeta.Add(tablename, rowList);
                    tableColumnsType.Add(tablename, typy);
                }

                foreach (var tab in tableNames)
                {
                    int lvl = getTableLvl(tab);
                    tableAccesLvl.Add(tab, lvl);
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
            var a = wybieranieTabeli.SelectedItem.ToString();
            var b = new AddNewRecord(wybieranieTabeli.SelectedItem.ToString(), tableColumns[wybieranieTabeli.SelectedItem.ToString()], dbConnection, tableColumnsType[wybieranieTabeli.SelectedItem.ToString()]);
            b.ShowDialog();
            gridRefresh();
        }

        //guzik zarządzania użytkownikai
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var a = new AdminMenu(dbConnection);
            a.Owner = this;
            a.ShowDialog();
            gridRefresh();
            if (usr == null)
                logowanie(true);
        }

        //Guzik logowania
        private void loginClick(object sender, RoutedEventArgs e)
        {
            logowanie(false);
            //this.Close();
        }

        //Guzik szukania rekordów
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var b = new Search(dbConnection, wybieranieTabeli.SelectedItem.ToString(), tableColumns[wybieranieTabeli.SelectedItem.ToString()], tableColumnsType[wybieranieTabeli.SelectedItem.ToString()]);
            b.Owner = this;
            b.ShowDialog();
        }

        //usuwanie rekordów
        private void usunClick(object sender, RoutedEventArgs e)
        {
            DataRowView selected = (DataRowView)dataGrid.SelectedItem;
            string tab = wybieranieTabeli.SelectedItem.ToString();
            /*var tables = Enum.GetNames(typeof(TabeleEnum));
            foreach (var table in tables)
            {
                if (wybieranieTabeli.SelectedItem.Equals(table))
                {
                    Database.tabele[(int)Enum.Parse(typeof(TabeleEnum), table)].Item2.Remove(selected);
                    dataGrid.Items.Refresh();
                    return;
                }
            }*/

            try
            {
                String query = $"DELETE FROM {tab} WHERE {tableColumns[tab][0]} like '{selected.Row.ItemArray[0]}' AND {tableColumns[tab][1]} like '{selected.Row.ItemArray[1]}'";
                SqlCommand cmd = new SqlCommand(query, dbConnection);
                dbConnection.Open();
                cmd.ExecuteNonQuery();
                dbConnection.Close();
                gridRefresh();
                if (tab == "UZYTKOWNIK" && (int)selected.Row.ItemArray[0] == this.usr.id)
                    logowanie(true);

                /*users.Remove(selected);
                var tmp = listaUzytkownikow.SelectedItem;
                listaUzytkownikow.Items.Remove(tmp);

                listaUzytkownikow.Text = "";

                selected = null;
                sliderEtykiet.IsEnabled = false;*/

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbConnection.Close();
            }
        }

        //ustawienia tabel
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            var b = new tableControl(tableNames, dbConnection);
            b.ShowDialog();
            gridRefresh();
        }


        //jak wybierzemy tabele z combo boxa
        private void ComboBox_ChangeTable(object sender, SelectionChangedEventArgs e)
        {
            /*dodawanieRekorduBtn.IsEnabled = true;
            usunBtn.IsEnabled = true;
            szukajBtn.IsEnabled = true;*/
            if (e.AddedItems.Count == 0) return;
            gridRefresh();

        }


        //dajemy dane na grida
        private void gridRefresh()
        {
            //dataGrid.Columns.Clear();
            dataGrid.ItemsSource = null;
            if (wybieranieTabeli.SelectedItem == null)
                return;
            string jakaTabela = wybieranieTabeli.SelectedItem.ToString();


            //może wyświetlać
            if (tableAccesLvl[jakaTabela] <= usr.etykieta || usr.etykieta == -1)
            {
                String query = $"SELECT * FROM {jakaTabela}";
                dbConnection.Open();
                //SqlCommand cmd = new SqlCommand(query, dbConnection);
                var dataAdapter = new SqlDataAdapter(query, dbConnection);
                DataTable ds = new DataTable();
                dataAdapter.Fill(ds);
                dataGrid.ItemsSource = ds.DefaultView;
                dbConnection.Close();
                enableControls(kontrolkiWyswietlania, true);
            }
            else
                enableControls(kontrolkiWyswietlania, false);
            //może edytować
            if (tableAccesLvl[jakaTabela] >= usr.etykieta || usr.etykieta == -1)
            {
                enableControls(kontrolkiEdycji, true);
            }
            else
                enableControls(kontrolkiEdycji, false);

        }

        private void logowanie(bool wylogowanie)
        {
            if (usr == null && !wylogowanie)
            {
                var b = new Logowanie(dbConnection);
                b.Owner = this;
                //b.usr = usr;
                b.ShowDialog();
                if (usr != null)
                {
                    wybieranieTabeli.IsEnabled = true;
                    if (usr.etykieta <= 0)
                    {
                        foreach (var o in kontrolkiAdmin)
                        {
                            o.IsEnabled = true;
                            o.Visibility = Visibility.Visible;
                        }
                    }
                    this.loginButton.Content = "Wyloguj";
                    this.ktoZalogowany.Content = $"Witaj: {usr.name} {usr.surname}";
                    /*dodawanieRekorduBtn.IsEnabled = false;
                    usunBtn.IsEnabled = false;
                    szukajBtn.IsEnabled = false;*/

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
                enableControls(kontrolki, false);
                foreach (var o in kontrolkiAdmin)
                {
                    o.IsEnabled = false;
                    o.Visibility = Visibility.Hidden;
                }
                dataGrid.ItemsSource = null;
                wybieranieTabeli.Items.Clear();
                /*dodawanieRekorduBtn.IsEnabled = false;
                usunBtn.IsEnabled = false;
                szukajBtn.IsEnabled = false;*/
                usr = null;
                this.loginButton.Content = "Zaloguj";
                this.ktoZalogowany.Content = "";
            }
        }

        private int getTableLvl(string name)
        {
            //var tables = Enum.GetNames(typeof(TabeleEnum));
            string jakaTabela = name;
            String query = $"SELECT [ETYKIETA] FROM [ETYKIETY] WHERE [NAZWA_TABELI] like '{jakaTabela}'";
            int etykieta = -1;

            SqlCommand cmd = new SqlCommand(query, dbConnection);
            try
            {
                cmd.ExecuteNonQuery();
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                    etykieta = reader.GetInt32(0);
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }

            return etykieta;
        }

        private void enableControls(List<Control> kontrolki, bool enable)
        {
            foreach (var k in kontrolki)
                k.IsEnabled = enable;
        }
    }

}
