using System.Collections.Generic;
using System.Collections;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

using FeatherLight.Pro;
using TMPro;

public class ProjectWindowPopup : MonoBehaviour, IPointerDownHandler
{
    public static ProjectWindowPopup Instance;

    [SerializeField] private RectTransform projectPopup;
    [SerializeField] private RectTransform projectItemPopup;
    [SerializeField] private TMP_InputField[] inputFields;

    private CanvasGroup projectPopupCG;
    private CanvasGroup projectItemPopupCG;
    private bool isPopupHighlighted = false;
    private ProjectWindowContentItem cachedItem;
    PointerEventData pointerEventData;
    private bool canUseShortcuts = true;

    public bool CanUseShortcuts
    {
        get { return canUseShortcuts; }
        set { canUseShortcuts = value; }
    }

    private void Awake()
    {
        Instance = this;

        projectPopupCG = projectPopup.GetComponent<CanvasGroup>();
        projectItemPopupCG = projectItemPopup.GetComponent<CanvasGroup>();
    }

    public void SetHighlighted(bool value)
    {
        isPopupHighlighted = value;
    }

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
        ProjectWindowManager.Instance.SetMidAction(true);
        
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

        CanvasGroupHelper.SetActive(projectPopupCG, value);
        CanvasGroupHelper.SetActive(projectItemPopupCG, value);
    }

    public void SpecialFunction_DeleteItem()
    {
        if (cachedItem != null)
        {
            ProjectWindowManager.Instance.DestroyItem(cachedItem);
        }
    }

    private void Update()
    {
        canUseShortcuts = true;

        foreach(TMP_InputField field in inputFields)
        {
            if (field.isFocused) canUseShortcuts = false;
        }
    }
}