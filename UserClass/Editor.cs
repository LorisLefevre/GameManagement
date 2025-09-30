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
    public class Editor : INotifyPropertyChanged
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

        public Editor()
        {

        }

        public Editor(User user, Jeu jeu, string nomEditeur)
        {
            this.user = user;
            this.jeu = jeu;
            NomEditeur = nomEditeur;
        }

        public Editor(List<Jeu> jeux, string nomEditeur)
        {
            this.jeuxAssocies = jeux;
            this.NomEditeur = nomEditeur;
        }

        public List<Jeu> ViewEditor(string editorName)
        {
            List<Jeu> jeuxAssocies = new List<Jeu>();

            if (!string.IsNullOrEmpty(editorName))
            {
                // Lire les lignes du fichier des éditeurs
                string[] lines = File.ReadAllLines(@"C:\Users\Loris\games.txt");

                // Parcourir les lignes du fichier des éditeurs
                foreach (string line in lines)
                {
                    // Séparer les données de chaque ligne
                    string[] data = line.Split(',');

                    if (data.Length >= 1 && data[0] == editorName)
                    {
                        string gameName = data[1];
                        string gameSupport = data[2];
                        string gameDescription = data[3];
                        DateTime gameReleaseDate = DateTime.Parse(data[4]);
                        string gameImage = data[5];
                        string gameVideo = data[6];

                        // Créer une instance de Jeu avec les arguments requis
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
                // Lire les lignes du fichier des éditeurs
                string[] lines = File.ReadAllLines($@"C:\Users\Loris\GameManagement\{user.Username}\games.txt");

                // Parcourir les lignes du fichier des éditeurs
                foreach (string line in lines)
                {
                    // Séparer les données de chaque ligne
                    string[] data = line.Split(',');

                    if (data.Length >= 1 && data[2].IndexOf(supportName, StringComparison.OrdinalIgnoreCase) >= 0)
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

                        // Ajouter le jeu à la liste des jeux associés à l'éditeur
                        jeuxAssocies.Add(jeu);
                    }
                }
            }

            // Retourner la liste des jeux associés à l'éditeur
            return jeuxAssocies;
        }

        public List<Jeu> ViewAllForUser(User user)
        {
            List<Jeu> jeuxAssocies = new List<Jeu>();
            this.user = user;
            // Construire correctement le chemin du fichier en utilisant l'interpolation de chaînes
            string filePath = $@"C:\Users\Loris\GameManagement\{user.Username}\games.txt";

            if (File.Exists(filePath))
            {
                // Lire les lignes du fichier des éditeurs
                string[] lines = File.ReadAllLines(filePath);

                // Parcourir les lignes du fichier des éditeurs
                foreach (string line in lines)
                {
                    // Séparer les données de chaque ligne
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

                        // Créer une instance de Jeu avec les arguments requis
                        Jeu jeu = new Jeu(editorName, gameName, gameSupport, gameDescription, gameReleaseDate, gameImage, gameVideo);

                        // Ajouter le jeu à la liste des jeux associés à l'éditeur
                        jeuxAssocies.Add(jeu);
                    }
                }
            }
            else
            {
                // Gérer le cas où le fichier n'existe pas
                Console.WriteLine($"Le fichier spécifié n'existe pas : {filePath}");
            }

            // Retourner la liste des jeux associés à l'éditeur
            return jeuxAssocies;
        }


        public List<Jeu> ViewAll()
        {
            List<Jeu> jeuxAssocies = new List<Jeu>();

            // Lire les lignes du fichier des éditeurs
            string[] lines = File.ReadAllLines(@"C:\Users\Loris\games.txt");

            // Parcourir les lignes du fichier des éditeurs
            foreach (string line in lines)
            {
                // Séparer les données de chaque ligne
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

                    // Créer une instance de Jeu avec les arguments requis
                    Jeu jeu = new Jeu(editorName, gameName, gameSupport, gameDescription, gameReleaseDate, gameImage, gameVideo);

                    // Ajouter le jeu à la liste des jeux associés à l'éditeur
                    jeuxAssocies.Add(jeu);
                }
            }


            // Retourner la liste des jeux associés à l'éditeur
            return jeuxAssocies;
        }

        public void SaveAllXML(User user)
        {
            this.user = user;
            try
            {
                // Lire toutes les lignes du fichier des jeux
                string[] lignes = File.ReadAllLines($@"C:\Users\Loris\GameManagement\{user.Username}\games.txt");

                // Créer une liste pour stocker les jeux associés à l'éditeur
                List<Jeu> jeux = new List<Jeu>();

                // Parcourir chaque ligne
                foreach (string ligne in lignes)
                {
                    // Séparer les données de chaque jeu
                    string[] data = ligne.Split(',');

                    // Vérifier si le titre du jeu correspond à celui de notre jeu associé à l'éditeur
                    if (data.Length >= 7)
                    {
                        // Créer une instance de Jeu avec les données du jeu
                        Jeu jeu = new Jeu
                        {
                            Editeur = data[0],
                            Titre = data[1],
                            Support = data[2],
                            Description = data[3],
                            DateSortie = DateTime.Parse(data[4]),
                            ImageUrl = data[5],
                            VideoUrl = data[6],
                        };

                        // Ajouter le jeu à la liste des jeux associés à l'éditeur
                        jeux.Add(jeu);
                    }
                }

                // Sérialiser la liste de jeux associés à l'éditeur au format XML
                XmlSerializer serializer = new XmlSerializer(typeof(List<Jeu>));
                using (TextWriter writer = new StreamWriter($@"C:\Users\Loris\GameManagement\{user.Username}\games.xml"))
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
            if (!string.IsNullOrEmpty(editorName))
            {
                try
                {
                    // Lire toutes les lignes du fichier des jeux
                    string[] lignes = File.ReadAllLines($@"C:\Users\Loris\GameManagement\{user.Username}\games.txt");

                    // Créer une liste pour stocker les jeux associés à l'éditeur
                    List<Jeu> jeuxAssocies = new List<Jeu>();

                    // Parcourir chaque ligne
                    foreach (string ligne in lignes)
                    {
                        // Séparer les données de chaque jeu
                        string[] data = ligne.Split(',');

                        // Vérifier si le titre du jeu correspond à celui de notre jeu associé à l'éditeur
                        if (data.Length >= 1 && data[0] == editorName)
                        {
                            // Créer une instance de Jeu avec les données du jeu
                            Jeu jeu = new Jeu
                            {
                                Titre = data[1],
                                Support = data[2],
                                Description = data[3],
                                DateSortie = DateTime.Parse(data[4]),
                                ImageUrl = data[5],
                                VideoUrl = data[6],
                            };

                            // Ajouter le jeu à la liste des jeux associés à l'éditeur
                            jeuxAssocies.Add(jeu);
                        }
                    }

                    // Sérialiser la liste de jeux associés à l'éditeur au format XML
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Jeu>));
                    using (TextWriter writer = new StreamWriter($@"C:\Users\Loris\GameManagement\{user.Username}{editorName}_games.xml"))
                    {
                        serializer.Serialize(writer, jeuxAssocies);
                    }

                    Console.WriteLine("Les jeux ont été sauvegardés au format XML avec succès.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Une erreur s'est produite lors de la sauvegarde au format XML : {ex.Message}");
                }
            }

        }

        public void DeleteXML()
        {
            string fileExtension = "xml";

            try
            {
                string[] filesToDelete = Directory.GetFiles(@"C:\Users\Loris\", $"*.{fileExtension}");

                foreach (string file in filesToDelete)
                {
                    File.Delete(file);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de la suppression du fichier XML");
            }

        }

        public void DeleteJSON()
        {
            string fileExtension = "json";

            try
            {
                string[] filesToDelete = Directory.GetFiles(@"C:\Users\Loris\", $"*.{fileExtension}");

                foreach (string file in filesToDelete)
                {
                    File.Delete(file);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de la suppression du fichier XML");
            }

        }

        public void SaveJSON(string editorName, User user)
        {
            this.user = user;
            if (!string.IsNullOrEmpty(editorName))
            {
                try
                {
                    // Lire toutes les lignes du fichier des jeux
                    string[] lignes = File.ReadAllLines($@"C:\Users\Loris\GameManagement\{user.Username}\games.txt");

                    // Créer une liste pour stocker les jeux associés à l'éditeur
                    List<Jeu> jeuxAssocies = new List<Jeu>();

                    // Parcourir chaque ligne
                    foreach (string ligne in lignes)
                    {
                        // Séparer les données de chaque jeu
                        string[] data = ligne.Split(',');

                        // Vérifier si le titre du jeu correspond à celui de notre jeu associé à l'éditeur
                        if (data.Length >= 1 && data[0] == editorName)
                        {
                            // Créer une instance de Jeu avec les données du jeu
                            Jeu jeu = new Jeu
                            {
                                Titre = data[1],
                                Support = data[2],
                                Description = data[3],
                                DateSortie = DateTime.Parse(data[4]),
                                ImageUrl = data[5],
                                VideoUrl = data[6],
                            };

                            // Ajouter le jeu à la liste des jeux associés à l'éditeur
                            jeuxAssocies.Add(jeu);
                        }
                    }

                    JsonSerializerOptions options = new()
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        WriteIndented = true
                    };

                    // Créer le chemin du fichier JSON
                    string jsonFilePath = $@"C:\Users\Loris\GameManagement\{user.Username}\{editorName}_games.json";

                    // Écrire la liste de jeux dans le fichier JSON
                    File.WriteAllText(jsonFilePath, System.Text.Json.JsonSerializer.Serialize(jeuxAssocies, options));

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Une erreur s'est produite lors de la sauvegarde au format XML : {ex.Message}");
                }
            }


        }

        public List<Jeu> SortByDate(string editorName)
        {
            List<Jeu> jeuxAssocies = new List<Jeu>();

            if (!string.IsNullOrEmpty(editorName))
            {
                // Lire les lignes du fichier des éditeurs
                string[] lines = File.ReadAllLines(@"C:\Users\Loris\games.txt");

                // Parcourir les lignes du fichier des éditeurs
                foreach (string line in lines)
                {
                    // Séparer les données de chaque ligne
                    string[] data = line.Split(',');

                    if (data.Length >= 1 && data[0] == editorName)
                    {
                        string gameName = data[1];
                        string gameSupport = data[2];
                        string gameDescription = data[3];
                        DateTime gameReleaseDate = DateTime.Parse(data[4]);
                        string gameImage = data[5];
                        string gameVideo = data[6];

                        // Créer une instance de Jeu avec les arguments requis
                        Jeu jeu = new Jeu(editorName, gameName, gameSupport, gameDescription, gameReleaseDate, gameImage, gameVideo);

                        // Ajouter le jeu à la liste des jeux associés à l'éditeur
                        jeuxAssocies.Add(jeu);

                    }
                }
            }

            // Retourner la liste des jeux associés à l'éditeur

            jeuxAssocies = jeuxAssocies.OrderBy(jeu => jeu.DateSortie).ToList();

            return jeuxAssocies;
        }

        public void ImporterFichierXML(string xmlFilePath, User user)
        {
            this.user = user;
            try
            {
                // Utiliser un StreamReader pour lire le fichier XML
                using (StreamReader fileStream = new StreamReader(xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Jeu>));
                    List<Jeu> jeux = (List<Jeu>)serializer.Deserialize(fileStream);

                    // Parcourir la liste des jeux désérialisés
                    foreach (Jeu jeu in jeux)
                    {
                        // Enregistrer chaque jeu dans le fichier games.txt
                        jeu.EnregistrerDansFichier($@"C:\Users\Loris\GameManagement\{user.Username}\games.txt");
                    }

                    Console.WriteLine("Les données ont été importées avec succès.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de l'importation du fichier XML : {ex.Message}");
            }
        }

        public void ImporterFichierJSON(string jsonFilePath, User user)
        {
            this.user = user;

            try
            {
                // Utiliser un StreamReader pour lire le fichier JSON
                using (StreamReader fileStream = new StreamReader(jsonFilePath))
                {
                    // Utiliser Newtonsoft.Json pour désérialiser le JSON en une liste de jeux
                    List<Jeu> jeux = JsonConvert.DeserializeObject<List<Jeu>>(fileStream.ReadToEnd());

                    // Parcourir la liste des jeux désérialisés
                    foreach (Jeu jeu in jeux)
                    {
                        // Enregistrer chaque jeu dans le fichier games.txt
                        jeu.EnregistrerDansFichier($@"C:\Users\Loris\GameManagement\{user.Username}\games.txt");
                    }

                    Console.WriteLine("Les données ont été importées avec succès.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de l'importation du fichier JSON : {ex.Message}");
            }
        }







    }
}
