using System.Collections.Generic;
using PCBuilder.Models;

namespace PCBuilder.Services
{
    public interface IBuildStorage
    {
        List<Build> Builds { get; }
        void LoadBuilds();
        void SaveBuilds();
        void AddBuild(Build build);
        void DeleteBuild(string buildId);
        Build GetBuildById(string id);
        event System.EventHandler BuildsLoaded;
        event System.EventHandler BuildsSaved;
        event System.EventHandler<BuildEventArgs> BuildAdded;
        event System.EventHandler<BuildEventArgs> BuildDeleted;
    }

    public class BuildEventArgs : System.EventArgs
    {
        public Build Build { get; }
        public BuildEventArgs(Build build) => Build = build;
    }
}