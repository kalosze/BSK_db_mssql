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

namespace interfejs
{
    /// <summary>
    /// Interaction logic for Logowanie.xaml
    /// </summary>
    public partial class Logowanie : Window
    {
        string pass { get; set; }
        string login { get; set; }
        //public CurrentUser usr { get; set; }
        public Logowanie()
        {
            InitializeComponent();
        }
        
           

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            login = this.loginBox.Text;
            pass = this.passwordBox.Password;
            //wycyckanie użytkownika z bazy danych
            //TO DO
            MainWindow mainWindow = Owner as MainWindow;
            mainWindow.usr =  new CurrentUser(login, pass, "Chuj", "Kutas","Administrator" ,5);

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
