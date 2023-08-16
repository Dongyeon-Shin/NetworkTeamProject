using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class BaseScene : MonoBehaviourPunCallbacks
{
    protected LoadingUI loadingUI;
    protected float progress;

    private void Awake()
    {
        LoadingUI loadingUI = Resources.Load<LoadingUI>("UI/LoadingUI");
        this.loadingUI = Instantiate(loadingUI);
        this.loadingUI.transform.SetParent(transform);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LoadingRoutine());
    }

    public IEnumerator LoadSceneRoutine(int sceneNumber)
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        loadingUI.gameObject.SetActive(true);
        loadingUI.FadeOut();
        yield return new WaitForSeconds(0.5f);
        loadingUI.SetLoadingMessage("씬을 불러오는 중");
        StartCoroutine(SetProgressTimerRoutine(3f));
        yield return StartCoroutine(UpdateProgressRoutine(0.1f));
        PhotonNetwork.LoadLevel(sceneNumber);
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public IEnumerator LoadSceneRoutine(string sceneName)
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        loadingUI.gameObject.SetActive(true);
        loadingUI.FadeOut();
        yield return new WaitForSeconds(0.5f);
        loadingUI.SetLoadingMessage("씬을 불러오는 중");
        progress = 0;
        StartCoroutine(SetProgressTimerRoutine(3f));
        yield return StartCoroutine(UpdateProgressRoutine(0.1f));
        PhotonNetwork.LoadLevel(sceneName);
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    protected IEnumerator SetProgressTimerRoutine(float settingTime)
    {
        progress = 0;
        while (progress < 1)
        {
            yield return null;
            progress += Time.deltaTime / settingTime;
        }
        progress = 1f;
    }

    protected IEnumerator UpdateProgressRoutine(float targetProgress)
    {
        float currentProgress = loadingUI.Progress;
        StartCoroutine(CountLoadingTimeRoutine());
        while (progress < 1f)
        {
            loadingUI.Progress = Mathf.Lerp(currentProgress, targetProgress, progress);
            yield return null;
        }
        yield return null;
        //progress = 0;
    }

    private IEnumerator CountLoadingTimeRoutine()
    {
        WaitForSeconds waitASecond = new WaitForSeconds(1);
        while (progress < 1f)
        {
            yield return waitASecond;
            loadingUI.AddLoadingMessage(".");
        }
    }

    protected abstract IEnumerator LoadingRoutine();
}
