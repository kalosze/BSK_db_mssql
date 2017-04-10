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
        //private List<TextBox> textFields { get; set; }
        public AddNewRecord(string selectedTable)
        {
            InitializeComponent();
            this.selectedTable = selectedTable;
            typ = Database.tabele[(int)Enum.Parse(typeof(TabeleEnum), this.selectedTable)].Item3;
            record = Activator.CreateInstance(typ);
            //this.textFields = new List<TextBox>();
            CreateFields();
        }

        private void CreateFields()
        {
            var top = 31;
            int i = 0;
            var propertisy = typ.GetProperties();
            var numProp = propertisy.GetLength(0);
            var forHeight = numProp + (propertisy[0].Name == "ID" ? 0 : 1);
            this.Height = forHeight * top + 10 + add.Height + 20;
            for (var j = 0; j < numProp; ++j)
            {
                if (propertisy[j].Name == "ID") continue;
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
                    Content = propertisy[j].Name
                };
                var newTextbox = new TextBox()
                {
                    Height = 23,
                    Margin = new Thickness(113, 0, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Width = 120,
                    Name = propertisy[j].Name
                };
                //this.textFields.Add(newTextbox);
                newGrid.Children.Add(newLabel);
                newGrid.Children.Add(newTextbox);
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
            foreach (var table in tables)
            {
                if (selectedTable.Equals(table))
                {
                    var tabela = Database.tabele[(int)Enum.Parse(typeof(TabeleEnum), table)].Item2;
                    var propertisy = typ.GetProperties();
                    int noKey = propertisy[0].Name == "ID" ? 0 : 1;
                    for (var i = 0; i < propertisy.GetLength(0); ++i)
                    {
                        if (propertisy[i].Name == "ID")
                        {
                            propertisy[i].SetValue(record, (tabela.Count + 1).ToString());
                            continue;
                        }
                        propertisy[i].SetValue(record, ((TextBox)((Grid)addRecord.Children[i + 1 + noKey]).Children[1]).Text);
                    }
                    tabela.Add(record);

                    this.Close();
                }
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
