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

        private LineRenderer currentPrefab;

        private void Update()
        {
            bool clickOnTexture = (Input.GetMouseButton(0) && painterTexture.Highlighted);
            bool clickedOnTexture = (Input.GetMouseButtonDown(0) && painterTexture.Highlighted);
            bool mouseMoving = (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0);

            Vector2 realMousePositionVector2 = GetMousePositionRelativeToUI(painterTexture.GetComponent<RectTransform>(), 2048, 2048);
            Vector3 realMousePositionVector3 = new Vector3(realMousePositionVector2.x, realMousePositionVector2.y, 0);
            Vector3 endPoint = Vector3.zero;

            RaycastHit hit;
            Ray ray = painterCamera.ScreenPointToRay(realMousePositionVector3);

            if (Physics.Raycast(ray, out hit))
            {
                endPoint = hit.point;
                Debug.Log("hi");
            }

            Vector3 worldPosition = new Vector3(endPoint.x, endPoint.y, 0) - lineRendererContainer.position;

            if (clickedOnTexture || (mouseMoving && clickOnTexture))
            {
                if (currentPrefab == null)
                {
                    LineRenderer newPrefab = Instantiate(lineRendererPrefab, lineRendererContainer);
                    newPrefab.positionCount = 0;
                    currentPrefab = newPrefab;
                }

                Vector3 lastPointPosition = currentPrefab.GetPosition(currentPrefab.positionCount - 2);
                float distanceBetweenPositions = Vector3.Distance(worldPosition, lastPointPosition);

                if (distanceBetweenPositions >= increment)
                {
                    currentPrefab.positionCount++;
                    currentPrefab.SetPosition(currentPrefab.positionCount - 1, worldPosition / rectSize);
                }
            }

            cursor.transform.position = new Vector3(endPoint.x, endPoint.y, 0);

            if (Input.GetMouseButtonUp(0))
            {
                currentPrefab = null;
            }
        }

        /*private void Update()
        {
            bool clickOnTexture = (Input.GetMouseButton(0) && painterTexture.Highlighted);
            bool clickedOnTexture = (Input.GetMouseButtonDown(0) && painterTexture.Highlighted);
            bool mouseMoving = (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0);



            if (clickedOnTexture || (mouseMoving && clickOnTexture))
            {
                if (currentPrefab == null)
                {
                    LineRenderer newPrefab = Instantiate(lineRendererPrefab, lineRendererContainer);
                    newPrefab.positionCount = 0;
                    currentPrefab = newPrefab;
                }

                Vector2 realMousePositionVector2 = GetMousePositionRelativeToUI(painterTexture.GetComponent<RectTransform>(), 2048, 2048);
                Vector3 realMousePositionVector3 = new Vector3(realMousePositionVector2.x, realMousePositionVector2.y, 0);
                Vector3 endPoint = Vector3.zero;

                RaycastHit hit;
                Ray ray = painterCamera.ScreenPointToRay(realMousePositionVector3);

                if (Physics.Raycast(ray, out hit))
                {
                    endPoint = hit.point;
                }

                Vector3 worldPosition = new Vector3(endPoint.x, endPoint.y, 0) - lineRendererContainer.position;

                Vector3 lastPointPosition = currentPrefab.GetPosition(currentPrefab.positionCount - 2);
                float distanceBetweenPositions = Vector3.Distance(worldPosition, lastPointPosition);

                if (distanceBetweenPositions >= increment)
                {
                    currentPrefab.positionCount++;
                    currentPrefab.SetPosition(currentPrefab.positionCount - 1, worldPosition / rectSize);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                currentPrefab = null;
            }
        }*/

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