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
    /// Interaction logic for newUser.xaml
    /// </summary>
    public partial class newUser : Window
    {
        SqlConnection dbConnection;
        public newUser(SqlConnection con) : base()
        {
            InitializeComponent();
            dbConnection = con;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String cmd;

            }
            catch
            {

            }
        }
    }
}
