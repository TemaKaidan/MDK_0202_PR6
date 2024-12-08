using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Imaging = Aspose.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace PR6_RegIN_Mohov.Pages
{
    /// <summary>
    /// Логика взаимодействия для Regin.xaml
    /// </summary>
    public partial class Regin : Page
    {
        OpenFileDialog FileDialogImage = new OpenFileDialog();
        private bool BSetImages = false;

        public Regin()
        {
            InitializeComponent();

            MainWindow.mainWindow.UserLogIn.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogIn.HandlerInCorrectLogin += InCorrectLogin;

            FileDialogImage.Filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg";
            FileDialogImage.RestoreDirectory = true;
            FileDialogImage.Title = "Choose a photo for your avatar";
        }

        private void CorrectLogin()
        {
            SetNotification("Login already in use.", Brushes.Red);
        }
        private void InCorrectLogin() => SetNotification("", Brushes.Black);

        public void SetLogin()
        {
            if (IsValidLogin(TbLogin.Text))
            {
                SetNotification("", Brushes.Black);
                MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);
            }
            else
            {
                SetNotification("Invalid login", Brushes.Red);
            }
            OnRegin();
        }
        public void SetPassword()
        {
            if (IsValidPassword(TbPassword.Password))
            {
                SetNotification("", Brushes.Black);
                ConfirmPassword(true);
                OnRegin();
            }
            else
            {
                SetNotification("Invalid password", Brushes.Red);
            }
        }

        public bool ConfirmPassword(bool Pass = false)
        {

            if (TbConfirmPassword.Password != TbPassword.Password)
            {
                SetNotification("Passwords do not match", Brushes.Red);
                return false;
            }
            else
            {
                SetNotification("", Brushes.Black);
                if (!Pass) SetPassword();
                return true;
            }
        }
        void OnRegin()
        {
            if (IsFormValid())
            {
                MainWindow.mainWindow.UserLogIn.Login = TbLogin.Text;
                MainWindow.mainWindow.UserLogIn.Password = TbPassword.Password;
                MainWindow.mainWindow.UserLogIn.Name = TbName.Text;
                if (BSetImages) MainWindow.mainWindow.UserLogIn.Image = File.ReadAllBytes("IUser.jpg");
                MainWindow.mainWindow.UserLogIn.DateUpdate = DateTime.Now;
                MainWindow.mainWindow.UserLogIn.DateCreate = DateTime.Now;
                MainWindow.mainWindow.OpenPage(new Confirmation(Confirmation.TypeConfirmation.Regin));
            }
        }

        private bool IsValidLogin(string login) =>
            new Regex(@"([a-zA-Z0-9._-]{4,}@[a-zA-Z0-9._-]{2,}\.[a-zA-Z0-9._-]{2,})").IsMatch(login);
        private bool IsValidPassword(string password) =>
            new Regex(@"(?=.*[0-9])(?=.*[!@#$%^&?*\-_=])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&?*\-_=]{10,}").IsMatch(password);
        private bool IsFormValid() =>
            !string.IsNullOrEmpty(TbName.Text) && IsValidLogin(TbLogin.Text) && IsValidPassword(TbPassword.Password) && ConfirmPassword(true);

        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }

        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetLogin();
        }

        private void SetLogin(object sender, RoutedEventArgs e) => SetLogin();
        private void SetPassword(object sender, RoutedEventArgs e) => SetPassword();
        private void ConfirmPassword(object sender, RoutedEventArgs e) => ConfirmPassword();
        private void OpenLogin(object sender, MouseButtonEventArgs e) => MainWindow.mainWindow.OpenPage(new Login());

        private void SetName(object sender, TextCompositionEventArgs e) => e.Handled = !(Char.IsLetter(e.Text, 0));

        private void SetPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetPassword();
        }

        private void ConfirmPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ConfirmPassword();
        }

        private void SelectImage(object sender, MouseButtonEventArgs e)
        {
            if (FileDialogImage.ShowDialog() == true)
            {
                ProcessImage(FileDialogImage.FileName);
                BSetImages = true;
            }
            else
            {
                BSetImages = false;
            }
        }

        private void ProcessImage(string filePath)
        {
            using (Imaging.Image image = Imaging.Image.Load(filePath))
            {
                int NewWidth = image.Width > image.Height ? (int)(image.Width / (256f / image.Height)) : 256;
                int NewHeight = image.Width > image.Height ? 256 : (int)(image.Height * (256f / image.Width));
                image.Resize(NewWidth, NewHeight);
                image.Save("IUser.jpg");
            }

            DisplayImage();
        }
        private void DisplayImage()
        {
            using (Imaging.RasterImage rasterImage = (Imaging.RasterImage)Imaging.Image.Load("IUser.jpg"))
            {
                if (!rasterImage.IsCached)
                {
                    rasterImage.CacheData();
                }

                int X = rasterImage.Width > rasterImage.Height ? (int)((rasterImage.Width - 256f) / 2) : 0;
                int Y = rasterImage.Height > rasterImage.Width ? (int)((rasterImage.Height - 256f) / 2) : 0;

                Imaging.Rectangle rectangle = new Imaging.Rectangle(X, Y, 256, 256);
                rasterImage.Crop(rectangle);
                rasterImage.Save("IUser.jpg");
            }

            AnimateImage();
        }
        private void AnimateImage()
        {
            DoubleAnimation startAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.6));
            startAnimation.Completed += delegate
            {
                IUser.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\IUser.jpg"));
                DoubleAnimation endAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1.2));
                IUser.BeginAnimation(System.Windows.Controls.Image.OpacityProperty, endAnimation);
            };
            IUser.BeginAnimation(System.Windows.Controls.Image.OpacityProperty, startAnimation);
        }
    }
}