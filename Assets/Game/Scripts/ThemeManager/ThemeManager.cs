using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemeManager : MonoBehaviour
{
    [SerializeField] private UITheme baseTheme;
    [SerializeField] private UITheme[] themes;
    [SerializeField] private int selectedTheme;

    private UITheme currentTheme;
    private List<Image> images = new List<Image>();
    private List<RawImage> rawImages = new List<RawImage>();
    private List<TMP_Text> texts = new List<TMP_Text>();

    public UITheme BaseTheme
    {
        get { return baseTheme; }
    }

    public UITheme CurrentTheme
    {
        get { return themes[selectedTheme]; }
    }

    public int SelectedTheme
    {
        get { return selectedTheme; }
    }

    public UITheme[] Themes
    {
        get { return themes; }
    }

    private void Start()
    {
        currentTheme = baseTheme;
        GatherSceneElements();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SelectTheme(0);
    }

    public void GatherSceneElements()
    {
        images.Clear();
        rawImages.Clear();
        texts.Clear();

        foreach (Image image in GameObject.FindObjectsOfType<Image>())
        {
            if (image.gameObject.GetComponent<ThemeIgnorer>() != null)
                continue;

            images.Add(image);
        }


        foreach (RawImage rawImage in GameObject.FindObjectsOfType<RawImage>())
        {
            if (rawImage.gameObject.GetComponent<ThemeIgnorer>() != null)
                continue;

            rawImages.Add(rawImage);
        }


        foreach (TMP_Text text in GameObject.FindObjectsOfType<TMP_Text>())
        {
            if (text.gameObject.GetComponent<ThemeIgnorer>() != null)
                continue;

            texts.Add(text);
        }
    }

    private int GetThemeIndex(int i)
    {
        if (i > themes.Length - 1)
        {
            i = themes.Length - 1;
        }

        if (i < 0)
        {
            i = 0;
        }

        return i;
    }

    public void SelectTheme(int i)
    {
        selectedTheme = GetThemeIndex(i);

        UpdateThemes();
    }

    public void UpdateThemes()
    {
        for (int colorModelsIndex = 0; colorModelsIndex < themes[selectedTheme].ColorModels.Length; colorModelsIndex++)
        {
            UITheme.ColorModel model = themes[selectedTheme].ColorModels[colorModelsIndex];

            if (model.Type == UITheme.ColorModel.ModelType.Image)
            {
                for (int i0 = 0; i0 < images.Count; i0++)
                {
                    //Debug.Log($"I'm checked if: {((Color32)images[i0].color).r},{((Color32)images[i0].color).g},{((Color32)images[i0].color).b},{((Color32)images[i0].color).a} is equal to {currentTheme.ColorModels[colorModelsIndex].Color.r},{currentTheme.ColorModels[colorModelsIndex].Color.g},{currentTheme.ColorModels[colorModelsIndex].Color.b},{currentTheme.ColorModels[colorModelsIndex].Color.a}");
                    
                    if (((Color32)images[i0].color).Equals(currentTheme.ColorModels[colorModelsIndex].Color))
                    {
                        //Debug.Log("hello");
                        images[i0].color = model.Color;
                    }
                }
            }
            else if (model.Type == UITheme.ColorModel.ModelType.RawImage)
            {
                for (int i1 = 0; i1 < rawImages.Count; i1++)
                {
                    if (rawImages[i1].color == currentTheme.ColorModels[colorModelsIndex].Color)
                    {
                        rawImages[i1].color = model.Color;
                    }
                }
            }
            else
            {
                for (int i2 = 0; i2 < texts.Count; i2++)
                {
                    if (texts[i2].color == currentTheme.ColorModels[colorModelsIndex].Color)
                    {
                        texts[i2].color = model.Color;
                    }
                }
            }
        }

        currentTheme = CurrentTheme;
    }
}
