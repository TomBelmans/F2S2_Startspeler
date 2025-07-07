namespace API.Controllers.API
{
    // Uitleg over gebruikersbeheer: https://jasonwatmore.com/post/2022/01/07/net-6-user-registration-and-login-tutorial-with-example-api

    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Gebruiker> _userManager;
        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly StartspelerContext _context;

        public AccountController(UserManager<Gebruiker> userManager, SignInManager<Gebruiker> signInManager, StartspelerContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: api/Gebruikers
        // Geeft een lijst terug van alle geregistreerde gebruikers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gebruiker>>> GetGebruikers()
        {
            if (_context.Gebruikers == null)
            {
                return NotFound();
            }
            return await _context.Gebruikers.ToListAsync();
        }

        // In de Register-methode kan een gebruiker aangemaakt worden met het opgegeven gebruikersnaam, email en wachtwoord.
        // Als de registratie succesvol is, retourneer een Ok-respons.
        // Als er fouten optreden, retourneer een BadRequest-respons met de foutmeldingen.
        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string email, string password, string voornaam, string achternaam)
        {
            var user = new Gebruiker
            {
                UserName = username,
                Email = email,
                Voornaam = voornaam,
                Achternaam = achternaam
            };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Rol 'Klant' toewijzen na succesvolle registratie
            var rolResult = await _userManager.AddToRoleAsync(user, "Klant");
            if (!rolResult.Succeeded)
            {
                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    // Als het verwijderen van de gebruiker mislukt, geef de foutmeldingen van dat proces
                    return BadRequest(deleteResult.Errors);
                }
                // Geef foutmeldingen van het rol toewijzen terug
                return BadRequest(rolResult.Errors);
            }

            return Ok(user);
        }

        // In de Login-methode wordt de gebruiker opgezocht op basis van de opgegeven gebruikersnaam.
        // Als de gebruiker wordt gevonden, controleer dan of het opgegeven wachtwoord overeenkomt.
        // Als het wachtwoord correct is, log de gebruiker in met behulp van SignInManager.
        // Retourneer een geschikte respons (bijvoorbeeld Ok of Unauthorized) op basis van het inlogresultaat.
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Unauthorized("Gebruiker niet gevonden");
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (signInResult.Succeeded)
            {
                // Maak een Gebruiker object met de benodigde gegevens
                var gebruiker = new Gebruiker
                {
                    Id = user.Id,
                    Email = user.Email
                };

                return Ok(gebruiker); // Retourneer de gebruikersgegevens in JSON-formaat
            }

            return Unauthorized("Ongeldige inloggegevens");
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("U bent uitgelogd");
        }

        // GET: api/Gebruikers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gebruiker>> GetGebruiker(string id)
        {
            if (_context.Gebruikers == null)
            {
                return NotFound();
            }
            var gebruiker = await _context.Gebruikers.FindAsync(id);

            if (gebruiker == null)
            {
                return NotFound();
            }

            return gebruiker;
        }

        // PUT: api/Gebruikers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGebruiker(string id, [FromBody] Gebruiker gebruikerUpdate)
        {
            if (id != gebruikerUpdate.Id)
            {
                return BadRequest("Gebruiker ID komt niet overeen");
            }

            var gebruiker = await _userManager.FindByIdAsync(id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            // Update properties using UserManager
            var setEmailResult = await _userManager.SetEmailAsync(gebruiker, gebruikerUpdate.Email);
            var setUserNameResult = await _userManager.SetUserNameAsync(gebruiker, gebruikerUpdate.UserName);

            if (!setEmailResult.Succeeded || !setUserNameResult.Succeeded)
            {
                return BadRequest(setEmailResult.Errors.Concat(setUserNameResult.Errors));
            }

            gebruiker.Voornaam = gebruikerUpdate.Voornaam;
            gebruiker.Achternaam = gebruikerUpdate.Achternaam;

            var result = await _userManager.UpdateAsync(gebruiker);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}
