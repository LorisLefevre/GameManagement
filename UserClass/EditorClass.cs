using GameManagementClassLibrary;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using UserClass;

namespace GameManagementClassLibrary
{
    public class EditorClass : INotifyPropertyChanged
    {
        private Jeu jeu;
        public List<Jeu> jeuxAssocies;
        private string _nomEditeur;
        private User user;
        private const string filePath2 = @"C:\Users\Loris\games.txt";


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string NomEditeur
        {
            get
            {
                return _nomEditeur;
            }

            set
            {
                _nomEditeur = value;
                OnPropertyChanged(nameof(NomEditeur));
            }
        }

        public EditorClass()
        {

        }

        public EditorClass(User user, Jeu jeu, string nomEditeur)
        {
            this.user = user;
            this.jeu = jeu;
            NomEditeur = nomEditeur;
        }

        public EditorClass(List<Jeu> jeux, string nomEditeur)
        {
            this.jeuxAssocies = jeux;
            this.NomEditeur = nomEditeur;
        }

        public List<Jeu> ViewEditor(string editorName)
        {
            List<Jeu> jeuxAssocies = new List<Jeu>();

            if (!string.IsNullOrEmpty(editorName))
            {
                // Chemin vers le dossier Documents de l'utilisateur actuel
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // Chemin complet vers le fichier games.txt
                string filePath = Path.Combine(documentsPath, "games.txt");

                if (!File.Exists(filePath))
                {
                    // Si le fichier n'existe pas, retourne une liste vide
                    return jeuxAssocies;
                }

                // Lire les lignes du fichier des éditeurs
                string[] lines = File.ReadAllLines(filePath);

                // Parcourir les lignes du fichier
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Séparer les données de chaque ligne
                    string[] data = line.Split(',');

                    if (data.Length >= 7 && data[0].Equals(editorName, StringComparison.OrdinalIgnoreCase))
                    {
                        string gameName = data[1];
                        string gameSupport = data[2];
                        string gameDescription = data[3];
                        DateTime gameReleaseDate = DateTime.Parse(data[4]);
                        string gameImage = data[5];
                        string gameVideo = data[6];

                        // Créer une instance de Jeu
                        Jeu jeu = new Jeu(editorName, gameName, gameSupport, gameDescription, gameReleaseDate, gameImage, gameVideo);

                        // Ajouter le jeu à la liste des jeux associés à l'éditeur
                        jeuxAssocies.Add(jeu);
                    }
                }
            }

            // Retourner la liste des jeux associés à l'éditeur
            return jeuxAssocies;
        }


        public List<Jeu> ViewSupport(string supportName, User user)
        {
            this.user = user;
            List<Jeu> jeuxAssocies = new List<Jeu>();

            if (!string.IsNullOrEmpty(supportName))
            {
                // Récupère le dossier Documents de l'utilisateur Windows
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // Construit le chemin vers le dossier spécifique à l'utilisateur de ton app
                string userFolder = Path.Combine(documentsPath, user.Username);

                // Construit le chemin complet vers games.txt
                string filePath = Path.Combine(userFolder, "games.txt");

                if (!File.Exists(filePath))
                {
                    // Si le fichier n’existe pas → retourne liste vide
                    return jeuxAssocies;
                }

                // Lire les lignes du fichier des jeux
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Séparer les données de chaque ligne
                    string[] data = line.Split(',');

                    if (data.Length >= 7 && data[2].IndexOf(supportName, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        string gameEditor = data[0];
                        string gameName = data[1];
                        string gameSupport = data[2];
                        string gameDescription = data[3];
                        DateTime gameReleaseDate = DateTime.Parse(data[4]);
                        string gameImage = data[5];
                        string gameVideo = data[6];

                        // Créer une instance de Jeu avec les arguments requis
                        Jeu jeu = new Jeu(gameEditor, gameName, gameSupport, gameDescription, gameReleaseDate, gameImage, gameVideo);

                        // Ajouter le jeu à la liste
                        jeuxAssocies.Add(jeu);
                    }
                }
            }

            return jeuxAssocies;
        }


