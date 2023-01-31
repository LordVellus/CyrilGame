using CyrilGame.Core.EditorGui;

namespace CyrilGame.Core.Gui
{
    public class GuiGroup
    {
        public Stack< GuiBase > Elements { get; set; } = new();

        private bool m_bIsEditor = false;

        public GuiGroup( bool bIsEditor )
        {
            m_bIsEditor= bIsEditor;
        }


        public void AddElement ( GuiBase InElement )
        {
            Elements.Push( InElement );
        }

        public void SetDrawIndex( float InDrawIndex )
        {
            foreach(var element in Elements)
            {
                element.DrawIndex = InDrawIndex;
            }
        }

        public void Init()
        { 
            foreach(var element in Elements)
            {
                element.Init( GuiManager.Instance.RendererSpecificItems.Content );
            }
        }

        public void Draw( bool bInActivatedEditor = false )
        {
            if( !bInActivatedEditor && m_bIsEditor )
            {
                return;
            }

            foreach(var element in Elements) 
            {
                element.Draw( GuiManager.Instance.RendererSpecificItems.SpriteBatch );
            }
        }

        public UpdateEvent Update()
        {
            var updateEvent = UpdateEvent.NotHandled;

            foreach( var element in Elements) 
            {
                if( updateEvent != UpdateEvent.Handled )
                {
                    updateEvent = element.Update
                    (
                        GuiManager.Instance.RendererSpecificItems.GameTime,
                        GuiManager.Instance.RendererSpecificItems.MouseState,
                        GuiManager.Instance.RendererSpecificItems.GraphicsDeviceManager
                    );
                }
                else
                {
                    break;
                }
            }

            return updateEvent;
        }
    }
}
