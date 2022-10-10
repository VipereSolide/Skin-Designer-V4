using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherLight.Pro
{
    public class Mouse : MonoBehaviour
    {
        public static Mouse instance;

        private Vector3 lastMousePosition;
        private bool startHoldingMouseButton;

        private float mouse0DownTime;

        private float mouseDistance;

        public float GetMouseDistance() => mouseDistance;
        public float LMBHeldTime() => mouse0DownTime;

        private void Awake()
        {
            instance = this;
        }

        private void HandleMouseDistance()
        {
            bool mouseButtonDown = Input.GetMouseButtonDown(0);
            bool mouseButtonUp = Input.GetMouseButtonUp(0);
            
            if (mouseButtonDown)
            {
                lastMousePosition = Input.mousePosition;
                startHoldingMouseButton = true;

                mouse0DownTime = 0;
            }

            if (mouseButtonUp)
            {
                mouseDistance = Vector3.Distance(lastMousePosition, Input.mousePosition);
                startHoldingMouseButton = false;
            }

            if (startHoldingMouseButton)
            {
                mouse0DownTime += Time.deltaTime;
            }
        }

        private void Update()
        {
            HandleMouseDistance();            
        }
    }
}