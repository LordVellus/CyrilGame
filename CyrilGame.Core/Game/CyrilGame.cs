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
        protected DefaultFont m_defaultFont = new DefaultFont();

        private bool m_bActivateEditor = false;

        public enum CyrilKeyState
        {
            None,
            KeyDown,
            Pressed,
            Held
        }

        private Dictionary<Keys, CyrilKeyState> m_KeyStates = new Dictionary<Keys, CyrilKeyState>();

        public CyrilGame()
        {
            _graphics = new GraphicsDeviceManager( this );
            _graphics.IsFullScreen = false;
            IsMouseVisible = true;

            foreach(var key in (Keys[])Enum.GetValues( typeof( Keys ) ) )
            {
                m_KeyStates.Add( key, CyrilKeyState.None );
            }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            GuiManager.Instance.RendererSpecificItems.GraphicsDeviceManager = _graphics;
            GuiManager.Instance.RendererSpecificItems.Content = Content;
            GuiManager.Instance.RendererSpecificItems.Font = m_defaultFont;

            AssetSystem.Instance.LoadAllAssets();

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            _graphics.IsFullScreen = true;

            _graphics.ApplyChanges();

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

            var guiGroup = new GuiGroup( true );
            guiGroup.AddElement( new ActiveWindow( "A Title", middleOfScreen, windowWidth, windowHeight ) );
           
            GuiManager.Instance.AddGui( guiGroup );
        }

        protected override void Update( GameTime gameTime )
        {
            GuiManager.Instance.RendererSpecificItems.GameTime = gameTime;
            GuiManager.Instance.RendererSpecificItems.MouseState = Mouse.GetState();

            foreach ( var ks in m_KeyStates )
            {
                var key = ks.Key;
                if( Keyboard.GetState().IsKeyDown( ks.Key ) )
                {
                    switch( ks.Value )
                    {
                        case CyrilKeyState.None:
                            m_KeyStates[ key ] = CyrilKeyState.KeyDown;
                            break;
                        case CyrilKeyState.KeyDown:
                            m_KeyStates[ key ] = CyrilKeyState.Held;
                            break;
                    }
                }
                else if( Keyboard.GetState().IsKeyUp( key ) )
                {
                    switch( ks.Value )
                    {
                        case CyrilKeyState.KeyDown:
                            m_KeyStates[ key ] = CyrilKeyState.Pressed;
                            break;
                        case CyrilKeyState.Pressed:
                            m_KeyStates[ key ] = CyrilKeyState.None;
                            break;
                    }
                }
            }

            if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown( Keys.Escape ) )
                Exit();

            if( m_KeyStates[ Keys.LeftShift ] == CyrilKeyState.Held && m_KeyStates[ Keys.E ] == CyrilKeyState.Pressed )
            {
                m_bActivateEditor = !m_bActivateEditor;
            }

            // TODO: Add your update logic here

            GuiManager.Instance.Update();

            base.Update( gameTime );
        }

        protected override void Draw( GameTime gameTime )
        {
            GraphicsDevice.Clear( Color.CornflowerBlue );
            
            _spriteBatch.Begin();

            GuiManager.Instance.Draw( m_bActivateEditor );

            // TODO: Add your drawing code here
            _spriteBatch.End();

            base.Draw( gameTime );
        }
    }
}