using UnityEngine;
using UnityEngine.UI;
public class GUIManager : MonoBehaviour
{
    public Text guiTextMode;
    public Slider sizeSlider;
    public TexturePainter painter;

    public void SetBrushMode(int newMode)
    {
        PainterBrushMode brushMode = newMode == 0 ? PainterBrushMode.DecalMode : PainterBrushMode.PaintMode;
        string colorText = brushMode == PainterBrushMode.PaintMode ? "orange" : "purple";
        guiTextMode.text = "<b>Mode:</b><color=" + colorText + ">" + brushMode.ToString() + "</color>";
        painter.SetBrushMode(brushMode);
    }
    public void UpdateSizeSlider()
    {
        painter.SetBrushSize(sizeSlider.value);
    }
}
