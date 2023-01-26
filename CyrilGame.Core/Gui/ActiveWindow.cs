using CyrilGame.Core.Assets;
using CyrilGame.Core.Extensions;
using CyrilGame.Core.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace CyrilGame.Core.EditorGui
{
    public class ActiveWindow : GuiBase
    {
        private string m_Title;
        private Texture2D m_InactiveTexture;

        private Texture2D m_DefaultTexture;

        public ActiveWindow( string InTitle, Vector2 InPosition, uint InWidth, uint InHeight ) 
            : base( InPosition, InWidth, InHeight )
        {
            m_Title = InTitle;
        }

        public override void Init( ContentManager InContent )
        {
            m_texture = AssetManager.Instance.GetAsset( @"editor\gui\Window_Header" );// InContent.Load< Texture2D >( @"editor\gui\Window_Header" );
            m_DefaultTexture = m_texture;

            m_InactiveTexture = AssetManager.Instance.GetAsset( @"editor\gui\Window_Header_Inactive" );

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

        float m_XDistance;
        float m_YDistance;

        public override void Draw( SpriteBatch InSpriteBatch )
        {
            base.Draw( InSpriteBatch );

            var headerPadding = new Vector2( 3, 6 );
            var titlePos = Position + headerPadding;

            GuiManager.Instance.RendererSpecificItems.Font.DrawString( InSpriteBatch, m_Title, m_HeaderStartPos );
        }

        public override UpdateEvent Update( GameTime InGameTime, MouseState InMouseState, GraphicsDeviceManager InGraphicsDeviceManager )
        {
            var mousePosition = new Vector2( InMouseState.X, InMouseState.Y );

            var topLeftRect = Slices[ SlicePart.TopLeft ].AddVector( Position );
            var topMiddleRect = Slices[ SlicePart.TopMiddle ].AddVector( Position ); ;
            var topRightRect = Slices[ SlicePart.TopRight ].AddVector( Position ); ;

            var mouseIsOnHeader = m_Header.Contains( InMouseState.X, InMouseState.Y );
                
                //topLeftRect.Contains( InMouseState.X, InMouseState.Y ) 
                //|| topMiddleRect.Contains( InMouseState.X, InMouseState.Y )
                //|| topRightRect.Contains( InMouseState.X, InMouseState.Y );

            if( !bIsDragging && InMouseState.LeftButton == ButtonState.Pressed && !m_Bounds.Contains( mousePosition ) )
            {
                m_texture = m_InactiveTexture;
            }
            else if( !bIsDragging && InMouseState.LeftButton == ButtonState.Pressed && m_Bounds.Contains( mousePosition ) )
            {
                m_texture = m_DefaultTexture;
            }

            if( InMouseState.LeftButton == ButtonState.Pressed )
            {
                Debug.WriteLine( $"LMB pressed" );
            }

            if ( !bIsDragging && mouseIsOnHeader && InMouseState.LeftButton == ButtonState.Pressed )
            {
                m_XDistance =  Vector2.Distance( new Vector2( topLeftRect.X, 0 ), new Vector2( mousePosition.X, 0 ) );
                m_YDistance =  Vector2.Distance( new Vector2( 0, topLeftRect.Y ), new Vector2( 0, mousePosition.Y ) );

                bIsDragging = true;
            } 

            if ( bIsDragging && InMouseState.LeftButton == ButtonState.Pressed )
            {
                var newPosition = new Vector2( mousePosition.X - m_XDistance, mousePosition.Y - m_YDistance );

                if( newPosition.X < 0 )
                { 
                    newPosition.X = 0;
                }

                if( ( newPosition.X + m_Bounds.Width ) > InGraphicsDeviceManager.PreferredBackBufferWidth )
                {
                    newPosition.X = InGraphicsDeviceManager.PreferredBackBufferWidth - m_Bounds.Width + 1;
                }

                if( newPosition.Y < 0 )
                {
                    newPosition.Y = 0;
                }

                if ( ( newPosition.Y + m_Bounds.Height ) > InGraphicsDeviceManager.PreferredBackBufferHeight )
                {
                    newPosition.Y = InGraphicsDeviceManager.PreferredBackBufferHeight - m_Bounds.Height + 1;
                }

                Position = newPosition;

                return UpdateEvent.Handled;
            }

            if ( bIsDragging && InMouseState.LeftButton == ButtonState.Released )
            {
                bIsDragging = false;
            }

            return UpdateEvent.NotHandled;
        }
    }
}
