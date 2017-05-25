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
//using static interfejs.Database;
using System.Data;
using interfejs.Others;
using System.Globalization;
using System.Windows.Markup;
using System.Configuration;

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

        //śmieci trzeba będzie przeczyścić
        SqlConnection dbConnection;
        List<Control> kontrolki;
        List<Control> kontrolkiEdycji;
        List<Control> kontrolkiWyswietlania;
        List<Control> kontrolkiAdmin;
        DataTable tablesFromDataBase;
        List<string> tableNames;
        Dictionary<string, DataTable> tables;
        Dictionary<string, List<string>> tableColumns;
        Dictionary<string, Dictionary<string, string>> tableColumnsType;
        public Dictionary<string, int> tableAccesLvl { get; set; }
        Dictionary<string, Dictionary<string, bool>> primaryKeys;


        public MainWindow()
        {
            InitializeComponent();


            //Ustawiamy język na język systemu
            /*
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(
                        CultureInfo.CurrentCulture.IetfLanguageTag)));
                        */

            //tutaj do listy dodajem kontrolki (po to by łatwiej je było włączać i wyłączać przy zalogowaniu/wylogowaniu)
            kontrolki = new List<Control>();
            kontrolkiEdycji = new List<Control>();
            kontrolkiWyswietlania = new List<Control>();
            kontrolkiAdmin = new List<Control>();
            kontrolki.Add(this.wybieranieTabeli);
            kontrolki.Add(this.szukajBtn);
            kontrolki.Add(this.dodawanieRekorduBtn);
            kontrolki.Add(this.usunBtn);
            kontrolki.Add(this.edytujBtn);

            //kontrolkiBezWybierania.Add(this.wybieranieTabeli);

            kontrolkiEdycji.Add(this.dodawanieRekorduBtn);


            //kontrolkiWyswietlania.Add(this.wybieranieTabeli);
            kontrolkiWyswietlania.Add(this.szukajBtn);


            //j.w. tylko że dla kontrolek administratora
            kontrolkiAdmin.Add(this.zarzadzanieTabelamiBtn);
            kontrolkiAdmin.Add(this.zarzadzanieUzytkownikamiBtn);

            //inicjalizujemy słowniki dla danych o tablicach w bazie danych
            tableNames = new List<string>();
            tables = new Dictionary<string, DataTable>();
            tableColumns = new Dictionary<string, List<string>>();
            tableColumnsType = new Dictionary<string, Dictionary<string, string>>();
            tableAccesLvl = new Dictionary<string, int>();
            primaryKeys = new Dictionary<string, Dictionary<string, bool>>();

            // uczniowie = new List<Uczen>();


            //dane do połączenia z bazą danych//////////////////////////////////////////////////////////////////////////

            //utworzenie połączenia przy użyciu danych w App.config
            dbServer = ConfigurationManager.ConnectionStrings["sqlDB"].ToString();

            //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            try
            {
                dbConnection = new SqlConnection(dbServer);

                //SqlCommand cmd = new SqlCommand(query, dbConnection);
                dbConnection.Open();

                //pobieramy metadane tabel z bazy danych
                tablesFromDataBase = dbConnection.GetSchema("Tables");
                SqlDataReader reader;
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
                    Dictionary<string, bool> keys = new Dictionary<string, bool>();
                    foreach (DataRow c in column.Rows)
                    {
                        //przechodzimy po uzyskanych wynikach (meta danych kolumn z bazy danych) i wyciągamy nazwę z tego (która znajduje się na 3 pozycji [3]  - [7] to typ zmiennej
                        lista.Add((string)c.ItemArray[3]);
                        rowList.Add(c);
                        typy.Add((string)c.ItemArray[3], (string)c.ItemArray[7]);

                        //////////////////////////////////////////////////////// tutaj sprawdzamy czy kolumna jest kluczem głównym tabeli
                        String query = $"SELECT K.TABLE_NAME, "
                                    + $"K.COLUMN_NAME, "
                                    + $"K.CONSTRAINT_NAME "
                                    + $"FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C "
                                    + $"JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K "
                                    + $"ON C.TABLE_NAME = K.TABLE_NAME "
                                    + $"AND C.CONSTRAINT_CATALOG = K.CONSTRAINT_CATALOG "
                                    + $"AND C.CONSTRAINT_SCHEMA = K.CONSTRAINT_SCHEMA "
                                    + $"AND C.CONSTRAINT_NAME = K.CONSTRAINT_NAME "
                                    + $"WHERE C.CONSTRAINT_TYPE = 'PRIMARY KEY' "
                                    + $"AND K.COLUMN_NAME = '{(string)c.ItemArray[3]}'";
                        SqlCommand keyCmd = new SqlCommand(query, dbConnection);
                        keyCmd.ExecuteNonQuery();


                        reader = keyCmd.ExecuteReader();
                        reader.Read();
                        string nazwaTabeli = "";
                        if (reader.HasRows)
                            nazwaTabeli = reader.GetString(0);

                        reader.Close();

                        if (nazwaTabeli == tablename)
                            keys.Add((string)c.ItemArray[3], true);
                        else
                            keys.Add((string)c.ItemArray[3], false);
                        ////////////////////////////////////////////////////////
                    }
                    //dodajemy naszą listę
                    tableColumns.Add(tablename, lista);
                    tableColumnsType.Add(tablename, typy);
                    primaryKeys.Add(tablename, keys);
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
            string table = wybieranieTabeli.SelectedItem.ToString();
            var a = wybieranieTabeli.SelectedItem.ToString();
            var b = new AddNewRecord(table,
                tableColumns[table],
                dbConnection,
                tableColumnsType[table],
                primaryKeys[table]
                );
            b.ShowDialog();
            gridRefresh();
        }

        //guzik zarządzania użytkownikami
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
            string table = wybieranieTabeli.SelectedItem.ToString();
            var b = new Search(dbConnection,
                table,
                tableColumns[table],
                tableColumnsType[table],
                primaryKeys[table]
                );
            b.Owner = this;
            b.ShowDialog();
        }

        //usuwanie rekordów
        private void usunClick(object sender, RoutedEventArgs e)
        {
            DataRowView selected = (DataRowView)dataGrid.SelectedItem;
            string tab = wybieranieTabeli.SelectedItem.ToString();
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
            b.Owner = this;
            b.ShowDialog();
            gridRefresh();
        }


        //jak wybierzemy tabele z combo boxa
        private void ComboBox_ChangeTable(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            gridRefresh();

        }


        //dajemy dane na grida
        private void gridRefresh()
        {
            //dataGrid.Columns.Clear();
            infoLabel.Content = "";
            dataGrid.ItemsSource = null;
            if (wybieranieTabeli.SelectedItem == null)
                return;
            string jakaTabela = wybieranieTabeli.SelectedItem.ToString();

            usunBtn.IsEnabled = false;
            edytujBtn.IsEnabled = false;
            //może wyświetlać
            if (tableAccesLvl[jakaTabela] <= usr.etykieta || usr.etykieta == -1)
            {
                String query = $"SELECT * FROM {jakaTabela}";
                try
                {
                    dbConnection.Open();

                    var dataAdapter = new SqlDataAdapter(query, dbConnection);
                    DataTable ds = new DataTable();
                    dataAdapter.Fill(ds);
                    dataGrid.ItemsSource = ds.DefaultView;

                    dbConnection.Close();
                    enableControls(kontrolkiWyswietlania, true);
                    infoLabel.Content = $"Ilość rekordów: {dataGrid.Items.Count.ToString()}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                enableControls(kontrolkiWyswietlania, false);
            }
            //może edytować
            if (tableAccesLvl[jakaTabela] >= usr.etykieta || usr.etykieta == -1)
            {
                enableControls(kontrolkiEdycji, true);
                if (tableAccesLvl[jakaTabela] == usr.etykieta || usr.etykieta == -1)
                {
                    usunBtn.IsEnabled = true;
                    edytujBtn.IsEnabled = true;
                }
            }
            else

                enableControls(kontrolkiEdycji, false);

        }

        //metoda logowania
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
                usr = null;
                this.loginButton.Content = "Zaloguj";
                this.ktoZalogowany.Content = "";
            }
        }


        //funkcja do uzyskania etykiety tabeli
        private int getTableLvl(string name)
        {
            //var tables = Enum.GetNames(typeof(TabeleEnum));
            //string jakaTabela = name;
            String query = $"SELECT [ETYKIETA] FROM [ETYKIETY] WHERE [NAZWA_TABELI] like '{name}'";
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


        //metoda włączająca lub wyłączające kontrolki z listy
        private void enableControls(List<Control> kontrolki, bool enable)
        {
            foreach (var k in kontrolki)
                k.IsEnabled = enable;
        }

        //menu kontekstowe usuwania
        private void RowDeleteCommand(Object sender, RoutedEventArgs e)
        {
            usunClick(sender, e);
        }

        //zformatowanie danych do wyświetlenia tak by data wyświetlała się w jakimś ludzkim formacie
        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd-MMMM-yyyy";
            }
        }

        //odświeżenie z menu kontekstowego
        private void gridRefreshMenu(object sender, RoutedEventArgs e)
        {
            gridRefresh();
        }


        //eydcja wiersza 
        private void gridRowEdit(object sender, RoutedEventArgs e)
        {
            DataRowView selected = (DataRowView)dataGrid.SelectedItem;
            string table = wybieranieTabeli.SelectedItem.ToString();
            var okno = new RowEdit(
                table,
                tableColumns[table],
                dbConnection,
                tableColumnsType[table],
                primaryKeys[table],
                selected
                );
            okno.ShowDialog();
            gridRefresh();
        }

        //Metoda ustawiająca zawartość menu kontekstowego, w zależności od poziomu uprawnień
        private void dataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var menu = dataGrid.Resources["RowMenu"] as ContextMenu;
            menu.Items.Clear();
            if (tableAccesLvl[wybieranieTabeli.SelectedItem.ToString()] == usr.etykieta || usr.etykieta == -1)
            {
                var item1 = new MenuItem()
                {
                    Header = "Usuń",

                };
                item1.Click += new RoutedEventHandler(RowDeleteCommand);


                var item3 = new MenuItem()
                {
                    Header = "Edytuj"
                };
                item3.Click += new RoutedEventHandler(gridRowEdit);

                menu.Items.Add(item1);

                menu.Items.Add(item3);
            }
            var item2 = new MenuItem()
            {
                Header = "Odśwież"
            };
            item2.Click += new RoutedEventHandler(gridRefreshMenu);
            menu.Items.Add(item2);
            //dataGrid.Resources.Add("RowMenu", menu);
        }
    }

}
