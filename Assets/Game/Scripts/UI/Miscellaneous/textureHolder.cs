using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

using System.IO;

using SkinDesigner.SkinSystem;
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

    private Texture2D m_HeldTexture;
    public Texture2D Texture { get { return m_HeldTexture; } }

    private string m_texturePath;

    public string LastFileLocation
    {
        get
        {
            return FileSystem.LastPath;
        }
    }

    public string TexturePath
    {
        get
        {
            return m_texturePath;
        }
    }

    public void SetFetchByUrlWindowActive(bool value)
    {
        StartCoroutine(CanvasGroupHelper.Fade(m_FetchByUrlWindow, value, m_FetchByUrlWindowAlphaTime));
    }

    public void TrashTexture()
    {
        UpdateCurrentTexture(null);
    }

    public void FetchTextureOnDisk()
    {
        string[] _paths = StandaloneFileBrowser.OpenFilePanel("Select a texture...", FileSystem.LastPath, "png", false);

        if (_paths.Length <= 0)
            return;
        
        string _currentPath = _paths[0];
        m_texturePath = _currentPath;

        FileSystem.LastPath = Directory.GetDirectoryRoot(_currentPath);

        byte[] _textureInBytes = File.ReadAllBytes(_currentPath);
        Texture2D _texture = new Texture2D(2,2);

        _texture.LoadImage(_textureInBytes);
        _texture.Apply();

        UpdateCurrentTexture(_texture);
        onSelectTexture.Invoke();
    }

    public void FetchTextureByURL()
    {
        Texture2D _texture = (Texture2D)m_FetchByUrlImage.texture;
        
        UpdateCurrentTexture(_texture);
    }

    private void UpdateCurrentTexture(Texture2D _newTexture)
    {
        m_HeldTexture = _newTexture;
        m_CurrentTextureImage.texture = m_HeldTexture;
    }

    public void UpdateCurrentTexture()
    {
        m_CurrentTextureImage.texture = m_HeldTexture;
    }

    public void SetHeldTexture(Texture texture)
    {
        m_HeldTexture = (Texture2D)texture;

        UpdateCurrentTexture();
    }
}