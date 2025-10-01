using GameManagementClassLibrary;
using Microsoft.Win32;
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

namespace EditorWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event EventHandler PreviewKeyDownEvent;
        private User user;
        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void SaveToXML_Click(object sender, RoutedEventArgs e)
        {
            EditorClass editor = new EditorClass();
            editor.SaveAllXML(user);
            MessageBox.Show("The games have been saved in the XML format.", "Save Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ImportXML_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers XML (*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                string xmlFilePath = openFileDialog.FileName;
                EditorClass editeur = new EditorClass();
                editeur.ImporterFichierXML(xmlFilePath, user);
            }
        }

        private void SaveToJSON_Click(object sender, RoutedEventArgs e)
        {
            string editorName = Microsoft.VisualBasic.Interaction.InputBox("Write the editor's name :", "Editor", "");

            EditorClass editor = new EditorClass();
            editor.SaveJSON(editorName, user);
            MessageBox.Show("The games have been saved in the JSON format.", "Save Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void IMPORTJSON_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers JSON (*.json)|*.json";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                string jsonFilePath = openFileDialog.FileName;
                EditorClass editeur = new EditorClass();
                editeur.ImporterFichierJSON(jsonFilePath, user);
            }
        }

        private void DeleteEditor_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Would you like to delete your JSON and XML files ?", "File deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Cancel || result == MessageBoxResult.No)
            {
                return;
            }

            EditorClass editor = new EditorClass();

            if (result == MessageBoxResult.Yes)
            {
                editor.DeleteXML(user);
                editor.DeleteJSON(user);
            }

            MessageBox.Show($"The JSON and XML files have been deleted.", "Deletion succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            EditorClass editor = new EditorClass();

            // Obtenir la liste des jeux associés à l'éditeur
            List<Jeu> jeuxAssocies = editor.ViewAllForUser(user);

            // Définir l'ItemsSource de MyDataGrid
            MyDataGrid.ItemsSource = jeuxAssocies;

            Combo.Text = "VIEW ALL";
        }

        private void ViewEditor_Click(object sender, RoutedEventArgs e)
        {
            // Demander à l'utilisateur le nom de l'éditeur
            string editorName = Microsoft.VisualBasic.Interaction.InputBox("Write the editor's name :", "Editor", "");

            // Créer une instance de la classe Editor
            EditorClass editor = new EditorClass();

            // Obtenir la liste des jeux associés à l'éditeur
            List<Jeu> jeuxAssocies = editor.ViewEditor(editorName);

            // Définir l'ItemsSource de MyDataGrid
            MyDataGrid.ItemsSource = jeuxAssocies;

            Combo.Text = "VIEW BY EDITOR";
        }

        private void ViewSupport_Click(object sender, RoutedEventArgs e)
        {
            string supportName = Microsoft.VisualBasic.Interaction.InputBox("Write the support's name :", "Editor", "");

            // Créer une instance de la classe Editor
            EditorClass editor = new EditorClass();

            // Obtenir la liste des jeux associés à l'éditeur
            List<Jeu> jeuxAssocies = editor.ViewSupport(supportName, user);

            // Définir l'ItemsSource de MyDataGrid
            MyDataGrid.ItemsSource = jeuxAssocies;

            Combo.Text = "VIEW BY SUPPORT";
        }

        private void MyDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string editorName = Microsoft.VisualBasic.Interaction.InputBox("Write the editor's name :", "Editor", "");

            EditorClass editor = new EditorClass();
            editor.SaveXML(editorName, user);
            MessageBox.Show("The games have been saved in the XML format.", "Save Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MyDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                PreviewKeyDownEvent?.Invoke(this, EventArgs.Empty);

            }
        }
    }
}