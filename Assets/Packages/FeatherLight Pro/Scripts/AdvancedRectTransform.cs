using System.Collections.Generic;
using System.Collections;

using UnityEngine.UI;
using UnityEngine;

namespace FeatherLight.Pro
{
    [ExecuteAlways]
    public class AdvancedRectTransform : MonoBehaviour
    {
        public enum AnchorType
        {
            TopLeft,
            TopMiddle,
            TopRight,
            CenterLeft,
            CenterMiddle,
            CenterRight,
            BottomLeft,
            BottomMiddle,
            BottomRight
        }

        private RectTransform rectTransform;
        private RectTransform group;

        [SerializeField]
        private bool adaptXSize = false;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            group = transform.GetComponentInParent<LayoutGroup>().GetComponent<RectTransform>();
        }

        private void Update()
        {
            SetAdaptedSize();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(group);
        }

        private void SetAdaptedSize()
        {
            // Determine what anchor the rect transform is using.
            AnchorType anchor = GetAnchorFromRectTransform(rectTransform);

            // If the adaptXSize...
            if (adaptXSize)
            {
                // Get the correct size of the object, based on the anchor & on the deltaSize.
                if (anchor == AnchorType.TopLeft)
                    rectTransform.anchoredPosition = new Vector3(rectTransform.sizeDelta.x / 2, rectTransform.sizeDelta.y / 2 * -1f, transform.position.z);

                if (anchor == AnchorType.TopMiddle)
                    rectTransform.anchoredPosition = new Vector3(0, rectTransform.sizeDelta.y / 2 * -1f, transform.position.z);

                if (anchor == AnchorType.TopRight)
                    rectTransform.anchoredPosition = new Vector3(rectTransform.sizeDelta.x / 2 * -1f, rectTransform.sizeDelta.y / 2 * -1f, transform.position.z);


                if (anchor == AnchorType.CenterLeft)
                    rectTransform.anchoredPosition = new Vector3(rectTransform.sizeDelta.x / 2, 0, transform.position.z);

                if (anchor == AnchorType.CenterMiddle)
                    rectTransform.anchoredPosition = new Vector3(0, 0, transform.position.z);

                if (anchor == AnchorType.CenterRight)
                    rectTransform.anchoredPosition = new Vector3(rectTransform.sizeDelta.x / 2 * -1f, 0, transform.position.z);


                if (anchor == AnchorType.BottomLeft)
                    rectTransform.anchoredPosition = new Vector3(rectTransform.sizeDelta.x / 2, rectTransform.sizeDelta.y / 2, transform.position.z);

                if (anchor == AnchorType.BottomMiddle)
                    rectTransform.anchoredPosition = new Vector3(0, rectTransform.sizeDelta.y / 2, transform.position.z);

                if (anchor == AnchorType.BottomRight)
                    rectTransform.anchoredPosition = new Vector3(rectTransform.sizeDelta.x / 2 * -1f, rectTransform.sizeDelta.y / 2, transform.position.z);
            }
        }

        public static AnchorType GetAnchorFromRectTransform(RectTransform _rectTransform)
        {
            Vector2 _min = _rectTransform.anchorMin;
            Vector2 _max = _rectTransform.anchorMax;
            Vector4 _total = new Vector4(_min.x, _min.y, _max.x, _max.y);
            AnchorType _output = AnchorType.TopLeft;

            // Presets
            Vector4 _topLeft = new Vector4(0f, 1f, 0f, 1f);
            Vector4 _topMiddle = new Vector4(0.5f, 1f, 0.5f, 1f);
            Vector4 _topRight = new Vector4(1f, 1f, 1f, 1f);

            Vector4 _centerLeft = new Vector4(0f, 0.5f, 0f, 0.5f);
            Vector4 _centerMiddle = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
            Vector4 _centerRight = new Vector4(1f, 0.5f, 1f, 0.5f);

            Vector4 _bottomLeft = new Vector4(0, 0, 0, 0);
            Vector4 _bottomMiddle = new Vector4(0.5f, 0, 0.5f, 0);
            Vector4 _bottomRight = new Vector4(1f, 0f, 1f, 0);

            if (_total == _topLeft)
                _output = AnchorType.TopLeft;

            if (_total == _topMiddle)
                _output = AnchorType.TopMiddle;

            if (_total == _topRight)
                _output = AnchorType.TopRight;

            if (_total == _centerLeft)
                _output = AnchorType.CenterLeft;

            if (_total == _centerMiddle)
                _output = AnchorType.CenterMiddle;

            if (_total == _centerRight)
                _output = AnchorType.CenterRight;

            if (_total == _bottomLeft)
                _output = AnchorType.BottomLeft;

            if (_total == _bottomMiddle)
                _output = AnchorType.BottomMiddle;

            if (_total == _bottomRight)
                _output = AnchorType.BottomRight;

            return _output;
        }
    }
}