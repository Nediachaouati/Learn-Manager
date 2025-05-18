namespace LearnManager.Models.ViewModels
{
    public class ApprenantInscriptionViewModel
    {
       
        public int ApprenantId { get; set; }
        public string ApprenantNom { get; set; }
        public string ApprenantPrenom { get; set; }
        public string ApprenantEmail { get; set; }
        public string FormationTitre { get; set; }
        public DateTime FormationDateDebut { get; set; }
        public DateTime FormationDateFin { get; set; }
        public DateTime DateInscription { get; set; }
        public string FormateurNom { get; set; }
        public string FormateurPrenom { get; set; }
    }
}

