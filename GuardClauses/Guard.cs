namespace GuardClauses
{
    public interface IGuardClause
    {
    }

    // TODO: Extract into a NuGet
    public class Guard : IGuardClause
    {
        public static IGuardClause Against { get; } = new Guard();
    }
}
