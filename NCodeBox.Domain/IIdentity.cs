namespace NCodeBox.Domain
{
    public interface IIdentity<T>
    {
        T Id { get; set; }
    }
}
 