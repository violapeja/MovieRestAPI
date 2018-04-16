using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestAPI.Models
{
    public interface IMovieRepository
    {
        IEnumerable<MovieViewModel> GetAll();

        MovieViewModel GetMovie(int Id);

        int AddMovie(MovieViewModel movie);

        bool UpdateMovie(int Id, MovieViewModel movie);

        bool RemoveMovie(int Id);

        IEnumerable<MovieViewModel> Search(string Year, string OrderBy);
    }
}
