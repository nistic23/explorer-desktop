using System.Collections;
using System.Collections.Generic;
using DCL.Components.Video.Plugin;
using UnityEngine;
using UnityEngine.Assertions;

public class AVProPluginWrapper_Tests : MonoBehaviour
{

    private List<TestLink> linksToTest;
    
    void Start()
    {
        linksToTest = new List<TestLink>();
        /*linksToTest.Add(new TestLink("MP3", "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3"));
        linksToTest.Add(new TestLink("loop","https://player.vimeo.com/external/691621058.m3u8?s=a2aa7b62cd0431537ed53cd699109e46d0de8575"));
        linksToTest.Add(new TestLink("HTTPS+HLS 1", "https://player.vimeo.com/external/552481870.m3u8?s=c312c8533f97e808fccc92b0510b085c8122a875"));
        linksToTest.Add(new TestLink("HTTPS+HLS 2",
            "https://player.vimeo.com/external/575854261.m3u8?s=d09797037b7f4f1013d337c04836d1e998ad9c80"));
        linksToTest.Add(new TestLink("HTTP+MP4", "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));
        linksToTest.Add(new TestLink("HTTP+WEBM", "http://techslides.com/demos/sample-videos/small.webm"));
        linksToTest.Add(new TestLink("HTTP+OGV", "http://techslides.com/demos/sample-videos/small.ogv"));
        linksToTest.Add(new TestLink("HTTPS+HLS", "https://player.vimeo.com/external/691415562.m3u8?s=65096902279bbd8bb19bf9e2b9391c4c7e510402"));*/
        linksToTest.Add(new TestLink("JPEG", "https://ironapeclub.com/wp-content/uploads/2022/01/ironape-club-poster.jpg"));

        StartCoroutine(TestFormatCo());
    }



    IEnumerator TestFormatCo()
    {

        TestLink testLink = linksToTest[0];
        string id = testLink.id;
        string url = testLink.url;
        
        Debug.Log("BEGINING TEST FOR " + id + " AT URL " + url);
        VideoPluginWrapper_AVPro pluginWrapperAvPro = new VideoPluginWrapper_AVPro();
        pluginWrapperAvPro.Create(id,url,true);

        while (pluginWrapperAvPro.GetState(id) == VideoState.LOADING)
        {
            yield return null;
        }
        
        pluginWrapperAvPro.Play(id,0);
        pluginWrapperAvPro.SetVolume(id, 1);

        if (pluginWrapperAvPro.GetError(id) == null)
        {
            Debug.Log(id + " loaded and played succesfully!");
        }
        else
        {
            Debug.Log(id + " failed with error " + pluginWrapperAvPro.GetError(id));
        }
       // pluginWrapperAvPro.Remove(id);
        linksToTest.Remove(testLink);
        if (linksToTest.Count > 0)
        {
            StartCoroutine(TestFormatCo());
        }

    }
}

public class TestLink
{
    public string id;
    public string url;

    public TestLink(string id, string url)
    {
        this.id = id;
        this.url = url;
    }
}
