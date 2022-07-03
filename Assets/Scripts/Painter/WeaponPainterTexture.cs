using System.Collections.Generic;
using System.Collections;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace SkinDesigner.WeaponPainting
{
    public class WeaponPainterTexture : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private RawImage texture;

        private bool highlighted = false;

        public RawImage Texture
        {
            get { return texture; }
        }
        
        public bool Highlighted
        {
            get { return highlighted; }
        }

        public void OnPointerEnter(PointerEventData data)
        {
            highlighted = true;
        }

        public void OnPointerExit(PointerEventData data)
        {
            highlighted = false;
        }
    }
}
