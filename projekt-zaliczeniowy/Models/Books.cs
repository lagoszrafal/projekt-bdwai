using projekt_zaliczeniowy.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using projekt_zaliczeniowy.Models;

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
        public DateOnly DataWypozyczenia { get; set; } = new DateOnly();

        
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Categories Category { get; set; }
    }
}
