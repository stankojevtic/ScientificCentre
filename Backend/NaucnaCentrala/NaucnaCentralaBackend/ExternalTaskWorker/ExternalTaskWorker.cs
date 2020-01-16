using NaucnaCentralaBackend.Interfaces;

namespace NaucnaCentralaBackend.ExternalTaskWorkerNamespace
{
    public class ExternalTaskWorker : IExternalTaskWorker
    {

        private readonly ICamundaExternalTaskSync _camundaExternalTaskSync;

        public ExternalTaskWorker(ICamundaExternalTaskSync camundaExternalTaskSync)
        {
            _camundaExternalTaskSync = camundaExternalTaskSync;
        }

        public void Start()
        {
            _camundaExternalTaskSync.StartRegistrationDataCheck();
            _camundaExternalTaskSync.StartEmailSendingCheck();
            _camundaExternalTaskSync.StartActivateUserCheck();
            _camundaExternalTaskSync.StartActivationReviewerCheck();
            _camundaExternalTaskSync.StartActivateChiefEditor();
            _camundaExternalTaskSync.StartAddEditorsAndReviewersCheck();
            _camundaExternalTaskSync.StartMagazineActivationCheck();
            _camundaExternalTaskSync.StartMagazineCorrectionDataCheck();
        }
    }
}
