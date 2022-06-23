using System;

using UnityEngine;

namespace SkinDesigner.SkinSystem
{
    public class FileSystem : MonoBehaviour
    {
        public static string LastPath = "";

        private void SaveDiskLocation()
        {
            PlayerPrefs.SetString(nameof(LastPath), LastPath);
        }

        private void LoadDiskLocation()
        {
            string _location = PlayerPrefs.GetString(nameof(LastPath));

            if (_location != null)
                LastPath = _location;
            else
                LastPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        }

        private void Start()
        {
            LoadDiskLocation();
        }

        private void OnApplicationQuit()
        {
            SaveDiskLocation();
        }
    }
}