namespace MovieRestAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MovieActor")]
    public partial class MovieActor
    {
        public int MovieId { get; set; }

        public int ActorId { get; set; }

        public int MovieActorId { get; set; }

        public virtual Actor Actor { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
