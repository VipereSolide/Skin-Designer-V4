using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace SkinDesigner.Painter.UI
{
    public class LayerSettingsWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Layer associatedLayer;
        [SerializeField] private CanvasGroup settingsWindowCanvasGroup;
        [SerializeField] private CanvasGroup deleteWindowCanvasGroup;
        [SerializeField] private Toggle rememberDeleteWindowChoice;
        [SerializeField] private TMP_InputField layerNameInputField;

        private bool isActive;
        private bool isDeleteWindowActive;

        private bool shouldAutomaticallyDelete = false;

        private bool hover;

        private void Start()
        {
            shouldAutomaticallyDelete = PlayerPrefs.GetInt("ShouldAutomaticallyDeleteLayers") == 0 ? false : true;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !hover)
            {
                SetActive(false);
            }
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                layerNameInputField.text = associatedLayer.LayerName;
            }

            FeatherLight.Pro.CanvasGroupHelper.SetActive(settingsWindowCanvasGroup, active);
            isActive = active;
        }

        public void SetActiveDeleteWindow(bool active)
        {
            if (shouldAutomaticallyDelete && active)
            {
                DeleteLayer();
                return;
            }

            FeatherLight.Pro.CanvasGroupHelper.SetActive(deleteWindowCanvasGroup, active);
            isDeleteWindowActive = active;
        }

        public void ApplyChanges()
        {
            associatedLayer.SetName(layerNameInputField.text);
            SetActiveDeleteWindow(false);
            SetActive(false);
        }

        public void DiscardChanges()
        {
            SetActiveDeleteWindow(false);
            SetActive(false);
        }

        public void DeleteLayer()
        {
            if (rememberDeleteWindowChoice.isOn)
            {
                shouldAutomaticallyDelete = true;
                PlayerPrefs.SetInt("ShouldAutomaticallyDeleteLayers", (shouldAutomaticallyDelete) ? 1 : 0);
            }

            associatedLayer.Delete();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hover = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hover = true;
        }
    }
}