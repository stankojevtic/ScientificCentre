using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NaucnaCentralaBackend.DataContextNamespace;
using NaucnaCentralaBackend.Interfaces;
using NaucnaCentralaBackend.Models.Camunda;
using Newtonsoft.Json;

namespace NaucnaCentralaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminTaskController : ControllerBase
    {
        private readonly ICamundaExecutor _camundaExecutor;
        private readonly DataContext _dataContext;

        public AdminTaskController(ICamundaExecutor camundaService, DataContext dbContext)
        {
            _camundaExecutor = camundaService;
            _dataContext = dbContext;
        }

        [Route("decline/{id}")]
        [HttpPost]
        public IActionResult DeclineMagazine(int id)
        {
            var taskId = _camundaExecutor.GetAssignedTaskId("AdminProveravaPodatke", "administrator");

            dynamic content = new
            {
                PodaciValidni = new CamundaValueItem<bool>(false)
            };
            var actionSucceeded = _camundaExecutor.CompleteTask(taskId, JsonConvert.SerializeObject(content));

            if (actionSucceeded)
            {
                var magazine = _dataContext.Magazines.FirstOrDefault(x => x.Id == id);
                magazine.AdminReviewed = true;
                magazine.DataValid = false;
                _dataContext.SaveChanges();
            }

            return Ok();
        }

        [Route("approve/{id}")]
        [HttpPost]
        public IActionResult ApproveMagazine(int id)
        {
            var taskId = _camundaExecutor.GetAssignedTaskId("AdminProveravaPodatke", "administrator");

            dynamic content = new
            {
                PodaciValidni = new CamundaValueItem<bool>(true)
            };
            var actionSucceeded = _camundaExecutor.CompleteTask(taskId, JsonConvert.SerializeObject(content));

            if (actionSucceeded)
            {
                var magazine = _dataContext.Magazines.FirstOrDefault(x => x.Id == id);
                magazine.AdminReviewed = true;
                magazine.DataValid = true;
                _dataContext.SaveChanges();
            }

            return Ok();
        }

        [Route("getMagazinesWaitingForApproval")]
        [HttpGet]
        public IActionResult GetMagazinesWaitingForApproval()
        {
            var taskId = _camundaExecutor.GetAssignedTaskId("AdminProveravaPodatke", "administrator");

            var magazines = _dataContext.Magazines.Where(x => x.AdminReviewed == false);
            return Ok(magazines);
        }

        [Route("getMagazinesForCorrection")]
        [HttpGet]
        public IActionResult GetMagazinesForCorrection()
        {
            var taskId = _camundaExecutor.GetAssignedTaskId("GlavniUrednikKorigujePodatke", "urednik");

            var magazines = _dataContext.Magazines.Where(x => x.AdminReviewed == true && x.DataValid == false);
            return Ok(magazines);
        }
    }
}