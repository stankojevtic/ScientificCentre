using System;
using System.Collections.Generic;
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
    public class EditorTaskController : ControllerBase
    {
        private readonly ICamundaExecutor _camundaExecutor;
        private readonly DataContext _dataContext;

        public EditorTaskController(ICamundaExecutor camundaService, DataContext dbContext)
        {
            _camundaExecutor = camundaService;
            _dataContext = dbContext;
        }

        [Route("correct/{id}")]
        [HttpPost]
        public IActionResult Correct(int id, [FromBody] EditMagazineInformationModel editMagazineInformationModel)
        {
            try
            {
                string taskId = _camundaExecutor.GetUnassignedTaskId("GlavniUrednikKorigujePodatke");

                dynamic content = new
                {
                    Name = new CamundaValueItem<string>(editMagazineInformationModel.Name),
                    ISSN = new CamundaValueItem<string>(editMagazineInformationModel.ISSN),
                    IsOpenAccess = new CamundaValueItem<bool>(editMagazineInformationModel.IsOpenAccess),
                    ScientificAreas = new CamundaValueItem<string>(string.Join(",", editMagazineInformationModel.ScientificAreas.Select(x => x.Name)))
                };

                bool actionSucceeded = _camundaExecutor.SubmitTaskForm(taskId, JsonConvert.SerializeObject(content));

                if (actionSucceeded)
                {
                    var magazine = _dataContext.Magazines.FirstOrDefault(x => x.Id == id);
                    magazine.ISSN = editMagazineInformationModel.ISSN;
                    magazine.IsOpenAccess = editMagazineInformationModel.IsOpenAccess;
                    magazine.Name = editMagazineInformationModel.Name;
                    magazine.ScientificAreas = string.Join(",", editMagazineInformationModel.ScientificAreas.Select(x => x.Name));

                    _dataContext.SaveChanges();
                }

                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("getAddingReviewersTask")]
        [HttpGet]
        public IActionResult GetAddingReviewersTask()
        {
            var taskId = _camundaExecutor.GetAssignedTaskId("DodavanjeUrednikaiRecenzenataCasopisa", "administrator");

            var activeTasks = _camundaExecutor.GetAllActiveTasks("DodavanjeUrednikaiRecenzenataCasopisa");

            var magazines = GetActiveTasksForUser(taskId, activeTasks);
            
            return Ok(magazines);
        }

        [Route("addingReviewersForMagazin/{id}")]
        [HttpPost]
        public IActionResult AddEditorsAndReviewers(int id, [FromBody]InsertEditorsAndReviewersForMagazineModel model)
        {
            try
            {
                var task = _camundaExecutor.GetAssignedTaskId("DodavanjeUrednikaiRecenzenataCasopisa", "administrator");
                var activeTasks = _camundaExecutor.GetAllActiveTasks("DodavanjeUrednikaiRecenzenataCasopisa");
                var taskId = _camundaExecutor.GetUnassignedTaskId("DodavanjeUrednikaiRecenzenataCasopisa");


                dynamic content = new
                {
                    Urednici = new CamundaValueItem<string>(string.Join(",", model.Editors.Select(x => x.EditorName))),
                    Recenzenti = new CamundaValueItem<string>(string.Join(",", model.Reviewers.Select(x => x.ReviewerName)))
                };

                bool actionSucceeded = _camundaExecutor.SubmitTaskForm(taskId, JsonConvert.SerializeObject(content));

                if (actionSucceeded)
                {
                    var magazine = _dataContext.Magazines.FirstOrDefault(x => x.Id == id);
                    magazine.Editors = string.Join(",", model.Editors.Select(x => x.EditorName));
                    magazine.Reviewers = string.Join(",", model.Reviewers.Select(x => x.ReviewerName));
                    _dataContext.SaveChanges();
                }

                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private List<Magazine> GetActiveTasksForUser(string taskId, bool activeTasks)
        {
            var magazines = _dataContext.Magazines;
            var magazinesWaitingForAddingReviewers = magazines.Where(x => x.Editors == null && x.Reviewers == null);
            return magazinesWaitingForAddingReviewers.ToList();
        }
    }
}