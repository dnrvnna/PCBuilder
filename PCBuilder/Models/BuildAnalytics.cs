using System.Collections.Generic;
using System.Linq;

namespace PCBuilder.Models
{
    public class BuildAnalytics
    {
        public List<Build> Builds { get; set; } = new List<Build>();

        public Build CheapestBuild => Builds.Where(b => b.IsComplete)
            .OrderBy(b => b.TotalPrice)
            .FirstOrDefault();

        public Build MostExpensiveBuild => Builds.Where(b => b.IsComplete)
            .OrderByDescending(b => b.TotalPrice)
            .FirstOrDefault();

        public Build MostComponentsBuild => Builds.Where(b => b.IsComplete)
            .OrderByDescending(b => b.ComponentCount)
            .FirstOrDefault();

        public Build BestPricePerComponent => Builds.Where(b => b.IsComplete && b.ComponentCount > 0)
            .OrderBy(b => b.TotalPrice / b.ComponentCount)
            .FirstOrDefault();

        public double AverageBuildPrice => Builds.Where(b => b.IsComplete)
            .Average(b => (double)b.TotalPrice);

        public int TotalBuilds => Builds.Count;
        public int CompleteBuilds => Builds.Count(b => b.IsComplete);

        public Dictionary<string, int> MostPopularComponents
        {
            get
            {
                return Builds
                    .SelectMany(b => b.Components)
                    .GroupBy(c => c.Id)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .ToDictionary(g => g.First().Name, g => g.Count());
            }
        }

        public Dictionary<string, int> MostPopularCategories
        {
            get
            {
                return Builds
                    .SelectMany(b => b.Components)
                    .GroupBy(c => c.Category)
                    .OrderByDescending(g => g.Count())
                    .ToDictionary(g => g.Key, g => g.Count());
            }
        }
    }
}