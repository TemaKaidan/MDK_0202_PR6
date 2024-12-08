using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PR6_RegIN_Mohov.Pages
{
    /// <summary>
    /// Логика взаимодействия для Recovery.xaml
    /// </summary>
    public partial class Recovery : Page
    {
        string OldLogin;
        bool IsCapture = false;

        public Recovery()
        {
            InitializeComponent();

            MainWindow.mainWindow.UserLogIn.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogIn.HandlerInCorrectLogin += InCorrectLogin;
            Capture.HandlerCorrectCapture += CorrectCapture;
        }

        public void CorrectLogin()
        {
            if (TbLogin.Text != MainWindow.mainWindow.UserLogIn.Name)
            {
                SetNotification($"Hi, {MainWindow.mainWindow.UserLogIn.Name}", Brushes.Black);
                AnimateUserImage(MainWindow.mainWindow.UserLogIn.Image);
                SendNewPassword();
            }
        }
        public void InCorrectLogin()
        {
            if (!string.IsNullOrEmpty(LNameUser.Content.ToString()))
            {
                LNameUser.Content = "";
                AnimateUserImage(new Uri("pack://application:,,,/Images/ic-user.png"));
            }

            if (!string.IsNullOrEmpty(TbLogin.Text))
            {
                SetNotification("Login is incorrect", Brushes.Red);
            }
        }
        private void CorrectCapture()
        {
            Capture.IsEnabled = false;
            IsCapture = true;
            SendNewPassword();
        }
        public void SendNewPassword()
        {
            if (IsCapture && !string.IsNullOrEmpty(MainWindow.mainWindow.UserLogIn.Password))
            {
                AnimateUserImage(new Uri("pack://application:,,,/Images/ic-mail.png"));
                SetNotification("An email has been sent to your email.", Brushes.Black);
                MainWindow.mainWindow.UserLogIn.CreateNewPassword();
            }
        }

        private void AnimateUserImage(object imageSource)
        {
            try
            {
                ImageSource imgSrc = imageSource is byte[] imageData
                    ? ImageSourceFromByte(imageData)
                    : new BitmapImage((Uri)imageSource);

                DoubleAnimation startAnimation = Animation(1, 0, 0.6);
                startAnimation.Completed += (sender, e) =>
                {
                    IUser.Source = imgSrc;
                    DoubleAnimation endAnimation = Animation(0, 1, 1.2);
                    IUser.BeginAnimation(Image.OpacityProperty, endAnimation);
                };

                IUser.BeginAnimation(Image.OpacityProperty, startAnimation);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private ImageSource ImageSourceFromByte(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
            }
            return biImg;
        }
        private DoubleAnimation Animation(double from, double to, double durationInSeconds)
        {
            return new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(durationInSeconds)
            };
        }

        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }

        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);
        }

        private void SetLogin(object sender, RoutedEventArgs e) => MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);

        private void OpenLogin(object sender, MouseButtonEventArgs e) => MainWindow.mainWindow.OpenPage(new Login());
    }
}