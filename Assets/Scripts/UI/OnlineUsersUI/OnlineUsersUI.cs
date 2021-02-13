using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class OnlineUsersUI : MonoBehaviour
{
    public static OnlineUsersUI Instance;
    public GameObject onlineUsersPanel, onlineUserTextPrefab;
    public TextMeshProUGUI channelLabel, onlineUserCountLabel;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            canvasGroup = GetComponentInChildren<CanvasGroup>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Connect")
        {
            CanvasUtils.Hide(canvasGroup);
        }
        else
        {
            CanvasUtils.Show(canvasGroup);
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void SetChannelLabel(string channel)
    {
        Instance.channelLabel.text = channel;
    }

    public void ClearList()
    {
        TextMeshProUGUI[] users = onlineUsersPanel.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI user in users)
        {
            Destroy(user.gameObject);
        }
    }

    public static void SetOnlineUsers(HashSet<string> users)
    {
        Instance.onlineUserCountLabel.text = "[" + users.Count.ToString() + "]";
        Instance.ClearList();
        TextMeshProUGUI textObject;
        foreach (string user in users)
        {
            textObject = Instantiate(Instance.onlineUserTextPrefab, Instance.onlineUsersPanel.transform).GetComponent<TextMeshProUGUI>();
            textObject.text = user;
        }
    }
}
