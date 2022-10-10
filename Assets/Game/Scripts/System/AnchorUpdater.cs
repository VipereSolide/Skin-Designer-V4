using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FeatherLight.Pro
{
    [ExecuteAlways]
    public class AnchorUpdater : MonoBehaviour
    {
        [SerializeField] private bool updateWidth;
        [SerializeField] private bool updateHeight;

        public bool UpdateWidth() => updateWidth;
        public bool UpdateHeight() => updateHeight;

        private RectTransform m_rectTransform;

        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (m_rectTransform == null)
            {
                return;
            }

            float targetWidth = m_rectTransform.anchoredPosition.x;
            float targetHeight = m_rectTransform .anchoredPosition.y;

            if (updateWidth)
            {
                targetWidth = m_rectTransform.sizeDelta.x / 2;
            }

            if (updateHeight)
            {
                targetHeight = -m_rectTransform.sizeDelta.y / 2;
            }

            m_rectTransform.anchoredPosition = new Vector2(targetWidth, targetHeight);
        }
    }
}