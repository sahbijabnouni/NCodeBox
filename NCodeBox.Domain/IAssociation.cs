namespace NCodeBox.Domain
{
    public interface IAssociation<TId, TP, T> : IIdentity<TId>
        where TP : IEntity<TId>
        where T : IEntity<TId>
    {
      
    }
}