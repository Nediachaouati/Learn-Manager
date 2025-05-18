namespace LearnManager.Models
{
    public class Inscription
    {
        public int Id { get; set; }
        public int ApprenantId { get; set; }
        public User? Apprenant { get; set; }
        public int FormationId { get; set; }
        public Formation? Formation { get; set; }
        public DateTime DateInscription { get; set; }
    }
}
