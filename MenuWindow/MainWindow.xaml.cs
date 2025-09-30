using GameManagementClassLibrary;
using Microsoft.Win32;
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

namespace MenuWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Jeu JeuAjoute { get; private set; }
        public Jeu JeuModifie { get; private set; }

        private User user;
        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Filtre pour les fichiers d'images
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp, *.mp4)|*.jpg; *.jpeg; *.png; *.bmp; *.mp4|All files (*.*)|*.*";

            // Afficher la boîte de dialogue
            if (openFileDialog.ShowDialog() == true)
            {
                // Récupérer le chemin d'accès du fichier sélectionné
                string selectedImagePath = openFileDialog.FileName;

                // Définir le chemin d'accès de l'image dans le TextBox ImageUrlTextBox
                ImageUrlTextBox.Text = selectedImagePath;

                MessageBox.Show("The image has been added", "Image added", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Video_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Filtre pour les fichiers vidéo
            openFileDialog.Filter = "Video files (*.mp4, *.avi, *.mov, *.wmv)|*.mp4; *.avi; *.mov; *.wmv|All files (*.*)|*.*";

            // Afficher la boîte de dialogue
            if (openFileDialog.ShowDialog() == true)
            {
                // Récupérer le chemin d'accès du fichier vidéo sélectionné
                string selectedVideoPath = openFileDialog.FileName;

                // Définir le chemin d'accès de la vidéo dans le TextBox correspondant
                VideoUrlTextBox.Text = selectedVideoPath;

                MessageBox.Show("The video has been added", "Image added", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            string editeur = EditeurTextBox.Text;
            string titre = TitreTextBox.Text;
            string support = SupportTextBox.Text;
            string description = DescriptionTextBox.Text;
            DateTime dateSortie = DateSortieDatePicker.SelectedDate ?? DateTime.Now;
            string imageUrl = ImageUrlTextBox.Text;
            string videoUrl = VideoUrlTextBox.Text;

            // Créer une nouvelle instance de Jeu avec les données saisies
            Jeu nouveauJeu = new Jeu(editeur, titre, support, description, dateSortie, imageUrl, videoUrl);

            // Enregistrer les données du jeu dans un fichier
            nouveauJeu.EnregistrerDansFichier(GetUserFilePath());

            // Fermer la fenêtre
            Close();
        }

        private void EditGame_Click(object sender, RoutedEventArgs e)
        {
            string editeur = EditeurTextBox.Text;
            string titre = TitreTextBox.Text;
            string support = SupportTextBox.Text;
            string description = DescriptionTextBox.Text;
            DateTime dateSortie = DateSortieDatePicker.SelectedDate ?? DateTime.Now;
            string imageUrl = ImageUrlTextBox.Text;
            string videoUrl = VideoUrlTextBox.Text;

            Jeu nouveauJeu = new Jeu(editeur, titre, support, description, dateSortie, imageUrl, videoUrl);

            nouveauJeu.EnregistrerDansFichier(GetUserFilePath());

            Close();
        }

        public void UpdateData(Jeu jeuUpdate)
        {
            EditeurTextBox.Text = jeuUpdate.Editeur;
            TitreTextBox.Text = jeuUpdate.Titre;
            SupportTextBox.Text = jeuUpdate.Support;
            DescriptionTextBox.Text = jeuUpdate.Description;
            DateSortieDatePicker.SelectedDate = jeuUpdate.DateSortie;
            ImageUrlTextBox.Text = jeuUpdate.ImageUrl;
            VideoUrlTextBox.Text = jeuUpdate.VideoUrl;
        }

        private string GetUserFilePath()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string userFolder = System.IO.Path.Combine(documentsPath, user.Username);

            if (!Directory.Exists(userFolder))
                Directory.CreateDirectory(userFolder);

            string filePath = System.IO.Path.Combine(userFolder, "games.txt");

            // Crée le fichier s'il n'existe pas
            if (!File.Exists(filePath))
                File.Create(filePath).Close();

            return filePath;
        }

    }
}