namespace ReaperKing.Core
{
    public interface IRkModule
    {
        public void AcceptConfiguration(ProjectConfigurationManager config);
    }
    
    public interface IRkDocumentProcessorModule : IRkModule
    {
        public void PostProcessDocument(string uri, ref IntermediateGenerationResult result);
    }

    public interface IRkResourceProcessorModule : IRkModule
    {
        public void ProcessResource(string filePath, ref string diskPath, ref string uri);
    }

    public interface IRkResourceResolverModule : IRkModule
    {
        public bool CanAccept(string ns, string virtualPath);
        public bool ResolveResource(string ns, string virtualPath, out string result);
    }
}