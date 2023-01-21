using System.Text.Json;
using System.Text.Json.Serialization;

namespace CyrilGame.Core.Projects
{
    public class ProjectCollection
    {
        private string ProjectCollectionFile = Path.Combine( "projects", "project-collection.json" );
        public bool IsBrandNewProject()
        {
            return !File.Exists( ProjectCollectionFile );
        }
    }
}
