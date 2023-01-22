using CyrilGame.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CyrilGame.Core.EditorGui
{
    public class ActiveWindow : EditorGuiBase
    {
        public ActiveWindow( Vector2 InPosition, uint InWidth, uint InHeight ) 
            : base( InPosition, InWidth, InHeight )
        {
        }

        public override void Init( ContentManager InContent )
        {
            m_texture = InContent.Load< Texture2D >( @"editor\gui\Window_Header" );

            Slices.Add( SlicePart.TopLeft, new Rectangle( 0, 0, 16, 32 ) );
            Slices.Add( SlicePart.TopMiddle, new Rectangle( 16, 0, 16, 32 ) );
            Slices.Add( SlicePart.TopRight, new Rectangle( 32, 0, 16, 32 ) );

            Slices.Add( SlicePart.MiddleLeft, new Rectangle( 0, 32, 16, 12 ) );
            Slices.Add( SlicePart.MiddleMiddle, new Rectangle( 16, 32, 16, 12 ) );
            Slices.Add( SlicePart.MiddleRight, new Rectangle( 32, 32, 16, 12 ) );

            Slices.Add( SlicePart.BottomLeft, new Rectangle( 0, 44, 16, 4 ) );
            Slices.Add( SlicePart.BottomMiddle, new Rectangle( 16, 44, 16, 4 ) );
            Slices.Add( SlicePart.BottomRight, new Rectangle( 32, 44, 16, 4 ) );
        }

        bool bIsDragging = false;

        public override void Update( GameTime InGameTime, MouseState InMouseState )
        {
            var mousePosition = new Vector2( InMouseState.X, InMouseState.Y );

            var topLeftRect = Slices[ SlicePart.TopLeft ].AddVector( Position );
            var topMiddleRect = Slices[ SlicePart.TopMiddle ].AddVector( Position ); ;
            var topRightRect = Slices[ SlicePart.TopRight ].AddVector( Position ); ;

            var mouseIsOnHeader = m_Header.Contains( InMouseState.X, InMouseState.Y );
                
                //topLeftRect.Contains( InMouseState.X, InMouseState.Y ) 
                //|| topMiddleRect.Contains( InMouseState.X, InMouseState.Y )
                //|| topRightRect.Contains( InMouseState.X, InMouseState.Y );

            if ( !bIsDragging && mouseIsOnHeader && InMouseState.LeftButton == ButtonState.Pressed )
            {
                bIsDragging = true;
            }

            if ( bIsDragging && InMouseState.LeftButton == ButtonState.Pressed )
            {
                Position = mousePosition - Position;
            }

            if ( bIsDragging && InMouseState.LeftButton == ButtonState.Released )
            {
                bIsDragging = false;
            }
        }
    }
}
