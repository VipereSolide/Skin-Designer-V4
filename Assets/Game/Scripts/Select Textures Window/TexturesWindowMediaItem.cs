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

        [SerializeField]
        private new string name = "";

        [SerializeField]
        private Texture2D texture;

        [SerializeField]
        private string textureLink = "";

        [SerializeField]
        private bool itemIsNone = false;

        [Space()]

        [SerializeField]
        private TexturesWindowManager manager;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Sprite TextureInSprite
        {
            get { return texture.ToSprite(); }
            set { texture = value.texture; }
        }

        public string TextureLink
        {
            get { return textureLink; }
            set { textureLink = value; }
        }
        
        public bool IsItemNone
        {
            get { return itemIsNone; }
            set { itemIsNone = value; }
        }

        public TexturesWindowMediaItem CreateItem(TexturesWindowManager manager)
        {
            this.manager = manager;

            return this;
        }

        public void SetItemData(string name, Texture2D texture, string textureLink)
        {
            itemNameText.text = name;
            itemTextureObject.texture = texture;

            transform.name = name;

            this.name = name;
            this.texture = texture;
            this.textureLink = textureLink;
        }

        public void SetItemData(string name, Sprite texture, string textureLink)
        {
            Texture2D t2d = texture.texture;

            itemNameText.text = name;
            itemTextureObject.texture = t2d;

            transform.name = name;

            this.name = name;
            this.texture = t2d;
            this.textureLink = textureLink;
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (data.clickCount > 0)
            {
                if (manager == null)
                    return;

                manager.SetItem(this);
            }
        }
    }
}
