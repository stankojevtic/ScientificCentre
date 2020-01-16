using NaucnaCentralaBackend.Models.ExternalTask;

namespace NaucnaCentralaBackend.Interfaces
{
    public interface ICamundaExecutor
    {
        bool CompleteTask(string taskId, string content);
        string GetUnassignedTaskId(string taskDefinitionKey);
        ExternalTaskResponse FetchAndLockExternalTask(string workerId, string topicName, string[] variables);
        bool CompleteExternalTask(string taskId, string workerId, string content);
        bool SubmitTaskForm(string taskId, string content);
        void StartProcess(string processId, long? userId);
        string GetAssignedTaskId(string taskDefinitionKey, string userId);
        bool GetAllActiveTasks(string v);
    }
}
