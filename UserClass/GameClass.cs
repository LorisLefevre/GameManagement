using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;

namespace GameManagementClassLibrary
{
    public class Jeu : INotifyPropertyChanged
    {
        private string _editeur;
        private string _titre;
        private string _support;
        private string _description;
        private DateTime _dateSortie;
        private string _imageUrl;
        private string _videoUrl;
        private string _informationMessage;

        public string Editeur
        {
            get
            {
                return _editeur;
            }
            set
            {
                _editeur = value;
                OnPropertyChanged(nameof(Editeur));
            }
        }

        public string Titre
        {
            get
            {
                return _titre;
            }
            set
            {
                _titre = value;
                OnPropertyChanged(nameof(Titre));
            }
        }

        public string getTitre()
        {
            return _titre;
        }

        public string Support
        {
            get
            {
                return _support;
            }
            set
            {
                _support = value;
                OnPropertyChanged(nameof(Support));
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime DateSortie
        {
            get
            {
                return _dateSortie;
            }
            set
            {
                _dateSortie = value;
                OnPropertyChanged(nameof(DateSortie));
            }
        }

        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                _imageUrl = value;
                OnPropertyChanged(nameof(ImageUrl));
            }
        }

        public string VideoUrl
        {
            get
            {
                return _videoUrl;
            }
            set
            {
                _videoUrl = value;
                OnPropertyChanged(nameof(VideoUrl));
            }
        }

