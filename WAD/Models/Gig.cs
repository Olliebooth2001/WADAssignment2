using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAD.Models
{
    public class Gig
    {
        [Key]
        public int GigID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string GigTitle { get; set; }

        [Column(TypeName = "text")]
        public string GigDescription { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string GigImage { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal GigPrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
    }
}
