using System.Collections.Generic;
using System.Collections;
using System;

using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace FeatherLight.Pro
{
    [ExecuteAlways]
    public class AdvancedContentSizeFitter : MonoBehaviour
    {
        private RectTransform rectTransform;

        [SerializeField]
        private ContentSizeFitterObject[] fitterObjects;

        [SerializeField]
        private bool vertical;

        [SerializeField]
        private bool horizontal;

        [SerializeField]
        private bool addSize = false;

        public ContentSizeFitterObject[] FitterObjects
        {
            get { return fitterObjects; }
        }

        public bool Vertical
        {
            get { return vertical; }
        }

        public bool Horizontal
        {
            get { return horizontal; }
        }

        private float startRectTransformX;
        private float startRectTransformY;

        private void Awake()
        {
            rectTransform = transform.GetComponent<RectTransform>();
        }

        private void Start()
        {
            startRectTransformX = rectTransform.sizeDelta.x;
            startRectTransformY = rectTransform.sizeDelta.y;
        }

        private void Update()
        {
            float totalSizeX = 0;
            float totalSizeY = 0;

            foreach (ContentSizeFitterObject obj in fitterObjects)
            {
                totalSizeX += obj.TargetObject.sizeDelta.x;
                totalSizeX += obj.Padding.x + obj.Padding.y;

                totalSizeY += obj.TargetObject.sizeDelta.y;
                totalSizeY += obj.Padding.z + obj.Padding.w;
            }

            if (addSize)
            {
                rectTransform.sizeDelta += new Vector2(
                    (horizontal) ? totalSizeX : rectTransform.sizeDelta.x,
                    (vertical) ? totalSizeY : rectTransform.sizeDelta.y
                );
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(
                    (horizontal) ? totalSizeX : rectTransform.sizeDelta.x,
                    (vertical) ? totalSizeY : rectTransform.sizeDelta.y
                );
            }
        }

        [Serializable]
        public class ContentSizeFitterObject
        {
            [SerializeField]
            private RectTransform targetObject;

            [SerializeField]
            private Vector4 padding;

            [SerializeField]
            [InspectorName("Vertical")]
            private bool useVertical = false;

            [SerializeField]
            [InspectorName("Horizontal")]
            private bool useHorizontal = false;

            public Vector4 Padding
            {
                get { return padding; }
                set { padding = value; }
            }

            public RectTransform TargetObject
            {
                get { return targetObject; }
            }

            public bool Vertical
            {
                get { return useVertical; }
                set { useVertical = value; }
            }

            public bool Horizontal
            {
                get { return useHorizontal; }
                set { useHorizontal = value; }
            }

            public ContentSizeFitterObject(RectTransform _targetObject, Vector4 _padding)
            {
                this.targetObject = _targetObject;
                this.padding = _padding;
            }
        }
    }
}