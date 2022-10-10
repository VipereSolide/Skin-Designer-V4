using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DraggingItemGhost : MonoBehaviour
{
    public enum GhostMode { Single, Multiple }

    [Header("References")]
    [SerializeField] private GameObject single;
    [SerializeField] private GameObject multiple;
    [SerializeField] private TMP_Text itemCountText;

    [Header("Properties")]
    [SerializeField] private GhostMode currentMode;

    public GameObject Single { get { return single; } }
    public GameObject Multiple { get { return multiple; } }
    public GhostMode CurrentMode { get { return currentMode; } }

    public void Init(GhostMode mode, int count = 0)
    {
        currentMode = mode;
        itemCountText.text = count.ToString();

        single.SetActive(currentMode == GhostMode.Single);
        multiple.SetActive(currentMode == GhostMode.Multiple);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}