namespace Companion.Models
{
    public class Producttype
    {
        public int id { get; set; }
        public string naam { get; set; } = default!;
        public override bool Equals(object? obj)
        {
            return obj is Producttype producttype &&
                   id == producttype.id
                   && naam == producttype.naam;
        }
        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
