using CyrilGame.Core.Projects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CyrilGame
{
    public class CyrilGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ProjectCollection m_projectCollection = new ProjectCollection();

        Texture2D m_ballTexture;
        Vector2 m_ballPosition;
        float m_ballSpeed;

        public CyrilGame()
        {
            _graphics = new GraphicsDeviceManager( this );
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            m_ballPosition = new Vector2( _graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2 );
            m_ballSpeed = 100f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch( GraphicsDevice );

            // TODO: use this.Content to load your game content here

            m_ballTexture = Content.Load<Texture2D>( @"BallExample\ball" );
        }

        protected override void Update( GameTime gameTime )
        {
            if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown( Keys.Escape ) )
                Exit();

            // TODO: Add your update logic here

            base.Update( gameTime );
        }

        protected override void Draw( GameTime gameTime )
        {
            GraphicsDevice.Clear( Color.CornflowerBlue );

            if( m_projectCollection.IsBrandNewProject() )
            {
                return;
            }

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw
            ( 
                m_ballTexture, 
                m_ballPosition,
                null,
                Color.White,
                0f,
                new Vector2( m_ballTexture.Width / 2, m_ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.End();

            base.Draw( gameTime );
        }
    }
}