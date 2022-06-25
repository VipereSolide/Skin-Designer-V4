using System.Collections.Generic;
using System.Collections;

using UnityEngine.Events;
using UnityEngine;

using TMPro;

namespace SkinDesigner.Settings
{
    [ExecuteAlways]
    public class SettingsItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private SettingsItemOption itemOptionObject;

        [Header("Item Settings")]
        [SerializeField] private string itemName = "";

        [Header("Saving Settings")]
        [SerializeField] private string itemSavingName = "";

        [SerializeField]
        private bool syncItemSavingName = true;

        public TMP_Text ItemNameText
        {
            get { return itemNameText; }
        }

        public SettingsItemOption ItemOption
        {
            get { return itemOptionObject; }
        }

        public string ItemName
        {
            get { return itemName; }
        }

        public string ItemSavingName
        {
            get { return itemSavingName; }
        }

        public bool SyncItemSavingName
        {
            get { return syncItemSavingName; }
        }

        private void Update()
        {
            if (itemNameText == null)
                return;

            itemNameText.text = itemName;

            if (syncItemSavingName)
            {
                itemSavingName = itemName;
            }
        }
    }
}