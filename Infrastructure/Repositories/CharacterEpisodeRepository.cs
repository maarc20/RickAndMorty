using System.Linq.Expressions;
using PruebaEurofirms.Domain.Entities;
using PruebaEurofirms.Infrastructure.Interfaces;
using System.Data.SQLite;

namespace PruebaEurofirms.Infrastructure.Repositories
{
    public class CharacterEpisodeRepository : ICharacterEpisodeRepository
    {
        private readonly DbContext _dbContext;

        public CharacterEpisodeRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CharacterEpisode> GetAllCharacterEpisodes()
        {
            var charactersEpisodes = new List<CharacterEpisode>();
            using var connection = _dbContext.GetConnection();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM CharacterEpisode";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var characterEpisode = new CharacterEpisode
                {
                    CharacterId = reader.GetInt32(0),
                    EpisodeId = reader.GetInt32(1),
                };
                charactersEpisodes.Add(characterEpisode);
            }
            return charactersEpisodes;
        }

        public void AddCharacterEpisodes(Dictionary<int, List<Episode>> CharacterEpisodes)
        {
            using var connection = _dbContext.GetConnection();
            connection.Open();
            foreach(var kpv in CharacterEpisodes)
            {
                foreach(var episode in kpv.Value){
                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        INSERT INTO CharacterEpisode (CharacterId, EpisodeId)
                        VALUES (@character_id, @episode_id)
                    ";
                    command.Parameters.AddWithValue("@character_id", kpv.Key);
                    command.Parameters.AddWithValue("@episode_id", episode.Id);
                    try{
                        command.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex) when (ex.ErrorCode == 19){}
                }
            }
        }

        public void DeleteEpisodesFromCaracterId(int CharacterId)
        {
            using var connection = _dbContext.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = 
            @"
                DELETE FROM CharacterEpisode where CharacterId = @characterId
            ";
            command.Parameters.AddWithValue("@characterId", CharacterId);
            command.ExecuteNonQuery();
        }

        public List<int> GetEpisodeIdsFromCharacterId(int CharacterId)
        {
            var episodeIds = new List<int>();
            using var connection = _dbContext.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = 
            @"
                SELECT EpisodeId FROM CharacterEpisode where CharacterId = @characterId
            ";
            command.Parameters.AddWithValue("@characterId", CharacterId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                episodeIds.Add( reader.GetInt32(0));
            }
            
            return episodeIds;
        }
    }
}