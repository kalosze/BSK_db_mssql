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
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        private string selectedTable { get; set; }
        //private object record { get; set; }
        //private Type typ { get; set; }
        List<string> columns;
        SqlConnection con;
        Dictionary<string, string> types;
        Dictionary<string, bool> keys;
        public Search(SqlConnection dbConnection,string selectedName, List<string> nt, Dictionary<string, string> t, Dictionary<string, bool> keys)
        {
            InitializeComponent();
            con = dbConnection;
            types = t;
            columns = nt;
            selectedTable = selectedName;
            this.keys = keys;
            CreateFields();
        }

        private void CreateFields()
        {
            var top = 31;
            int i = 0;
            var numProp = columns.Count;
            var forHeight = numProp + (keys[columns[0]] ? 0 : 1);
            this.Height = forHeight * top + 10 + szukajBtn.Height + 20;
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
                searchGrid.Children.Add(newGrid);
                ++i;
            }
            var addMargin = szukajBtn.Margin;
            szukajBtn.Margin = new Thickness(addMargin.Left, top * forHeight + 5 - szukajBtn.Height, addMargin.Right, addMargin.Bottom);
            var cancelMargin = anulujBtn.Margin;
            anulujBtn.Margin = new Thickness(cancelMargin.Left, top * forHeight + 5 - anulujBtn.Height, cancelMargin.Right, cancelMargin.Bottom);
        }

        private void szukajBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Owner as MainWindow;
            mainWindow.dataGrid.Columns.Clear();
            string a;
            String query = $"SELECT * FROM {selectedTable} WHERE ";
            bool first = true;
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
                        a = ((DatePicker)((Grid)searchGrid.Children[i + 1 + noKey]).Children[1]).Text;
                        if (a == "")
                            continue;
                        if (!first)
                        {
                            query += $" AND ";
                        }
                        else
                            first = false;
                        query += $"[{columns[i]}] like convert(date,'{a}',103)";
                        break;
                    default:
                        a = ((TextBox)((Grid)searchGrid.Children[i + 1 + noKey]).Children[1]).Text;
                        if (a == "")
                            continue;
                        if (!first)
                            query += $" AND ";
                        else
                            first = false;
                        query += $"[{columns[i]}] like '{a}'";
                        break;
                }
                // propertisy[i].SetValue(record, ((TextBox)((Grid)addRecord.Children[i + 1 + noKey]).Children[1]).Text);
            }
            //tabela.Add(record);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                var dataAdapter = new SqlDataAdapter(query, con);
                System.Data.DataTable ds = new System.Data.DataTable();
                dataAdapter.Fill(ds);
                mainWindow.dataGrid.ItemsSource = ds.DefaultView;
                con.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void anulujBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                szukajBtn_Click(this, e);
            }
            else if (e.Key == Key.Escape)
            {
                e.Handled = true;
                anulujBtn_Click(this, e);
            }
        }
    }
}
