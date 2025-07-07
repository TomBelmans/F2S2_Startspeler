namespace API.Controllers.API
{
    [Route("[controller]")]
    [ApiController]
    public class BestellingController : ControllerBase
    {
        private readonly StartspelerContext _context;

        public BestellingController(StartspelerContext context)
        {
            _context = context;
        }

        // GET: api/Bestelling
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bestelling>>> GetBestellingen()
        {
            if (_context.Bestellingen == null)
            {
                return NotFound();
            }
            return await _context.Bestellingen
                .Include(g => g.Gebruiker)
                .Include(o => o.Orderlijnen)
                .ThenInclude(p => p.Product)
                .ToListAsync();
        }

        // GET: api/Bestelling/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bestelling>> GetBestelling(int id)
        {
            if (_context.Bestellingen == null)
            {
                return NotFound();
            }
            var bestelling = await _context.Bestellingen
                .Include(b => b.Orderlijnen)
                .ThenInclude(ol => ol.Product)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bestelling == null)
            {
                return NotFound();
            }

            return bestelling;
        }

        // PUT: api/Bestelling/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBestelling(int id, Bestelling bestelling)
        {
            if (id != bestelling.Id)
            {
                return BadRequest();
            }

            _context.Entry(bestelling).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BestellingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bestelling
        [HttpPost]
        public async Task<IActionResult> PostBestelling([FromBody] Bestelling bestelling)
        {
            if (bestelling == null)
            {
                return BadRequest("Ongeldige bestelling.");
            }

            _context.Bestellingen.Add(bestelling);

            await _context.SaveChangesAsync();

            return Ok(bestelling.Id);
        }

        [HttpPost("VoegProductAanBestellingToe")]
        public async Task<IActionResult> AddProductToBestelling([FromBody] Orderlijn orderlijn)
        {
            if (orderlijn == null)
            {
                return BadRequest("Ongeldige gegevens.");
            }

            var bestelling = await _context.Bestellingen
                .Include(b => b.Orderlijnen)
                .FirstOrDefaultAsync(b => b.Id == orderlijn.BestellingId);

            if (bestelling == null)
            {
                return NotFound("Bestelling niet gevonden.");
            }

            var product = await _context.Producten
                .FirstOrDefaultAsync(p => p.Id == orderlijn.ProductId);

            if (product == null)
            {
                return NotFound("Product niet gevonden.");
            }

            // Check of product in stock is
            if (product.Aantal < orderlijn.TotaalAantal)
            {
                return BadRequest("Product niet op voorraad.");
            }

            var order = new Orderlijn
            {
                BestellingId = orderlijn.BestellingId,
                ProductId = orderlijn.ProductId,
                TotaalAantal = orderlijn.TotaalAantal,
                Bestelling = bestelling,
                Product = product
            };

            // Update stock
            product.Aantal = (short?)(product.Aantal - orderlijn.TotaalAantal);

            bestelling.Orderlijnen.Add(order);
            bestelling.TotaalPrijs += product.Prijs * order.TotaalAantal;

            _context.Orderlijnen.Add(order);
            _context.Entry(bestelling).State = EntityState.Modified;
            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Product toegevoegd aan bestelling." });
        }

        // DELETE: api/Bestelling/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBestelling(int id)
        {
            if (_context.Bestellingen == null)
            {
                return NotFound();
            }
            var bestelling = await _context.Bestellingen.FindAsync(id);
            if (bestelling == null)
            {
                return NotFound();
            }

            _context.Bestellingen.Remove(bestelling);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("lijst")]
        public async Task<ActionResult<IEnumerable<Bestelling>>> GetBestellingenLijst()
        {
            return await _context.Bestellingen.ToListAsync();
        }

        private bool BestellingExists(int id)
        {
            return (_context.Bestellingen?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}