using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace SkinDesigner.Painter.UI
{
    public class ToolboxItem : MonoBehaviour,
        IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public enum ItemState { Normal, Hover, Selected }

        [SerializeField] private ItemState currentState;
        [SerializeField] private ToolboxList associatedList;
        [SerializeField] private Image itemIcon;
        
        [Space]
        [SerializeField] [Range(5,15)] private float itemTransitionSpeed = 10f;

        [Space]
        [SerializeField] [Range(0, 1)] private float itemNormalAlpha = 0.13f;
        [SerializeField] [Range(0, 1)] private float itemHoverAlpha = 0.5f;
        [SerializeField] [Range(0, 1)] private float itemSelectedAlpha = 1;

        public ItemState CurrentState => currentState;

        public void SetState(ItemState state)
        {
            currentState = state;
        }

        private float GetAlphaFromItemState()
        {
            return
                (currentState == ItemState.Hover) ? itemHoverAlpha :
                (currentState == ItemState.Selected) ? itemSelectedAlpha :
                itemNormalAlpha;
        }

        private void Update()
        {
            if (itemIcon.color.a != GetAlphaFromItemState())
            {
                float transition = Mathf.Lerp(itemIcon.color.a, GetAlphaFromItemState(), Time.deltaTime * itemTransitionSpeed);
                
                if (transition < GetAlphaFromItemState() + 0.1f && transition > GetAlphaFromItemState() - 0.1f)
                    transition = GetAlphaFromItemState();

                itemIcon.color = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, transition);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (associatedList == null)
                return;

            associatedList.SetSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (currentState != ItemState.Selected)
            {
                currentState = ItemState.Hover;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (currentState == ItemState.Hover)
            {
                currentState = ItemState.Normal;
            }
        }
    }
}