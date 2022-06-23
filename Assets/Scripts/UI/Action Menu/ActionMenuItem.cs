using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionMenuItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color32 m_disabledItemBackgroundColor = Color.white;
    [SerializeField] private Color32 m_highlightedItemBackgroundColor = Color.white;
    [SerializeField] private Color32 m_selectedItemBackgroundColor = Color.white;

    [Space()]
    [SerializeField] private Color32 m_disabledIconColor = Color.white;
    [SerializeField] private Color32 m_highlightedIconColor = Color.white;
    [SerializeField] private Color32 m_selectedIconColor = Color.white;

    [Space()]
    [SerializeField] private Image m_itemBackground;
    [SerializeField] private Image m_iconImage;

    private bool m_isHighlighted = false;
    private bool m_isSelected = false;

    public bool IsHighlighted
    {
        get { return m_isHighlighted; }
    }

    public bool IsSelected
    {
        get { return m_isSelected; }
    }

    public void SelectItem()
    {
        m_isSelected = true;
        UpdateItemImages();
    }

    public void UnselectItem()
    {
        m_isSelected = false;
        UpdateItemImages();
    }

    private void HighlightItem(bool _Value)
    {
        m_isHighlighted = _Value;
        UpdateItemImages();
    }

    private void UpdateItemImages()
    {
        if (m_isHighlighted)
        {
            m_iconImage.color = m_highlightedIconColor;
            m_itemBackground.color = m_highlightedItemBackgroundColor;
        }
        else
        {
            m_iconImage.color = m_disabledIconColor;
            m_itemBackground.color = m_disabledItemBackgroundColor;
        }

        if (m_isSelected)
        {
            m_iconImage.color = m_selectedIconColor;
            m_itemBackground.color = m_selectedItemBackgroundColor;
        }
        else
        {
            m_iconImage.color = m_disabledIconColor;
            m_itemBackground.color = m_disabledItemBackgroundColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ActionMenuManager.Instance.SetItemActive(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!m_isSelected)
            HighlightItem(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_isHighlighted)
            HighlightItem(false);
    }
}
