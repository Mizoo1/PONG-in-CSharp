using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Pong.Engine.Defaults;
using static SDL2.SDL;

namespace Pong.Engine.Models
{
    /// <summary>
    /// Describes an element that should be rendered to the display
    /// </summary>
    public class RenderObject
        {
        /// <summary>
        /// The type of RenderObject you are creating
        /// </summary>
        [Required]
        public RenderObjectTypes Type { get; init; }

         /// <summary>
        /// Whether or not a Rect should be drawn with
        /// SDL_DrawFillRect or SDL_DrawRect
        /// </summary>
        public bool Fill = false;

        /// <summary>
        /// Describes when it will be rendered.
        /// A higher number will be rendered later in the queue and appear above other elements
        /// </summary>
        public int ZIndex = 0;

        /// <summary>
        /// The SDL_Rect(s) for the Object(s) and their Colour
        /// </summary>
        public RectObject[] RectObjects { get; init; }
        
        /// <summary>
        /// The SDL_Point(s) for the Object(s)
        /// </summary>
        public PointObject[] PointObjects { get; init; }
        
        /// <summary>
        /// The various points to create a line
        /// </summary>
        public LineObject[] LineObjects { get; init; }
        
        /// <summary>
        /// The texture to be rendered
        /// </summary>
        public IntPtr Texture { get; init; }
        
    }
}