using PruebaEurofirms.Domain.Entities;
using PruebaEurofirms.Infrastructure.Interfaces;
using System.Data.SQLite;

namespace PruebaEurofirms.Infrastructure.Repositories
{
    public class EpisodeRepository : IEpisodeRepository
    {
        private readonly DbContext _dbContext;

        public EpisodeRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Episode> GetAllEpisodes()
        {
            var episodes = new List<Episode>();
            using var connection = _dbContext.GetConnection();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Character";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var episode = new Episode
                {
                    Id = reader.GetInt32(0),
                };
                episodes.Add(episode);
            }
            return episodes;
        }

        public void AddEpisode(Episode episode)
        {
            using var connection = _dbContext.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO Episode (Id, Name, Code, AirDate, Url)
                VALUES (@id, @name, @code, @airdate, @url)
            ";
            command.Parameters.AddWithValue("@id", episode.Id);
            command.Parameters.AddWithValue("@name", episode.Name);
            command.Parameters.AddWithValue("@code", episode.Code);
            command.Parameters.AddWithValue("@airdate", episode.AirDate);
            command.Parameters.AddWithValue("@url", episode.Url);
            try{
                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex) when (ex.ErrorCode == 19){}
        }
        public void AddEpisodes(IEnumerable<Episode> episodes)
        {
            foreach (var episode in episodes)
            {
                AddEpisode(episode);  
            }
        }

    }
}