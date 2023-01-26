using CyrilGame.Core.Gui;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace CyrilGame.Core.Systems
{
    public class AssetSystem
    {
        private static AssetSystem m_Instance = new AssetSystem();

        public static AssetSystem Instance => m_Instance;

        public Dictionary<string, Texture2D> ImageAssets { get; private set; } = new();
        public Dictionary<string, string> TextAssets { get; private set; } = new();

        public Texture2D GetImageAsset(string InPath)
        {
            return ImageAssets[InPath];
        }

        public string GetTextAsset( string InPath )
        {
            return TextAssets[ InPath ];
        }

        public void LoadAllAssets()
        {
            string[] searchPattern = new string[] { "*.png", "*.txt" };
            var allAssets = searchPattern.SelectMany(x => Directory.EnumerateFiles("Assets", x, SearchOption.AllDirectories));

            foreach (var asset in allAssets)
            {
                var fileName = Path.GetFileNameWithoutExtension(asset);
                var extension = Path.GetExtension( asset ).ToLower();

                var pathWithoutFile = Path.GetFullPath(asset).Replace(Path.GetFileName(asset), "");
                const string root = "Assets\\";

                var indexOfAssets = pathWithoutFile.IndexOf(root);

                StringBuilder pathKey = new StringBuilder();

                for (int i = indexOfAssets + root.Length; i < pathWithoutFile.Length; i++)
                {
                    pathKey.Append(pathWithoutFile[i]);
                }


                var  path =  Path.Combine( pathKey.ToString(), fileName );

                switch ( extension )
                {
                    case ".png":
                        ImageAssets[ path ] = Texture2D.FromFile( GuiManager.Instance.RendererSpecificItems.GraphicsDeviceManager.GraphicsDevice, asset );
                        break;
                    case ".txt":
                        TextAssets[ path ] = File.ReadAllText( asset );
                        break;
                }
            }
        }
        private AssetSystem() { }
    }
}
