using System.Linq.Expressions;
using PruebaEurofirms.Domain.Entities;
using PruebaEurofirms.Infrastructure.Interfaces;
using System.Data.SQLite;

namespace PruebaEurofirms.Infrastructure.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly DbContext _dbContext;

        public CharacterRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Character> GetAllCharacters()
        {
            var characters = new List<Character>();
            using var connection = _dbContext.GetConnection();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Character";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var character = new Character
                {
                    Id = reader.GetInt32(0),
                };
                characters.Add(character);
            }
            return characters;
        }

        public void AddCharacter(Character character)
        {
            using var connection = _dbContext.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO Character (Id, Name, Status, Gender)
                VALUES (@id, @name, @status, @gender)
            ";
            command.Parameters.AddWithValue("@id", character.Id);
            command.Parameters.AddWithValue("@name", character.Name);
            command.Parameters.AddWithValue("@status", character.Status);
            command.Parameters.AddWithValue("@gender", character.Gender);
            try{
                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex) when (ex.ErrorCode == 19){}
        }

        public void AddCharacters(List<Character> characters)
        {
            foreach (Character character in characters)
            {
                AddCharacter(character);
            }
        }

    }
}