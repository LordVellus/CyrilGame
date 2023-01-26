using CyrilGame.Core.Gui;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using System.Text;

namespace CyrilGame.Core.Assets
{
    public class AssetManager
    {
        private static AssetManager m_Instance = new AssetManager();

        public static AssetManager Instance => m_Instance;

        public Dictionary<string, Texture2D > Assets { get; private set; } = new();

        public Texture2D GetAsset( string InPath )
        {
            return Assets[ InPath ];
        }

        public void LoadAllAssets()
        {
            string[] searchPattern = new string[] { "*.png" };
            var allAssets = searchPattern.SelectMany( x => Directory.EnumerateFiles(  "Assets", x, SearchOption.AllDirectories ) );
            
            foreach(var asset in allAssets)
            {
                var fileName = Path.GetFileNameWithoutExtension( asset );
                
                var pathWithoutFile = Path.GetFullPath( asset ).Replace( Path.GetFileName( asset ), "" );
                const string root = "Assets\\";

                var indexOfAssets = pathWithoutFile.IndexOf( root );

                StringBuilder pathKey = new StringBuilder();

                for( int i = indexOfAssets + root.Length; i < pathWithoutFile.Length; i++ )
                {
                    pathKey.Append( pathWithoutFile[i] );
                }

                Assets[ Path.Combine( pathKey.ToString(), fileName ) ] = Texture2D.FromFile( GuiManager.Instance.RendererSpecificItems.GraphicsDeviceManager.GraphicsDevice, asset );
            }
        }
        private AssetManager() { }
    }
}
