using System;
using System.Collections.Generic;
using System.Linq;
using static SDL2.SDL;

namespace Pong.Engine
{
    public class Window
    {
        public IntPtr SdlWindow { get; }

        public List<SDL_DisplayMode> ValidResolutions;

        public Window(string title, int width = 0, int height = 0, bool fullscreen = false)
        {
            ValidResolutions = new List<SDL_DisplayMode>();

            //Get the number of available "modes" from the display
            //Currently, we only get the main one, might have to change in the future
            var numModes = SDL_GetNumDisplayModes(0);
            for (int i = 0; i < numModes; i++)
            {
                SDL_GetDisplayMode(0, i, out var mode);
                ValidResolutions.Add(mode);
            }

            //Filter modes to be distinct by their width and height. We only care about resolution
            //so we only need to grab one copy of each available resolution
            ValidResolutions = ValidResolutions.DistinctBy(vr => new {vr.w, vr.h}).ToList();

            if (width == 0 && height == 0 && fullscreen)
            {
                width = ValidResolutions[0].w;
                height = ValidResolutions[0].h;
            }
            else if (width == 0 && height == 0 && !fullscreen)
            {
                width = ValidResolutions[2].w;
                height = ValidResolutions[2].h;
            }

            SdlWindow = SDL_CreateWindow(title,
                SDL_WINDOWPOS_UNDEFINED,
                SDL_WINDOWPOS_UNDEFINED,
                width,
                height,
                fullscreen ? SDL_WindowFlags.SDL_WINDOW_FULLSCREEN : SDL_WindowFlags.SDL_WINDOW_SHOWN);
        }

        /// <summary>
        /// Destroy the Window
        /// </summary>
        public void Destroy()
        {
            SDL_DestroyWindow(SdlWindow);
        }


        public IntPtr GetWindowSurface()
        {
            return SDL_GetWindowSurface(SdlWindow);
        }

        public Tuple<int, int> GetWindowSize()
        {
            SDL_GetWindowSize(SdlWindow, out int width, out int height);
            return new Tuple<int, int>(width, height);
        }

        public void SetSize(int width, int height)
        {
            SDL_SetWindowSize(SdlWindow, width, height);
        }

        public void SetPosition(int x, int y)
        {
            SDL_SetWindowPosition(SdlWindow, x, y);
        }

        public void SetFullscreen(bool fullscreen)
        {
            SDL_SetWindowFullscreen(SdlWindow, fullscreen ? (uint) SDL_WindowFlags.SDL_WINDOW_FULLSCREEN : 0);
        }
    }
}