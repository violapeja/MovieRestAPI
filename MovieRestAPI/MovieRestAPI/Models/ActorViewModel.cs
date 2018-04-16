using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieRestAPI.Models
{
    public class ActorViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? ActorId { get; set; }
    }
}