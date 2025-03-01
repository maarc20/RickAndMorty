namespace PruebaEurofirms.Domain.Entities
{
    public class CharacterAPI
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; } 
        public List<int> EpisodeIds { get; set; }
        public List<string> EpisodeUrls { get; set; }
    }
}