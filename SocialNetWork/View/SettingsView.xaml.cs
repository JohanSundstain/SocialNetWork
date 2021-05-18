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
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        private Users _currentUser;
        private IRepository _repository;
        private UserSpace _userSpace;
        public SettingsView(Users user,UserSpace userSpace)
        {
            InitializeComponent();
            _currentUser = user;
            _userSpace = userSpace;
            _repository = new RepositoryIm(new Entities());
        }



        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (old_pass.Password == _currentUser.Password && new_pass.Password == conf_pass.Password)
            {
                _currentUser.Name = name.Text;
                _currentUser.Surname = surname.Text;
                _currentUser.Email = mail.Text;
                _currentUser.Birthday = (DateTime) birth.SelectedDate;
                _currentUser.Password = new_pass.Password;
                await _repository.ChangeUser(_currentUser);
                _userSpace.InitUserSpace();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong Password!");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            name.Text = _currentUser.Name;
            surname.Text = _currentUser.Surname;
            mail.Text = _currentUser.Email;
            birth.SelectedDate = _currentUser.Birthday;
        }
    }
}
