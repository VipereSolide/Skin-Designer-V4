using System.Collections.Generic;
using System.Collections;
using System.Reflection;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

using Michsky.UI.ModernUIPack;

namespace SkinDesigner.Settings
{
    public class SettingsSaver : MonoBehaviour
    {
        [Header("References")]

        [SerializeField]
        protected UniversalAdditionalCameraData mainCamera;

        [Space]

        [SerializeField]
        protected Toggle togglePostProcessing;

        [SerializeField]
        protected Toggle toggleFullScreen;

        [SerializeField]
        protected Toggle toggleVSync;

        [SerializeField]
        protected CustomDropdown fullScreenModeDropdown;

        [SerializeField]
        protected CustomDropdown graphicLevelCustomDropdown;

        protected virtual void Start()
        {
            LoadSavedSettings(true);
        }

        protected virtual void OnApplicationQuit()
        {
            SaveCurrentSettings();
        }

        // Sets the current graphic level to the current index of the graphic level
        // custom dropdown. Called by the quality dropdown when it's value is changed.
        public virtual void UpdateCurrentGraphicLevel()
        {
            // If the current quality level does not correspond to the selected item index
            // for the quality level dropdown, update it.
            if (QualitySettings.GetQualityLevel() != graphicLevelCustomDropdown.selectedItemIndex)
            {
                QualitySettings.SetQualityLevel(graphicLevelCustomDropdown.selectedItemIndex, true);
            }
        }

        // Sets post processing active or not.
        public virtual void SetPostProcessingActive(bool value)
        {
            mainCamera.renderPostProcessing = value;
        }

        // Called by the post processing toggle when it's value changed.
        public virtual void UpdatePostProcessingState()
        {
            // If post processing state is different from what the user is
            // wanting, update it.
            if (mainCamera.renderPostProcessing != togglePostProcessing.isOn)
            {
                SetPostProcessingActive(togglePostProcessing.isOn);
            }
        }

        // Returns true if the application is in full screen mode.
        protected virtual bool IsFullScreen()
        {
            return Screen.fullScreenMode == FullScreenMode.FullScreenWindow ||
                   Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen ||
                   Screen.fullScreenMode == FullScreenMode.MaximizedWindow &&
                   Screen.fullScreen;
        }

        // Returns a full screen mode from an int.
        protected FullScreenMode IntToFullScreenMode(int value)
        {
            switch (value)
            {
                case 0:
                    return FullScreenMode.FullScreenWindow;
                case 1:
                    return FullScreenMode.ExclusiveFullScreen;
                case 2:
                    return FullScreenMode.MaximizedWindow;
                default:
                    return FullScreenMode.FullScreenWindow;
            }
        }

        // Called by the full screen toggle when it's value changed. Checks
        // whether the current full screen mode corresponds to the full screen
        // toggle's value. If not, changes it.
        public virtual void UpdateFullScreenMode()
        {
            // The application shouldn't update the full screen mode if it's in windowed mode (not in full screen).
            bool changedFullScreenMode = IsFullScreen() && Screen.fullScreenMode != IntToFullScreenMode(fullScreenModeDropdown.selectedItemIndex);

            // Checks whether the user changed the application to full screen or windowed, or if they changed the full screen mode. If it's the,
            // update it.
            if (IsFullScreen() != toggleFullScreen.isOn || changedFullScreenMode)
            {
                SetFullScreenMode(toggleFullScreen.isOn);
            }
        }

        // Sets the application in fullscreen or not.
        public virtual void SetFullScreenMode(bool value)
        {
            if (value)
            {
                Screen.fullScreenMode = IntToFullScreenMode(fullScreenModeDropdown.selectedItemIndex);
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }

            Screen.fullScreen = value;
        }

        // Checks whether the current vsync is active or not, and if it matches
        // the vsync toggle.
        public virtual void UpdateVSync()
        {
            if (QualitySettings.vSyncCount > 0 != toggleVSync.isOn)
            {
                SetVSync(toggleVSync.isOn);
            }
        }

        // Enables or disable vertical synchronisation. When enabled, the target frame
        // rate is set to 60 and the vertical synchronisation to 1 (syncs to the screen
        // refresh rate).
        public virtual void SetVSync(bool value)
        {
            if (value)
            {
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = 60;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = -1;
            }
        }

        // Saves the current settings in the PlayerPrefs Unity class.
        public virtual void SaveCurrentSettings()
        {
            PlayerPrefs.SetInt(nameof(togglePostProcessing), togglePostProcessing.isOn ? 1 : 0);
            PlayerPrefs.SetInt(nameof(toggleFullScreen), toggleFullScreen.isOn ? 1 : 0);
            PlayerPrefs.SetInt(nameof(toggleVSync), toggleVSync.isOn ? 1 : 0);

            PlayerPrefs.SetInt(nameof(fullScreenModeDropdown), fullScreenModeDropdown.selectedItemIndex);
            PlayerPrefs.SetInt(nameof(graphicLevelCustomDropdown), graphicLevelCustomDropdown.selectedItemIndex);
        }

        // Loads the saved settings from the PlayerPrefs Unity class.
        // If any of the settings were to not be saved, it would be ignored.
        public virtual void LoadSavedSettings(bool updateGraphics = true)
        {
            if (PlayerPrefs.HasKey(nameof(togglePostProcessing)))
            {
                togglePostProcessing.isOn = PlayerPrefs.GetInt(nameof(togglePostProcessing)) == 0 ? false : true;
            }

            if (PlayerPrefs.HasKey(nameof(toggleFullScreen)))
            {
                toggleFullScreen.isOn = PlayerPrefs.GetInt(nameof(toggleFullScreen)) == 0 ? false : true;
            }

            if (PlayerPrefs.HasKey(nameof(toggleVSync)))
            {
                toggleVSync.isOn = PlayerPrefs.GetInt(nameof(toggleVSync)) == 0 ? false : true;
            }

            if (PlayerPrefs.HasKey(nameof(fullScreenModeDropdown)))
            {
                fullScreenModeDropdown.selectedItemIndex = PlayerPrefs.GetInt(nameof(fullScreenModeDropdown));
            }

            if (PlayerPrefs.HasKey(nameof(graphicLevelCustomDropdown)))
            {
                graphicLevelCustomDropdown.selectedItemIndex = PlayerPrefs.GetInt(nameof(graphicLevelCustomDropdown));
            }

            if (updateGraphics)
            {
                UpdateCurrentGraphicLevel();
                UpdateFullScreenMode();
                UpdatePostProcessingState();
                UpdateVSync();
            }
        }
    }
}