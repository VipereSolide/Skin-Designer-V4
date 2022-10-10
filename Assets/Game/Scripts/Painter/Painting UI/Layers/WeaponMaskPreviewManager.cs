using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace SkinDesigner.Painter.UI
{
    public class WeaponMaskPreviewManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] masks;

        [SerializeField] private LayerMask uiLayerMask;
        [SerializeField] private Vector2 weaponTextureStartCoord;
        [SerializeField] private Vector2 weaponTextureEndCoord;

        public void DisableAllMasks()
        {
            for (int i = 0; i < masks.Length; i++)
                masks[i].SetActive(false);
        }

        public void ActivateMask(int i)
        {
            for(int j = 0; j < masks.Length; j++)
            {
                masks[j].SetActive(j == i);
            }
        }

        public void ActivateMask(GameObject mask)
        {
            int index = -1;

            for (int i = 0; i < masks.Length; i++)
            {
                if (masks[i]== mask)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                Debug.LogError($"The mask \"{mask}\" couldn't be found.");
                return;
            }

            ActivateMask(index);
        }

        public void ActivateMask(string maskName)
        {
            int index = -1;

            for(int i = 0; i < masks.Length; i++)
            {
                if (masks[i].transform.name == maskName)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                Debug.LogError($"The mask \"{maskName}\" couldn't be found.");
                return;
            }

            ActivateMask(index);
        }

        public void Update()
        {
            bool hoveringTexture = Input.mousePosition.x < weaponTextureEndCoord.x && Input.mousePosition.x > weaponTextureStartCoord.x && Input.mousePosition.y < weaponTextureEndCoord.y && Input.mousePosition.y > weaponTextureStartCoord.y;

            if (hoveringTexture)
            {
                RaycastHit hit;
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (!Physics.Raycast(r, out hit, uiLayerMask))
                {
                    return;
                }

                Texture2D tex = new Texture2D(Screen.width, Screen.height);
                tex.ReadPixels(new Rect(0, 0, Screen.width - 1, Screen.height - 1), 0, 0);
                tex.Apply();
                Debug.Log(tex.GetPixel(Mathf.RoundToInt(Input.mousePosition.x), Mathf.RoundToInt(Input.mousePosition.y)) == Color.white);
            }
        }
    }
}