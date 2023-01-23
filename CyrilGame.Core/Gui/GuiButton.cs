using CyrilGame.Core.EditorGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static CyrilGame.Core.EditorGui.GuiManager;

namespace CyrilGame.Core.Gui
{
    public class GuiButton : GuiBase
    {
        private Texture2D m_DefaultTexture;

        private Texture2D m_PressedTexture;

        public GuiButton( Vector2 InPosition, uint InWidth, uint InHeight ) 
            : base( InPosition, InWidth, InHeight )
        {
            Slices[ SlicePart.TopLeft ] = new Rectangle( 0, 0, 2, 2 );
            Slices[ SlicePart.TopMiddle ] = new Rectangle( 2, 0, 11, 2 );
            Slices[ SlicePart.TopRight ] = new Rectangle( 13, 0, 2, 2 );

            Slices[ SlicePart.MiddleLeft ] = new Rectangle( 0, 2, 2, 11 );
            Slices[ SlicePart.MiddleMiddle ] = new Rectangle( 2, 2, 11, 11 );
            Slices[ SlicePart.MiddleRight ] = new Rectangle( 13, 2, 2, 11 );

            Slices[ SlicePart.BottomLeft ] = new Rectangle( 0, 13, 2, 2 );
            Slices[ SlicePart.BottomMiddle ] = new Rectangle( 2, 13, 11, 2 );
            Slices[ SlicePart.BottomRight ] = new Rectangle( 13, 13, 2, 2 );
        }

        public override void Init( ContentManager InContent )
        {
            base.Init( InContent );

            m_texture = InContent.Load< Texture2D >( @"editor\gui\Windows_Button" );
            m_DefaultTexture = m_texture;

            m_PressedTexture = InContent.Load< Texture2D >( @"editor\gui\Windows_Button_Pressed" );
        }

        public override UpdateEvent Update( GameTime InGameTime, MouseState InMouseState, GraphicsDeviceManager InGraphicsDeviceManager )
        {
            switch( InMouseState.LeftButton )
            {
                case ButtonState.Pressed:

                    var mousePosition = InMouseState.Position;

                    if( m_Bounds.Contains( mousePosition ) )
                    {
                        m_texture = m_PressedTexture;
                        return UpdateEvent.Handled;
                    }

                    break;
                default: 
                    m_texture = m_DefaultTexture;
                    break;
            }

            return UpdateEvent.NotHandled;
        }
    }
}
