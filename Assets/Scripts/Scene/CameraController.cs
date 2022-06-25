using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SkinDesigner.Scene
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera camera;

        [SerializeField]
        private float rotationSpeed;

        [SerializeField]
        private float panningSpeed;

        private bool isRotationEnabled = false;
        private bool isPanningEnabled = false;

        private void Update()
        {
            if (isRotationEnabled)
            {
                float x = Input.GetAxisRaw("Mouse X");
                float y = -Input.GetAxisRaw("Mouse Y");

                Vector3 rotation = new Vector3(y, x, 0) * rotationSpeed * Time.deltaTime;
                transform.localEulerAngles += rotation;
            }

            if (isPanningEnabled)
            {
                float x = Input.GetAxis("Mouse X");
                float y = Input.GetAxis("Mouse Y");

                Vector3 position = new Vector3(x, y, 0) * -1f * panningSpeed * Time.deltaTime;
                camera.transform.Translate(position, Space.Self);
            }

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                isRotationEnabled = false;
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                isPanningEnabled = false;
            }

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                isRotationEnabled = true;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isPanningEnabled = true;
            }
        }
    }
}