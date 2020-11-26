using System.Collections.Generic;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        private readonly List<RkModule> _modules = new();

        protected void AddModule<T>(T module)
            where T : RkModule
        {
            _modules.Add(module);
        }
        
        public T GetModuleInstance<T>()
            where T : RkModule
        {
            foreach (RkModule module in _modules)
            {
                if (module is T rkModule)
                {
                    return rkModule;
                }
            }

            return null;
        }
        
        public IEnumerable<T> GetModuleInstances<T>()
            where T : RkModule
        {
            foreach (RkModule module in _modules)
            {
                if (module is T rkModule)
                {
                    yield return rkModule;
                }
            }
        }
    }
}