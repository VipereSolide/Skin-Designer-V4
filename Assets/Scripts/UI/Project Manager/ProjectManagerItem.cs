using System;
using System.IO;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

using SkinDesigner.Project;
using FeatherLight.Pro;
using TMPro;

public class ProjectManagerItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ProjectReaderInfo m_associatedInfo;
    [SerializeField] private Image m_itemBackground;
    [SerializeField] private Color m_itemBackgroundHighlightColor;

    [Space()]
    [SerializeField] private Sprite m_background;
    [SerializeField] private Image m_backgroundImage;
    [SerializeField] private Vector3 m_highlightedSize;
    [SerializeField] private float m_highlightSpeed;

    [Space()]
    [SerializeField] private Color32 m_textDisabledColor = Color.white;
    [SerializeField] private Color32 m_textHighlightedColor = Color.white;
    [SerializeField] private TMP_Text m_itemNameText;
    [SerializeField] private string m_name;
    private bool m_isHighlighted = false;

    public Sprite Background
    {
        get { return m_background; }
    }

    public string Name
    {
        get { return m_name; }
    }

    public ProjectReaderInfo AssociatedInfo
    {
        get { return m_associatedInfo; }
    }

    protected void StartItem()
    {
        UpdateItem();
    }

    private void Start()
    {
        StartItem();
    }

    public void SetData(string _Name, Sprite _Background)
    {
        this.m_name = _Name;
        this.m_background = _Background;

        UpdateItem();
    }

    public void AssociateWithInfo(ProjectReaderInfo info)
    {
        m_associatedInfo = info;

        m_name = info.Name;

        if (info.Weapons.Count > 0)
        {
            string texturePath = info.Weapons[0].MainTextures[0].TexturePath;
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
                Texture2D backgroundTexture = (Texture2D)info.Weapons[0].MainTextures[0].Texture;

                /* if (backgroundTexture == null)
                {
                    byte[] backgroundTextureBytes = File.ReadAllBytes(texturePath);
                    backgroundTexture.LoadImage(backgroundTextureBytes);
                    backgroundTexture.Apply();
                    m_background = backgroundTexture.ToSprite();
                } */

                backgroundTexture = new Texture2D(2,2);
            }
        }
        else
        {
            Texture2D backgroundTexture = new Texture2D(1, 1);
            backgroundTexture.SetPixel(0, 0, Color.black);
            backgroundTexture.Apply();
            m_background = backgroundTexture.ToSprite();
        }

        UpdateItem();
    }

    private void UpdateItem()
    {
        m_itemNameText.text = m_name;
        m_backgroundImage.sprite = m_background;

        if (m_isHighlighted)
        {
            m_itemNameText.color = m_textHighlightedColor;
            m_itemBackground.color = m_itemBackgroundHighlightColor;
        }
        else
        {
            m_itemNameText.color = m_textDisabledColor;
            m_itemBackground.color = new Color32(0, 0, 0, 0);
        }
    }

    private void OnDisable()
    {
        HighlightItem(false);
    }

    private void HighlightItem(bool _Value)
    {
        m_isHighlighted = _Value;
        UpdateItem();
    }

    void Update()
    {
        UpdateGraphics();
    }

    protected void UpdateGraphics()
    {
        if (m_isHighlighted)
        {
            m_backgroundImage.transform.localScale = Vector3.Lerp(m_backgroundImage.transform.localScale, m_highlightedSize, Time.deltaTime * m_highlightSpeed);
        }
        else
        {
            m_backgroundImage.transform.localScale = Vector3.Lerp(m_backgroundImage.transform.localScale, Vector3.one, Time.deltaTime * m_highlightSpeed);
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.clickCount >= 1)
        {
            ProjectManagerHUD.Instance.SelectProject(this.m_associatedInfo);
            ProjectManagerHUD.Instance.m_info = this.m_associatedInfo;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
