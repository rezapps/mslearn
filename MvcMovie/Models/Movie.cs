using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models
{
	public class Movie
	{
		[Key]
		public int Id { get; set; }
		[StringLength(60, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 60 characters long!")]
		public required string Title { get; set; }

		public string? Poster { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Release Date")]
		public required DateTime ReleaseDate { get; set; }

		[RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Genre must start with a capital letter!")]
		[StringLength(30)]
		public required string Genre { get; set; }
		[DataType(DataType.Currency)]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[Column(TypeName = "decimal(18, 2)")]
		public required decimal Price { get; set; }
		[RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$", ErrorMessage = "Rating must start with a capital letter!")]
		[StringLength(5)]
		public required string Rating { get; set; }
	}
}
