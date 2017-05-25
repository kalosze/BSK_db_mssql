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
    //Wspomagane tworzenie użytkowników dla administratora (szyfrowanie hasła i inne takie)
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

        //dodanie nowego użytkownika
        private void button_Click(object sender, RoutedEventArgs e)
        {
            //sprawdzamy czy hasło jest minimalnej długości 5
            if (pass1.Password.Length < 5)
            {
                hasloLabel.Content = "Minimalna długość hasła to 5 znaków.";
                return;
            }
            int accesLvl;
            //ustawiamy etykietę w zależności od wybranego stanowiska
            switch (this.listaStanowisk.Text)
            {
                case "Administrator":
                    accesLvl = 0;
                    break;
                case "Sekretarka":
                    accesLvl = 1;
                    break;
                case "Nauczyciel":
                    accesLvl = 2;
                    break;
                case "Dyrektor":
                    accesLvl = 3;
                    break;
                default:
                    accesLvl = 4;
                    break;
            }
            try
            {
                //sprawdzamy czy hasła są takie same
                if (this.pass1.Password == this.pass2.Password)
                {
                    //szyfrujemy
                    var pass = Encryptor.Encrypt(pass1.Password, this.login.Text);
                    //ustawiamy skrypt
                    String query = $"INSERT INTO UZYTKOWNIK (LOGIN, HASLO, IMIE, NAZWISKO, STANOWISKO, ETYKIETA) VALUES (" +
                        $"@login, " +
                        $"@pass, " +
                        $"@imie, " +
                        $"@nazwisko, " +
                        $"@stanowisko, " +
                        $"@etykieta)";
                    SqlCommand cmd = new SqlCommand(query, dbConnection);
                    //ustawiamy parametry
                    cmd.Parameters.AddWithValue("@login", this.login.Text);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    cmd.Parameters.AddWithValue("@imie", this.imie.Text);
                    cmd.Parameters.AddWithValue("@nazwisko", this.nazwisko.Text);
                    cmd.Parameters.AddWithValue("@stanowisko", this.listaStanowisk.Text);
                    cmd.Parameters.AddWithValue("@etykieta", accesLvl);
                    //i wysyłamy do bazy danych
                    dbConnection.Open();
                    cmd.ExecuteNonQuery();
                    dbConnection.Close();
                    this.Close();

                }
                else
                {
                    hasloLabel.Content = "Hasła się nie zgadzają!";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbConnection.Close();
                this.Close();
            }
        }

        //obsługa klawiszy
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
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
