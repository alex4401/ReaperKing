namespace ReaperKing.Core
{
    public abstract class RkModule
    {
        protected Site Site { get; }

        public RkModule(Site site)
        {
            Site = site;
        }
    }
}