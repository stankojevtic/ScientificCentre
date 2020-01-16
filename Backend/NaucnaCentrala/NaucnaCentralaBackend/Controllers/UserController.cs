using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NaucnaCentralaBackend.DataContextNamespace;
using NaucnaCentralaBackend.Interfaces;
using NaucnaCentralaBackend.Models.API;
using NaucnaCentralaBackend.Models.Camunda;
using NaucnaCentralaBackend.Models.Database;
using NaucnaCentralaBackend.Models.Enums;
using Newtonsoft.Json;

namespace NaucnaCentralaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICamundaExecutor _camundaExecutor;
        private readonly IEmailSender _emailSender;
        private readonly DataContext _dataContext;
        private readonly string _jwtSecret = "2124hj421gh214f241";

        public UserController(ICamundaExecutor camundaService, IEmailSender emailService, DataContext dbContext)
        {
            _camundaExecutor = camundaService;
            _emailSender = emailService;
            _dataContext = dbContext;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] CreateUserModel createUserModel)
        {
            try
            {
                _camundaExecutor.StartProcess("RegistrationProcess", null);

                string taskId = _camundaExecutor.GetUnassignedTaskId("UnosPodataka");

                dynamic content = new
                {
                    Username = new CamundaValueItem<string>(createUserModel.Username),
                    Password = new CamundaValueItem<string>(createUserModel.Password),
                    Firstname = new CamundaValueItem<string>(createUserModel.Firstname),
                    Lastname = new CamundaValueItem<string>(createUserModel.Lastname),
                    City = new CamundaValueItem<string>(createUserModel.City),
                    Country = new CamundaValueItem<string>(createUserModel.Country),
                    Vocation = new CamundaValueItem<string>(createUserModel.Vocation),
                    IsReviewer = new CamundaValueItem<bool>(createUserModel.IsReviewer),
                    ScientificAreas = new CamundaValueItem<string>(string.Join(",", createUserModel.ScientificAreas.Select(x => x.Name)))
                };

                bool actionSucceeded = _camundaExecutor.SubmitTaskForm(taskId, JsonConvert.SerializeObject(content));

                if (actionSucceeded)
                {
                    _dataContext.Users.Add(new User
                    {
                        ScientificAreas = string.Join(",", createUserModel.ScientificAreas.Select(x => x.Name)),
                        City = createUserModel.City,
                        Username = createUserModel.Username,
                        Lastname = createUserModel.Lastname,
                        Firstname = createUserModel.Firstname,
                        Password = createUserModel.Password,
                        Token = Guid.NewGuid().ToString("n"),
                        Country = createUserModel.Country,
                        IsReviewer = createUserModel.IsReviewer,
                        Vocation = createUserModel.Vocation,
                        Role = createUserModel.IsReviewer ? UserRoles.Reviewer.ToString() : UserRoles.User.ToString()
                    });
                    _dataContext.SaveChanges();
                }

                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("login")]
        public IActionResult Authenticate([FromBody]AuthenticationModel authenticationModel)
        {
            var user = _dataContext.Users.FirstOrDefault
                (x => x.Username == authenticationModel.Username && x.Password == authenticationModel.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }

        [Route("verify")]
        [HttpGet]
        public IActionResult VerifyEmail([FromQuery] string token)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.Token == token);

            if (user == null)
                return BadRequest();

            user.EmailConfirmed = true;
            _dataContext.SaveChanges();

            var taskId = _camundaExecutor.GetUnassignedTaskId("PotvrdaEmaila");

            dynamic content = new { };
            var actionSucceeded = _camundaExecutor.CompleteTask(taskId, JsonConvert.SerializeObject(content));
            if (actionSucceeded)
                return Ok("Mail verified successfully.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route("role")]
        [HttpGet]
        public IActionResult GetRole()
        {
            var username = User.Identity.Name;
            var role = _dataContext.Users.FirstOrDefault(x => x.Username == username)?.Role;
            return Ok(role);
        }

        [Route("getReviewersWaiting")]
        [HttpGet]
        public IActionResult GetReviewersWaiting()
        {
            var taskId = _camundaExecutor.GetAssignedTaskId("PotvrdaRecenzenta", "administrator");

            var users = _dataContext.Users.Where(x => x.AdminConfirmed == false && x.IsReviewer == true && x.EmailConfirmed == true);
            return Ok(users);
        }

        [Route("editors/{id}")]
        [HttpGet]
        public IActionResult GetEditors(int id)
        {
            var magazineScientificAreas = _dataContext.Magazines.FirstOrDefault(x => x.Id == id).ScientificAreas.Split(",");
            var users = _dataContext.Users.ToList();

            var filteredUsers = users.Where(
                            x => x.AdminConfirmed == true
                            && x.Role == UserRoles.Editor.ToString()
                            && x.Username != User.Identity.Name
                            && x.ScientificAreas.Split(",", StringSplitOptions.None).Select(y => y).Intersect(magazineScientificAreas).Any());

            return Ok(filteredUsers.Select(x => x.Username).ToList());
        }

        [Route("reviewers/{id}")]
        [HttpGet]
        public IActionResult GetReviewers(int id)
        {
            var magazineScientificAreas = _dataContext.Magazines.FirstOrDefault(x => x.Id == id).ScientificAreas.Split(",");
            var users = _dataContext.Users.ToList();
            var filteredUsers = users.Where(
                x => x.AdminConfirmed == true 
                && x.Role == UserRoles.Reviewer.ToString()
                && x.ScientificAreas.Split(",", StringSplitOptions.None).Select(y => y).Intersect(magazineScientificAreas).Any());

            return Ok(filteredUsers.Select(x => x.Username).ToList());
        }

        [Route("approve/{id}")]
        [HttpPost]
        public IActionResult ApproveReviewer(int id)
        {
            var taskId = _camundaExecutor.GetAssignedTaskId("PotvrdaRecenzenta", "administrator");

            dynamic content = new { };
            var actionSucceeded = _camundaExecutor.CompleteTask(taskId, JsonConvert.SerializeObject(content));

            return Ok();
        }
    }
}
