using Microsoft.AspNetCore.Identity;
using projekt_zaliczeniowy.Areas.Identity.Data;
using projekt_zaliczeniowy.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace projekt_zaliczeniowy.Models
{
    public class Books
    {
        [Key]
        public long Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Nazwa { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Autor { get; set; }
        [Column(TypeName = "int")]
        public int Regal { get; set; }
        [Column(TypeName = "date")]
        [AllowNull]
        public DateOnly? DataWypozyczenia { get; set; } = new DateOnly();

        [AllowNull]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Categories? Category { get; set; }
    }
}
