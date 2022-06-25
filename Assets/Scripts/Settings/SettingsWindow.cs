using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using FeatherLight.Pro;

namespace SkinDesigner.Settings
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup windowContainer;

        public void SetWindowActive(bool value)
        {
            CanvasGroupHelper.SetActive(windowContainer, value);
        }
    }
}