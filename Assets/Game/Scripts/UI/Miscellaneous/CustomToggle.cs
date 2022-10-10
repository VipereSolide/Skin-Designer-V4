using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class CustomToggle : MonoBehaviour
{
    [SerializeField] private Image onImage;
    [SerializeField] private Image offImage;

    [SerializeField] private bool isOn;
    public bool IsOn => isOn;

    private void Start()
    {
        UpdateToggle();
    }

    public void Toggle()
    {
        isOn = !isOn;
        UpdateToggle();
    }

    private void UpdateToggle()
    {
        onImage.gameObject.SetActive(isOn);
        offImage.gameObject.SetActive(!isOn);
    }
}
