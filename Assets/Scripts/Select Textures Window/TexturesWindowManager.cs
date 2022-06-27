using System.Collections.Generic;
using System.Collections;

using UnityEngine.Events;
using UnityEngine;

using SkinDesigner.Textures;
using SkinDesigner.Project;
using FeatherLight.Pro;

namespace SkinDesigner.SelectTexturesWindow
{
    public class TexturesWindowManager : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup windowCanvasGroup;

        [SerializeField]
        private Transform itemContainer;

        [SerializeField]
        private TexturesWindowMediaItem itemPrefab;

        private textureHolder currentCaller;
        private List<TexturesWindowMediaItem> spawnedPrefabs = new List<TexturesWindowMediaItem>();

        private bool isOpened = false;
        private Texture currentTexture;

        public Texture Texture
        {
            get { return currentTexture; }
        }

        public bool IsOpened
        {
            get { return isOpened; }
        }

        public UnityEvent onItemSelected;

        public void Call(textureHolder caller)
        {
            currentCaller = caller;

            isOpened = true;
            CanvasGroupHelper.SetActive(windowCanvasGroup, true);
            UpdateItemList();
        }

        public void Close()
        {
            currentTexture = null;
            CanvasGroupHelper.SetActive(windowCanvasGroup, false);
            isOpened = false;
        }

        private void UpdateItemList()
        {
            if (spawnedPrefabs.Count > 0)
            {
                foreach(TexturesWindowMediaItem item in spawnedPrefabs)
                {
                    Destroy(item.gameObject);
                }

                spawnedPrefabs.Clear();
            }

            ProjectWindowContentItem[] medias = ProjectWindowManager.Instance.Items;

            foreach(ProjectWindowContentItem media in medias)
            {
                if (media.GetType() == typeof(ProjectWindowContentFolder) || media.GetType() == typeof(ProjectWindowContentWeapon))
                    continue;

                Sprite texture = media.Background;
                string name = media.Name;

                TexturesWindowMediaItem item = Instantiate(itemPrefab, itemContainer);
                item.CreateItem(this);
                item.SetItemData(name, texture, media.HeldTexture.TexturePath);

                spawnedPrefabs.Add(item);
            }
        }

        public void SetItem(TexturesWindowMediaItem item)
        {
            if (currentCaller == null)
                return;

            if (!item.IsItemNone)
            {
                currentTexture = new TextureObject(item.TextureLink).GetTextureFromPath();
            }
            else
            {
                currentTexture = null;
            }
            
            CanvasGroupHelper.SetActive(windowCanvasGroup, false);

            onItemSelected.Invoke();
        }
    }
}
