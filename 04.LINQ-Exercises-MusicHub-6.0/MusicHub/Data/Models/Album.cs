using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateOnly ReleaseDate { get; set; }

        public decimal Price { get; set; }

        public int? ProducerId { get; set; }


    }
}
