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
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static interfejs.Database;

namespace interfejs
{
    /// <summary>
    /// Interaction logic for AddNewRecord.xaml
    /// </summary>
    public partial class AddNewRecord : Window
    {
        private string selectedTable { get; set; }
        private object record { get; set; }
        private Type typ { get; set; }
        private List<string> columns;
        SqlConnection con;
        Dictionary<string, string> types;
        Dictionary<string, bool> keys;
        //private List<TextBox> textFields { get; set; }
        public AddNewRecord(string selectedTable, List<string> c, SqlConnection con, Dictionary<string, string> types, Dictionary<string, bool> keys)
        {
            InitializeComponent();
            this.selectedTable = selectedTable;
            columns = c;
            this.types = types;
            this.con = con;
            this.keys = keys;
            ////////////////////////////////////////////////////////////////////////////////////////
            /*
            typ = Database.tabele[(int)Enum.Parse(typeof(TabeleEnum), this.selectedTable)].Item3;
            record = Activator.CreateInstance(typ);
            */
            //this.textFields = new List<TextBox>();
            CreateFields();
        }

        private void CreateFields()
        {
            var top = 31;
            int i = 0;
            //var propertisy = typ.GetProperties();
            //var numProp = propertisy.GetLength(0);
            var numProp = columns.Count;
            var forHeight = numProp + (keys[columns[i]] ? 0 : 1);
            this.Height = forHeight * top + 10 + add.Height + 20;
            for (var j = 0; j < numProp; ++j)
            {
                if (keys[columns[j]]) continue;
                var newGrid = new Grid()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 26,
                    Margin = new Thickness(10, 10 + top * i, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 233
                };
                var newLabel = new Label()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    //Content = propertisy[j].Name
                    Content = columns[j]
                };
                newGrid.Children.Add(newLabel);
                var elo = types[columns[j]];
                switch (types[columns[j]])
                {
                    case "date":

                        var newTextboxDate = new DatePicker()
                        {
                            Height = 23,
                            Margin = new Thickness(113, 0, 0, 0),
                            //TextWrapping = TextWrapping.Wrap,
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Width = 120,
                            Name = columns[j]

                        };
                        newGrid.Children.Add(newTextboxDate);
                        break;
                    default:
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

                //this.textFields.Add(newTextbox);


                addRecord.Children.Add(newGrid);
                ++i;
            }
            var addMargin = add.Margin;
            add.Margin = new Thickness(addMargin.Left, top * forHeight + 5 - add.Height, addMargin.Right, addMargin.Bottom);
            var cancelMargin = cancel.Margin;
            cancel.Margin = new Thickness(cancelMargin.Left, top * forHeight + 5 - cancel.Height, cancelMargin.Right, cancelMargin.Bottom);
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            var tables = Enum.GetNames(typeof(TabeleEnum));

            /* String query2 = $"INSERT INTO UZYTKOWNIK (LOGIN, HASLO, IMIE, NAZWISKO, STANOWISKO, ETYKIETA) VALUES " +
                         $"('{this.login.Text}', " +
                         $"'{this.pass1.Password}', " +
                         $"'{this.imie.Text}', " +
                         $"'{this.nazwisko.Text}', " +
                         $"'{this.listaStanowisk.Text}', " +
                         $"5)";*/

            String query = $"INSERT INTO {selectedTable} (";
            for (int i = 0; i < columns.Count; ++i)
            {
                if (keys[columns[i]])
                    continue;
                query += $"{columns[i]}";

                if (i != columns.Count - 1)
                    query += $", ";
            }
            query += $") VALUES (";

            /*selectedTable = "Uczen";
            typ = Database.tabele[(int)Enum.Parse(typeof(TabeleEnum), this.selectedTable)].Item3;
            record = Activator.CreateInstance(typ);*/
            //foreach (var table in tables)
            //{
            //if (selectedTable.Equals(table))
            //{
            // var tabela = Database.tabele[(int)Enum.Parse(typeof(TabeleEnum), table)].Item2;
            // var propertisy = typ.GetProperties();
            int noKey = keys[columns[0]] ? 0 : 1;
            for (var i = 0; i < columns.Count; ++i)
            {
                if (keys[columns[i]])
                {
                    //propertisy[i].SetValue(record, (tabela.Count + 1).ToString());
                    continue;
                }
                switch (types[columns[i]])
                {
                    case "date":
                        string a = ((DatePicker)((Grid)addRecord.Children[i + 1 + noKey]).Children[1]).Text;
                        query += $"convert(date,'{a}',103)";
                        break;
                    default:
                        query += $"'{((TextBox)((Grid)addRecord.Children[i + 1 + noKey]).Children[1]).Text}'";
                        break;
                }
                if (i != columns.Count - 1)
                    query += $", ";
                // propertisy[i].SetValue(record, ((TextBox)((Grid)addRecord.Children[i + 1 + noKey]).Children[1]).Text);
            }
            query += $")";
            //tabela.Add(record);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
            //this.Close();
            // }
            //}
            //SqlCommand cmd = new SqlCommand(query, con);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

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
