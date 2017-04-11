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
    /// Interaction logic for Logowanie.xaml
    /// </summary>
    public partial class Logowanie : Window
    {
        string pass { get; set; }
        string login { get; set; }

        SqlConnection con;
        //public CurrentUser usr { get; set; }
        public Logowanie(SqlConnection dbConnection)
        {
            con = dbConnection;
            InitializeComponent();
        }
        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            login = this.loginBox.Text;
            pass = this.passwordBox.Password;

            MainWindow mainWindow = Owner as MainWindow;
            if (login == "admin" && pass == "admin")
            {
                mainWindow.usr = new CurrentUser(0, "", "", "super", "user", "SU", -1);
                this.Close();
            }
            //wycyckanie użytkownika z bazy danych
            //TO DO
            pass = Encryptor.Encrypt(pass, login);
                try
            {
                String query = $"SELECT * FROM UZYTKOWNIK WHERE [LOGIN] like '{login}' AND [HASLO] like '{pass}'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                reader.Read();
                mainWindow.usr = new CurrentUser(reader.GetInt32(0),reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6));
                reader.Close();
                con.Close();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                con.Close();
            }
            
            

            //jeśli się udało
            if (mainWindow.usr != null)
            {
                this.Close();
            }
            else
            {
                errorInfo.Visibility = Visibility.Visible;
            }
        }
    }
}
