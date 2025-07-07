namespace Companion.Models
{
    public class Communitytype
    {
        public int Id { get; set; }
        public string Naam { get; set; } = default!;
        public override bool Equals(object? obj)
        {
            return obj is Communitytype communitytype &&
                   Id == communitytype.Id
                   && Naam == communitytype.Naam;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
