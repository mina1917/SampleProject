namespace SampleProject.Framework.Contracts
{
    public interface IEntity
    {
        Guid Id
        {
            get;
        }

        byte[] RowVersion
        {
            get;
        }
    }
}
