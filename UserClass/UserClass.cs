using GameManagementClassLibrary;

using System.ComponentModel;

namespace UserClass
{
    public class User : INotifyPropertyChanged
    {
        private string _username;
        private string _password;

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public User()
        {

        }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    public class UserManager : INotifyPropertyChanged
    {
        private string filePath; // Chemin portable vers users.txt

        private User _user;

        private string _information;

        public User User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        public string Information
        {
            get { return _information; }

            set
            {
                if (_information != value)
                {
                    _information = value;
                    OnPropertyChanged(nameof(Information));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserManager()
        {
            User = new User();

            // Chemin du fichier users.txt directement dans Documents
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = Path.Combine(documentsPath, "users.txt");

            // Créer le fichier s'il n'existe pas
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
        }

        public LoginResult Connect(string username, string password)
        {
            if (!File.Exists(filePath))
                return LoginResult.NotExist;

            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length == 0)
                return LoginResult.NotExist;

            string hashedPassword = PasswordHelper.HashPassword(password);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2 && parts[0].Trim() == username.Trim())
                    return parts[1].Trim() == hashedPassword ? LoginResult.Success : LoginResult.InvalidPassword;
            }

            return LoginResult.InvalidUsername;
        }



        public void CreateUser(string username, string password)
        {
            if (!UserExists(username))
            {
                string hashedPassword = PasswordHelper.HashPassword(password);

                using (StreamWriter sw = File.AppendText(filePath))
                    sw.WriteLine($"{username},{hashedPassword}");

                CreateUserDirectory(username);
            }
        }


        public void DeleteUser(string username)
        {
            if (!File.Exists(filePath)) return;

            var linesConservees = File.ReadAllLines(filePath)
                                      .Where(line => line.Split(',')[0] != username)
                                      .ToList();

            File.WriteAllLines(filePath, linesConservees);

            DeleteUserDirectory(username);

            Console.WriteLine($"Utilisateur '{username}' supprimé avec succès.");
        }

        public void CreateUserDirectory(string username)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string userDirectoryPath = Path.Combine(documentsPath, username);

            if (!Directory.Exists(userDirectoryPath))
                Directory.CreateDirectory(userDirectoryPath);

            Console.WriteLine($"Dossier pour l'utilisateur '{username}' créé : {userDirectoryPath}");
        }

        public void DeleteUserDirectory(string username)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string userDirectoryPath = Path.Combine(documentsPath, username);

            if (Directory.Exists(userDirectoryPath))
                Directory.Delete(userDirectoryPath, true);

            Console.WriteLine($"Dossier pour l'utilisateur '{username}' supprimé.");
        }


        public bool UserExists(string username)
        {
            if (!File.Exists(filePath)) return false;

            return File.ReadAllLines(filePath)
                       .Any(line => line.Split(',')[0] == username);
        }
    }


    public enum LoginResult
    {
        Success,
        InvalidUsername,
        InvalidPassword,
        NotExist
    }
}
