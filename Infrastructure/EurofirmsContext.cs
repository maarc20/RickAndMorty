using System.Data.SQLite;

namespace PruebaEurofirms.Infrastructure
{
    public class DbContext
    {
        private readonly string _connectionString;

        public DbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la conexión
        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection (_connectionString);
        }

        // Inicialización de la base de datos
        public void InitializeDatabase()
        {
            using var connection = GetConnection();
            connection.Open();
            
            // Crear tablas si no existen
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Episode (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Code TEXT NOT NULL,
                    Url TEXT NOT NULL,
                    AirDate TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Character (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Gender TEXT NOT NULL,
                    Status TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS CharacterEpisode (
                    CharacterId INTEGER,
                    EpisodeId INTEGER,
                    FOREIGN KEY (CharacterId) REFERENCES Character(Id),
                    FOREIGN KEY (EpisodeId) REFERENCES Episode(Id)
                );
            ";
            command.ExecuteNonQuery();
        }
    }
}