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
using System.Security.Cryptography;

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
                if (this.pass1.Password == this.pass2.Password)
                {
                    var pass = Encryptor.Encrypt(pass1.Password, this.login.Text);
                    String query = $"INSERT INTO UZYTKOWNIK (LOGIN, HASLO, IMIE, NAZWISKO, STANOWISKO, ETYKIETA) VALUES "+
                        $"('{this.login.Text}', "+
                        $"'{pass}', "+
                        $"'{this.imie.Text}', "+
                        $"'{this.nazwisko.Text}', "+
                        $"'{this.listaStanowisk.Text}', "+
                        $"5)";
                    SqlCommand cmd = new SqlCommand(query, dbConnection);
                    dbConnection.Open();
                    cmd.ExecuteNonQuery();
                    dbConnection.Close();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbConnection.Close();
            }
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                e.Handled = true;
                button_Click(this, e);
            }
            else if (e.Key == Key.Escape)
            {
                e.Handled = true;
                button1_Click(this, e);
            }
        }
    }
}
