using System;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

using SkinDesigner.Textures;
using TMPro;

public class ProjectWindowContentItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected bool canBeDragged = true;

    [Space()]
    [SerializeField] protected Image m_itemBackground;
    [SerializeField] protected Color m_itemBackgroundHighlightColor;

    [Space()]
    [SerializeField] protected Sprite m_background;
    [SerializeField] protected Image m_backgroundImage;
    [SerializeField] protected Vector3 m_highlightedSize;
    [SerializeField] protected float m_highlightSpeed;

    [Space()]
    [SerializeField] protected Color32 m_textDisabledColor = Color.white;
    [SerializeField] protected Color32 m_textHighlightedColor = Color.white;
    [SerializeField] protected TMP_Text m_itemNameText;
    [SerializeField] protected string m_name;
    [SerializeField] protected string m_childrenPath = string.Empty;

    protected TextureObject heldTexture = null;

    protected Vector2 m_lastMousePosition;
    protected bool m_canBeDropped = true;
    protected bool m_isBeingDropped = false;

    protected Transform m_contentContainer;
    protected Transform m_dropItemContainer;
    protected bool m_isHighlighted = false;
    protected bool isSelected = false;

    public Action onClick;

    public TextureObject HeldTexture
    {
        get { return heldTexture; }
    }

    public string ChildrenPath
    {
        get { return m_childrenPath; }
    }

    public Sprite Background
    {
        get { return m_background; }
    }

    public string Name
    {
        get { return m_name; }
    }

    public bool CanBeDragged
    {
        get { return canBeDragged; }
    }

    public bool isHighlighted
    {
        get { return m_isHighlighted; }
    }

    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; if (isSelected) onClick?.Invoke(); }
    }

    public void SetDirectory(ProjectWindowContentFolder folder)
    {
        m_childrenPath = folder.Path;
        folder.AddChild(this);
    }
    public void SetDirectory(string folder)
    {
        m_childrenPath = folder;

        foreach (ProjectWindowContentItem item in ProjectWindowManager.Instance.Items)
        {
            ProjectWindowContentFolder folderVersion = (ProjectWindowContentFolder)(item as ProjectWindowContentFolder);

            if (folderVersion == null)
            {
                continue;
            }

            if (folderVersion.Path == folder)
            {
                folderVersion.AddChild(this);
            }
        }
    }
    protected void StartItem()
    {
        UpdateItem();
    }
    public void SetData(string _Name, Sprite _Background, string link)
    {
        this.m_name = _Name;
        this.m_background = _Background;
        this.heldTexture.TexturePath = link;

        UpdateItem();
    }
    public void SetData(string _Name, Sprite _Background)
    {
        this.m_name = _Name;
        this.m_background = _Background;

        UpdateItem();
    }
    private void Start()
    {
        this.StartItem();
    }
    protected void UpdateItem()
    {
        m_itemNameText.text = m_name;
        m_backgroundImage.sprite = m_background;
    }
    private void OnDisable()
    {
        HighlightItem(false);
    }
    private void HighlightItem(bool _Value)
    {
        m_isHighlighted = _Value;

        if (_Value)
        {
            ProjectWindowManager.Instance.SetHighlighted(this);
        }
        else
        {
            if (ProjectWindowManager.Instance.Highlighted == this)
            {
                ProjectWindowManager.Instance.SetHighlighted(null);
            }
        }

        UpdateItem();
    }
    protected void UpdateGraphics()
    {
        if (m_isHighlighted || isSelected)
        {
            m_backgroundImage.transform.localScale = Vector3.Lerp(m_backgroundImage.transform.localScale, m_highlightedSize, Time.deltaTime * m_highlightSpeed);
            m_itemNameText.color = m_textHighlightedColor;
            m_itemBackground.color = m_itemBackgroundHighlightColor;
        }
        else
        {
            m_backgroundImage.transform.localScale = Vector3.Lerp(m_backgroundImage.transform.localScale, Vector3.one, Time.deltaTime * m_highlightSpeed);
            m_itemNameText.color = m_textDisabledColor;
            m_itemBackground.color = new Color32(0, 0, 0, 1);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        HighlightItem(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HighlightItem(false);
    }
    void Update()
    {
        UpdateGraphics();

        if (Input.GetMouseButtonDown(0))
        {
            m_lastMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
}