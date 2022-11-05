using System.Collections.Generic;
using System.Collections;

using UnityEngine.Events;
using UnityEngine;

using SkinDesigner.Inspector;
using SkinDesigner.Textures;
using SkinDesigner.Project;

using FeatherLight.Pro.Console;
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

        private InspectorHolder currentCaller;
        private List<TexturesWindowMediaItem> spawnedPrefabs = new List<TexturesWindowMediaItem>();

        private bool isOpened = false;
        private TexturesWindowMediaItem current;

        public bool IsOpened
        {
            get { return isOpened; }
        }

        public TexturesWindowMediaItem Current
        {
            get { return current; }
        }

        public UnityEvent onItemSelected;

        public void Call(InspectorHolder caller)
        {
            currentCaller = caller;

            isOpened = true;
            CanvasGroupHelper.SetActive(windowCanvasGroup, true);
            UpdateItemList();
        }

        public void Close()
        {
            current = null;
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

                TexturesWindowMediaItem item = Instantiate(itemPrefab, itemContainer);

                item.CreateItem(this);
                item.SetItemData(media);
                spawnedPrefabs.Add(item);
            }
        }

        public void SetItem(TexturesWindowMediaItem item)
        {
            if (currentCaller == null)
            {
                Console.LogError($"Textures Window > You cannot set an item if there is no inspector holder active.");
                return;
            }
            
            currentCaller.Set(item.Item);
            CanvasGroupHelper.SetActive(windowCanvasGroup, false);

            onItemSelected.Invoke();
        }
    }
}
