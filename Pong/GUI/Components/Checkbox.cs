using System;
using System.Drawing;
using Pong.Engine;
using Pong.Engine.Defaults;
using Pong.Engine.Models;
using Pong.Game.Models;
using Pong.GUI.Components.Text;
using static SDL2.SDL;

namespace Pong.GUI.Components
{
    public class Checkbox
    {
        private Renderer _renderer;
        private int _zIndex;
        
        /// <summary>
        /// Indicates if the checkbox is checked or not
        /// </summary>
        public bool Checked;
        /// <summary>
        /// The size of the checkbox
        /// </summary>
        public int Size { get; init; }
        
        /// <summary>
        /// The RectObject used by the renderer
        /// </summary>
        public RectObject Rect { get; }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// Creates a new checkbox.
        /// </summary>
        /// <param name="renderer">The renderer the object should use</param>
        /// <param name="colour">The colour of the checkbox</param>
        /// <param name="position">The position of the checkbox</param>
        /// <param name="width">The width of the parent rect. Default = 200</param>
        /// <param name="height">The height of the parent rect. Default = 50</param>
        /// <param name="size">The size of the checkbox. Default = 50</param>
        /// <param name="hasLabel">Whether or not the checkbox has a label. Default = false</param>
        /// <param name="zIndex">What layer the checkbox should be rendered on. Default = 1</param>
        public Checkbox(Renderer renderer, Color colour, Position position, int width = 200, int height = 50, int size = 50, bool hasLabel = false, int zIndex = 1)
        {
            Size = size;
            Checked = false;
            
            _renderer = renderer;
            _zIndex = zIndex;

            var rightPos = new Position
            {
                X = position.X + width - size,
                
                //This aligns the bottom border of the checkbox
                //with the text :)
                Y = position.Y + (int)(size / 1.5)
            };

            Rect = new RectObject
            {
                Colour = colour,
                SdlRect = new SDL_Rect
                {
                    w = size,
                    h = size,
                    x = hasLabel ? rightPos.X : position.X,
                    y = hasLabel ? rightPos.Y : position.Y
                }
            };
        }


        public void OnClick()
        {
            SDL_Log("OnClick");
            var handler = Click;
            handler?.Invoke(this, EventArgs.Empty);
        }
        
        /// <summary>
        /// Toggles the checked status of checkbox
        /// </summary>
        /// <returns>the new checked value</returns>
        /// 
        public bool Toggle()
        {
            Checked = !Checked;
            return Checked;
        }

        /// <summary>
        /// Draws the checkbox
        /// </summary>
        public void Draw()
        {
            var renderObject = new RenderObject
            {
                Type = RenderObjectTypes.Rect,
                Fill = Checked,
                ZIndex = _zIndex,
                RectObjects = new []
                {
                    Rect
                }
            };
            
            _renderer.AddToRenderQueue(renderObject);
        }
    }
}