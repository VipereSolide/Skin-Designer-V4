using System.Collections.Generic;
using System.Collections;

using UnityEngine;

namespace SkinDesigner.SkinSystem
{
    public class RuntimeApplication : MonoBehaviour
    {
        public void CloseApplication()
        {
            Application.Quit();
        }

        public void OpenProjectFolder()
        {
            Application.OpenURL(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Skillwarz/Skin Designer/Projects/");
        }
    }
}