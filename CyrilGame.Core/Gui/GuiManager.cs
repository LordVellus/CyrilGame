using CyrilGame.Core.EditorGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CyrilGame.Core.Gui
{
    public class RendererSpecificItems
    {
        public GameTime GameTime;
        public MouseState MouseState;
        public GraphicsDeviceManager GraphicsDeviceManager;
        public SpriteBatch SpriteBatch;
        public ContentManager Content;
        public SpriteSheetFont Font;
    }

    public class GuiManager
    {
        public RendererSpecificItems RendererSpecificItems { get; set; } = new();
        public Stack<GuiGroup> GuiGroups { get; set; } = new();
        private static GuiManager m_instance = new GuiManager();

        public static GuiManager Instance
        {
            get { return m_instance; }
        }

        public void AddGui( GuiGroup InGuiGroup )
        {
            InGuiGroup.Init();

            if( GuiGroups.Count() > 0 )
            {
                GuiGroups.Peek().SetDrawIndex( 1f );
            }

            GuiGroups.Push( InGuiGroup );
        }

        public void Draw()
        {
            foreach( var gui in GuiGroups )
            {
                gui.Draw();
            }
        }

        public void Update()
        {
            foreach ( var gui in GuiGroups )
            {
               var eventHandled = gui.Update();

                if( eventHandled == UpdateEvent.Handled )
                {
                    break;
                }
            }
        }
        
        private GuiManager()
        {

        }
    }
}
