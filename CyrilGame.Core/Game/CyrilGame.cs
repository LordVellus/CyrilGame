using CyrilGame.Core.EditorGui;
using CyrilGame.Core.Gui;
using CyrilGame.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CyrilGame.Core
{
    public class CyrilGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        DefaultFont m_defaultFont = new DefaultFont();

        public CyrilGame()
        {
            _graphics = new GraphicsDeviceManager( this );
            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            GuiManager.Instance.RendererSpecificItems.GraphicsDeviceManager = _graphics;
            GuiManager.Instance.RendererSpecificItems.Content = Content;
            GuiManager.Instance.RendererSpecificItems.Font = m_defaultFont;

            AssetSystem.Instance.LoadAllAssets();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch( GraphicsDevice );
            GuiManager.Instance.RendererSpecificItems.SpriteBatch = _spriteBatch;

            // TODO: use this.Content to load your game content here

            var windowWidth =  150U;
            var windowHeight = 75U;
            var middleOfScreen = new Vector2( _graphics.PreferredBackBufferWidth / 2 - windowWidth / 2, _graphics.PreferredBackBufferHeight / 2 - windowHeight / 2 );

            var newPos = new Vector2( _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight );

            m_defaultFont.LoadContent( Content );

            var guiGroup = new GuiGroup();
            guiGroup.AddElement( new ActiveWindow( "A Title", middleOfScreen, windowWidth, windowHeight ) );
           
            GuiManager.Instance.AddGui( guiGroup );
        }

        protected override void Update( GameTime gameTime )
        {
            GuiManager.Instance.RendererSpecificItems.GameTime = gameTime;
            GuiManager.Instance.RendererSpecificItems.MouseState = Mouse.GetState();

            if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown( Keys.Escape ) )
                Exit();


            // TODO: Add your update logic here

            GuiManager.Instance.Update();

            base.Update( gameTime );
        }

        protected override void Draw( GameTime gameTime )
        {
            GraphicsDevice.Clear( Color.CornflowerBlue );
            
            _spriteBatch.Begin();

            GuiManager.Instance.Draw();

            // TODO: Add your drawing code here
            _spriteBatch.End();

            base.Draw( gameTime );
        }
    }
}