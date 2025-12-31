using projekt_zaliczeniowy.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projekt_zaliczeniowy.Models
{
    public class History
    {
        [Key]
        public long Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Dzialanie { get; set; }
        [Column(TypeName = "date")]
        public DateOnly Data { get; set; } = new DateOnly();


        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }
    }
}
