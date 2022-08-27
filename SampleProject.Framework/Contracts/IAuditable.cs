namespace SampleProject.Framework.Contracts
{
    public interface IAuditable<TUserId>
        where TUserId : struct, IComparable, IComparable<TUserId>
    {
        DateTime CreatedOn { get; }
        DateTime? ModifiedOn { get; }

        TUserId Creator { get; }
        TUserId? Modifire { get; }
    }
}
