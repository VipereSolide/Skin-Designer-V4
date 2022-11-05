using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

using SkinDesigner.Textures;
using SkinDesigner.Project;

using FeatherLight.Pro;

using TMPro;

public class ProjectManagerItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Item")]

    [SerializeField]
    private string m_name;

    [Header("Graphics")]

    [SerializeField]
    private float m_highlightSpeed;

    [Space]

    [Range(0,2)]
    [SerializeField]
    private float m_highlightedSize;

    [Space]

    [SerializeField]
    private Color m_textDisabledColor = Color.white;

    [SerializeField]
    private Color m_textHighlightedColor = Color.white;

    [SerializeField]
    private Color m_itemBackgroundHighlightColor;

    [Space]
    [SerializeField] private Sprite m_background;

    [Header("References")]

    [SerializeField]
    private TMP_Text m_itemNameText;

    [SerializeField]
    private Image m_backgroundImage;

    [SerializeField]
    private Image m_itemBackground;

    [SerializeField]
    private Project m_associatedInfo;


    private bool m_isHighlighted = false;

    public Sprite Background
    {
        get { return m_background; }
    }
    public string Name
    {
        get { return m_name; }
    }
    public Project AssociatedInfo
    {
        get { return m_associatedInfo; }
    }
    public void SetData(string _Name, Sprite _Background)
    {
        this.m_name = _Name;
        this.m_background = _Background;

        UpdateItem();
    }
    public void AssociateWithInfo(Project info)
    {
        m_associatedInfo = info;

        m_name = info.ProjectName;

        StartCoroutine(LoadData());

        UpdateItem();
    }

    private IEnumerator LoadData()
    {
        /*
         if (info.WeaponData.Length > 0)
        {
            string texturePath = info.WeaponData[0].WeaponTextures.Albedo;

            bool isPathNull = (string.IsNullOrEmpty(texturePath) || string.IsNullOrWhiteSpace(texturePath));

            if (texturePath == "NULL" || isPathNull)
            {
                Texture2D backgroundTexture = new Texture2D(1, 1);
                backgroundTexture.SetPixel(0, 0, Color.black);
                backgroundTexture.Apply();
                m_background = backgroundTexture.ToSprite();
            }
            else
            {
                TextureObject textureObject = new TextureObject(texturePath);
                m_background = ((Texture2D)textureObject.GetTextureFromPath()).ToSprite();
            }
        }
        else
        {
            Texture2D backgroundTexture = new Texture2D(1, 1);
            backgroundTexture.SetPixel(0, 0, Color.black);
            backgroundTexture.Apply();
            m_background = backgroundTexture.ToSprite();
        }
         */
        yield return null;
    }

    private void UpdateItem()
    {
        m_itemNameText.text = m_name;
        m_backgroundImage.sprite = m_background;
    }
    private void HighlightItem(bool _Value)
    {
        m_isHighlighted = _Value;
        UpdateItem();
    }
    protected void UpdateGraphics()
    {
        Color targetNameColor = m_textDisabledColor;
        Color targetBackgroundColor = new Color(1, 1, 1, 0);

        if (m_isHighlighted)
        {
            targetNameColor = m_textHighlightedColor;
            targetBackgroundColor = m_itemBackgroundHighlightColor;
        }

        m_itemNameText.color = Color.Lerp(m_itemNameText.color, targetNameColor, Time.deltaTime * m_highlightSpeed);
        m_itemBackground.color = Color.Lerp(m_itemBackground.color, targetBackgroundColor, Time.deltaTime * m_highlightSpeed);
        m_backgroundImage.transform.localScale = Vector3.Lerp(m_backgroundImage.transform.localScale, Vector3.one * ((m_isHighlighted) ? m_highlightedSize : 1), Time.deltaTime * m_highlightSpeed);
    }
    protected void StartItem()
    {
        UpdateItem();
    }

    private void Update()
    {
        UpdateGraphics();
    }
    private void Start()
    {
        StartItem();
    }
    private void OnDisable()
    {
        HighlightItem(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        HighlightItem(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HighlightItem(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.clickCount >= 1)
        {
            ProjectManagerUI.Instance.SelectProject(this.m_associatedInfo);
            //ProjectManagerHUD.Instance.m_info = this.m_associatedInfo;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
