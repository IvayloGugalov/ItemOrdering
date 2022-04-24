namespace GuidGenerator
{
    public class GuidGeneratorService : IGuidGeneratorService
    {
        public Guid GenerateGuid() => Guid.NewGuid();
    }
}
