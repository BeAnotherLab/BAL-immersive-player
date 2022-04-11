using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FullsphereVideoPlayerSettings : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;

    public RenderTexture stereoRenderTexture;
    public Renderer monoMeshRender;

    // Start is called before the first frame update
    void Start()
    {
        SetDepthMode(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDepthMode(bool stereo) {
        if (stereo) { 
            _videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            _videoPlayer.targetTexture = stereoRenderTexture;
        }

        else _videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
    }
}
