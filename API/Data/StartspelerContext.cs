namespace API.Data
{
    public class StartspelerContext : IdentityDbContext<Gebruiker>
    {
        public StartspelerContext(DbContextOptions<StartspelerContext> options) : base(options) { }

        public DbSet<Gebruiker> Gebruikers { get; set; } = default!;
        public DbSet<ProductType> ProductTypes { get; set; } = default!;
        public DbSet<Product> Producten { get; set; } = default!;
        public DbSet<Bestelling> Bestellingen { get; set; } = default!;
        public DbSet<Orderlijn> Orderlijnen { get; set; } = default!;
        public DbSet<CommunityType> CommunityTypes { get; set; } = default!;
        public DbSet<Evenement> Evenementen { get; set; } = default!;
        public DbSet<Inschrijving> Inschrijvingen { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Gebruiker>().ToTable("Gebruiker");
            modelBuilder.Entity<ProductType>().ToTable("ProductType");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Bestelling>().ToTable("Bestelling");
            modelBuilder.Entity<Orderlijn>().ToTable("Orderlijn");
            modelBuilder.Entity<CommunityType>().ToTable("CommunityType");
            modelBuilder.Entity<Evenement>().ToTable("Evenement");
            modelBuilder.Entity<Inschrijving>().ToTable("Inschrijving");

            // Bestellingen
            modelBuilder.Entity<Bestelling>()
                .HasOne(b => b.Gebruiker)
                .WithMany(g => g.Bestellingen)
                .HasForeignKey(b => b.GebruikerId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade: bestellingen worden verwijderd wanneer een Gebruiker wordt verwijderd.

            // Producten
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductType)
                .WithMany(pt => pt.Producten)
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict: om te voorkomen dat een producttype wordt verwijderd als er nog producten van dit type zijn.

            // BestellingProduct -> Bestelling
            modelBuilder.Entity<Orderlijn>()
                .HasOne(bp => bp.Bestelling)
                .WithMany(b => b.Orderlijnen)
                .HasForeignKey(bp => bp.BestellingId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade: omdat Orderlijn records niet zonder een bestelling mogen bestaan.

            // Relatie BestellingProduct -> Product
            modelBuilder.Entity<Orderlijn>()
                .HasOne(bp => bp.Product)
                .WithMany(p => p.Orderlijnen)
                .HasForeignKey(bp => bp.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict: om te voorkomen dat Producten worden verwijderd als ze nog deel uitmaken van bestellingen.

            // Inschrijving -> Gebruiker
            modelBuilder.Entity<Inschrijving>()
                .HasOne(i => i.Gebruiker)
                .WithMany(g => g.Inschrijvingen)
                .HasForeignKey(i => i.GebruikerId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade: inschrijvingen mogen niet bestaan zonder gebruiker.

            // Inschrijving -> Evenement
            modelBuilder.Entity<Inschrijving>()
                .HasOne(i => i.Evenement)
                .WithMany(e => e.Inschrijvingen)
                .HasForeignKey(i => i.EvenementId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict: om te voorkomen dat inschrijvingen worden verwijderd als ze nog deel uitmaken van een evenement.

            // Evenementen
            modelBuilder.Entity<Evenement>()
                .HasOne(e => e.CommunityType)
                .WithMany(ct => ct.Evenementen)
                .HasForeignKey(e => e.CommunityTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict: om te voorkomen dat een community-type wordt verwijderd als er nog evenementen van dit type zijn.
        }
    }
}
