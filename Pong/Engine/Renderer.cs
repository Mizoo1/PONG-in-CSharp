using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Pong.Engine.Defaults;
using Pong.Engine.Models;
using static SDL2.SDL;

namespace Pong.Engine
{
    public class Renderer
    {
        public Window Window { get; }
        public IntPtr SdlRenderer { get; }

        public List<RenderObject> RenderObjects;

        /// <summary>
        /// The logical size the game is actually rendered in
        /// </summary>
        public readonly LogicalSize LogicalSize;

        /// <summary>
        /// A list of available display modes
        /// </summary>
        public readonly List<SDL_DisplayMode> DisplayModes;

        /// <summary>
        /// The renderer that actually draws entities to the screen
        /// </summary>
        /// <param name="window">The window the renderer is in</param>
        /// <param name="logicalWidth">The width the renderer will draw it independent of screen size</param>
        /// <param name="logicalHeight">The height the renderer will draw it independent of screen size.</param>
        public Renderer(Window window, int logicalWidth = 1920, int logicalHeight = 1080)
        {
            Window = window;
            RenderObjects = new List<RenderObject>();
            SdlRenderer = SDL_CreateRenderer(Window.SdlWindow, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if (logicalWidth > 0 && logicalHeight > 0)
            {
                SDL_RenderSetLogicalSize(SdlRenderer, logicalWidth, logicalHeight);
            
                LogicalSize = new LogicalSize
                {
                    LogicalHeight = logicalHeight, 
                    LogicalWidth = logicalWidth
                };
            }
            else
            {
                var actualRes = window.GetWindowSize();
                LogicalSize = new LogicalSize
                {
                    LogicalHeight = actualRes.Item2, 
                    LogicalWidth = actualRes.Item1
                };
            }   
        }

        public void Clear()
        {
            SDL_SetRenderDrawColor(SdlRenderer, 51, 51, 51, 255);
            SDL_RenderClear(SdlRenderer);
        }

        public void AddToRenderQueue(RenderObject renderObject)
        {
            RenderObjects.Add(renderObject);
        }

        public void AddMultipleToRenderQueue(RenderObject[] renderObjects)
        {
            RenderObjects.AddRange(renderObjects);
        }
        
        public void Render()
        {
            RenderObjects = RenderObjects.OrderBy(ro => ro.ZIndex).ToList();
            foreach (var renderObject in RenderObjects)
            {
                switch (renderObject.Type)
                {
                    case RenderObjectTypes.Rect:
                        
                            foreach (var rectObject in renderObject.RectObjects)
                            {
                                SDL_SetRenderDrawColor(SdlRenderer, rectObject.Colour.R, rectObject.Colour.G,
                                    rectObject.Colour.B, rectObject.Colour.A);
                                if (renderObject.Fill)
                                {
                                    SDL_RenderFillRect(SdlRenderer, ref rectObject.SdlRect);
                                }
                                else
                                {
                                    SDL_RenderDrawRect(SdlRenderer, ref rectObject.SdlRect);
                                }
                            }
                            break;
                    case RenderObjectTypes.Line:
                        foreach (var lineObject in renderObject.LineObjects)
                        {
                            SDL_SetRenderDrawColor(SdlRenderer, lineObject.Colour.R, lineObject.Colour.G,
                                lineObject.Colour.B, lineObject.Colour.A);
                            SDL_RenderDrawLine(SdlRenderer, lineObject.X1, lineObject.Y1, lineObject.X2, lineObject.Y2);
                        }
                        break;
                    case RenderObjectTypes.Point:
                        foreach (var pointObject in renderObject.PointObjects)
                        {
                            SDL_SetRenderDrawColor(SdlRenderer, pointObject.Colour.R, pointObject.Colour.G,
                                pointObject.Colour.B, pointObject.Colour.A);
                            SDL_RenderDrawPoint(SdlRenderer, pointObject.SdlPoint.x, pointObject.SdlPoint.y);
                        }
                        break;
                    case RenderObjectTypes.Texture:
                        SDL_RenderCopy(SdlRenderer, renderObject.Texture, IntPtr.Zero, ref renderObject.RectObjects[0].SdlRect);
                        break;
                    default:
                        throw new Exception($"No render type was given! {renderObject.GetType().Name}");
                }
            }
            RenderObjects.Clear();
            SDL_RenderPresent(SdlRenderer);
        }
        
        public void Destroy()
        {
            SDL_DestroyRenderer(SdlRenderer);
        }
    }
}