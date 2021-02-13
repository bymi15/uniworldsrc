using FrostweepGames.Plugins.Native;
using FrostweepGames.WebGLPUNVoice;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager Instance;

    private Recorder recorder;
    private Listener listener;

    private void Awake()
    {
        if(Instance == null)
        {
            recorder = GetComponent<Recorder>();
            listener = GetComponent<Listener>();
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (this != Instance)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CustomMicrophone.RequestMicrophonePermission();
    }

    public static void MuteMic()
    {
        Instance.recorder.StopRecord();
    }
    public static void UnmuteMic()
    {
        Instance.recorder.StartRecord();
    }

    private static void ToggleRemoteMuteStatus(bool status)
    {
        Instance.listener.SetMuteStatus(status);
    }

    private static void ToggleDebugEcho(bool status)
    {
        Instance.recorder.debugEcho = status;
    }

    private static void ToggleReliableTransmission(bool status)
    {
        Instance.recorder.reliableTransmission = status;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Connect")
        {
            listener.StartListen();
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}