using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieRestAPI.Models;

namespace MovieRestAPI.Controllers
{
    public class MoviesController : ApiController
    {
        static readonly IMovieRepository repository = new MovieRepository();

        public IHttpActionResult Get()
        {
            return Ok(repository.GetAll());
        }

        public IHttpActionResult Get(int Id)
        {
            var movie = repository.GetMovie(Id);
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        public IHttpActionResult Post([FromBody] MovieViewModel movie)
        {
            var id = new
            {
                id = repository.AddMovie(movie)
            };
            return Created("/api/movie/" + id, id);
        }

        public IHttpActionResult Put(int Id, [FromBody] MovieViewModel movie)
        {
            if (!repository.UpdateMovie(Id, movie))
            {
                return NotFound();
            }
            return Ok(true);
        }

        public IHttpActionResult Delete(int Id)
        {
            if (!repository.RemoveMovie(Id))
            {
                return NotFound();
            }
            return Ok(true);
        }

        [HttpGet]
        [Route("api/movies/search/{Year}/{OrderBy}")]
        public IHttpActionResult Search(string Year, string OrderBy)
        {
            return Ok(repository.Search(Year, OrderBy));
        }
    }
}
