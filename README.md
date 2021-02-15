# UniWorld
Source code for UniWorld - UCL MEng Final Year Project with IBM

## Setup Guide
1. Install [Unity Hub](https://unity3d.com/get-unity/download) and [Unity 2019.4.10](https://unity3d.com/get-unity/download/archive)
2. Download this repository and move it to the Unity project directory
3. Launch Unity and wait for it to finish importing the assets
4. Go to Window > Photon Unity Network > Highlight Server Settings (or open `Assets/Photon/PhotonUnityNetworking/Resources/PhotonServerSettings.asset`). Make sure to fill in the App Id PUN value. This can be obtained by registering an account at [Photon Engine](https://www.photonengine.com/) and creating a Photon PUN app.

## Note
- The contents of `Assets/Scripts/Networking/Voice` will not be included
- You will need to purchase the [Voice Pro - WebGL, Mobile, Desktop](https://assetstore.unity.com/packages/tools/input-management/voice-pro-webgl-mobile-desktop-169274) asset from the Unity Asset Store and copy the contents of the asset into the empty Voice folder
