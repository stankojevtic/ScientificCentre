namespace NaucnaCentralaBackend.Models.Camunda
{
    public class CamundaValueItem<T>
    {
        public CamundaValueItem(T value)
        {
            this.value = value;
        }

        public T value { get; set; }
    }
}
