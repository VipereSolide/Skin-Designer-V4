using System.Collections.Generic;
using System.Collections;

using UnityEngine.EventSystems;
using UnityEngine;

using FeatherLight.Pro;

namespace SkinDesigner.WeaponPainting
{
    public class WeaponPainterManager : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer cursor;

        [SerializeField]
        private float increment = 0.08f;

        [SerializeField]
        private Camera painterCamera;

        [SerializeField]
        private LineRenderer lineRendererPrefab;

        [SerializeField]
        private Transform lineRendererContainer;

        [SerializeField]
        private float rectSize = 400;

        [SerializeField]
        private WeaponPainterTexture painterTexture;

        [SerializeField]
        private GameObject[] currentWeaponWireframes;

        private RectTransform m_rectTransform;
        private LineRenderer currentPrefab;

        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            /*
            RaycastHit hit;

            if (Physics.Raycast(cameraRay, out hit))
            {
                for(int i = 0; i < currentWeaponWireframes.Length; i++)
                {
                    currentWeaponWireframes[i].SetActive(hit.transform.gameObject == currentWeaponWireframes[i]);
                }
            }
            */
        }

        private void OnDrawGizmos()
        {
            Vector2 mousePos = GetMousePositionRelativeToUI(m_rectTransform, 2048, 2048);
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(mousePos, 20f);
            Gizmos.DrawLine(painterCamera.transform.position, mousePos);
        }

        private Vector2 GetMousePositionRelativeToUI(RectTransform rect, int width, int height)
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 realSize = new Vector2(rect.rect.width, rect.rect.height);
            Vector2 realPosition = new Vector2(painterTexture.Texture.transform.position.x, rect.transform.position.y);

            Vector2 paintedSize = new Vector2(width, height);
            Vector2 correctMousePosition = (mousePosition - realPosition - realSize / 2) * -1;
            Vector2 relativeMousePosition = paintedSize - (correctMousePosition / realSize.x * width);

            return relativeMousePosition;
        }
    }
}