using System;
using DCL.Components.Video.Plugin;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

public class AvProVideoPlayer : IDisposable
{

    private MediaPlayer avProMediaPlayer;
    private VideoState currentState;
    private Texture2D videoTexture;
    private Texture avProTexture;
    private string lastError;
    private string id;

    public AvProVideoPlayer(string id, string url)
    {
        avProMediaPlayer = GameObject.Instantiate(Resources.Load<MediaPlayer>("AVPro/AVProMediaPlayer"), null);
        this.id = id;
        avProMediaPlayer.name = "_AvProMediaFor_" + id;
        avProMediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, url, false);
        avProMediaPlayer.AudioVolume = 0;
        currentState = VideoState.LOADING;
        avProMediaPlayer.Events.AddListener(AvProStateUpdater);
    }
    
    private void AvProStateUpdater(MediaPlayer arg0, MediaPlayerEvent.EventType newState, ErrorCode arg2)
    {
        switch (newState)
        {
            case MediaPlayerEvent.EventType.ReadyToPlay:
                avProTexture = avProMediaPlayer.TextureProducer.GetTexture(0);
                currentState = VideoState.READY;
                break;
            case MediaPlayerEvent.EventType.Started:
                ResizeVideoTexture();
                currentState = VideoState.PLAYING;
                break;
            case MediaPlayerEvent.EventType.Error:
                lastError = "AVProError " + arg2 + " for id " + id;
                currentState = VideoState.ERROR;
                break;
            case MediaPlayerEvent.EventType.ResolutionChanged:
                ResizeVideoTexture();
                break;
        }
    }


    public void Dispose()
    {
        avProMediaPlayer.CloseMedia();
        GameObject.Destroy(avProMediaPlayer.gameObject);
    }

    public void UpdateVideoTexture()
    {
        if (avProTexture == null)
            return;
       
        Graphics.CopyTexture(avProTexture, videoTexture);
    }

    public void PrepareTexture()
    {
        //Required BGRA32 for compatibility with AVPro texture
        videoTexture = new Texture2D(1, 1, TextureFormat.BGRA32, false, false);
    }

    public Texture2D GetTexture()
    {
        return videoTexture;
    }

    public void Play()
    {
       currentState = VideoState.PLAYING;
       SetVolume(1);
       avProMediaPlayer.Play();
    }

    public void SetSeekTime(float startTime)
    {
        avProMediaPlayer.Control.SeekFast(startTime);
    }

    public void Paused()
    {
        currentState = VideoState.PAUSED;
        avProMediaPlayer.Pause();
    }

    public void SetVolume(float volume)
    {
        avProMediaPlayer.AudioVolume = volume;
    }

    public VideoState GetCurrentState()
    {
        return currentState;
    }

    public float GetTime()
    {
        return (float)avProMediaPlayer.Control.GetCurrentTime();
    }

    public float GetDuration()
    {
        return (float)avProMediaPlayer.Info.GetDuration();
    }

    public void SetPlaybackRate(float playbackRate)
    {
        avProMediaPlayer.Control.SetPlaybackRate(playbackRate);
    }

    public void SetLoop(bool doLoop)
    {
        avProMediaPlayer.Control.SetLooping(doLoop);
    }
    
    private void ResizeVideoTexture()
    {
        avProTexture = avProMediaPlayer.TextureProducer.GetTexture(0);
        
        if (videoTexture == null || avProTexture == null)
            return;
        
        videoTexture.Resize(avProMediaPlayer.TextureProducer.GetTexture(0).width,
            avProMediaPlayer.TextureProducer.GetTexture(0).height);
        videoTexture.Apply();
    }

    public string GetLastError()
    {
        return lastError;
    }

}
