using UnityEngine;

public class ActionMenuManager : MonoBehaviour
{
    [SerializeField]
    private ActionMenuItem[] m_menuItems;

    public static ActionMenuManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetItemActive(0);
    }

    public void SetItemActive(int _ActiveIndex)
    {
        foreach (ActionMenuItem item in m_menuItems)
        {
            item.UnselectItem();
        }

        m_menuItems[_ActiveIndex].SelectItem();
    }

    public void SetItemActive(ActionMenuItem _ActiveItem)
    {
        SetItemActive(FindObjectInArray(_ActiveItem));
    }

    private int FindObjectInArray(ActionMenuItem _Item)
    {
        int _output = 0;

        for (int i = 0; i < m_menuItems.Length; i++)
        {
            if (m_menuItems[i] == _Item)
                _output = i;
        }

        return _output;
    }
}
