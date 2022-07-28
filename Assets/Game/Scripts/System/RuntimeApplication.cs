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
    }
}