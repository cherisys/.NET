using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models
{
    public class EditGameModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Please select Genre.")]
        [Range(1,100,ErrorMessage = "Please select Genre.")]
        public int GenreId { get; set; }

        [Range(1, 100)]
        public decimal Price { get; set; }
        public DateOnly ReleaseDate { get; set; }
    }
}
