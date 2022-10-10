using System;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

using SkinDesigner.Textures;
using FeatherLight.Pro;
using TMPro;

public class ProjectWindowContentItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item Properties")]
    [SerializeField] protected new string name;
    [SerializeField] protected string childrenPath = string.Empty;
    [SerializeField] protected Sprite background;

    [Header("Behaviour Settings")]
    [SerializeField] protected bool canBeDragged = true;

    [Header("References")]
    [SerializeField] protected TMP_Text itemNameText;

    [Space]
    [SerializeField] protected Image backgroundObject;
    [SerializeField] protected Image backgroundImageObject;

    [Space]
    [SerializeField] protected CanvasGroup group;

    [Header("Graphic Settings")]
    [SerializeField] protected float highlightSpeed;
    [Space]
    [SerializeField] protected Color highlightBackgroundColor;
    [SerializeField] protected Color selectedBackgroundColor;

    [Space]
    [SerializeField] protected Color32 disabledTextColor = Color.white;
    [SerializeField] protected Color32 highlightedTextColor = Color.white;
    [SerializeField] protected Color32 selectedTextColor = Color.white;

    [Space]
    [SerializeField] protected Vector3 highlightedSize;

    protected TextureObject heldTexture = new TextureObject();
    
    protected Vector2 lastMousePosition;
    protected bool canBeDropped = true;
    protected bool isBeingDropped = false;

    protected Transform contentContainer;
    protected Transform dropItemContainer;
    protected bool isHighlighted = false;
    protected bool isSelected = false;

    public Action onClick;

    public CanvasGroup Group { get => group; }
    public TextureObject HeldTexture { get => heldTexture; }
    public string ChildrenPath { get => childrenPath; }
    public Sprite Background { get => background; }
    public string Name { get => name; }
    public bool CanBeDragged { get => canBeDragged; set => canBeDragged = value; }
    public bool IsHighlighted { get => isHighlighted; }
    public bool IsSelected
    { 
        get => isSelected;
        set
        {
            isSelected = value;

            if (isSelected)
            {
                onClick?.Invoke();
            }
        }
    }

    public virtual void SetDirectory(ProjectWindowContentFolder folder)
    {
        transform.SetSiblingIndex(folder.transform.GetSiblingIndex() + 1);
        childrenPath = folder.Path;
        folder.AddChild(this);
    }
    public void SetDirectory(string folder)
    {
        childrenPath = folder;

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
        this.name = _Name;
        this.background = _Background;

        this.heldTexture.TexturePath = link;
        this.heldTexture.GetFileInfo();

        UpdateItem();
    }
    public void SetData(string _Name, Sprite _Background)
    {
        this.name = _Name;
        this.background = _Background;

        UpdateItem();
    }
    private void Start()
    {
        this.StartItem();
    }
    protected void UpdateItem()
    {
        itemNameText.text = name;
        backgroundImageObject.sprite = background;
    }
    private void OnDisable()
    {
        HighlightItem(false);
    }
    private void HighlightItem(bool _Value)
    {
        isHighlighted = _Value;

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
        Color targetBackgroundColor = new Color32(255,255,255, 0);
        Color targetTextColor = disabledTextColor;

        if (isHighlighted)
        {
            targetBackgroundColor = highlightBackgroundColor;
            targetTextColor = highlightedTextColor;
        }

        if (isSelected)
        {
            targetBackgroundColor = selectedBackgroundColor;
            targetTextColor = selectedTextColor;
        }

        backgroundImageObject.transform.localScale = Vector3.Lerp(backgroundImageObject.transform.localScale, (isHighlighted || isSelected) ? highlightedSize : Vector3.one, Time.deltaTime * highlightSpeed);
        backgroundObject.color = Color.Lerp(backgroundObject.color, targetBackgroundColor, Time.deltaTime * highlightSpeed);
        itemNameText.color = targetTextColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        HighlightItem(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HighlightItem(false);
    }
    private void CheckIsHeldTextureUpToDate()
    {
        if (heldTexture.HasTextureFileChanged())
        {
            // Texture Has Changed.
            Debug.Log("Texture has Changed");
            Texture newBackground = heldTexture.GetTextureFromPath();
            heldTexture.GetFileInfo();
            
            background = TextureHelper.ToSprite((Texture2D)newBackground);

            UpdateItem();
        }
    }
    private void Update()
    {
        CheckIsHeldTextureUpToDate();
        UpdateGraphics();

        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
}