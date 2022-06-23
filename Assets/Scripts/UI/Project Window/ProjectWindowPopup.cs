using System.Collections.Generic;
using System.Collections;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

using FeatherLight.Pro;

public class ProjectWindowPopup : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private RectTransform projectPopup;
    [SerializeField] private RectTransform projectItemPopup;

    private CanvasGroup projectPopupCG;
    private CanvasGroup projectItemPopupCG;

    private bool isPopupHighlighted = false;
    private ProjectWindowContentItem cachedItem;

    private void Awake()
    {
        projectPopupCG = projectPopup.GetComponent<CanvasGroup>();
        projectItemPopupCG = projectItemPopup.GetComponent<CanvasGroup>();
    }

    public void SetHighlighted(bool value)
    {
        isPopupHighlighted = value;
    }

    PointerEventData pointerEventData;
    public void OnPointerDown(PointerEventData data)
    {
        if (!isPopupHighlighted)
        {
            if (projectPopup.gameObject.activeSelf || projectItemPopup.gameObject.activeSelf)
            {
                SetActive(false);
            }
        }
    }

    public void OpenPopup(int i, ProjectWindowContentItem caller)
    {
        cachedItem = caller;

        CanvasGroupHelper.SetActive(projectPopupCG, false);
        CanvasGroupHelper.SetActive(projectItemPopupCG, false);
        CanvasGroup currentPopup = null;

        switch (i)
        {
            case 0:
                currentPopup = projectPopupCG;
                break;
            case 1:
                currentPopup = projectItemPopupCG;
                break;
            default:
                currentPopup = projectPopupCG;
                break;
        }

        CanvasGroupHelper.SetActive(currentPopup, true);

        RectTransform popupTransform = currentPopup.GetComponent<RectTransform>();
        Vector2 screenPosition = (popupTransform.sizeDelta / 2) + new Vector2(-10, -10);
        popupTransform.transform.position = Input.mousePosition + screenPosition.ToVector3();
    }

    public void SetActive(bool value)
    {
        CanvasGroupHelper.SetActive(projectPopupCG, false);
        CanvasGroupHelper.SetActive(projectItemPopupCG, false);
    }

    public void SpecialFunction_DeleteItem()
    {
        if (cachedItem != null)
        {
            ProjectWindowManager.Instance.DestroyItem(cachedItem);
        }
    }
}
