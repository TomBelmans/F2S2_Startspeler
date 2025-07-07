namespace Kassa.Models
{
    public class Aspnetroles
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;

        public override bool Equals(object? obj)
        {
            return obj is Aspnetroles aspnetrole &&
                   Id == aspnetrole.Id
                   && Name == aspnetrole.Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
