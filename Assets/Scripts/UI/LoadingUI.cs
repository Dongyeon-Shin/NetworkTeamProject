using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    // 임시 싱글용 로딩 UI
    private Slider loadingBar;
    public float Progress { get { return loadingBar.value; } set { loadingBar.value = value; } }
    private Animator animator;
    private GraphicRaycaster graphicRaycaster;
    private GameObject currentLoadingImage;
    private TMP_Text loadingText;
    private StringBuilder loadingMessage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        loadingBar = GetComponentInChildren<Slider>();
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        loadingText = GetComponentInChildren<TMP_Text>();
        loadingMessage = new StringBuilder();
        currentLoadingImage = transform.GetChild(0).GetChild(0).gameObject;
    }

    public void SetImage(int imageIndex)
    {
        currentLoadingImage.SetActive(false);
        currentLoadingImage = transform.GetChild(0).GetChild(imageIndex).gameObject;
        currentLoadingImage.SetActive(true);
    }

    public void FadeIn()
    {
        animator.SetTrigger("FadeIn");
        graphicRaycaster.enabled = false;
    }

    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
        graphicRaycaster.enabled = true;
    }

    public void SetLoadingMessage(string message)
    {
        loadingMessage.Clear();
        loadingMessage.Append(message);
        loadingText.text = message;
    }

    public void AddLoadingMessage(string message)
    {
        loadingMessage.Append(message);
        loadingText.text = loadingMessage.ToString();
    }
}
