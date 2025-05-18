using System;
using System.ComponentModel.DataAnnotations;

    namespace LearnManager.Models
    {
        public class Depense
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Le type de dépense est requis.")]
            public string? Type { get; set; } 
            public string? Montant { get; set; }

            public int? FormateurId { get; set; } 
            public User? Formateur { get; set; }

            public int? FormationId { get; set; } 
            public Formation? Formation { get; set; }

            [Required(ErrorMessage = "La date est requise.")]
            public DateTime DateDepense { get; set; }

            public string? Description { get; set; } 
        }
    }
