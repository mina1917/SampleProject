namespace SampleProject.Framework.Contracts
{
    public interface ISoftDeletable<TUserId>
        where TUserId : struct, IComparable, IComparable<TUserId>
    {
        DateTime? DeletedOn { get; }
        TUserId? Deleter { get; }
    }
}
