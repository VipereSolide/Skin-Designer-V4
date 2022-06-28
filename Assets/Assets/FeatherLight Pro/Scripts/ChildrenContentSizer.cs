using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FeatherLight.Pro
{
    [ExecuteAlways]
    public class ChildrenContentSizer : MonoBehaviour
    {
        [SerializeField]
        private bool useVertical = true;

        [SerializeField]
        private bool useHorizontal = false;

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

        private void Update()
        {
            float height = 0;
            float width = 0;

            foreach (RectTransform t in transform)
            {
                height += t.sizeDelta.y;
                width += t.sizeDelta.x;
            }

            if (useVertical)
            {
                GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, height);
            }

            if (useHorizontal)
            {
                GetComponent<RectTransform>().sizeDelta = new Vector2(width, GetComponent<RectTransform>().sizeDelta.y);
            }
        }
    }
}