using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace interfejs
{

    public partial class AddNewRecord : Window
    {
        private string selectedTable { get; set; }
        private object record { get; set; }
        private Type typ { get; set; }
        private List<string> columns;
        SqlConnection con;

        Dictionary<string, string> types;
        Dictionary<string, bool> keys;
        public AddNewRecord(string selectedTable, List<string> c, SqlConnection con, Dictionary<string, string> types, Dictionary<string, bool> keys)
        {
            InitializeComponent();
            this.selectedTable = selectedTable;
            columns = c;
            this.types = types;
            this.con = con;
            this.keys = keys;

            CreateFields();
        }

        //budujemy pola w naszym okienku
        private void CreateFields()
        {
            var top = 31;
            int i = 0;
            //ustawiamy wymiary
            var numProp = columns.Count;
            var forHeight = numProp + (keys[columns[i]] ? 0 : 1);
            this.Height = forHeight * top + 10 + add.Height + 20;
            //pokolei dodajemy kolejne pola
            for (var j = 0; j < numProp; ++j)
            {
                if (keys[columns[j]]) continue;
                //najpierw grid, by reszta elementw miała do czego się przyczepić
                var newGrid = new Grid()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 26,
                    Margin = new Thickness(10, 10 + top * i, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 233
                };
                //opis pola
                var newLabel = new Label()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,

                    Content = columns[j]
                };
                newGrid.Children.Add(newLabel);
                //var elo = types[columns[j]];
                //tutaj dodajemy już same pola
                switch (types[columns[j]])
                {
                    case "date":
                        //jeżeli typ pola to dbdate to wstawiamy panel wyboru daty
                        var newTextboxDate = new DatePicker()
                        {
                            Height = 23,
                            Margin = new Thickness(113, 0, 0, 0),
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Width = 120,
                            Name = columns[j]

                        };
                        newGrid.Children.Add(newTextboxDate);
                        break;
                    default:
                        //a w innych przypadkach zwykły textBox
                        var newTextbox = new TextBox()
                        {
                            Height = 23,
                            Margin = new Thickness(113, 0, 0, 0),
                            TextWrapping = TextWrapping.Wrap,
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Width = 120,
                            Name = columns[j]

                        };
                        newGrid.Children.Add(newTextbox);
                        break;
                }
                addRecord.Children.Add(newGrid);
                ++i;
            }
            //ustawiamy nową pozycję guzików
            var addMargin = add.Margin;
            add.Margin = new Thickness(addMargin.Left, top * forHeight + 5 - add.Height, addMargin.Right, addMargin.Bottom);
            var cancelMargin = cancel.Margin;
            cancel.Margin = new Thickness(cancelMargin.Left, top * forHeight + 5 - cancel.Height, cancelMargin.Right, cancelMargin.Bottom);
        }

        //jeżeli zakończymy edycję, to wysyłamy do bazy danych dane
        private void add_Click(object sender, RoutedEventArgs e)
        {
            //piszemy skrypt
            String query = $"INSERT INTO {selectedTable} (";
            SqlCommand cmd = new SqlCommand(query, con);
            //wyliczamy kolumny jakie trzeba dodać
            for (int i = 0; i < columns.Count; ++i)
            {
                if (keys[columns[i]])
                    continue;
                query += $"{columns[i]}";

                if (i != columns.Count - 1)
                    query += $", ";
            }
            //rozwijamy skrypt, teraz będziemy dodawać wartości do kolumn
            query += $") VALUES (";

            //sprawdzamy czy klucz główny
            int noKey = keys[columns[0]] ? 0 : 1;
            //pokolei lecimy po polach i dodajemy wartości do skryptu
            for (var i = 0; i < columns.Count; ++i)
            {
                //jeżeli klucz główny to pomiń, klucz główny jest generowany automatycznie przez bazę danych
                if (keys[columns[i]])
                {
                    continue;
                }
                switch (types[columns[i]])
                {
                    case "date":
                        //jeżeli data, to pobieramy dane
                        string a = ((DatePicker)((Grid)addRecord.Children[i + 1 + noKey]).Children[1]).Text;
                        //niestety trzeba przekonwertować z europejskiego formatu na bazodanowych (US)
                        query += $"convert(date,@date{i},103)";
                        //wstawiamy wartość parametru (w celach bezpieczeństwa w ten sposób),
                        //gdyż taki sposób dodawania wartości uratuje nas przed destruktównym działaniem użytkowników,
                        //którzy wpisali by sobie w pole IMIE np DELETE db etc.
                        cmd.Parameters.AddWithValue($"@date{i}", a);
                        break;
                    default:
                        //wszystko inne co nie data obrabiamy tu
                        //przy okazji sprawdzamy czy ocena albo etykieta nie wybiega poza skalę
                        if(columns[i] == "OCENA")
                        {
                            try
                            {
                                var ocena = Convert.ToDouble((((TextBox)((Grid)addRecord.Children[i + 1 + noKey]).Children[1]).Text));
                                if (ocena < 1 || ocena > 6)
                                    throw new Exception();
                            }
                            catch
                            {
                                MessageBox.Show("Zły format oceny, dopuszczalne wartości to [1,6]");
                                return;
                            }
                        }
                        if (columns[i] == "ETYKIETA")
                        {
                            try
                            {
                                var etykieta = Convert.ToInt32((((TextBox)((Grid)addRecord.Children[i + 1 + noKey]).Children[1]).Text));
                                if (etykieta < 0 || etykieta > 3)
                                    throw new Exception();
                            }
                            catch
                            {
                                MessageBox.Show("Zła etykieta, dopuszczalne wartości to [0,3]");
                                return;
                            }

                        }
                        query += $"@value{i}";
                        cmd.Parameters.AddWithValue($"@value{i}", ((TextBox)((Grid)addRecord.Children[i + 1 + noKey]).Children[1]).Text);
                        break;
                }
                //jeżeli to nie jest ostatni wpis to dodaj przecinek
                if (i != columns.Count - 1)
                    query += $", ";
            }
            //na koniec zamykamy nawias
            query += $")";
            try
            {
                //łączymy się z bd i wykonujemy skrypt
                con.Open();
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                con.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        //jak anuluj to anuluj
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //ustawienie działania wciśnięć klawiszy klawiatury na okno
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                add_Click(this, e);
            }
            else if (e.Key == Key.Escape)
            {
                e.Handled = true;
                cancel_Click(this, e);
            }
        }
    }
}
