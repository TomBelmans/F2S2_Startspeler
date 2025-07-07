namespace API.Controllers.API
{
    [ApiController]
    [Route("[controller]")]
    public class ProductenController : ControllerBase
    {
        private readonly StartspelerContext _context;

        public ProductenController(StartspelerContext context)
        {
            _context = context;
        }

        // GET: api/Producten
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_context.Producten == null)
            {
                return NotFound();
            }
            return await _context.Producten
                .Include(p => p.ProductType)
                .ToListAsync();
        }

        // GET: api/Producten/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_context.Producten == null)
            {
                return NotFound();
            }
            var product = await _context.Producten.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
    }
}