        public string InformationMessage
        {
            get
            {
                return _informationMessage;
            }
            set
            {
                _informationMessage = value;
                OnPropertyChanged(nameof(InformationMessage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<Jeu> _listeJeux;
        private int _indexCourant;

        public Jeu()
        {
            _listeJeux = new List<Jeu>();
            _indexCourant = -1;
        }

        public Jeu(string editeur, string titre, string support, string description, DateTime dateSortie, string imageUrl, string videoUrl)
        {
            Editeur = editeur;
            Titre = titre;
            Support = support;
            Description = description;
            DateSortie = dateSortie;
            ImageUrl = imageUrl;
            VideoUrl = videoUrl;
        }

        public void EnregistrerDansFichier(string filePath)
        {
            try
            {
                // Préparer les données du jeu sous forme de ligne CSV
                string jeuData = $"{Editeur},{Titre},{Support},{Description},{DateSortie:yyyy-MM-dd},{ImageUrl},{VideoUrl}";

                // Crée le fichier si nécessaire et ajoute la ligne
                File.AppendAllText(filePath, jeuData + Environment.NewLine);

                Console.WriteLine("Les données du jeu ont été enregistrées avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'enregistrement du jeu : {ex.Message}");
            }
        }

        public void VoirJeu(string gameName, string filePath)
        {
            if (!string.IsNullOrEmpty(gameName))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] data = line.Split(',');

                    if (data.Length >= 1 && data[1] == gameName)
                    {
                        Console.WriteLine($"Titre du jeu: {data[1]}");
                        Console.WriteLine($"Support: {data[2]}");
                        Console.WriteLine($"Description: {data[3]}");
                        Console.WriteLine($"Date de sortie: {data[4]}");
                        Console.WriteLine($"URL de l'image: {data[5]}");
                        Console.WriteLine($"URL de la vidéo: {data[6]}");
                        return;
                    }
                }

                Console.WriteLine("Le jeu spécifié n'a pas été trouvé.");
            }
        }

        public void VoirJeuDeListe(List<Jeu> jeux)
        {
            if (jeux != null && jeux.Count > 0)
            {
                _listeJeux = jeux;
                _indexCourant = 0;

                AfficherJeuCourant();
            }
            else
            {
                Console.WriteLine("La liste de jeux est vide ou invalide.");
            }
        }

        private void AfficherJeuCourant()
        {
            Jeu jeuCourant = _listeJeux[_indexCourant];
            Console.WriteLine($"Titre du jeu: {jeuCourant.Titre}");
            Console.WriteLine($"Support: {jeuCourant.Support}");
            Console.WriteLine($"Description: {jeuCourant.Description}");
            Console.WriteLine($"Date de sortie: {jeuCourant.DateSortie}");
            Console.WriteLine($"URL de l'image: {jeuCourant.ImageUrl}");
            Console.WriteLine($"URL de la vidéo: {jeuCourant.VideoUrl}");
        }

        public void SupprimerDuFichier(string titre, string filePath)
        {
            try
            {
                string[] lignes = File.ReadAllLines(filePath);
                List<string> lignesConservees = new List<string>();

                foreach (string ligne in lignes)
                {
                    string[] elements = ligne.Split(',');
                    if (elements.Length > 1 && elements[1] == titre)
                    {
                        Console.WriteLine($"Le jeu '{titre}' a été trouvé et supprimé.");
                    }
                    else
                    {
                        lignesConservees.Add(ligne);
                    }
                }

                File.WriteAllLines(filePath, lignesConservees);
                Console.WriteLine("Suppression terminée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la suppression des données du jeu : {ex.Message}");
            }
        }

        public void ModifierDansFichier(string filePath)
        {
            try
            {
                string[] lignes = File.ReadAllLines(filePath);
                var lignesModifiees = new List<string>();
                bool firstOccurrence = true;

                foreach (string line in lignes)
                {
                    string[] data = line.Split(',');

                    if (data.Length >= 2 && string.Equals(data[1].Trim(), this.Titre.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (firstOccurrence)
                        {
                            string modifiedGameData = $"{this.Editeur},{this.Titre},{this.Support},{this.Description},{this.DateSortie},{this.ImageUrl},{this.VideoUrl}";
                            lignesModifiees.Add(modifiedGameData);
                            firstOccurrence = false;
                        }
                        else
                        {
                            lignesModifiees.Add(line);
                        }
                    }
                    else
                    {
                        lignesModifiees.Add(line);
                    }
                }

                File.WriteAllLines(filePath, lignesModifiees);
                Console.WriteLine("Modification terminée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la modification des données du jeu : {ex.Message}");
            }
        }


        public void SaveXML(string filePath)
        {
            try
            {
                string[] lignes = File.ReadAllLines(filePath);
                List<Jeu> jeux = new List<Jeu>();

                foreach (string ligne in lignes)
                {
                    string[] data = ligne.Split(',');

                    if (data.Length >= 7)
                    {
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

                        jeux.Add(jeu);
                    }
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<Jeu>));
                using (TextWriter writer = new StreamWriter(Path.ChangeExtension(filePath, "xml")))
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

        public void DeleteXML(string filePath)
        {
            try
            {
                string xmlFilePath = Path.ChangeExtension(filePath, "xml");

                if (File.Exists(xmlFilePath))
                {
                    File.Delete(xmlFilePath);
                    Console.WriteLine("Fichier XML supprimé avec succès.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la suppression du fichier XML : {ex.Message}");
            }
        }

        public void OpenFile(string filePath)
        {
            try
            {
                Process.Start("notepad.exe", filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de l'ouverture du fichier : {ex.Message}");
            }
        }

        public static Jeu LoadFromFile(string gameName, string filePath)
        {
            try
            {
                string[] lignes = File.ReadAllLines(filePath);

                foreach (string line in lignes)
                {
                    string[] data = line.Split(',');

                    if (data.Length >= 2 && string.Equals(data[1].Trim(), gameName.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        return new Jeu
                        {
                            Editeur = data[0],
                            Titre = data[1],
                            Support = data[2],
                            Description = data[3],
                            DateSortie = DateTime.Parse(data[4]),
                            ImageUrl = data[5],
                            VideoUrl = data[6]
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors du chargement des données du jeu : {ex.Message}");
            }

            return null;
        }
    }
}
