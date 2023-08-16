using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class InGameChat : MonoBehaviour
{
    [SerializeField] public InputField input;
    public void InputSelect()
    {
        input.Select();
    }
}
