namespace PruebaEurofirms.Domain.Entities
{
    public class Character
    {
        public int Id { get; set; }  // ID único para cada personaje
        public string? Name { get; set; }  // Nombre del personaje
        public string? Status { get; set; }  // Estado (Alive, Dead, Unknown)
        public string? Gender { get; set; }  // Género del personaje
    }
}