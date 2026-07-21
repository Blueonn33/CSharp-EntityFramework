using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;

using static MoviesApp.Common.EntityValidation;

namespace MoviesApp.DTOs.Xml
{
    [XmlType("Movie")]
    public class XmlGenreGroupMovieDto
    {
        [Range(1, int.MaxValue)]
        [XmlAttribute("id")]
        public int Id
        {
            get; set;
        }

        [Range(MovieDurationMinValue, MovieDurationMaxValue)]
        [XmlAttribute("duration")]
        public int Duration
        {
            get; set;
        }

        [Required]
        [MaxLength(MovieTitleMaxLength)]
        [XmlElement("Title")]
        public string Title { get; set; } = null!;

        [Required]
        [XmlElement("Details")]
        public XmlMovieDetailsDto Details { get; set; } = null!;

        [Required]
        [MinLength(MovieDescriptionMinLength)]
        [MaxLength(MovieDescriptionMaxLength)]
        [XmlAnyElement("Description")]
        public string Description { get; set; } = null!;

        [XmlAnyElement("Media")]
        public XmlElement MediaElement { get; set; } = null!;

        [MaxLength(MovieImageUrlMaxLength)]
        public string? ImageUrl
            => MediaElement.GetElementsByTagName("ImageUrl").Item(0)?.Value;
    }
}