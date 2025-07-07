namespace API.Models
{
    public class CommunityType
    {
        [Key]
        public int Id { get; set; }

        [StringLength(256)]
        public string Naam { get; set; } = default!;
        
        [JsonIgnore]
        public ICollection<Evenement> Evenementen { get; set; } = default!;
    }
}
