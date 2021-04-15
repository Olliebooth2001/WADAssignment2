using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WAD.Models
{
    public class GigForm
    {    
        [Key]
        public int GigID { get; set; }

        [Required(ErrorMessage = "Don't make us guess.  What Gig are we talking about?")]
        public string GigTitle { get; set; }

        public string GigDescription { get; set; }

        public string GigImage { get; set; }

        public decimal GigPrice { get; set; }
        public decimal GigStars{ get; set; }


        public DateTime ReleaseDate { get; set; }

    }
}

