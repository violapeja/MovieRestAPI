using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieRestAPI.Models
{
    public class MovieViewModel
    {
        public int? MovieId { get; set; }

        public string Title { get; set; }

        public string Director { get; set; }

        public string Year { get; set; }

        public List<ActorViewModel> Actors { get; set; }
    }
}