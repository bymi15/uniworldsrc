﻿using UnityEngine;
using UnityEngine.UI;

public class MicToggleButton : MonoBehaviour
{
    private Button button;
    private RawImage rawImage;
    public Texture micIcon, micMuteIcon;
    public bool muted = true;

    void Awake()
    {
        button = GetComponent<Button>();
        rawImage = GetComponent<RawImage>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (muted)
        {
            muted = false;
            rawImage.texture = micIcon;
            VoiceManager.UnmuteMic();
        }
        else
        {
            muted = true;
            rawImage.texture = micMuteIcon;
            VoiceManager.MuteMic();
        }
    }
}
