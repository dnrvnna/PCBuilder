using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PCBuilder.Models;

namespace PCBuilder.Services
{
    public class BuildStorage : IBuildStorage
    {
        private static readonly string StoragePath = Path.Combine(Application.StartupPath, "data", "builds.json");
        private List<Build> builds = new List<Build>();

        public List<Build> Builds => builds;

        public event EventHandler BuildsLoaded;
        public event EventHandler BuildsSaved;
        public event EventHandler<BuildEventArgs> BuildAdded;
        public event EventHandler<BuildEventArgs> BuildDeleted;

        public BuildStorage()
        {
            LoadBuilds();
        }

        public void LoadBuilds()
        {
            try
            {
                if (!File.Exists(StoragePath))
                {
                    builds = new List<Build>();
                    SaveBuilds();
                    return;
                }

                string json = File.ReadAllText(StoragePath);
                builds = JsonConvert.DeserializeObject<List<Build>>(json) ?? new List<Build>();
                BuildsLoaded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки сборок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                builds = new List<Build>();
            }
        }

        public void SaveBuilds()
        {
            try
            {
                string directory = Path.GetDirectoryName(StoragePath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string json = JsonConvert.SerializeObject(builds, Formatting.Indented);
                File.WriteAllText(StoragePath, json);
                BuildsSaved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения сборок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddBuild(Build build)
        {
            builds.Add(build);
            SaveBuilds();
            BuildAdded?.Invoke(this, new BuildEventArgs(build));
        }

        public void DeleteBuild(string buildId)
        {
            var build = builds.FirstOrDefault(b => b.Id == buildId);
            if (build != null)
            {
                builds.Remove(build);
                SaveBuilds();
                BuildDeleted?.Invoke(this, new BuildEventArgs(build));
            }
        }

        public Build GetBuildById(string id)
        {
            return builds.FirstOrDefault(b => b.Id == id);
        }
    }
}