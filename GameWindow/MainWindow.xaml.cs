using GameManagementClassLibrary;
using GameManagementClassLibrary;
using System.Collections.ObjectModel;
using System.IO;
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

namespace GameWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentGameIndex = 0;
        private User user;
        private string username;
        private ObservableCollection<Jeu> jeux;
        private Jeu jeuCourant;
        //private List<Jeu> jeux2;
        //private EditorWindow editorWindow;
        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            User.Text = user.Username;
            InformationsMessage.Visibility = Visibility.Collapsed;


            jeuCourant = new Jeu();
            jeux = new ObservableCollection<Jeu>();
            DataContext = jeuCourant;

            LoadJeux();

            //editorWindow = new EditorWindow(user);
            //editorWindow.PreviewKeyDownEvent += EditorWindow_PreviewKeyDownEvent1;
            GameDate.Text = "";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow.MainWindow menuWindow = new MenuWindow.MainWindow(user);
            menuWindow.Owner = this;
            menuWindow.ShowDialog();

            if (menuWindow.JeuAjoute != null)
            {
                jeux.Add(menuWindow.JeuAjoute);
                string userFilePath = GetUserFilePath();
                menuWindow.JeuAjoute.EnregistrerDansFichier(userFilePath);
            }

            string message = $"The game has been added by user {user.Username}.";
            InformationsMessage.Text = message;
            InformationsMessage.Visibility = Visibility.Visible;
            InformationsMessage.Foreground = Brushes.Black;
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow.MainWindow menuWindow = new MenuWindow.MainWindow(user);

            string gameName = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of the game you would like to modify:", "Game Display", "");

            if (!string.IsNullOrEmpty(gameName))
            {
                // Cherche le jeu existant dans la collection
                Jeu jeu = jeux.FirstOrDefault(j => j.Titre.Equals(gameName, StringComparison.OrdinalIgnoreCase));

                if (jeu != null)
                {
                    string userFilePath = GetUserFilePath();

                    // Supprime l’ancien jeu du fichier
                    jeu.SupprimerDuFichier(gameName, userFilePath);

                    // Passe le vrai jeu à la fenêtre de menu
                    menuWindow.Owner = this;
                    menuWindow.UpdateData(jeu);
                    menuWindow.ShowDialog();

                    string message = $"The game '{jeu.Titre}' has been updated by user {user.Username}.";
                    InformationsMessage.Text = message;
                    InformationsMessage.Visibility = Visibility.Visible;
                    InformationsMessage.Foreground = Brushes.Black;
                }
                else
                {
                    MessageBox.Show("Game not found.");
                }
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string gameName = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of the game you would like to delete:", "Game Display", "");

            if (!string.IsNullOrEmpty(gameName))
            {
                Jeu jeu = new Jeu();
                string userFilePath = GetUserFilePath();
                jeu.SupprimerDuFichier(gameName, userFilePath);

                string message = $"The game has been deleted by user {user.Username}.";
                InformationsMessage.Text = message;
                InformationsMessage.Visibility = Visibility.Visible;
                InformationsMessage.Foreground = Brushes.Black;
            }
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Would you like to do a manual research?", "Game management", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                string gameName = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of game you want to see:", "Game display", "");

                if (!string.IsNullOrEmpty(gameName))
                {
                    string userFilePath = GetUserFilePath(); // Assuming you have a method to get the current user's file path
                    string[] lines = File.ReadAllLines(userFilePath);
                    List<Jeu> foundGames = new List<Jeu>();

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        string[] data = line.Split(',');

                        if (data.Length < 7)
                        {
                            continue;
                        }

                        if (data[1].IndexOf(gameName, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Jeu foundGame = new Jeu
                            {
                                Editeur = data[0],
                                Titre = data[1],
                                Support = data[2],
                                Description = data[3],
                                DateSortie = DateTime.Parse(data[4]),
                                ImageUrl = data[5],
                                VideoUrl = data[6]
                            };

                            foundGames.Add(foundGame);
                        }
                    }

                    if (foundGames.Count > 0)
                    {
                        MyDG.ItemsSource = foundGames;
                        MyDG.Visibility = Visibility.Visible;

                        InformationsMessage.Text = $"{foundGames.Count} game(s) found.";
                        InformationsMessage.Foreground = Brushes.Black;
                        InformationsMessage.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        InformationsMessage.Text = "The game couldn't be found.";
                        InformationsMessage.Foreground = Brushes.Red;
                        InformationsMessage.Visibility = Visibility.Visible;
                    }
                }
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
                Editor editor = new Editor();
                List<Jeu> jeuxAssocies = editor.ViewAllForUser(user);  // Pass the current user's username
                MyDG.ItemsSource = jeux;
                MyDG.Visibility = Visibility.Visible;
            }
            else if (messageBoxResult == MessageBoxResult.Cancel)
            {
                MyDG.Visibility = Visibility.Hidden;
            }
        }

        private void EditorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PHOTO_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(jeuCourant.ImageUrl))
            {
                GameImage.Visibility = Visibility.Visible;
                GameImage.Source = new BitmapImage(new Uri(jeuCourant.ImageUrl, UriKind.RelativeOrAbsolute));
                GameVideo.Visibility = Visibility.Collapsed;
                GameVideo.Stop();
            }
        }

        private void VIDEO_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(jeuCourant.VideoUrl))
            {
                GameImage.Visibility = Visibility.Collapsed;
                GameVideo.Visibility = Visibility.Visible;
                GameVideo.Source = new Uri(jeuCourant.VideoUrl, UriKind.RelativeOrAbsolute);
                GameVideo.Play();
            }
            else
            {
                // Afficher un message d'erreur ou un message d'information à l'utilisateur
                InformationsMessage.Text = "No video available for the selected game.";
                InformationsMessage.Foreground = Brushes.Red;
                InformationsMessage.Visibility = Visibility.Visible;
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            jeuCourant.Titre = "";
            jeuCourant.Support = "";
            jeuCourant.Description = "";
            jeuCourant.DateSortie = DateTime.ParseExact("01/01/2001", "dd/MM/yyyy", null);
            jeuCourant.ImageUrl = "";

            jeuCourant.VideoUrl = "";
         
            MyDG.Visibility = Visibility.Hidden;
        }

        private void MyDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VoirDonneesJeu();
        }

        private string GetUserFilePath()
        {
            // Chemin vers le dossier Documents de l'utilisateur
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Dossier spécifique à l'utilisateur
            string userFolder = System.IO.Path.Combine(documentsPath, user.Username);

            // Crée le dossier s'il n'existe pas
            if (!Directory.Exists(userFolder))
                Directory.CreateDirectory(userFolder);

            // Chemin complet vers le fichier games.txt
            string filePath = System.IO.Path.Combine(userFolder, "games.txt");

            // On n'a plus besoin de File.Create ici, AppendAllText le fera si nécessaire
            return filePath;
        }


        private void VoirDonneesJeu()
        {
            // Vérifier si un jeu est sélectionné dans la DataGrid
            if (MyDG.SelectedItem != null)
            {
                // Récupérer le jeu sélectionné dans la DataGrid
                jeuCourant = (Jeu)MyDG.SelectedItem;

                // Mettre à jour le DataContext des contrôles de l'interface utilisateur avec le jeu sélectionné
                DataContext = jeuCourant;

                // Mettre à jour la vidéo avec le jeu sélectionné
                if (!string.IsNullOrEmpty(jeuCourant.VideoUrl))
                {
                    GameImage.Visibility = Visibility.Collapsed;
                    GameVideo.Visibility = Visibility.Visible;
                    GameVideo.Source = new Uri(jeuCourant.VideoUrl, UriKind.RelativeOrAbsolute);
                    GameVideo.Play();
                }
                else
                {
                    GameVideo.Visibility = Visibility.Collapsed;
                    GameVideo.Stop(); // Assurez-vous d'arrêter la vidéo
                    GameImage.Visibility = Visibility.Visible;

                    
                    jeuCourant.InformationMessage = "No video available for the selected game.";
                    InformationsMessage.Foreground = Brushes.Red;
                    InformationsMessage.Visibility = Visibility.Visible;
                }

                jeuCourant.InformationMessage = "Game found.";
                InformationsMessage.Foreground = Brushes.Black;
                InformationsMessage.Visibility = Visibility.Visible;
            }
            else
            {
                jeuCourant.InformationMessage = "No game selected.";
                InformationsMessage.Foreground = Brushes.Red;
                InformationsMessage.Visibility = Visibility.Visible;
            }
        }

        private void GameVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            GameVideo.Position = TimeSpan.Zero;
            GameVideo.Play();
        }

        public ObservableCollection<Jeu> Jeux
        {
            get { return jeux; }
            set { jeux = value; }
        }

        public void LoadJeux()
        {
            string userFilePath = GetUserFilePath();
            if (File.Exists(userFilePath))
            {
                string[] lines = File.ReadAllLines(userFilePath);
                foreach (var line in lines)
                {
                    string[] data = line.Split(',');
                    if (data.Length == 7)
                    {
                        jeux.Add(new Jeu
                        {
                            Editeur = data[0],
                            Titre = data[1],
                            Support = data[2],
                            Description = data[3],
                            DateSortie = DateTime.Parse(data[4]),
                            ImageUrl = data[5],
                            VideoUrl = data[6]
                        });
                    }
                }
            }
        }

    }
}