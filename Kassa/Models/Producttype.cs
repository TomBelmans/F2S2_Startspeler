namespace Kassa.Models
{
    public class Producttype
    {
        public int Id { get; set; }
        public string Naam { get; set; } = default!;


        public override bool Equals(object? obj)
        {
            return obj is Producttype producttype &&
                   Id == producttype.Id
                   && Naam == producttype.Naam;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
