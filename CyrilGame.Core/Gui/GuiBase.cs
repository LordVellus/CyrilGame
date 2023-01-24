using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using CyrilGame.Core.Gui;

namespace CyrilGame.Core.EditorGui
{
    public class SpriteSheetFont
    {
        protected uint Rows;
        protected uint Cols;
        protected uint CharHeight;
        protected uint CharWidth;
        protected Texture2D? m_Texture = null;
        protected Dictionary< string, int > m_FontDef = new Dictionary<string, int>();

        protected Rectangle Bounds = new Rectangle();

        public virtual void LoadContent( ContentManager InContent ) { }

        public void DrawString( SpriteBatch InSpriteBatch, string InString, Vector2 InPosition )
        {
            var position = InPosition;

            foreach( var character in InString )
            {
                var ascii = (int)character;

                var top = ( ascii - 32 ) / 16 * 12;

                var left = ( ascii - 32 ) % 16 * 8;

                var realCharacterWidth = m_FontDef[ character.ToString() ];


                var sourceRect = new Rectangle();
                sourceRect.X = ( int ) left;
                sourceRect.Y = ( int ) top;
                sourceRect.Width = ( int ) realCharacterWidth;
                sourceRect.Height = ( int ) CharHeight;

                InSpriteBatch.Draw( m_Texture, new Rectangle( ( int ) position.X, ( int ) position.Y, ( int ) realCharacterWidth, ( int ) CharHeight ), sourceRect, Color.White, 0f, Vector2.One, SpriteEffects.None, 1f );

                position.X += realCharacterWidth;
            }
        }
    }

    public class DefaultFont : SpriteSheetFont 
    {
        public DefaultFont()
        {
            Rows = 6;
            Cols = 16;
            CharHeight = 12;
            CharWidth = 8;
        }

        public override void LoadContent( ContentManager InContent )
        {
            m_Texture = InContent.Load<Texture2D>( @"fonts\font" );

            var fontDef = File.ReadAllText( @"Config\fontdef.txt" );

            var lines = fontDef.Split( Environment.NewLine );

            foreach(var line in lines ) 
            {
                var splitLine = line.Split( "[SEP]" );

                m_FontDef.Add( splitLine[ 0 ], int.Parse( splitLine[ 1 ] ) );
            }
        }

}

public abstract class GuiBase
    {
        public float DrawIndex = 0f;

        public Rectangle Bounds { get { return m_Bounds; } }

        public Vector2 Position { get; protected set; }
        public void SetPosition( Vector2 InPosition )
        {
            Position = InPosition;
        }
        public Guid Id { get; private set;}

        protected Texture2D? m_texture = null;
        protected uint m_width;
        protected uint m_Height;

        protected Rectangle m_Header = new Rectangle();
        protected Rectangle m_Bounds = new Rectangle();
        protected Vector2 m_HeaderStartPos = new Vector2();

        public GuiBase( Vector2 InPosition, uint InWidth, uint InHeight )
        {
            Position = InPosition;
            m_width = InWidth;
            m_Height = InHeight;
        }

        public virtual void Init( ContentManager InContent ) 
        {
            Id = Guid.NewGuid();
        }

