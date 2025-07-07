namespace Kassa.Models
{
    public class Aspnetuserroles
    {
        public string UserId { get; set; } = default!;
        public Gebruiker Gebruiker { get; set; } = default!;
        public string RoleId { get; set; } = default!;
        public Aspnetroles Rol { get; set; } = default!;
    }
}
