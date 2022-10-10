using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace SkinDesigner.Painter.UI
{
    public class ToolboxList : MonoBehaviour
    {
        [SerializeField] private int itemIndex = 0;
        [SerializeField] private ToolboxItem[] items = null;

        public ToolboxItem SelectedItem => items[itemIndex];

        private void Start()
        {
            SetSelected(itemIndex);
        }

        public void SetSelected(int index)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (i == index)
                {
                    items[i].SetState(ToolboxItem.ItemState.Selected);
                    itemIndex = i;
                }
                else
                {
                    if (items[i].CurrentState == ToolboxItem.ItemState.Selected)
                    {
                        items[i].SetState(ToolboxItem.ItemState.Normal);
                    }
                }
            }
        }

        public void SetSelected(ToolboxItem item)
        {
            int index = -1;

            for(int i = 0; i < items.Length; i++)
            {
                if (item == items[i])
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
            {
                Debug.LogError("The given ToolboxItem is not present in the list's items!");
                return;
            }

            SetSelected(index);
        }
    }
}