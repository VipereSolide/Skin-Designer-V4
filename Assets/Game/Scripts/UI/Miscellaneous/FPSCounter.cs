using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
[RequireComponent(typeof(TMP_Text))]
public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text text;

    [SerializeField]
    protected int fpsAverageCount = 50;

    protected float[] fpsArray;
    protected int index;

    private void Start()
    {
            InitFPSArray();
    }

    private void Update()
    {
        if (index < fpsAverageCount)
        {
            fpsArray[index] = 1.0f / Time.deltaTime;
            index++;
        }
        else
        {
            float fps = 0;

            for (int i = 0; i < fpsArray.Length; i++)
                fps += fpsArray[i];

            text.text = Mathf.FloorToInt(fps / fpsAverageCount).ToString() + " fps";

            InitFPSArray();
        }
    }

    protected void InitFPSArray()
    {
        fpsArray = new float[fpsAverageCount];
        index = 0;
    }
}
