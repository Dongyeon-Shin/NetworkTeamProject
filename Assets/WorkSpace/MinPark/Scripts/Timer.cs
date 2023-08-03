using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TMP_Text tmp;
    private float time;

    private void Awake()
    {
        tmp = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        tmp.text = $"{200-(int)time}";
    }
}
