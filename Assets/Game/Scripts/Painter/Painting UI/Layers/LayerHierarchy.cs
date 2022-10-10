using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FeatherLight.Pro;

namespace SkinDesigner.Painter.UI
{
    public class LayerHierarchy : MonoBehaviour
    {
        [SerializeField] private CanvasGroup windowCanvasGroup;
        [SerializeField] private KeyShortcut toggleWindowShortcut;

        private bool isActive = true;

        public void SetActive(bool value)
        {
            CanvasGroupHelper.SetActive(windowCanvasGroup, value);
            isActive = value;
        }

        private void Update()
        {
            if (toggleWindowShortcut.IsPressed())
            {
                SetActive(!isActive);
            }
        }
    }
}