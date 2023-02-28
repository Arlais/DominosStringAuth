using Microsoft.AspNetCore.Mvc;
using Proyecto_Inalambria.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Proyecto_Inalambria.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DominoController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<DominoController> _logger;

        public DominoController(ILogger<DominoController> logger)
        {
            _logger = logger;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if (ModelState.IsValid)
            {
                // Validate the user's credentials
                if (user.Username == "Admin" && user.Password=="Admin")
                {
                    // Create claims for the authenticated user
                    var claims = new List<Claim>{
                        new Claim(ClaimTypes.Name, user.Username)
                    };

                    // Create the authentication cookie
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                        authProperties);

                    // Return a success response
                    return Ok();
                }
            }

            // If we got this far, the user's credentials were invalid
            return Unauthorized();
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect the user to the home page
            return Ok();
        }

        [HttpPost]
        [Route("Solution")]
        public IActionResult Get([FromBody] List<DominoPiece> prueba)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            else
            {
                var res = DominoService.ConstruirCadena(prueba.Select(objeto => (objeto.SideOne, objeto.SideTwo)).ToList());
                if (res == null) return Ok(new List<DominoPiece>() {
                new DominoPiece(){SideOne=-1, SideTwo=-1}
            });
                return Ok(res.Select(objeto => new DominoPiece { SideOne = objeto.Item1, SideTwo = objeto.Item2 }).ToList());
            }
            
        }
    }

}