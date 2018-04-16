namespace MovieRestAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Movie
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Movie()
        {
            MovieActors = new HashSet<MovieActor>();
        }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Year { get; set; }

        [StringLength(50)]
        public string Director { get; set; }

        [Key]
        public int MovieId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovieActor> MovieActors { get; set; }
    }
}
