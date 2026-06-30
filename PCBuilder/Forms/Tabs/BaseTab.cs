using System.Windows.Forms;
using PCBuilder.Services;

namespace PCBuilder.Forms.Tabs
{
    public abstract class BaseTab
    {
        protected IDataManager dataManager;
        protected IBuildManager buildManager;

        public BaseTab(IDataManager dataManager, IBuildManager buildManager = null)
        {
            this.dataManager = dataManager;
            this.buildManager = buildManager;
        }

        public abstract Control Create();
        public abstract string TabName { get; }
        public virtual void Refresh() { }
        public virtual void OnDataUpdated() { }
    }
}