using SocialNetWork.Model;
using SocialNetWork.Repository;
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

namespace SocialNetWork.View
{
    /// <summary>
    /// Логика взаимодействия для SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        private IRepository _repository;
        public SignIn()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _repository = new RepositoryIm(new Entities());
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            this.Hide();
            this.Close();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Users user = await _repository.GetUsersByMailAndPassword(mail.Text, password.Password);
            if (user == default)
            {
                MessageBox.Show("Not Correct");
                return;
            }
            else
            {
                var window = new UserSpace(users: user);
                window.Show();
                this.Hide();
                this.Close();
            }
        }
    }
}
