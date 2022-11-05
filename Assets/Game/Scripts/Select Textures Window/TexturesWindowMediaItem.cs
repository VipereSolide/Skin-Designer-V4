using System.Collections.Generic;
using System.Collections;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

using FeatherLight.Pro;
using TMPro;

namespace SkinDesigner.SelectTexturesWindow
{
    public class TexturesWindowMediaItem : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private TMP_Text itemNameText;

        [SerializeField]
        private RawImage itemTextureObject;

        [Space]

        [SerializeField] private ProjectWindowContentItem item;

        [Space()]

        [SerializeField]
        private TexturesWindowManager manager;

        public ProjectWindowContentItem Item
        {
            get { return item; }
            set { item = value; }
        }

        public TexturesWindowMediaItem CreateItem(TexturesWindowManager manager)
        {
            this.manager = manager;

            return this;
        }

        public void SetItemData(ProjectWindowContentItem item)
        {
            itemNameText.text = item.Name;
            itemTextureObject.texture = item.Background.texture;

            this.item = item;
            transform.name = item.Name;
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (data.clickCount > 1)
            {
                if (manager == null)
                    return;

                manager.SetItem(this);
            }
        }
    }
}
