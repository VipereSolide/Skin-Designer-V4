using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UITheme
{
    [SerializeField] private ColorModel[] colorModels;

    public ColorModel[] ColorModels
    {
        get { return colorModels; }
        set { colorModels = value; }
    }

    [System.Serializable]
    public class ColorModel
    {
        public enum ModelType { Text, Image, RawImage }

        [SerializeField]
        private ModelType type = ModelType.Image;

        [SerializeField]
        private Color32 color = new Color32(255,255,255,255);

        public Color32 Color { get { return color; } set { color = value; } }
        public ModelType Type { get { return type; } set { type = value; } }

        public ColorModel(Color32 color, ModelType type)
        {
            this.color = color;
            this.type = type;
        }
    }
}
