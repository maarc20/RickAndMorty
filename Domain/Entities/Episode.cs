namespace PruebaEurofirms.Domain.Entities
{
    public class Episode
    {
        public int Id { get; set; }  // ID único para cada episodio
        public string? Name { get; set; }  // Codigo del Episodio
        public string? Code { get; set; }  // Nombre del Episodio
        public string? Url { get; set; }  // Nombre del Episodio
        public string? AirDate { get; set; }  // Nombre del Episodio

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            Episode other = (Episode)obj;
            return Id == other.Id;
        }
        public override int GetHashCode()
        {
            return (Id).GetHashCode();
        }

    }
}