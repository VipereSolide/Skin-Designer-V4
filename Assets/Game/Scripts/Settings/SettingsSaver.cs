using System.Collections.Generic;
using System.Collections;

using UnityEngine;

namespace SkinDesigner.Settings
{
    public class SettingsSaver : MonoBehaviour
    {
        [SerializeField]
        private bool loadOnStart = true;

        [SerializeField]
        private List<SettingsItem> savedSettingsItem = new List<SettingsItem>();

        public bool LoadOnStart
        {
            get { return loadOnStart; }
        }

        private void Start()
        {
            if (loadOnStart)
            {
                LoadSettings();
            }
        }

        public void LoadSettings()
        {
            foreach(SettingsItem item in savedSettingsItem)
            {
                SettingsItemOption option = item.ItemOption;
                string valueType = option.ValueType;
                object output = null;
                
                if (valueType == "bool")
                {
                    int outputIntValue = PlayerPrefs.GetInt(item.ItemSavingName, 0);
                    output = (outputIntValue == 0) ? false : true;
                }
                else if (valueType == "float")
                {
                    output = PlayerPrefs.GetFloat(item.ItemSavingName, 0);
                }
                else if (valueType == "int")
                {
                    output = PlayerPrefs.GetInt(item.ItemSavingName, 0);
                }
                else if (valueType == "string")
                {
                    output = PlayerPrefs.GetString(item.ItemSavingName, "");
                }

                if (output != null) item.ItemOption.Value = output;
            }
        }

        public void SaveSettings()
        {
            foreach(SettingsItem item in savedSettingsItem)
            {
                SettingsItemOption option = item.ItemOption;
                object savedValue = option.Value;
                string valueType = option.ValueType;

                if (valueType == "bool")
                {
                    int boolInInt = (bool)(savedValue) ? 1 : 0;
                    PlayerPrefs.SetInt(item.ItemSavingName, boolInInt);
                }
                else if (valueType == "int")
                {
                    PlayerPrefs.SetInt(item.ItemSavingName, (int)savedValue);
                }
                else if (valueType == "float")
                {
                    PlayerPrefs.SetFloat(item.ItemSavingName, (float)savedValue);
                }
                else if (valueType == "string")
                {
                    PlayerPrefs.SetString(item.ItemSavingName, (string)savedValue);
                }
            }
        }
    }
}