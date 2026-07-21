using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;

using static MoviesApp.Common.EntityValidation;

namespace MoviesApp.DTOs.Xml
{
    [XmlType("Details")]
    public class XmlMovieDetailsDto
    {
        [Required]
        [MinLength(MovieGenreMinLength)]
        [MaxLength(MovieGenreMaxLength)]
        [XmlElement("Genre")]
        public string Genre { get; set; } = null!;

        [Required]
        [MinLength(MovieDirectorMinLength)]
        [MaxLength(MovieDirectorMaxLength)]
        [XmlElement("Director")]
        public string Director { get; set; } = null!;


        public XmlElement ReleaseElement { get; set; } = null!;

        [Required]
        [RegularExpression(MovieReleaseDateRegexPattern)]
        public string ReleaseDate
            => ReleaseElement.GetAttribute("date");
    }
}
