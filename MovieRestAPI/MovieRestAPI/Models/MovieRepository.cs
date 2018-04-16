using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MovieRestAPI.Models
{
    public class MovieRepository : IMovieRepository
    {

        public IEnumerable<MovieViewModel> GetAll()
        {
            using (var db = new MovieContext())
            {
                var movies = db.Movies.Select(x => new MovieViewModel()
                {
                    MovieId = x.MovieId,
                    Director = x.Director,
                    Title = x.Title,
                    Year = x.Year
                }).ToList();

                foreach (var movie in movies)
                {
                    movie.Actors = db.MovieActors.Include(x => x.Actor).Where(x => x.MovieId == movie.MovieId).Select(x => new ActorViewModel()
                    {
                        ActorId = x.Actor.ActorId,
                        FirstName = x.Actor.FirstName,
                        LastName = x.Actor.LastName
                    }).ToList();
                }

                return movies;
            }
        }

        public MovieViewModel GetMovie(int Id)
        {
            using (var db = new MovieContext())
            {
                var movieInDb = db.Movies.Find(Id);
                if (movieInDb == null)
                {
                    return null;
                }
                var actors = db.MovieActors.Include(x => x.Actor).Where(x => x.MovieId == movieInDb.MovieId).Select(x => new ActorViewModel()
                {
                    ActorId = x.Actor.ActorId,
                    FirstName = x.Actor.FirstName,
                    LastName = x.Actor.LastName
                }).ToList();

                var movie = new MovieViewModel()
                {
                    MovieId = movieInDb.MovieId,
                    Director = movieInDb.Director,
                    Title = movieInDb.Title,
                    Year = movieInDb.Year,
                    Actors = actors
                };

                return movie;
            }
        }

        public int AddMovie(MovieViewModel movie)
        {
            using (var db = new MovieContext())
            {
                var newMovie = new Movie()
                {
                    Title = movie.Title,
                    Year = movie.Year,
                    Director = movie.Director
                };
                db.Movies.Add(newMovie);
                db.SaveChanges();

                foreach (var actor in movie.Actors)
                {
                    var actorId = actor.ActorId;
                    bool actorIsInDb = false;
                    if (actorId.HasValue)
                    {
                        var actorInDb = db.Actors.Find(actorId);
                        if (actorInDb != null)
                        {
                            actorIsInDb = true;
                        }
                    }

                    if(!actorIsInDb)
                    {
                        var newActor = new Actor()
                        {
                            FirstName = actor.FirstName,
                            LastName = actor.LastName
                        };
                        db.Actors.Add(newActor);
                        db.SaveChanges();

                        actorId = newActor.ActorId;
                    }

                    db.MovieActors.Add(new MovieActor()
                    {
                        ActorId = actorId.Value,
                        MovieId = newMovie.MovieId
                    });
                }
                db.SaveChanges();
                return newMovie.MovieId;

            }
        }

        public bool UpdateMovie(int Id, MovieViewModel movie)
        {
            using (var db = new MovieContext())
            {
                var movieInDb = db.Movies.Find(Id);
                if (movie == null)
                {
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(movie.Title))
                {
                    movieInDb.Title = movie.Title;
                }
                if (!string.IsNullOrWhiteSpace(movie.Director))
                {
                    movieInDb.Director = movie.Director;
                }
                if (!string.IsNullOrWhiteSpace(movie.Year))
                {
                    movieInDb.Year = movie.Year;
                }

                foreach (var actor in movie.Actors)
                {
                    if (actor.ActorId.HasValue)
                    {
                        var actorInDb = db.Actors.Find(actor.ActorId.Value);
                        if (actorInDb != null)
                        {
                            var movieActorInDb = db.MovieActors.Find(actorInDb.ActorId);
                            if (movieActorInDb == null)
                            {
                                db.MovieActors.Add(new MovieActor()
                                {
                                    ActorId = actorInDb.ActorId,
                                    MovieId = movieInDb.MovieId
                                });
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var newActor = new Actor()
                        {
                            FirstName = actor.FirstName,
                            LastName = actor.LastName
                        };
                        db.Actors.Add(newActor);
                        db.SaveChanges();

                        db.MovieActors.Add(new MovieActor()
                        {
                            ActorId = newActor.ActorId,
                            MovieId = movieInDb.MovieId
                        });
                        db.SaveChanges();
                    }
                }

                return true;
            }
        }

        public bool RemoveMovie(int Id)
        {
            using (var db = new MovieContext())
            {
                var movieInDb = db.Movies.Find(Id);
                if (movieInDb != null)
                {
                    var movieActorsInDb = db.MovieActors.Where(x => x.MovieId == Id).ToList();
                    if (movieActorsInDb.Any())
                    {
                        db.MovieActors.RemoveRange(movieActorsInDb);
                    }
                    db.Movies.Remove(movieInDb);
                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public IEnumerable<MovieViewModel> Search(string Year, string OrderBy)
        {
            var movies = GetAll();
            if(!string.IsNullOrWhiteSpace(Year))
            {
                movies = movies.Where(x => !string.IsNullOrWhiteSpace(x.Year) && x.Year.Trim() == Year.Trim()).ToList();
            }
            if (!string.IsNullOrWhiteSpace(OrderBy))
            {
                if(OrderBy.ToUpper().Trim() == "TITLE")
                {
                    movies = movies.OrderBy(x => x.Title).ToList();
                }
                else if (OrderBy.ToUpper().Trim() == "DIRECTOR")
                {
                    movies = movies.OrderBy(x => x.Director).ToList();
                }
            }

            return movies;
        }
    }
}