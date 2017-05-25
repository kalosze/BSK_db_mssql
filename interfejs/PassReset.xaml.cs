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
    /// Interaction logic for PassReset.xaml
    /// </summary>
    public partial class PassReset : Window
    {
        User who;
        SqlConnection con;
        public PassReset(User who, SqlConnection dbConnection)
        {
            InitializeComponent();
            this.who = who;
            this.con = dbConnection;

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //resetowanie hasła
        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            string pass = passBox.Password;
            if (pass == passBox2.Password)
            {
                this.info.Visibility = Visibility.Hidden;
                //zaszyfruj
                pass = Encryptor.Encrypt(pass, who.login);

                //ustawiamy skrypt
                String query = $"UPDATE UZYTKOWNIK SET HASLO='{pass}' WHERE [ID_UZYTKOWNIKA] like '{who.id}'";
                SqlCommand cmd = new SqlCommand(query, con);
                try
                {
                    //wysyłamy skrypt na serwer
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    who.pass = pass;
                    this.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                this.info.Visibility = Visibility.Visible;
            }
        }

        private void passBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            passBox.SelectAll();
        }

        private void passBox2_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            passBox2.SelectAll();
        }

        private void passBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            passBox.SelectAll();
        }

        private void passBox2_GotMouseCapture(object sender, MouseEventArgs e)
        {
            passBox2.SelectAll();
        }
    }
}
