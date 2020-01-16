namespace NaucnaCentralaBackend.Interfaces
{
    public interface ICamundaExternalTaskSync
    {
        void StartRegistrationDataCheck();
        void StartEmailSendingCheck();
        void StartActivateUserCheck();
        void StartActivationReviewerCheck();
        void StartActivateChiefEditor();
        void StartAddEditorsAndReviewersCheck();
        void StartMagazineActivationCheck();
        void StartMagazineCorrectionDataCheck();
    }
}
