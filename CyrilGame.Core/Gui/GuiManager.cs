using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CyrilGame.Core.EditorGui
{
    public class GuiManager
    {
        public Stack<GuiBase> Gui { get; set; } = new();
        private static GuiManager m_instance = new GuiManager();

        public static GuiManager Instance
        {
            get { return m_instance; }
        }

        public void AddGui( GuiBase InGui, ContentManager InContent )
        {
            InGui.Init( InContent );

            if( Gui.Count() > 0 )
            {
                Gui.Peek().DrawIndex = 1f;
            }

            Gui.Push( InGui );
        }

        public void Draw( SpriteBatch InSpriteBatch )
        {
            foreach( var gui in Gui.Reverse() )
            {
                gui.Draw( InSpriteBatch );
            }
        }

        public void Update( GameTime InGameTime, MouseState InMouseState, GraphicsDeviceManager InGraphicsDeviceManager )
        {
            foreach ( var gui in Gui )
            {
               var eventHandled = gui.Update( InGameTime, InMouseState, InGraphicsDeviceManager );

                if( eventHandled == UpdateEvent.Handled )
                {
                    break;
                }
            }

            //var topGui = Gui.Peek();

            //if( topGui != null )
            //{
            //    topGui.Update( InGameTime, InMouseState, InGraphicsDeviceManager );
            //}
        }

        private GuiManager()
        {

        }

        public enum UpdateEvent
        {
            NotHandled,
            Handled
        }
    }
}
