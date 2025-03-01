namespace PruebaEurofirms.Domain.Entities
{
    public class CharacterAPI
    {
        public int Id { get; set; }  // ID único para cada personaje
        public string? Name { get; set; }  // Nombre del personaje
        public string? Status { get; set; }  // Estado (Alive, Dead, Unknown)
        public string? Gender { get; set; }  // Género del personaje
        public List<int> EpisodeIds { get; set; }
        public List<string> EpisodeUrls { get; set; }
    }
}