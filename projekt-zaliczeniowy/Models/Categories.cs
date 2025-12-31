using projekt_zaliczeniowy.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projekt_zaliczeniowy.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Nazwa { get; set; }
    }
}
