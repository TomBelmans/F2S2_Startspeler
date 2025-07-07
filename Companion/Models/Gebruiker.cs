namespace Companion.Models
{
    public class Gebruiker
    {
        public string voornaam { get; set; } = default!;
        public string achternaam { get; set; } = default!;
        public string id { get; set; } = default!;
        public string userName { get; set; } = default!;
        public string normalizedUserName { get; set; } = default!;
        public string email { get; set; } = default!;
        public string normalizedEmail { get; set; } = default!;
        public bool emailConfirmed { get; set; }
        public string passwordHash { get; set; } = default!;
        public string securityStamp { get; set; } = default!;
        public string concurrencyStamp { get; set; } = default!;
        public string phoneNumber { get; set; } = default!;
        public bool phoneNumberConfirmed { get; set; }
        public bool twoFactorEnabled { get; set; }
        public DateTime? lockoutEnd { get; set; }
        public bool lockoutEnabled { get; set; }
        public int accessFailedCount { get; set; }

        public string VolledigeNaam => $"{voornaam} {achternaam}";
    }
}
