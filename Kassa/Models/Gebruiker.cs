namespace Kassa.Models
{
    public class Gebruiker
    {
        public string Id { get; set; } = default!;
        public string Voornaam { get; set; } = default!;
        public string Achternaam { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string NormalizedUserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string NormalizedEmail { get; set; } = default!;
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; } = default!;
        public string SecurityStamp { get; set; } = default!;
        public string ConcurrencyStamp { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public Aspnetroles Rol { get; set; } = default!;
        public Aspnetuserroles GebruikerRol { get; set; } = default!;

        public string VolledigeNaam => $"{Voornaam} {Achternaam}";
    }
}