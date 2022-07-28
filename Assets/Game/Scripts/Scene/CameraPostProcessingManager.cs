using System.Collections.Generic;
using System.Collections;

using UnityEngine;

namespace SkinDesigner.Scene
{
    public class CameraPostProcessingManager : MonoBehaviour
    {
        [SerializeField]
        private Camera postProcessingCamera;

        [SerializeField]
        private MonoBehaviour[] postProcessingComponents;

        private bool isPostProcessingActive = true;

        public void SetPostProcessing(bool value)
        {
            foreach(MonoBehaviour behaviour in postProcessingComponents)
            {
                behaviour.enabled = value;
            }

            isPostProcessingActive = value;
        }

        public void TogglePostProcessing()
        {
            SetPostProcessing(!isPostProcessingActive);
        }
    }
}