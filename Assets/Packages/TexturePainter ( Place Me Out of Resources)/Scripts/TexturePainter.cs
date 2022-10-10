/// <summary>
/// CodeArtist.mx 2015
/// This is the main class of the project, its in charge of raycasting to a model and place brush prefabs infront of the canvas camera.
/// If you are interested in m_saving the painted texture you can use the method at the end and should save it to a file.
/// </summary>


using System.Collections;
using System.IO;
using UnityEngine;

public enum PainterBrushMode
{
    [InspectorName("Paint")] PaintMode,
    [InspectorName("Decal")] DecalMode
};
public class TexturePainter : MonoBehaviour
{
    public Sprite m_cursorPaint, m_cursorDecal; // Cursor for the differen functions 

    [SerializeField] private GameObject m_brushCursor;
    [SerializeField] private GameObject m_brushContainer;
    [SerializeField] private Camera m_sceneCamera;
    [SerializeField] private Camera m_canvasCamera;
    [SerializeField] private RenderTexture m_canvasTexture;
    [SerializeField] private LineRenderer[] m_lineRenderer;
    [SerializeField] private Material m_baseMaterial;

    [Space()]
    [SerializeField] private float m_distanceBetweenPaintPoints = 0.1f;
    [SerializeField] private Transform m_sprite;

    private PainterBrushMode m_paintMode = PainterBrushMode.PaintMode; //Our painter m_paintMode (Paint brushes or decals)
    private float m_brushSize = 1.0f; //The size of our brush
    private Color m_brushColor; //The selected color
    private int m_brushCounter = 0;
    private int m_maxBrushCount = 1000; //To avoid having millions of brushes
    private bool m_saving = false; //Flag to check if we are m_saving the texture

    private bool m_isMouseDown = false;


    private void Update()
    {
        //m_brushColor = ColorSelector.GetColor();

        if (m_isMouseDown)
        {
            PaintPoint();
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_isMouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_isMouseDown = false;
            CreateNewRenderer();
        }

        //UpdateBrushCursor ();
    }

    private void CreateNewRenderer()
    {
        m_brushCounter = 0;

        LineRenderer _newLineRenderer = Instantiate(m_lineRenderer[0], m_lineRenderer[0].transform.position, m_lineRenderer[0].transform.rotation);
        _newLineRenderer.positionCount = 1;

        m_lineRenderer[0] = _newLineRenderer;
    }

    /// <summary>
    /// The main action, instantiates a brush or decal entity at the clicked position on the UV map
    /// </summary>
    private void PaintPoint()
    {
        if (m_saving)
            return;

        m_lineRenderer[0].positionCount = m_brushCounter + 2;

        bool _isEnoughDistance = Vector3.Distance(m_lineRenderer[0].GetPosition(m_brushCounter), m_lineRenderer[0].GetPosition(m_brushCounter + 1)) >= m_distanceBetweenPaintPoints;

        if (_isEnoughDistance)
        {
            Vector3 uvWorldPosition = Vector3.zero;
            if (HitTestUVPosition(ref uvWorldPosition))
            {
                Vector3 _localPosition = m_sprite.position + new Vector3(0.5f, 0.5f, 0);
                m_lineRenderer[0].SetPosition(m_brushCounter, m_lineRenderer[0].transform.position + uvWorldPosition);
            }

            m_lineRenderer[0].SetPosition(m_lineRenderer[0].positionCount - 1, m_lineRenderer[0].GetPosition(m_lineRenderer[0].positionCount - 2));

        }

        m_brushCounter++;
    }

    /// <summary>
    /// To update at realtime the painting cursor on the mesh
    /// </summary>
    private void UpdateBrushCursor()
    {
        Vector3 uvWorldPosition = Vector3.zero;

        if (HitTestUVPosition(ref uvWorldPosition) && !m_saving)
        {
            m_brushCursor.SetActive(true);
            m_brushCursor.transform.position = uvWorldPosition + m_brushContainer.transform.position;
        }
        else
        {
            m_brushCursor.SetActive(false);
        }
    }

    /// <summary>
    /// Returns the position on the texuremap according to a hit in the mesh collider
    /// </summary>
    private bool HitTestUVPosition(ref Vector3 uvWorldPosition)
    {
        RaycastHit hit;
        Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        Ray cursorRay = m_sceneCamera.ScreenPointToRay(cursorPos);

        if (Physics.Raycast(cursorRay, out hit, 200))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;

            if (meshCollider == null || meshCollider.sharedMesh == null)
                return false;

            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            uvWorldPosition.x = pixelUV.x - m_canvasCamera.orthographicSize; //To center the UV on X
            uvWorldPosition.y = pixelUV.y - m_canvasCamera.orthographicSize; //To center the UV on Y
            uvWorldPosition.z = 0.0f;

            return true;
        }
        else
        {
            return false;
        }

    }
    /// <summary>
    /// Sets the base material with a our canvas texture, then removes all our brushes
    /// </summary>
    void SaveTexture()
    {
        m_brushCounter = 0;

        RenderTexture.active = m_canvasTexture;
        Texture2D _texture = new Texture2D(m_canvasTexture.width, m_canvasTexture.height, TextureFormat.RGB24, false);
        _texture.ReadPixels(new Rect(0, 0, m_canvasTexture.width, m_canvasTexture.height), 0, 0);
        _texture.Apply();

        RenderTexture.active = null;
        m_baseMaterial.mainTexture = _texture;

        foreach (Transform child in m_brushContainer.transform)
        {
            Destroy(child.gameObject);
        }

        Invoke("DisableSaving", 0.1f);
    }

    private void DisableSaving()
    {
        m_saving = false;
    }

    /// <summary>
    /// Sets if we are painting or placing decals
    /// </summary>
    public void SetBrushMode(PainterBrushMode brushMode)
    {
        m_paintMode = brushMode;
        Sprite _paintedCursor = m_cursorPaint;

        if (brushMode == PainterBrushMode.DecalMode)
        {
            _paintedCursor = m_cursorDecal;
        }

        m_brushCursor.GetComponent<SpriteRenderer>().sprite = _paintedCursor;
    }

    /// <summary>
    /// Sets the size of the cursor brush or decal
    /// </summary>
    public void SetBrushSize(float newBrushSize)
    {
        m_brushSize = newBrushSize;
        m_brushCursor.transform.localScale = Vector3.one * m_brushSize;
    }

#if !UNITY_WEBPLAYER

    private IEnumerator SaveTextureToFile(Texture2D _SavedTexture, string _SavePath)
    {
        m_brushCounter = 0;

        if (!Directory.Exists(Path.GetDirectoryName(_SavePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_SavePath));
        }

        byte[] bytes = _SavedTexture.EncodeToPNG();
        File.WriteAllBytes(_SavePath, bytes);

        yield return null;
    }

#endif
}
