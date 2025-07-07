namespace API.Controllers.API
{
    [Route("[controller]")]
    [ApiController]
    public class InschrijvingController : ControllerBase
    {
        private readonly StartspelerContext _context;

        public InschrijvingController(StartspelerContext context)
        {
            _context = context;
        }

        // GET: api/Inschrijving
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inschrijving>>> GetInschrijvingen()
        {
            if (_context.Inschrijvingen == null)
            {
                return NotFound();
            }
            return await _context.Inschrijvingen
                .Include(g => g.Gebruiker)
                .Include(e => e.Evenement)
                .ThenInclude(ct => ct.CommunityType)
                .ToListAsync();
        }

        // GET: api/Inschrijving/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inschrijving>> GetInschrijving(int id)
        {
            if (_context.Inschrijvingen == null)
            {
                return NotFound();
            }
            var inschrijving = await _context.Inschrijvingen.FindAsync(id);

            if (inschrijving == null)
            {
                return NotFound();
            }

            return inschrijving;
        }

        // PUT: api/Inschrijving/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInschrijving(int id, Inschrijving inschrijving)
        {
            if (id != inschrijving.Id)
            {
                return BadRequest();
            }

            _context.Entry(inschrijving).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InschrijvingExists(id))
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

        // POST: api/Inschrijving
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<Inschrijving>> PostInschrijving(Inschrijving inschrijving)
        {
            if (_context.Inschrijvingen == null)
            {
                return Problem("Entity set 'HelloCoreContext.Inschrijvingen'  is null.");
            }
            _context.Inschrijvingen.Add(inschrijving);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInschrijving", new { id = inschrijving.Id }, inschrijving);
        }

        // DELETE: api/Inschrijving/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInschrijving(int id)
        {
            if (_context.Inschrijvingen == null)
            {
                return NotFound();
            }
            var inschrijving = await _context.Inschrijvingen.FindAsync(id);
            if (inschrijving == null)
            {
                return NotFound();
            }

            _context.Inschrijvingen.Remove(inschrijving);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("lijst")]
        public async Task<ActionResult<IEnumerable<Inschrijving>>> GetInschrijvingenLijst()
        {
            return await _context.Inschrijvingen.ToListAsync();
        }
        private bool InschrijvingExists(int id)
        {
            return (_context.Inschrijvingen?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
