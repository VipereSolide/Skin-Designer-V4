using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace SkinDesigner.Painter.UI
{
    public class LayerInspector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject inspectedLayerObject;
        [SerializeField] private GameObject nothingInspectedObject;
        [SerializeField] private TMP_Text inspectedLayerNameText;
        [SerializeField] private TMP_Dropdown currentMaskDropdown;

        public static LayerInspector instance;

        private Layer inspectedLayer;

        private bool hover = false;

        private void Awake()
        {
            instance = this;
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !hover)
            {
                StopInspecting();
            }
        }

        public void StopInspecting()
        {
            inspectedLayerObject.SetActive(false);
            nothingInspectedObject.SetActive(true);

            inspectedLayerNameText.text = "";
            inspectedLayer = null;
        }

        public void InspectLayer(Layer layer)
        {
            inspectedLayerObject.SetActive(true);
            nothingInspectedObject.SetActive(false);

            inspectedLayerNameText.text = layer.LayerName;
            inspectedLayer = layer;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hover = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hover = false;
        }
    }
}