namespace Noglin.Core.Types
{
    public enum RepositoryService
    {
        GitLab,
    }
    
    public struct RepositoryInfo
    {
        public RepositoryService Service;
        public string Name;
        public bool IssueTracker;
    }
}