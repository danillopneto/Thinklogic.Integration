namespace Thinklogic.Integration.Domain.Asana
{
    public abstract class AsanaResponse<T>
    {
        public T Data { get; set; }
    }
}
