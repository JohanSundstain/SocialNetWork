using Microsoft.Win32;
using SocialNetWork.Items;
using SocialNetWork.Model;
using SocialNetWork.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для UserSpace.xaml
    /// </summary>
    public partial class UserSpace : Window
    {
        private Users _currentUser;
        private Images _images;
        private BitmapImage _bitImage;
        private IRepository _repository;
        public UserSpace(Users users)
        {
            InitializeComponent();
            _repository = new RepositoryIm(new Entities());
            _currentUser = users;
            InitUserSpace();
        }

        public async void InitUserSpace()
        {
            mainName.Content = _currentUser.Name;
            mainInfo.FontSize = 20;
            mainInfo.Text = "Name: " + _currentUser.Name +"\n"
                + "Surname: " + _currentUser.Surname +"\n"
                + "Age: " + (DateTime.Now.Year- _currentUser.Birthday.Year) +"\n"
                + "Mail: " + _currentUser.Email;
            if (_currentUser.Avatar_id == null)
            {
                _bitImage = new BitmapImage(new Uri(@"C:\Users\johan\source\repos\SocialNetWork\SocialNetWork\img\default.png"));
                avatar.ImageSource = _bitImage;
            }
            else
            {
                LoadAvatar();
            }
            //Adding all users images 
            listOfImages.ItemsSource = GetListOfUserImage(await _repository.GetAllUserImages(_currentUser));
            listOfMessages.ItemsSource = await _repository.GetMessages();

        }

        private List<UserImage> GetListOfUserImage(List<Images> li)
        {
            var tmp = new List<UserImage>();
            foreach (Images image in li)
            {
                var bitImage = new BitmapImage();
                var date = image.Date_added.ToShortDateString();
                Console.WriteLine(date);
                using (var ms = new MemoryStream(image.Image))
                {
                    bitImage = new BitmapImage();
                    bitImage.BeginInit();
                    bitImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitImage.StreamSource = ms;
                    bitImage.EndInit();
                }
                tmp.Add(new UserImage(bitImage, date));
            }
            return tmp;
        }

        private async void LoadAvatar()
        {
            _images = await _repository.GetUserAvatarById(_currentUser.Avatar_id);

            using (var ms = new MemoryStream(_images.Image))
            {
                _bitImage = new BitmapImage();
                _bitImage.BeginInit();
                _bitImage.CacheOption = BitmapCacheOption.OnLoad;
                _bitImage.StreamSource = ms;
                _bitImage.EndInit();
            }

            dataOfAvatarSet.Content = _images.Date_added;
            avatar.ImageSource = _bitImage;
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        //Setting menu
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new SettingsView(user: _currentUser, this);
            window.Show();
        }

        //Exit menu
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new SignIn();
            window.Show();
            this.Hide();
            this.Close();
        }

        //Upload new image to list
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var bitImage = new BitmapImage();
            openFileDialog.Filter = "JPG|*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                bitImage = new BitmapImage(new Uri(openFileDialog.FileName));
            }

            using (MemoryStream ms = new MemoryStream())
            {
                JpegBitmapEncoder bitmapEncoder = new JpegBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(bitImage));
                bitmapEncoder.Save(ms);
                _currentUser.Images = new Model.Images()
                {
                    Date_added = DateTime.Now,
                    Image = ms.ToArray()
                };
                _currentUser.Images1.Add(_currentUser.Images);
                listOfImages.ItemsSource = GetListOfUserImage(await _repository.GetAllUserImages(_currentUser));
            }


        }

        //Chose new avatar
        private async void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPG|*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                _bitImage = new BitmapImage(new Uri(openFileDialog.FileName));
            }

            avatar.ImageSource = _bitImage;

            using (MemoryStream ms = new MemoryStream())
            {
                JpegBitmapEncoder bitmapEncoder = new JpegBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(_bitImage));
                bitmapEncoder.Save(ms);
                _currentUser.Images = new Model.Images()
                {
                    Date_added = DateTime.Now,
                    Image = ms.ToArray()
                };

            }

            _currentUser = await _repository.ChangeAvatar(_currentUser);
            listOfImages.ItemsSource = GetListOfUserImage(await _repository.GetAllUserImages(_currentUser));
        }

        //Send message
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (message.Text.Length == 0) return;
            await _repository.SendMessage(_currentUser.Id, message.Text);
            listOfMessages.ItemsSource = await _repository.GetMessages();
            message.Text = "";
        }

        //update 
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var window = new UserSpace(_currentUser);
            this.Hide();
            window.Show();
            this.Close();
        }
    }
}
