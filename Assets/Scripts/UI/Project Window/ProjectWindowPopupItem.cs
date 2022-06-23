using System.Text;

using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;

using FeatherLight.Pro;
using TMPro;

public class ProjectWindowPopupItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Settings")]

    [SerializeField]
    private bool disableOnClick = true;

    [SerializeField]
    private bool hasShortcut = false;

    [SerializeField]
    private bool applyShortcutOnSelected = false;

    [SerializeField]
    private bool showShortcut = true;

    [Space]

    [SerializeField]
    private float subWindowFadeSpeed = 0.25f;

    [SerializeField]
    private float timeToOpenSubWindow = 0.75f;

    [SerializeField]
    private float timeToCloseSubWindow = 1.25f;

    [Space]

    [SerializeField]
    private KeyCode[] shortCut;

    [SerializeField]
    public UnityEvent onClick;

    [SerializeField]
    public UnityEvent<ProjectWindowContentItem> onClickSelected;

    [Header("References")]

    [SerializeField]
    private ProjectWindowPopup popupManager;

    [SerializeField]
    private GameObject itemBackground;

    [SerializeField]
    private CanvasGroup subWindow;

    [SerializeField]
    private GameObject shortcutObject;

    [SerializeField]
    private TMP_Text shortcutText;

    private float highlightTime;
    private float unhighlightTime;
    private bool highlighted = false;

    private RectTransform subWindowRectTransform;

    private void Start()
    {
        if (subWindow != null)
            subWindowRectTransform = subWindow.GetComponent<RectTransform>();

        if (hasShortcut)
        {
            shortcutObject.SetActive(hasShortcut && showShortcut);

            StringBuilder keycodeText = new StringBuilder();

            foreach (KeyCode key in shortCut)
            {
                string keyText = key.ToString().ReplaceAll(new string[] { "Left", "Right" }, "");
                keyText = keyText.Replace("Control", "Ctrl");

                keycodeText.Append(keyText);

                if (key != shortCut[shortCut.Length - 1])
                {
                    keycodeText.Append(" + ");
                }
            }

            shortcutText.text = keycodeText.ToString();
        }
        else
        {
            shortcutObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (subWindow != null)
        {
            if (highlighted)
            {
                highlightTime += Time.deltaTime;

                if (!subWindow.interactable)
                {
                    if (highlightTime >= timeToOpenSubWindow)
                    {
                        StartCoroutine(CanvasGroupHelper.Fade(subWindow, true, subWindowFadeSpeed));
                    }
                }
            }
            else
            {
                if (subWindow.interactable)
                {
                    unhighlightTime += Time.deltaTime;

                    if (unhighlightTime >= timeToCloseSubWindow)
                    {
                        StartCoroutine(CanvasGroupHelper.Fade(subWindow, false, subWindowFadeSpeed));
                        highlightTime = 0;
                        unhighlightTime = 0;
                    }
                }
                else
                {
                    highlightTime = 0;
                    unhighlightTime = 0;
                }
            }
        }

        if (hasShortcut)
        {
            if (applyShortcutOnSelected)
            {
                if (KeyCodeHelper.AreKeyPressed(shortCut) && ProjectWindowManager.Instance.Selected != null)
                {
                    ExecuteOnSelected();
                }
            }
            else
            {
                if (KeyCodeHelper.AreKeyPressed(shortCut))
                {
                    Execute();
                }
            }
        }
    }

    private void Execute()
    {
        onClick.Invoke();

        if (disableOnClick)
        {
            if (subWindow != null)
            {
                CanvasGroupHelper.SetActive(subWindow, false);
            }

            itemBackground.SetActive(false);
            popupManager.SetActive(false);
        }
    }

    private void ExecuteOnSelected()
    {
        onClickSelected.Invoke(ProjectWindowManager.Instance.Selected);

        if (disableOnClick)
        {
            if (subWindow != null)
            {
                CanvasGroupHelper.SetActive(subWindow, false);
            }

            itemBackground.SetActive(false);
            popupManager.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        highlighted = true;

        if (itemBackground == null)
            return;

        itemBackground.SetActive(true);

        unhighlightTime = 0;
        highlightTime = 0;

        if (subWindow != null)
        {
            Vector2 realPos = new Vector2(subWindow.transform.position.x, subWindow.transform.position.y) + subWindowRectTransform.sizeDelta;
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        highlighted = false;

        if (itemBackground == null)
            return;

        itemBackground.SetActive(false);

        unhighlightTime = 0;
        highlightTime = 0;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left)
            return;

        Execute();
    }
}
