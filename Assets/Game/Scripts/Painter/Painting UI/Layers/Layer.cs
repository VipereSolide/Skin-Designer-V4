using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace SkinDesigner.Painter.UI
{
    public class Layer : MonoBehaviour, IPointerClickHandler
    {
        private string layerName;
        public string LayerName => layerName;

        [Space]
        [SerializeField] private TMP_Text layerNameText;
        [SerializeField] private Button layerQuickDeleteButton;
        
        private bool usesLayer;
        private int currentLayer;

        public Action<Layer> onDeleteLayer;

        public void SetLayer(bool value)
        {
            usesLayer = value;
        }

        public void SetUsedLayer(int layer)
        {
            currentLayer = layer;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                layerQuickDeleteButton.gameObject.SetActive(true);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                layerQuickDeleteButton.gameObject.SetActive(false);
            }
        }

        public void Delete()
        {
            onDeleteLayer.Invoke(this);
        }
        
        public void SetName(string layerName)
        {
            this.layerName = layerName;

            UpdateLayer();
        }

        public void UpdateLayer()
        {
            layerNameText.text = layerName;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            LayerInspector.instance.InspectLayer(this);
        }
    }
}