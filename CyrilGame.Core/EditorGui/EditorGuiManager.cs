using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CyrilGame.Core.EditorGui
{
    public class EditorGuiManager
    {
        public Stack<EditorGuiBase> Gui { get; set; } = new();
        private static EditorGuiManager m_instance = new EditorGuiManager();

        public static EditorGuiManager Instance
        {
            get { return m_instance; }
        }

        public void AddGui( EditorGuiBase InGui, ContentManager InContent )
        {
            InGui.Init( InContent );
            Gui.Push( InGui );
        }

        public void Draw( SpriteBatch InSpriteBatch )
        {
            foreach( var gui in Gui )
            {
                gui.Draw( InSpriteBatch );
            }
        }

        public void Update( GameTime InGameTime, MouseState InMouseState )
        {
            var topGui = Gui.Peek();

            if( topGui != null )
            {
                topGui.Update( InGameTime, InMouseState );
            }
        }

        private EditorGuiManager()
        {

        }
    }
}