        public List<Jeu> ViewAllForUser(User user)
        {
            List<Jeu> jeuxAssocies = new List<Jeu>();
            this.user = user;

            // Récupère le chemin du dossier Documents
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Construit le chemin vers le dossier spécifique de l’utilisateur
            string userFolder = Path.Combine(documentsPath, user.Username);

            // Chemin complet vers games.txt
            string filePath = Path.Combine(userFolder, "games.txt");

            if (File.Exists(filePath))
            {
                // Lire toutes les lignes du fichier
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] data = line.Split(',');

                    if (data.Length >= 7)
                    {
                        string editorName = data[0];
                        string gameName = data[1];
                        string gameSupport = data[2];
                        string gameDescription = data[3];
                        DateTime gameReleaseDate = DateTime.Parse(data[4]);
                        string gameImage = data[5];
                        string gameVideo = data[6];

                        // Crée une instance de Jeu
                        Jeu jeu = new Jeu(editorName, gameName, gameSupport, gameDescription, gameReleaseDate, gameImage, gameVideo);

                        jeuxAssocies.Add(jeu);
                    }
                }
            }
            else
            {
                // Gérer le cas où le fichier n'existe pas
                Console.WriteLine($"Le fichier spécifié n'existe pas : {filePath}");
            }

            return jeuxAssocies;
        }



        public List<Jeu> ViewAll(User user)
        {
            List<Jeu> jeuxAssocies = new List<Jeu>();

            // Récupère le chemin du dossier Documents
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Dossier utilisateur = Documents\<Username>
            string userFolder = Path.Combine(documentsPath, user.Username);

            // Fichier games.txt dans ce dossier
            string filePath = Path.Combine(userFolder, "games.txt");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Le fichier n'existe pas : {filePath}");
                return jeuxAssocies; // retourne liste vide si pas de fichier
            }

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] data = line.Split(',');

                if (data.Length >= 7)
                {
                    string editorName = data[0];
                    string gameName = data[1];
                    string gameSupport = data[2];
                    string gameDescription = data[3];
                    DateTime gameReleaseDate = DateTime.Parse(data[4]);
                    string gameImage = data[5];
                    string gameVideo = data[6];

                    jeuxAssocies.Add(new Jeu(editorName, gameName, gameSupport, gameDescription, gameReleaseDate, gameImage, gameVideo));
                }
            }

            return jeuxAssocies;
        }


        public void SaveAllXML(User user)
        {
            this.user = user;
            try
            {
                // Dossier Documents de l'utilisateur
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // Dossier spécifique à l'utilisateur : Documents\<Username>
                string userFolder = Path.Combine(documentsPath, user.Username);

                // Crée le dossier s'il n'existe pas
                if (!Directory.Exists(userFolder))
                    Directory.CreateDirectory(userFolder);

                // Chemin complet du fichier TXT existant
                string filePath = Path.Combine(userFolder, "games.txt");

                // Lire toutes les lignes du fichier TXT
                string[] lignes = File.Exists(filePath) ? File.ReadAllLines(filePath) : new string[0];

                List<Jeu> jeux = new List<Jeu>();

                foreach (string ligne in lignes)
                {
                    string[] data = ligne.Split(',');

                    if (data.Length >= 7)
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

                // Chemin complet pour le fichier XML dans le même dossier
                string xmlFilePath = Path.Combine(userFolder, "games.xml");

                // Sérialisation XML
                XmlSerializer serializer = new XmlSerializer(typeof(List<Jeu>));
                using (TextWriter writer = new StreamWriter(xmlFilePath))
                {
                    serializer.Serialize(writer, jeux);
                }

                Console.WriteLine("Les jeux ont été sauvegardés au format XML avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la sauvegarde au format XML : {ex.Message}");
            }
        }



        public void SaveXML(string editorName, User user)
        {
            this.user = user;
            if (string.IsNullOrEmpty(editorName))
                return;

            try
            {
                // Dossier Documents de l'utilisateur
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // Dossier spécifique à l'utilisateur : Documents\<Username>
                string userFolder = Path.Combine(documentsPath, user.Username);

                // Crée le dossier s'il n'existe pas
                if (!Directory.Exists(userFolder))
                    Directory.CreateDirectory(userFolder);

                // Chemin complet du fichier TXT existant
                string txtFilePath = Path.Combine(userFolder, "games.txt");

                if (!File.Exists(txtFilePath))
                {
                    Console.WriteLine($"Le fichier des jeux n'existe pas : {txtFilePath}");
                    return;
                }

                // Lire toutes les lignes du fichier TXT
                string[] lignes = File.ReadAllLines(txtFilePath);

                List<Jeu> jeuxAssocies = new List<Jeu>();

                foreach (string ligne in lignes)
                {
                    string[] data = ligne.Split(',');

                    if (data.Length >= 7 && data[0].Equals(editorName, StringComparison.OrdinalIgnoreCase))
                    {
                        jeuxAssocies.Add(new Jeu
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

                // Chemin complet pour le fichier XML
                string xmlFilePath = Path.Combine(userFolder, $"{editorName}_games.xml");

                // Sérialisation XML
                XmlSerializer serializer = new XmlSerializer(typeof(List<Jeu>));
                using (TextWriter writer = new StreamWriter(xmlFilePath))
                {
                    serializer.Serialize(writer, jeuxAssocies);
                }

                Console.WriteLine($"Les jeux de '{editorName}' ont été sauvegardés au format XML avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde XML : {ex.Message}");
            }
        }


        public void DeleteXML(User user)
        {
            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string userFolder = Path.Combine(documentsPath, user.Username);

                if (!Directory.Exists(userFolder))
                    return;

                string[] filesToDelete = Directory.GetFiles(userFolder, "*.xml");

                foreach (string file in filesToDelete)
                {
                    File.Delete(file);
                }

                Console.WriteLine("Tous les fichiers XML ont été supprimés pour cet utilisateur.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression des fichiers XML : {ex.Message}");
            }
        }

        public void DeleteJSON(User user)
        {
            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string userFolder = Path.Combine(documentsPath, user.Username);

                if (!Directory.Exists(userFolder))
                    return;

                string[] filesToDelete = Directory.GetFiles(userFolder, "*.json");

                foreach (string file in filesToDelete)
                {
                    File.Delete(file);
                }

                Console.WriteLine("Tous les fichiers JSON ont été supprimés pour cet utilisateur.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression des fichiers JSON : {ex.Message}");
            }
        }


        public void SaveJSON(string editorName, User user)
        {
            if (user == null || string.IsNullOrEmpty(editorName))
                return;

            try
            {
                // Dossier Documents\<Username>
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string userFolder = Path.Combine(documentsPath, user.Username);

                // Crée le dossier si nécessaire
                if (!Directory.Exists(userFolder))
                    Directory.CreateDirectory(userFolder);

                // Chemin complet du fichier JSON
                string jsonFilePath = Path.Combine(userFolder, $"{editorName}_games.json");

                // Lire toutes les lignes du fichier des jeux
                string filePath = Path.Combine(userFolder, "games.txt");
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Le fichier games.txt n'existe pas pour cet utilisateur.");
                    return;
                }

                string[] lignes = File.ReadAllLines(filePath);

                // Créer une liste pour stocker les jeux associés à l'éditeur
                List<Jeu> jeuxAssocies = new List<Jeu>();

                foreach (string ligne in lignes)
                {
                    string[] data = ligne.Split(',');
                    if (data.Length >= 7 && data[0] == editorName)
                    {
                        jeuxAssocies.Add(new Jeu
                        {
                            Titre = data[1],
                            Support = data[2],
                            Description = data[3],
                            DateSortie = DateTime.Parse(data[4]),
                            ImageUrl = data[5],
                            VideoUrl = data[6]
                        });
                    }
                }

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                File.WriteAllText(jsonFilePath, System.Text.Json.JsonSerializer.Serialize(jeuxAssocies, options));

                Console.WriteLine($"Le fichier JSON a été sauvegardé avec succès : {jsonFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde JSON : {ex.Message}");
            }
        }

        public void ImporterFichierXML(string xmlFilePath, User user)
        {
            if (user == null || string.IsNullOrEmpty(xmlFilePath))
                return;

            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string userFolder = Path.Combine(documentsPath, user.Username);

                if (!Directory.Exists(userFolder))
                    Directory.CreateDirectory(userFolder);

                string gamesFilePath = Path.Combine(userFolder, "games.txt");

                using (StreamReader fileStream = new StreamReader(xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Jeu>));
                    List<Jeu> jeux = (List<Jeu>)serializer.Deserialize(fileStream);

                    foreach (Jeu jeu in jeux)
                    {
                        jeu.EnregistrerDansFichier(gamesFilePath);
                    }
                }

                Console.WriteLine("Les données XML ont été importées avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'importation XML : {ex.Message}");
            }
        }


        public void ImporterFichierJSON(string jsonFilePath, User user)
        {
            if (user == null || string.IsNullOrEmpty(jsonFilePath))
                return;

            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string userFolder = Path.Combine(documentsPath, user.Username);

                if (!Directory.Exists(userFolder))
                    Directory.CreateDirectory(userFolder);

                string gamesFilePath = Path.Combine(userFolder, "games.txt");

                using (StreamReader fileStream = new StreamReader(jsonFilePath))
                {
                    List<Jeu> jeux = JsonConvert.DeserializeObject<List<Jeu>>(fileStream.ReadToEnd());

                    foreach (Jeu jeu in jeux)
                    {
                        jeu.EnregistrerDansFichier(gamesFilePath);
                    }
                }

                Console.WriteLine("Les données JSON ont été importées avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'importation JSON : {ex.Message}");
            }
        }

    }
}
