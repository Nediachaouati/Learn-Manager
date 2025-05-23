﻿namespace LearnManager.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Nom { get; set; } 
        public string? Prenom { get; set; } 
        public string? Email { get; set; } 
        public string? Password { get; set; } 
        public string? Phone { get; set; } 
        public string? Role { get; set; } 
        public DateTime CreatedAt { get; set; }

        public List<Inscription> ?Inscriptions { get; set; }
    }
}
