using System;
using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Sentimiento { get; set; } = string.Empty; // "like" o "dislike"
        public DateTime Fecha { get; set; }
    }
}