        public virtual void Draw( SpriteBatch InSpriteBatch )
        {
            Debug.Assert( m_texture != null );
            Debug.Assert( m_width >= m_texture.Width );
            Debug.Assert( m_Height >= m_texture.Height );

            var position = Position;

            //  Top left slice
            var topLeftSlice = Slices[ SlicePart.TopLeft ];
            Draw( InSpriteBatch, ref position, topLeftSlice );

            m_HeaderStartPos.X = position.X + topLeftSlice.Width / 2;
            m_HeaderStartPos.Y = position.Y + topLeftSlice.Height / 4;

            m_Header.X = ( int ) position.X;
            m_Header.Y = ( int ) position.Y;

            m_Bounds.X = ( int ) position.X;
            m_Bounds.Y = ( int ) position.Y;

            //  Top middle slices
            var topMiddleSlice = Slices[ SlicePart.TopMiddle ];
            position += new Vector2( topMiddleSlice.X, 0 );

            //  Top right slice
            var topRightSlice = Slices[ SlicePart.TopRight ];

            DrawRepeatingX( InSpriteBatch, ref position, topLeftSlice, topRightSlice, topMiddleSlice, ( uint ) topMiddleSlice.Height );

            //  Draw top right slice
            Draw( InSpriteBatch, ref position, topRightSlice );

            m_Header.Width = ( int ) ( ( position.X + topMiddleSlice.Width ) - m_Header.X );
            m_Header.Height = ( int ) ( ( position.Y + topMiddleSlice.Height ) - m_Header.Y );

            //  Middle slices
            var middleLeftSlice = Slices[ SlicePart.MiddleLeft ];
            var middleMiddleSlice = Slices[ SlicePart.MiddleMiddle ];
            var middleRightSlice = Slices[ SlicePart.MiddleRight ];
            var bottomLeftSlice = Slices[ SlicePart.BottomLeft ];

            var remainingHeight = m_Height - ( uint ) topLeftSlice.Height - ( uint ) bottomLeftSlice.Height;
            uint numberOfHeightSections = ( uint ) remainingHeight / (uint) middleLeftSlice.Height;
            if ( ( numberOfHeightSections * middleLeftSlice.Height ) < ( uint ) middleLeftSlice.Height )
            {
                numberOfHeightSections++;
            }

            position = Position + new Vector2( middleLeftSlice.X, middleLeftSlice.Y );
            var initialX = position.X;

            for ( int  i = 0; i < numberOfHeightSections; i++ ) 
            {
                var nextSliceHeight = remainingHeight >= middleLeftSlice.Height ? ( uint )middleLeftSlice.Height : remainingHeight;

                Draw( InSpriteBatch, ref position, middleLeftSlice, nextSliceHeight );


                position += new Vector2( middleMiddleSlice.X, 0 );

               DrawRepeatingX( InSpriteBatch, ref position, middleLeftSlice, middleRightSlice, middleMiddleSlice, nextSliceHeight );

                //  Draw middle right slice
                Draw( InSpriteBatch, ref position, middleRightSlice, nextSliceHeight );

                position.Y += nextSliceHeight;
                position.X = initialX;
                remainingHeight -= nextSliceHeight;
            }

            position.X = initialX;

            //  Bottom left slice
            //position = position + new Vector2( bottomLeftSlice.X, bottomLeftSlice.Y );
            Draw( InSpriteBatch, ref position, bottomLeftSlice );

            //  Bottom middle slices
            var bottomMiddleSlice = Slices[ SlicePart.BottomMiddle ];
            position += new Vector2( bottomMiddleSlice.X, 0 );

            //  Bottom right slice
            var bottomRightSlice = Slices[ SlicePart.BottomRight ];

            DrawRepeatingX( InSpriteBatch, ref position, bottomLeftSlice, bottomRightSlice, bottomMiddleSlice, ( uint ) bottomMiddleSlice.Height );

            //  Draw bottom right slice
            Draw( InSpriteBatch, ref position, bottomRightSlice );

            m_Bounds.Width = Math.Abs( (int)position.X + bottomRightSlice.Width - m_Bounds.X );
            m_Bounds.Height = Math.Abs( (int)position.Y + bottomRightSlice.Height - m_Bounds.Y );
        }

        private void Draw( SpriteBatch InSpriteBatch, ref Vector2 InPosition, Rectangle InSlice, uint InHeight = 0 )
        {
            var height = InHeight == 0 ? ( uint ) InSlice.Height : InHeight;

            InSpriteBatch.Draw( m_texture, new Rectangle( ( int ) InPosition.X, ( int ) InPosition.Y, InSlice.Width, ( int ) height ), InSlice, Color.White, 0f, Vector2.One, SpriteEffects.None, DrawIndex );
        }

        public abstract UpdateEvent Update( GameTime InGameTime, MouseState InMouseState, GraphicsDeviceManager InGraphicsDeviceManager );

        protected enum SlicePart
        {
            TopLeft, 
            TopMiddle,
            TopRight,

            MiddleLeft,
            MiddleMiddle,
            MiddleRight,
            
            BottomLeft,
            BottomMiddle,
            BottomRight,
        };

        protected bool IsASlicePartMiddle( SlicePart InPart )
        {
            return InPart == SlicePart.TopMiddle
                   || InPart == SlicePart.MiddleMiddle
                   || InPart == SlicePart.BottomMiddle;
        }

        protected Dictionary<SlicePart, Rectangle> Slices { get; set; } = new();

        private void DrawRepeatingX( SpriteBatch InSpriteBatch, ref Vector2 position, Rectangle InLeftRectangle, Rectangle InRightRectangle, Rectangle InMiddleRectangle, uint InSliceHeight )
        {
            var remainingPixels = m_width - ( uint ) InLeftRectangle.Width - ( uint ) InRightRectangle.Width;
            var numberOfPanelsNeeded = remainingPixels / ( uint ) InMiddleRectangle.Width;

            if ( ( numberOfPanelsNeeded * InMiddleRectangle.Width ) < remainingPixels )
            {
                numberOfPanelsNeeded++;
            }

            if ( numberOfPanelsNeeded < 1 )
            {
                numberOfPanelsNeeded = 1;
            }

            for ( uint i = 0; i < numberOfPanelsNeeded; i++ )
            {
                var nextSliceWidth = remainingPixels >= ( uint ) InMiddleRectangle.Width  ? ( uint ) InMiddleRectangle.Width  : remainingPixels;


                var sourceRectangle = InMiddleRectangle;
                sourceRectangle.Width = ( int ) nextSliceWidth;
                InSpriteBatch.Draw( m_texture, new Rectangle( ( int ) position.X, ( int ) position.Y, ( int ) nextSliceWidth, ( int ) InSliceHeight ), sourceRectangle, Color.White, 0f, Vector2.One, SpriteEffects.None, DrawIndex );

                position.X += nextSliceWidth;

                remainingPixels -= nextSliceWidth;
            }
        }
    }
}
