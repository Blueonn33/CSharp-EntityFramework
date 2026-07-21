using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using static MoviesApp.Common.EntityValidation;

namespace MoviesApp.DTOs.Xml
{
    [XmlType("GenreGroup")]
    public class ImportXmlGenreGroupDto
    {
        [Required]
        [MinLength(MovieGenreMinLength)]
        [MaxLength(MovieGenreMaxLength)]
        [XmlAttribute("name")]
        public string GenreName { get; set; } = null!;

        [XmlElement("Movie")]
        public XmlGenreGroupMovieDto Movie { get; set; } = null!;
    }
}
