using System;
using System.IO;
using SDL2;
using static SDL2.SDL;
using static SDL2.SDL_mixer;
namespace Pong.Game.Models;

public class Music
{
    private IntPtr l;
    bool success = true;
    private String file;


    public Music(String s)
    {
        this.file = s;
        Instalize();
        loadMedia();
    }
    public void playMusik()
    {
        Mix_PlayMusic(l, 0);
    }
    public void Instalize()
    {
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO) < 0)
        {
            SDL_Log("IMAGE_FAILD");
            success = false;
        }

        if (Mix_OpenAudio(44100, MIX_DEFAULT_FORMAT, 1, 1024) < 0)
        {
            SDL_Log("MIXER_FAILD");
            success = false;
        }
    }
    bool loadMedia()
    {
        //Loading success flag

        //Load music
        l = Mix_LoadMUS(file);
        if (l == IntPtr.Zero)
        {
            SDL_Log("LoadMUS_FAILD");
            success = false;
        }
        return success;
    }

    public void close()
    {
        Mix_HaltMusic();
    }
}