using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace FeatherLight.Pro
{
    [ExecuteAlways]
    public class ChildrenContentSizer : MonoBehaviour
    {
        [SerializeField]
        private bool useTarget = false;

        [SerializeField]
        [ShowIf("useTarget")]
        private RectTransform target;

        [Space]

        [SerializeField]
        private bool useVertical = true;

        [SerializeField]
        private bool useHorizontal = false;

        [Space]

        [SerializeField]
        private Rect padding;

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
            float height = padding.yMin + padding.yMax;
            float width = padding.xMin + padding.xMax;

            if (!useTarget)
            {
                foreach (RectTransform t in transform)
                {
                    height += t.sizeDelta.y;
                    width += t.sizeDelta.x;
                }
            }
            else
            {
                if (target == null)
                {
                    return;
                }

                height += target.sizeDelta.y;
                width += target.sizeDelta.x;
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