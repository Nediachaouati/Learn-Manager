namespace LearnManager.Models
{
    public class Formation
    {
        public int Id { get; set; }
        public string? Titre { get; set; }
        public string? Description { get; set; }
        public string? Categorie { get; set; } 
        public string? Niveau { get; set; }
        public int? FormateurId { get; set; } 
        public User? Formateur { get; set; } 
        public string? Format { get; set; } 
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string? Duree { get; set; } 
        public String? Prix { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
