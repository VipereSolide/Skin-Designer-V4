using System.Collections.Generic;
using System.Collections;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace SkinDesigner.WeaponPainting
{
    public class WeaponWireframeObject : MonoBehaviour
    {
        [SerializeField]
        private Image wireframePrefab;

        [SerializeField]
        private Sprite[] wireframes;
        
        private List<Image> wireframeObjects = new List<Image>();

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < wireframes.Length; i++)
            {
                Image instantiated = Instantiate(wireframePrefab, transform);
                instantiated.transform.name = i.ToString();
                instantiated.sprite = wireframes[i];
                wireframeObjects.Add(instantiated);

                var pointerEnter = new EventTrigger.Entry();
                pointerEnter.eventID = EventTriggerType.PointerEnter;
                pointerEnter.callback.AddListener((e) => { HoverItem(instantiated); });

                var pointerExit = new EventTrigger.Entry();
                pointerExit.eventID = EventTriggerType.PointerExit;
                pointerExit.callback.AddListener((e) => { StopHovering(); });

                EventTrigger instantiatedTrigger = instantiated.gameObject.AddComponent<EventTrigger>();
                instantiatedTrigger.triggers.Add(pointerEnter);
                instantiatedTrigger.triggers.Add(pointerExit);

                wireframeObjects[i] = instantiated;
            }

            StopHovering();
        }

        public void HoverItem(Image i)
        {
            foreach(Image img in wireframeObjects)
            {
                img.color = new Color(0,0,0,0);
            }

            i.color = new Color(1,1,1,1);
        }

        public void StopHovering()
        {
            foreach(Image img in wireframeObjects)
            {
                img.color = new Color(0,0,0,0);
            }
        }
    }
}