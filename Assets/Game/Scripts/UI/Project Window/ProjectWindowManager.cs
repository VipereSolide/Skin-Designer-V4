using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

using SkinDesigner.SkinSystem;
using SkinDesigner.Inspector;
using SkinDesigner.Textures;
using SkinDesigner.Weapon;
using FeatherLight.Pro;
using SkinDesigner;
using TMPro;
using SFB;

public class ProjectWindowManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static ProjectWindowManager Instance;

    [SerializeField] private ProjectWindowPopup windowPopupManager;
    [SerializeField] private Transform projectWindowItemContainer;
    [SerializeField] private ProjectWindowContentItem projectWindowItemPrefab;
    [SerializeField] private ProjectWindowContentWeapon projectWindowWeaponPrefab;
    [SerializeField] private ProjectWindowContentFolder projectWindowFolderPrefab;
    [SerializeField] private Sprite[] weaponsSprite;

    [SerializeField] private Transform m_dropItemContainer;
    [SerializeField] private Transform m_pathContainer;
    [SerializeField] private Transform m_contentContainer;
    [SerializeField] private TMP_Text m_textPrefab;
    [SerializeField] private GameObject m_arrowPrefab;

    [SerializeField]
    private List<string> m_media = new List<string>();

    private string currentPath = "Root";
    private bool isHighlighted = false;
    private bool isMidAction = false;

    private List<ProjectWindowContentItem> items = new List<ProjectWindowContentItem>();
    private List<ContentItemTextureQueueItem> mediaTextureQueue = new List<ContentItemTextureQueueItem>();

    private bool isDragging = false;
    private ProjectWindowContentItem dragging;
    private ProjectWindowContentItem selected;
    private ProjectWindowContentItem lastDragged;
    private ProjectWindowContentItem highlighted;
    private PointerEventData pointerEventData;

    public string[] Medias
    {
        get { return m_media.ToArray(); }
    }

    public ProjectWindowContentItem[] Items
    {
        get { return items.ToArray(); }
    }

    public ProjectWindowContentItem Selected
    {
        get { return selected; }
    }

    public void ResetMedias()
    {
        m_media.Clear();
    }

    public void SetMedias(string[] values)
    {
        m_media = values.ToList();
    }

    public void AddMedia(string value)
    {
        for (int i = 0; i < m_media.Count; i++)
        {
            if (m_media[i] == value)
            {
                return;
            }
        }

        m_media.Add(value);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdatePath("Root");
    }

    public void UpdatePath(ProjectWindowContentFolder _Folder)
    {
        UpdatePath(_Folder.Path);
    }

    public ProjectWindowContentWeapon CreateWeapon(SkinDesigner.SkinSystem.Weapon weapon)
    {
        int weaponIndex = SkinDesigner.SkinSystem.Environment.WeaponToInt(weapon);

        return CreateWeapon(weaponIndex, currentPath);
    }

    public ProjectWindowContentWeapon CreateWeapon(int weapon, string path)
    {
        string weaponName = SkinDesigner.SkinSystem.Environment.IntToWeapon(weapon).ToString();
        Sprite weaponTexture = weaponsSprite[weapon];

        SkinDesigner.SkinSystem.Weapon weaponType = SkinDesigner.SkinSystem.Environment.IntToWeapon(weapon);

        ProjectWindowContentWeapon instantiated = Instantiate(projectWindowWeaponPrefab, projectWindowItemContainer);
        instantiated.SetData(weaponName, weaponTexture);
        instantiated.SetDirectory(path);

        instantiated.onClick = () =>
        {
            InspectorManager.Instance.SetInspectedWeapon(weaponType);
        };

        instantiated.Weapon = weaponType;

        items.Add(instantiated);
        return instantiated;
    }

    public void CreateWeapon(int weapon)
    {
        this.CreateWeapon(weapon, currentPath);
    }

    public void CreateFolder(TMP_InputField folderName)
    {
        this.CreateFolder(folderName.text);
    }

    public ProjectWindowContentFolder CreateFolder(string folderName)
    {
        ProjectWindowContentFolder instantiated = Instantiate(projectWindowFolderPrefab, projectWindowItemContainer);
        instantiated.SetName(folderName);
        instantiated.SetChildrenPath(currentPath);
        instantiated.SetPath(currentPath + "/" + folderName);

        ProjectWindowContentFolder returnInstantiated = Instantiate(projectWindowFolderPrefab, projectWindowItemContainer);
        returnInstantiated.SetName("..");
        returnInstantiated.SetChildrenPath(currentPath + "/" + folderName);
        returnInstantiated.SetPath(currentPath);
        instantiated.AddChild(returnInstantiated);

        UpdatePath(currentPath);
        
        items.Add(returnInstantiated);
        items.Add(instantiated);

        return instantiated;
    }

    public ProjectWindowContentFolder CreateFolder(string folderName, string _childrenPath, string _currentPath)
    {
        ProjectWindowContentFolder instantiated = Instantiate(projectWindowFolderPrefab, projectWindowItemContainer);
        instantiated.SetName(folderName);
        instantiated.SetChildrenPath(_childrenPath);
        instantiated.SetPath(_currentPath + "/" + folderName);

        ProjectWindowContentFolder returnInstantiated = Instantiate(projectWindowFolderPrefab, projectWindowItemContainer);
        returnInstantiated.SetName("..");
        returnInstantiated.SetChildrenPath(_childrenPath + "/" + folderName);
        returnInstantiated.SetPath(_currentPath);
        instantiated.AddChild(returnInstantiated);

        UpdatePath(_currentPath);
        
        items.Add(returnInstantiated);
        items.Add(instantiated);

        return instantiated;
    }

    public void ImportMedia()
    {
        string[] selectedFiles = StandaloneFileBrowser.OpenFilePanel("Select a media...", FileSystem.LastPath, "png", true);

        if (selectedFiles.Length <= 0)
        {
            return;
        }

        foreach (string path in selectedFiles)
        {
            FileSystem.LastPath = path;

            CreateMedia(Path.GetFileNameWithoutExtension(path), path);
        }
    }

    public void CreateMedia(string name, Texture texture)
    {
        ProjectWindowContentItem instantiated = Instantiate(projectWindowItemPrefab, projectWindowItemContainer);
        instantiated.SetData(name, TextureHelper.ToSprite((Texture2D)texture));
        instantiated.SetDirectory(currentPath);

        items.Add(instantiated);
    }

    public void CreateMedia(string name, Sprite texture)
    {
        ProjectWindowContentItem instantiated = Instantiate(projectWindowItemPrefab, projectWindowItemContainer);
        instantiated.SetData(name, texture);
        instantiated.SetDirectory(currentPath);

        items.Add(instantiated);
    }

    public void CreateMedia(string name, Sprite texture, Action onClick)
    {
        ProjectWindowContentItem instantiated = Instantiate(projectWindowItemPrefab, projectWindowItemContainer);
        instantiated.SetData(name, texture);
        instantiated.SetDirectory(currentPath);
        instantiated.onClick = onClick;

        items.Add(instantiated);
    }

    public ProjectWindowContentItem CreateMedia(string name, string texture)
    {
        ProjectWindowContentItem instantiated = Instantiate(projectWindowItemPrefab, projectWindowItemContainer);
        instantiated.SetData(name, TextureHelper.ToSprite(Texture2D.blackTexture), texture);
        instantiated.SetDirectory(currentPath);

        mediaTextureQueue.Add(new ContentItemTextureQueueItem(texture, instantiated));
        items.Add(instantiated);

        DisplayContentItemTextures();

        return instantiated;
    }

    public void UpdatePath(string _NewPath)
    {
        currentPath = _NewPath;

        foreach (Transform _GameObject in m_pathContainer)
            Destroy(_GameObject.gameObject);

        CreatePathObjectsFromString(_NewPath);

        foreach (Transform _contentItem in m_contentContainer)
        {
            ProjectWindowContentItem _item = _contentItem.GetComponent<ProjectWindowContentItem>();

            if (_item == null)
            {
                continue;
            }

            _item.gameObject.SetActive(_item.ChildrenPath == _NewPath);
        }
    }

    public void UpdateProject()
    {
        foreach (Transform _contentItem in m_contentContainer)
        {
            ProjectWindowContentItem _item = _contentItem.GetComponent<ProjectWindowContentItem>();

            if (_item == null)
            {
                continue;
            }

            _item.gameObject.SetActive(_item.ChildrenPath == currentPath);
        }
    }

    private GameObject[] CreatePathObjectsFromString(string _path)
    {
        List<GameObject> _output = new List<GameObject>();

        string[] _names = _path.Split('/');

        for (int i = 0; i < _names.Length; i++)
        {
            string _name = _names[i];

            TMP_Text _instantiated = Instantiate(m_textPrefab, m_pathContainer);
            _instantiated.text = _name;


            if (i != _names.Length - 1)
            {
                if (i > 0)
                {
                    string _newPath = "";

                    for (int c = 0; c <= i; c++)
                    {
                        _newPath += _names[c] + ((c < i) ? "/" : "");
                    }

                    EventTrigger trigger = _instantiated.gameObject.AddComponent<EventTrigger>();
                    var pointerDown = new EventTrigger.Entry();
                    pointerDown.eventID = EventTriggerType.PointerDown;
                    pointerDown.callback.AddListener((e) => UpdatePath(_newPath));
                    trigger.triggers.Add(pointerDown);
                }
                else
                {
                    EventTrigger trigger = _instantiated.gameObject.AddComponent<EventTrigger>();
                    var pointerDown = new EventTrigger.Entry();
                    pointerDown.eventID = EventTriggerType.PointerDown;
                    pointerDown.callback.AddListener((e) => UpdatePath(_names[0]));
                    trigger.triggers.Add(pointerDown);
                }
            }

            _output.Add(_instantiated.gameObject);

            if (i < _names.Length - 1)
            {
                GameObject _arrow = Instantiate(m_arrowPrefab, m_pathContainer);
                _output.Add(_arrow);
            }
        }

        LayoutGroup _layout = m_pathContainer.GetComponent<LayoutGroup>();

        if (_layout != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layout.GetComponent<RectTransform>());
        }

        return _output.ToArray();
    }

    public void DestroyItem(ProjectWindowContentItem item)
    {
        if (item.GetType() == typeof(ProjectWindowContentFolder))
        {
            ProjectWindowContentFolder folder = (ProjectWindowContentFolder)(item as ProjectWindowContentFolder);

            foreach (ProjectWindowContentItem i in folder.Children)
            {
                DestroyItem(i);
            }
        }

        WeaponManager manager = WeaponManager.Instance;

        for(int i = 0; i < manager.CurrentTextures.Length; i++)
        {
            TextureObject held = item.HeldTexture;
            TextureMap map = SkinDesigner.SkinSystem.Environment.IntToTextureMap(i);

            Debug.Log(map);

            if (held.TexturePath != string.Empty && !string.IsNullOrWhiteSpace(held.TexturePath))
            {
                Debug.Log("has a texture.");
                if (manager.CurrentTextures[i].TexturePath == held.TexturePath || manager.CurrentTextures[i].Texture == held.Texture)
                {
                    Debug.Log("the texture objects correspond.");
                    manager.RemoveTexture(map);
                }
            }
            else
            {
            }
        }

        Destroy(item.gameObject);
        items.Remove(item);
    }

    public void DestroySelectedItem()
    {
        if (selected != null)
        {
            DestroyItem(selected);
        }
    }

    public void DisplayContentItemTextures()
    {
        StartCoroutine(DisplayContentItemTexturesCoroutine());
    }

    public IEnumerator DisplayContentItemTexturesCoroutine()
    {
        while (mediaTextureQueue.Count != 0)
        {
            ContentItemTextureQueueItem i = mediaTextureQueue.Last<ContentItemTextureQueueItem>();

            if (i.item.Background != null)
            {
                mediaTextureQueue.Remove(i);
                yield return null;
            }

            string texturePath = i.texture;

            Texture2D output = new Texture2D(2, 2);
            yield return output.LoadImage(File.ReadAllBytes(texturePath));
            TextureHelper.Scale(output, 128, 128);
            output.Apply();

            i.item.SetData(i.item.Name, TextureHelper.ToSprite(output));
            mediaTextureQueue.Remove(i);
        }
    }

    public ProjectWindowContentItem Highlighted
    {
        get
        {
            return highlighted;
        }
    }

    public void SetHighlighted(ProjectWindowContentItem item)
    {
        highlighted = item;
    }

    private void ResetSelected()
    {
        selected.IsSelected = false;
        selected = null;
    }

    private void SetSelected(ProjectWindowContentItem item)
    {
        selected = item;
        selected.IsSelected = true;
    }

    public void RenameSelectedItem(TMP_InputField name)
    {
        if (selected != null) selected.SetData(name.text, selected.Background);
    }

    public void SetMidAction(bool value)
    {
        isMidAction = value;
    }

    private void Update()
    {
        bool isClickingOnHighlighted = (Input.GetMouseButtonDown(0) && highlighted != null);
        bool isClickingOnSelected = (selected != null && Input.GetMouseButtonDown(0) && highlighted == selected);
        bool isClickingNowhere = (Input.GetMouseButtonDown(0) && highlighted == null);

        if (isClickingOnHighlighted)
        {
            if (selected != null)
            {
                ResetSelected();
            }

            SetSelected(highlighted);
        }

        if (selected != null)
        {
            if (isClickingOnSelected && selected.CanBeDragged)
            {
                StartDragging();
            }

            if (isClickingNowhere && !isMidAction)
            {
                ResetSelected();
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            StopDragging();
        }

        if (isDragging && dragging != null)
        {
            dragging.transform.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (highlighted != null)
            {
                windowPopupManager.OpenPopup(1, highlighted);
            }
            else if (isHighlighted)
            {
                windowPopupManager.OpenPopup(0, null);
            }
        }
    }

    public void StartDragging()
    {
        isDragging = true;
        dragging = highlighted;

        dragging.transform.SetParent(m_dropItemContainer);
        dragging.transform.localEulerAngles = new Vector3(0, 0, 15);

        dragging.GetComponent<CanvasGroup>().blocksRaycasts = false;
        dragging.GetComponent<CanvasGroup>().interactable = false;
    }

    public void StopDragging()
    {
        pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {

                ProjectWindowContentFolder folder = result.gameObject.GetComponent<ProjectWindowContentFolder>();

                if (folder != null)
                {
                    dragging.SetDirectory(folder);

                    if (folder.Path != currentPath)
                    {
                        dragging.gameObject.SetActive(false);
                    }
                }

                textureHolder _textureHolder = result.gameObject.GetComponent<textureHolder>();

                if (_textureHolder != null && dragging.GetType() != typeof(ProjectWindowContentWeapon))
                {
                    string textureName = _textureHolder.transform.parent.parent.name;
                    TextureMap textureMap = (TextureMap)System.Enum.Parse(typeof(TextureMap), textureName);

                    SkinDesigner.Weapon.WeaponManager.Instance.SetTexture(textureMap, dragging.HeldTexture);
                    InspectorManager.Instance.UpdateTextureHolder(textureMap);
                }
            }
        }

        UpdateProject();

        isDragging = false;

        dragging.transform.SetParent(projectWindowItemContainer);
        dragging.transform.localEulerAngles = new Vector3(0, 0, 0);

        dragging.GetComponent<CanvasGroup>().interactable = true;
        dragging.GetComponent<CanvasGroup>().blocksRaycasts = true;
        dragging = null;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.isHighlighted = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.isHighlighted = true;
    }

    public class ContentItemTextureQueueItem
    {
        public string texture;
        public ProjectWindowContentItem item;

        public ContentItemTextureQueueItem(string _texture, ProjectWindowContentItem _item)
        {
            this.texture = _texture;
            this.item = _item;
        }
    }
}
