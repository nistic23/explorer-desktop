using System.Collections.Generic;
using DCL.Components.Video.Plugin;
using UnityEngine;

public class VideoPluginWrapper_AVPro : IVideoPluginWrapper
{

    private Dictionary<string, AvProVideoPlayer> videoPlayers = new Dictionary<string, AvProVideoPlayer>();

    public void Create(string id, string url, bool useHls)
    {
        videoPlayers.Add(id, new AvProVideoPlayer(id, url));
    }

    public void Remove(string id)
    {
        videoPlayers[id].Dispose();
    }

    public void TextureUpdate(string id)
    {
        videoPlayers[id].UpdateVideoTexture();
    }

    public Texture2D PrepareTexture(string id)
    {
        videoPlayers[id].PrepareTexture();
        return videoPlayers[id].GetTexture();
    }

    public void Play(string id, float startTime)
    {
        videoPlayers[id].Play();
        if (startTime > 0)
        {
            videoPlayers[id].SetSeekTime(startTime);
        }
    }

    public void Pause(string id)
    {
        videoPlayers[id].Paused();
    }

    public void SetVolume(string id, float volume)
    {
        videoPlayers[id].SetVolume(volume);
    }

    public int GetHeight(string id)
    {
        return videoPlayers[id].GetTexture().height;
    }

    public int GetWidth(string id)
    {
        return videoPlayers[id].GetTexture().width;
    }

    public float GetTime(string id)
    {
        return videoPlayers[id].GetTime();
    }

    public float GetDuration(string id)
    {
        return videoPlayers[id].GetDuration();
    }

    public VideoState GetState(string id)
    {
        return videoPlayers[id].GetCurrentState();
    }

    public string GetError(string id)
    {
        return videoPlayers[id].GetLastError();
    }

    public void SetTime(string id, float second)
    {
        videoPlayers[id].SetSeekTime(second);
    }

    public void SetPlaybackRate(string id, float playbackRate)
    {
        videoPlayers[id].SetPlaybackRate(playbackRate);
    }

    public void SetLoop(string id, bool loop)
    {
        videoPlayers[id].SetLoop(loop);
    }
    
}