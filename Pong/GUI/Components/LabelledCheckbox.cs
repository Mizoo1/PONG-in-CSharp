using System;
using System.Drawing;
using Pong.Engine;
using Pong.Engine.Models;
using Pong.Game.Models;
using Pong.GUI.Components.Text;
using static SDL2.SDL;

namespace Pong.GUI.Components
{
    /// <summary>
    /// A checkbox with a label.
    /// </summary>
    public class LabelledCheckbox : Checkbox
    {
        private TextDisplay _textDisplay;

        public LabelledCheckbox(Renderer renderer, string label, Color colour, Position position, int width = 400, int height = 50, int size = 25)
        : base(renderer, colour, position, width, height, size, true)
        {
            var textRect = new SDL_Rect
            {
                w = width - (int)(size + size * 0.20),
                h = height,
                x = position.X,
                y = position.Y
            };
            _textDisplay = new TextDisplay(renderer, label, colour, textRect);
        }

        /// <summary>
        /// Draws the base checkbox and the text
        /// </summary>
        public new void Draw()
        {
            base.Draw();
            _textDisplay.Draw();
        }
    }
}