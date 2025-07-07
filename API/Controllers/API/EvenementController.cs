namespace API.Controllers.API
{
    [Route("[controller]")]
    [ApiController]
    public class EvenementController : ControllerBase
    {
        private readonly StartspelerContext _context;

        public EvenementController(StartspelerContext context)
        {
            _context = context;
        }

        // GET: api/Evenementen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evenement>>> GetEvenementen()
        {
            if (_context.Evenementen == null)
            {
                return NotFound();
            }
            return await _context.Evenementen.ToListAsync();
        }

        // GET: api/Evenement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evenement>> GetEvenement(int id)
        {
            if (_context.Evenementen == null)
            {
                return NotFound();
            }
            var evenement = await _context.Evenementen.FindAsync(id);

            if (evenement == null)
            {
                return NotFound();
            }

            return evenement;
        }

        // POST: api/Evenement
        [HttpPost]
        public async Task<ActionResult<Evenement>> CreateEvenement(Evenement evenement)
        {
            if (_context.Evenementen == null)
            {
                return Problem("De databank bevat geen evenementen.");
            }
            _context.Evenementen.Add(evenement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvenement), new { id = evenement.Id }, evenement);
        }

        // PUT: api/Evenement/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvenement(int id, Evenement evenement)
        {
            if (id != evenement.Id)
            {
                return BadRequest();
            }

            _context.Entry(evenement).State = EntityState.Modified;

            await _context.SaveChangesAsync();


            return NoContent();
        }

        // DELETE: api/Evenement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvenement(int id)
        {
            if (_context.Evenementen == null)
            {
                return NotFound();
            }
            var evenement = await _context.Evenementen.FindAsync(id);
            if (evenement == null)
            {
                return NotFound();
            }

            _context.Evenementen.Remove(evenement);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}