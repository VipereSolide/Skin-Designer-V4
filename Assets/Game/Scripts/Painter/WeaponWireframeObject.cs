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

        /*[SerializeField]
        private Sprite[] wireframes;*/

        [SerializeField]
        private List<Image> wireframeObjects = new List<Image>();

        private bool[] GetFilledPixels(Color[] pixels)
        {
            bool[] output = new bool[pixels.Length];

            for (int i = 0; i < pixels.Length; i++)
            {
                output[i] = (pixels[i].a != 0);
            }

            return output;
        }

        private int GetIn2DArray(int arrayWidthSize, Vector2Int position)
        {
            return arrayWidthSize * position.y + position.x;
        }

        private void Start()
        {
            foreach (Image img in wireframeObjects)
            {
                img.raycastPadding = GetRaycastPadding(img);
            }

            // Init();
        }

        private Vector4 GetRaycastPadding(Image img)
        {
            Debug.Log(img,img.gameObject);

            // Texture information
            Texture2D pixelsTexture = img.sprite.texture;
            Color[] pixelsColor = pixelsTexture.GetPixels(0, 0, pixelsTexture.width, pixelsTexture.height);
            bool[] pixels = GetFilledPixels(pixelsColor);

            List<Vector2Int> filled = new List<Vector2Int>();

            // Loops through all the pixels in the texture.
            for (int y = 0; y < pixelsTexture.height; y++)
            {
                for (int x = 0; x < pixelsTexture.width; x++)
                {
                    bool isFilled = pixels[GetIn2DArray(4096, new Vector2Int(x, y))];

                    if (isFilled)
                    {
                        filled.Add(new Vector2Int(x, y));
                    }
                }
            }

            int maxY = -1;
            int maxX = -1;
            int minY = 4097;
            int minX = 4097;

            foreach (Vector2Int v2i in filled)
            {
                if (v2i.y > maxY)
                {
                    maxY = v2i.y + 1;
                }

                if (v2i.y < minY)
                {
                    minY = v2i.y;
                }

                if (v2i.x > maxX)
                {
                    maxX = v2i.x + 1;
                }

                if (v2i.x < minX)
                {
                    minX = v2i.x;
                }
            }

            Debug.Log(new Rect(minX, 4096 - maxX, minY, 4096 - maxY));

            float sizeX = img.rectTransform.sizeDelta.x;
            float sizeY = img.rectTransform.sizeDelta.y;

            float relativeMinX = minX * sizeX / 4096;
            float relativeMaxX = (4096 - maxX) * sizeX / 4096;
            float relativeMinY = minY * sizeY / 4096;
            float relativeMaxY = (4096 - maxY) * sizeY / 4096;
            return new Vector4(relativeMinX, relativeMinY, relativeMaxX, relativeMaxY);
        }

        /*private void Init()
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
        }*/

        public void HoverItem(Image i)
        {
            foreach (Image img in wireframeObjects)
            {
                img.color = new Color(0, 0, 0, 0);
            }

            i.color = new Color(1, 1, 1, 1);
        }

        public void StopHovering()
        {
            foreach (Image img in wireframeObjects)
            {
                img.color = new Color(0, 0, 0, 0);
            }
        }
    }
}