using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    //=====================================
    private static PoolManager poolManager;
    private static ResourceManager resourceManager;
    private static UIManager uiManager;
    private static EventManager eventManager;
    private static SoundManager soundManager;
    private static SceneManager sceneManager;

    public static GameManager Instance { get { return instance; } }
    //==============================================================
    public static PoolManager Pool { get { return poolManager; } }
    public static ResourceManager Resource { get { return resourceManager; } }
    public static UIManager UI { get { return uiManager; } }
    public static EventManager Event { get { return eventManager; } }
    public static SoundManager Sound { get { return soundManager; } }
    public static SceneManager Scene { get { return sceneManager; } }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        GameObject resourceObj = new GameObject();
        resourceObj.name = "ResourceManager";
        resourceObj.transform.parent = transform;
        resourceManager = resourceObj.AddComponent<ResourceManager>();

        GameObject poolObj = new GameObject();
        poolObj.name = "PoolManager";
        poolObj.transform.parent = transform;
        poolManager = poolObj.AddComponent<PoolManager>();

        GameObject uiObj = new GameObject();
        uiObj.name = "UIManager";
        uiObj.transform.parent = transform;
        uiManager = uiObj.AddComponent<UIManager>();

        GameObject eventObj = new GameObject();
        eventObj.name = "EventManager";
        eventObj.transform.parent = transform;
        eventManager = eventObj.AddComponent<EventManager>();

        GameObject soundObj = new GameObject();
        soundObj.name = "SoundManager";
        soundObj.transform.parent = transform;
        soundManager = soundObj.AddComponent<SoundManager>();

        GameObject sceneObj = new GameObject();
        sceneObj.name = "SceneManager";
        sceneObj.transform.parent = transform;
        sceneManager = sceneObj.AddComponent<SceneManager>();
    }
}
