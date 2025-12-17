using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace websqliteapp.Models
{
    public class Assessment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Range(0, 100)]
        public int Puntaje { get; set; }

        [Required]
        public DateTime FechaEvaluacion { get; set; }

        [MaxLength(500)]
        public string? Descripcion { get; set; }
    }
}