using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserClass;

namespace LoginApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserManager _userManager;
        private GameWindow.MainWindow _gameWindow;
        public MainWindow()
        {
            InitializeComponent();
            _userManager = new UserManager();
            DataContext = _userManager;
        }

        private void LOGIN_Click(object sender, RoutedEventArgs e)
        {
            string username = _userManager.User.Username;
            _userManager.User.Password = PasswordBoxPassword.Password;
            string password = _userManager.User.Password;

            bool isNewUser = false;

            if (!_userManager.UserExists(username))
            {
                _userManager.CreateUser(username, password);
                _userManager.Information = "A new user has been created";
                InformationsMessage.Foreground = Brushes.Black;
                isNewUser = true; // marque que c'est un nouvel utilisateur
            }

            // Vérifie le login pour les utilisateurs existants et le nouvel utilisateur
            LoginResult loginResult = _userManager.Connect(username, password);

            if (loginResult == LoginResult.Success)
            {
                _userManager.Information = isNewUser ? "User created and logged in" : "The user is logged";
                InformationsMessage.Foreground = Brushes.Black;

                if (_gameWindow == null || !_gameWindow.IsVisible)
                {
                    _gameWindow = new GameWindow.MainWindow(_userManager.User);
                    _gameWindow.Show();
                }
                else
                {
                    _gameWindow.Activate(); // si elle existe déjà, on la ramène au premier plan
                }
            }
            else if (loginResult == LoginResult.InvalidPassword)
            {
                _userManager.Information = "The password is wrong.";
                InformationsMessage.Foreground = Brushes.Red;
            }
        }



        private void LOGOUT_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window != this)
                {
                    window.Close();
                }

                _userManager.User.Username = "";
                PasswordBoxPassword.Password = "";
                _userManager.Information = "Goodbye.";
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}