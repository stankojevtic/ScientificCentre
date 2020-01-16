using NaucnaCentralaBackend.DataContextNamespace;
using NaucnaCentralaBackend.Interfaces;
using NaucnaCentralaBackend.Models.Camunda;
using NaucnaCentralaBackend.Models.ExternalTask;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NaucnaCentralaBackend.CamundaExternalTaskSyncNamespace
{
    public class CamundaExternalTaskSync : ICamundaExternalTaskSync
    {
        private readonly ICamundaExecutor _camundaExecutor;
        private readonly IEmailSender _emailSender;
        private readonly DataContext _dataContext;
        private readonly string _workerId = "worker";

        public CamundaExternalTaskSync(ICamundaExecutor camundaService, IEmailSender emailService, DataContext dbContext)
        {
            _camundaExecutor = camundaService;
            _emailSender = emailService;
            _dataContext = dbContext;
        }

        public void StartMagazineCorrectionDataCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    MagazineCorrectionDataCheck();
                }
            });

            task.Start();
        }

        public void StartMagazineActivationCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    MagazineActivationCheck();
                }
            });

            task.Start();
        }

        public void StartAddEditorsAndReviewersCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    AddEditorsAndReviewersCheck();
                }
            });

            task.Start();
        }

        public void StartActivateChiefEditor()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    ActivateChiefEditor();
                }
            });

            task.Start();
        }

        public void StartActivationReviewerCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    ActivationReviewerCheck();
                }
            });

            task.Start();
        }

        public void StartActivateUserCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    ActivateUserCheck();
                }
            });

            task.Start();
        }

        public void StartEmailSendingCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    EmailSendingCheck();
                }
            });

            task.Start();
        }

        public void StartRegistrationDataCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    RegistrationDataCheck();
                }
            });

            task.Start();
        }

        private void ActivateChiefEditor()
        {
            try
            {
                ExternalTaskResponse response = _camundaExecutor.FetchAndLockExternalTask(_workerId, "DodelaGlavnogUrednikaInicijatoru", new[] { "ISSN" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var ISSN = (string)response.ResponseData.variables["ISSN"].value;
                var magazine = _dataContext.Magazines.FirstOrDefault(x => x.ISSN == ISSN);
                _dataContext.SaveChanges();


                dynamic content = new { };
                _camundaExecutor.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception exception)
            {
                return;
            }
        }

        private void ActivateUserCheck()
        {
            try
            {
                ExternalTaskResponse response = _camundaExecutor.FetchAndLockExternalTask(_workerId, "SistemAktiviraKorisnika", new[] { "Username" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var username = (string)response.ResponseData.variables["Username"].value;
                var user = _dataContext.Users.FirstOrDefault(x => x.Username == username);
                user.IsActive = true;
                _dataContext.SaveChanges();


                dynamic content = new { };
                _camundaExecutor.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception exception)
            {
                return;
            }
        }

        private void EmailSendingCheck()
        {
            try
            {
                ExternalTaskResponse response = _camundaExecutor.FetchAndLockExternalTask(_workerId, "SistemSaljeMejl", new[] { "Username" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var username = (string)response.ResponseData.variables["Username"].value;
                var userToken = _dataContext.Users.FirstOrDefault(x => x.Username == username).Token;

                _emailSender.Send("Confirm email", "Please verify your email by clicking on this link: <a href=\"https://localhost:44372/api/user/verify?token=" + userToken + "\">here</a>");

                _camundaExecutor.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(new { }));
            }
            catch (Exception exception)
            {
                return;
            }
        }

        private void RegistrationDataCheck()
        {
            try
            {
                ExternalTaskResponse response = _camundaExecutor.FetchAndLockExternalTask(_workerId, "RegistrationDataValidation", new[] { "Username", "Password" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                dynamic content = new { PodaciValidni = new CamundaValueItem<bool>(true) };
                _camundaExecutor.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch
            {
                return;
            }
        }

        private void ActivationReviewerCheck()
        {
            try
            {
                ExternalTaskResponse response = _camundaExecutor.FetchAndLockExternalTask(_workerId, "AktivacijaRecenzenta", new[] { "Username" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var username = (string)response.ResponseData.variables["Username"].value;
                var user = _dataContext.Users.FirstOrDefault(x => x.Username == username);
                user.AdminConfirmed = true;
                _dataContext.SaveChanges();


                dynamic content = new { };
                _camundaExecutor.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception exception)
            {
                return;
            }
        }

        private void AddEditorsAndReviewersCheck()
        {
            try
            {
                ExternalTaskResponse response = _camundaExecutor.FetchAndLockExternalTask(_workerId, "ObradaUrednikaiRecenzenta", new[] { "Username" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }
                dynamic content = new { };
                _camundaExecutor.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception exception)
            {
                return;
            }
        }

        private void MagazineActivationCheck()
        {
            try
            {
                ExternalTaskResponse response = _camundaExecutor.FetchAndLockExternalTask(_workerId, "AktivacijaCasopisa", new[] { "Name" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var magazineName = (string)response.ResponseData.variables["Name"].value;
                var magazine = _dataContext.Magazines.FirstOrDefault(x => x.Name == magazineName);
                magazine.IsActive = true;
                _dataContext.SaveChanges();


                dynamic content = new { };
                _camundaExecutor.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception exception)
            {
                return;
            }
        }

        private void MagazineCorrectionDataCheck()
        {
            try
            {
                ExternalTaskResponse response = _camundaExecutor.FetchAndLockExternalTask(_workerId, "UrednikKorigovanjePodataka", new[] { "Name" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var magazineName = (string)response.ResponseData.variables["Name"].value;

                dynamic content = new { };
                _camundaExecutor.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception exception)
            {
                return;
            }
        }
    }
}
