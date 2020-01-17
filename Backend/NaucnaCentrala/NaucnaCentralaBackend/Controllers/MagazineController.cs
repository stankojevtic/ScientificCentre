using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaucnaCentralaBackend.DataContextNamespace;
using NaucnaCentralaBackend.Interfaces;
using NaucnaCentralaBackend.Models.API;
using NaucnaCentralaBackend.Models.Camunda;
using NaucnaCentralaBackend.Models.Database;
using Newtonsoft.Json;

namespace NaucnaCentralaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MagazineController : ControllerBase
    {
        private readonly ICamundaExecutor _camundaExecutor;
        private readonly IEmailSender _emailSender;
        private readonly DataContext _dataContext;

        public MagazineController(ICamundaExecutor camundaService, IEmailSender emailService, DataContext dbContext)
        {
            _camundaExecutor = camundaService;
            _emailSender = emailService;
            _dataContext = dbContext;
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddMagazineModel addMagazineModel)
        {
            try
            {
                _camundaExecutor.StartProcess("ProcesKreiranjaNovogCasopisa", User.Identity.Name);

                string taskId = _camundaExecutor.GetUnassignedTaskId("UnosenjePodatakaZaCasopis");

                dynamic content = new
                {
                    Name = new CamundaValueItem<string>(addMagazineModel.Name),
                    ISSN = new CamundaValueItem<string>(addMagazineModel.ISSN),
                    IsOpenAccess = new CamundaValueItem<bool>(addMagazineModel.IsOpenAccess),
                    ScientificAreas = new CamundaValueItem<string>(string.Join(",", addMagazineModel.ScientificAreas.Select(x => x.Name)))
                };

                bool actionSucceeded = _camundaExecutor.SubmitTaskForm(taskId, JsonConvert.SerializeObject(content));

                if (actionSucceeded)
                {
                    _dataContext.Magazines.Add(new Magazine
                    {
                        ScientificAreas = string.Join(",", addMagazineModel.ScientificAreas.Select(x => x.Name)),
                        ISSN = addMagazineModel.ISSN,
                        ChiefEditor = User.Identity.Name,
                        IsActive = false,
                        IsOpenAccess = addMagazineModel.IsOpenAccess,
                        Name = addMagazineModel.Name
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
    }
}