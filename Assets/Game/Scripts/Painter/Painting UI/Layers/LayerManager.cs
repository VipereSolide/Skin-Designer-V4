using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace SkinDesigner.Painter.UI
{
    public class LayerManager : MonoBehaviour
    {
        [SerializeField] private Layer layerPrefab;
        [SerializeField] private List<Layer> layers = new List<Layer>();

        public void CreateLayer()
        {
            CreateLayer($"Layer {layers.Count}");
        }

        public void CreateLayer(string layerName)
        {
            Layer createdLayer = Instantiate(layerPrefab, transform);
            createdLayer.SetName(layerName);
            createdLayer.onDeleteLayer += DeleteLayer;

            layers.Add(createdLayer);
        }

        public void DeleteLayer(Layer layer)
        {
            layers.Remove(layer);
            Destroy(layer.gameObject);
        }
    }
}