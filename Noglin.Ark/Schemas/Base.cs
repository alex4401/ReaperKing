namespace Noglin.Ark.Schemas
{
    public abstract record PackageSchema
    {
        public string ApiVersion { get; init; }
    }
}