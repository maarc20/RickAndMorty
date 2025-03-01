namespace PruebaEurofirms.Domain.Entities
{
    public class Episode
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; } 
        public string? Url { get; set; }
        public string? AirDate { get; set; }

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