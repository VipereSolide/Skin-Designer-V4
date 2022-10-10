using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

using System.IO;

using SkinDesigner.SkinSystem;
using SkinDesigner.Textures;
using FeatherLight.Pro;
using SFB;

public class textureHolder : MonoBehaviour
{
    [SerializeField] private RawImage m_CurrentTextureImage;

    [Space()]

    [SerializeField] private CanvasGroup m_FetchByUrlWindow;
    [SerializeField] private float m_FetchByUrlWindowAlphaTime;
    [SerializeField] private RawImage m_FetchByUrlImage;

    [Space()]

    [SerializeField] private System.Environment.SpecialFolder m_DiskFetchFolder;
    [SerializeField] private bool m_RememberLastPathLocation;

    [Space()]
    public UnityEvent onSelectTexture;

    private TextureObject m_HeldTexture;

    public TextureObject Texture => m_HeldTexture;
    public string LastFileLocation => FileSystem.LastPath;

    public void SetFetchByUrlWindowActive(bool value)
    {
        StartCoroutine(CanvasGroupHelper.Fade(m_FetchByUrlWindow, value, m_FetchByUrlWindowAlphaTime));
    }
    public void TrashTexture()
    {
        SetHeldTexture((Texture2D)null);
    }
    public void FetchTextureOnDisk()
    {
        string[] _paths = StandaloneFileBrowser.OpenFilePanel("Select a texture...", FileSystem.LastPath, "png", false);

        if (_paths.Length <= 0)
            return;

        string _currentPath = _paths[0];
        FileSystem.LastPath = Directory.GetDirectoryRoot(_currentPath);

        SetHeldTexture(_currentPath);
        onSelectTexture.Invoke();
    }
    /*public void FetchTextureByURL()
    {
        Texture2D _texture = (Texture2D)m_FetchByUrlImage.texture;
        UpdateCurrentTexture(_texture);
    }*/
    public void UpdateCurrentTexture()
    {
        if (m_HeldTexture == null)
        {
            return;
        }

        if (m_HeldTexture.IsNull())
        {
            return;
        }

        if (m_HeldTexture.Texture == null)
            m_CurrentTextureImage.texture = m_HeldTexture.GetTextureFromPath();
        else
            m_CurrentTextureImage.texture = m_HeldTexture.Texture;
    }
    public void SetHeldTexture(Texture texture)
    {
        m_HeldTexture = new TextureObject(texture);

        UpdateCurrentTexture();
    }
    public void SetHeldTexture(string path)
    {
        m_HeldTexture = new TextureObject(path);

        UpdateCurrentTexture();
    }
    private void Update()
    {
        if (m_HeldTexture == null)
        {
            return;
        }

        if (m_HeldTexture.IsNull())
        {
            return;
        }
            
        if (m_HeldTexture.HasTextureFileChanged())
        {
            UpdateCurrentTexture();
            m_HeldTexture.GetFileInfo();

            onSelectTexture.Invoke();
        }
    }
}