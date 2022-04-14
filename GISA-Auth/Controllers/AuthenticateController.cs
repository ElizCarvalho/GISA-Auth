using GISA_Auth.Auth;
using GISA_Auth.Auth.Model;
using GISA_Auth.Auth.Model.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GISA_Auth.Controllers
{
    [Route("account")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userUpdate = (ApplicationUser)user;
                userUpdate.Latitude = model.Latitude;
                userUpdate.Longitude = model.Longitude;
                userUpdate.LastAccess = DateTime.Now;
                var result = await _userManager.UpdateAsync(userUpdate);
                var userRoles = await _userManager.GetRolesAsync(user);
                var token = GetToken(GenClaim(user, userRoles));

                return Ok(new ResponseLogin
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    User = new UserResponse { Role = GetRoleUserResponse(userRoles.First()), Username = user.UserName }
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = StatusCategory.Error, Message = MessageCategory.UserAlreadyExists.Value });

            ApplicationUser user = new()
            {
                Email = model.Email?.Trim().ToLower(),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email?.Trim().ToUpper().Split('@')[0],
                DateCreated = DateTime.Now,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = StatusCategory.Error, Message = MessageCategory.UserCreationFailed.Value });

            await CheckAndCreateRole();
            await AddUserToRole(model.Role, user);

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = GetToken(GenClaim(user, userRoles));

            return Ok(new ResponseLogin
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                User = new UserResponse { Role = GetRoleUserResponse(userRoles.First()), Username = user.UserName }
            });
        }

        [HttpGet("userinfo")]
        public async Task<ActionResult> GetUserInfo()
        {
            HttpClient client = new HttpClient();
            string token = HttpContext.Request.Headers.First(x => x.Key == "access_token").Value;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var user = await _userManager.FindByNameAsync(jwtSecurityToken.Claims.First(c => c.Type.Contains("name")).Value);
            var role = jwtSecurityToken.Claims.First(c => c.Type.Contains("role")).Value;

            return Ok(new UserResponse
            {
                Role = GetRoleUserResponse(role),
                Username = user.UserName
            });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(int.Parse(_configuration["JWT:Expiration"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private int GetRoleUserResponse(string userRole)
        {
            int role;
            switch (userRole)
            {
                case Role.Admin:
                    role = (int)EnumUserRoles.Admin;
                    break;
                case Role.Prestador:
                    role = (int)EnumUserRoles.Prestador; ;
                    break;
                default:
                    role = (int)EnumUserRoles.Associado; ;
                    break;
            }
            return role;
        }

        private static List<Claim> GenClaim(ApplicationUser user, IList<string> userRoles)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return authClaims;
        }

        private async Task CheckAndCreateRole()
        {
            if (!await _roleManager.RoleExistsAsync(EnumUserRoles.Admin.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(EnumUserRoles.Admin.ToString()));
            if (!await _roleManager.RoleExistsAsync(EnumUserRoles.Associado.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(EnumUserRoles.Associado.ToString()));
            if (!await _roleManager.RoleExistsAsync(EnumUserRoles.Prestador.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(EnumUserRoles.Prestador.ToString()));
        }

        private async Task AddUserToRole(int modelRole, ApplicationUser user)
        {
            switch (modelRole)
            {
                case 1:
                    await _userManager.AddToRoleAsync(user, EnumUserRoles.Admin.ToString());
                    break;
                case 2:
                    await _userManager.AddToRoleAsync(user, EnumUserRoles.Associado.ToString());
                    break;
                case 3:
                    await _userManager.AddToRoleAsync(user, EnumUserRoles.Prestador.ToString());
                    break;
                default:
                    await _userManager.AddToRoleAsync(user, EnumUserRoles.Associado.ToString());
                    break;
            }
        }
    }
}
