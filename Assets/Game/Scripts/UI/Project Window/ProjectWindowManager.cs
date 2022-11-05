using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

using SkinDesigner.SkinSystem;
using SkinDesigner.Inspector;
using SkinDesigner.Textures;
using SkinDesigner.Weapon;
using SkinDesigner;

using TMPro;
using SFB;

using FeatherLight.Pro.Console;
using FeatherLight.Pro;
using UnityEngine.Events;

public class ProjectWindowManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static ProjectWindowManager Instance;

    [Header("General")]
    [SerializeField] private ProjectWindowPopup windowPopupManager;

    [Space]
    [SerializeField] private Transform itemContainer;

    [Header("Paths")]
    [SerializeField] private GameObject m_arrowPrefab;
    [SerializeField] private TMP_Text m_textPrefab;
    [SerializeField] private Transform m_pathContainer;

    [Header("Dragging")]
    [SerializeField] private DraggingItemGhost draggingItemGhost;
    [SerializeField] private ScrollRect scrollRect;

    [Space]
    [SerializeField] public UnityEvent<ProjectWindowContentItem[]> onStartDragging;
    [SerializeField] public UnityEvent<ProjectWindowContentItem[]> onStopDragging;

    [Header("Items")]
    [SerializeField] private ProjectWindowContentItem projectWindowItemPrefab;
    [SerializeField] private ProjectWindowContentWeapon projectWindowWeaponPrefab;
    [SerializeField] private ProjectWindowContentFolder projectWindowFolderPrefab;
    [SerializeField] private ProjectWindowContentFolder projectWindowFolderReturnPrefab;


    [Header("Behaviour")]
    [SerializeField] private Sprite[] weaponsSprite;
    
    [SerializeField]
    private List<string> m_media = new List<string>();

    private string currentPath = "Root";
    private bool isHighlighted = false;
    private bool isMidAction = false;

    private List<ProjectWindowContentItem> items = new List<ProjectWindowContentItem>();
    private List<ContentItemTextureQueueItem> mediaTextureQueue = new List<ContentItemTextureQueueItem>();

    private bool isDragging = false;
    private List<ProjectWindowContentItem> dragging = new List<ProjectWindowContentItem>();
    private List<ProjectWindowContentItem> selected = new List<ProjectWindowContentItem>();
    private ProjectWindowContentItem lastDragged;
    private ProjectWindowContentItem highlighted;
    private PointerEventData pointerEventData;

    private Vector3 lastMousePosition;
    private bool isMouseMoving;

    public string[] Medias
    {
        get { return m_media.ToArray(); }
    }
    public ProjectWindowContentItem[] Items
    {
        get { return items.ToArray(); }
    }
    public ProjectWindowContentItem[] Selected
    {
        get { return selected.ToArray(); }
    }
    public ProjectWindowContentItem Highlighted
    {
        get
        {
            return highlighted;
        }
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

        Weapon weaponType = SkinDesigner.SkinSystem.Environment.IntToWeapon(weapon);

        ProjectWindowContentWeapon instantiated = Instantiate(projectWindowWeaponPrefab, itemContainer);
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
        return CreateFolder(folderName, currentPath, currentPath);
    }
    public ProjectWindowContentFolder CreateFolder(string folderName, string _childrenPath, string _currentPath)
    {
        ProjectWindowContentFolder instantiated = Instantiate(projectWindowFolderPrefab, itemContainer);
        instantiated.SetName(folderName);
        instantiated.SetChildrenPath(_childrenPath);
        instantiated.SetPath(_currentPath + "/" + folderName);
        instantiated.transform.SetAsFirstSibling();

        ProjectWindowContentFolder returnInstantiated = Instantiate(projectWindowFolderReturnPrefab, itemContainer);
        returnInstantiated.SetName("Go Back");
        returnInstantiated.SetChildrenPath(_childrenPath + "/" + folderName);
        returnInstantiated.SetPath(_currentPath);
        returnInstantiated.transform.SetSiblingIndex(1);
        instantiated.AddChild(returnInstantiated);
        instantiated.ReturnFolder = returnInstantiated;

        UpdatePath(_currentPath);
        
        items.Insert(0, returnInstantiated);
        items.Insert(0, instantiated);

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
    public void ResetAllItems()
    {
        foreach(ProjectWindowContentItem i in this.items)
        {
            Destroy(i.gameObject);
        }

        ResetMedias();
        this.items.Clear();
    }
    public void CreateMedia(string name, Texture texture)
    {
        ProjectWindowContentItem instantiated = Instantiate(projectWindowItemPrefab, itemContainer);
        instantiated.SetData(name, TextureHelper.ToSprite((Texture2D)texture));
        instantiated.SetDirectory(currentPath);

        items.Add(instantiated);
    }
    public void CreateMedia(string name, Sprite texture)
    {
        ProjectWindowContentItem instantiated = Instantiate(projectWindowItemPrefab, itemContainer);
        instantiated.SetData(name, texture);
        instantiated.SetDirectory(currentPath);

        items.Add(instantiated);
    }
    public void CreateMedia(string name, Sprite texture, System.Action onClick)
    {
        ProjectWindowContentItem instantiated = Instantiate(projectWindowItemPrefab, itemContainer);
        instantiated.SetData(name, texture);
        instantiated.SetDirectory(currentPath);
        instantiated.onClick = onClick;

        items.Add(instantiated);
    }
    public ProjectWindowContentItem CreateMedia(string name, string texture)
    {
        ProjectWindowContentItem instantiated = Instantiate(projectWindowItemPrefab, itemContainer);
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

        foreach (Transform _contentItem in itemContainer)
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
        foreach (Transform _contentItem in itemContainer)
        {
            ProjectWindowContentItem _item = _contentItem.GetComponent<ProjectWindowContentItem>();

            if (_item == null)
            {
                continue;
            }

            _item.gameObject.SetActive(_item.ChildrenPath == currentPath);
        }
    }
    private GameObject[] CreatePathObjectsFromString(string initialPath)
    {
        // all the objects that will be returned as the path objects.
        List<GameObject> pathObjects = new List<GameObject>();
        // we split the path into several segments to treat them all separately.
        string[] pathSegments = initialPath.Split('/');

        // we do the path object treatment for all the path segments.
        for (int i = 0; i < pathSegments.Length; i++)
        {
            string currentSegment = pathSegments[i];

            // create the segment name text.
            TMP_Text segmentText = Instantiate(m_textPrefab, m_pathContainer);
            segmentText.text = currentSegment;

            if (i != pathSegments.Length - 1)
            {
                string pathToUpdate = pathSegments[0];

                if (i > 0)
                {
                    string correctedPath = "";

                    for (int c = 0; c <= i; c++)
                    {
                        correctedPath += pathSegments[c] + ((c < i) ? "/" : "");
                    }

                    pathToUpdate = correctedPath;
                }

                EventTrigger.Entry pointerDown = new EventTrigger.Entry();
                pointerDown.eventID = EventTriggerType.PointerDown;
                pointerDown.callback.AddListener((e) => { UpdatePath(pathToUpdate); });

                EventTrigger trigger = segmentText.gameObject.GetComponent<EventTrigger>();
                trigger.triggers.Add(pointerDown);
            }

            pathObjects.Add(segmentText.gameObject);

            if (i < pathSegments.Length - 1)
            {
                GameObject pathSeparator = Instantiate(m_arrowPrefab, m_pathContainer);
                pathObjects.Add(pathSeparator);
            }
        }

        LayoutGroup containerLayoutGroup = m_pathContainer.GetComponent<LayoutGroup>();

        if (containerLayoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(containerLayoutGroup.GetComponent<RectTransform>());
        }

        return pathObjects.ToArray();
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

        if (manager.CurrentWeapon == null)
        {
            Debug.Log($"Project > Current weapon is null. Destroying item \"{item.Name}\" nonetheless...");
            Destroy(item.gameObject);
            items.Remove(item);

            return;
        }

        if (manager.CurrentTextures == null)
        {
            Debug.Log($"Project > Current weapon textures are null. Destroying item \"{item.Name}\" nonetheless...");
            Destroy(item.gameObject);
            items.Remove(item);

            return;
        }

        Destroy(item.gameObject);
        items.Remove(item);
    }
    public void DestroySelectedItem()
    {
        if (selected == null)
        {
            return;
        }

        if (selected.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < selected.Count; i++)
        {
            DestroyItem(selected[i]);
        }

        ResetSelected();
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
            output.filterMode = FilterMode.Point;
            output.wrapMode = TextureWrapMode.Repeat;

            if (output.width > 128)
            {
                TextureHelper.Scale(output, 128, 128);
            }

            output.Apply();

            i.item.SetData(i.item.Name, TextureHelper.ToSprite(output));
            mediaTextureQueue.Remove(i);
        }
    }
    public void SetHighlighted(ProjectWindowContentItem item)
    {
        highlighted = item;
    }
    private void ResetSelected()
    {
        for (int i = 0; i < selected.Count; i++)
        {
            selected[i].IsSelected = false;
        }
     
        selected.Clear();
    }
    private void SetSelected(ProjectWindowContentItem item)
    {
        selected.Add(item);
        item.IsSelected = true;
    }
    private void RemoveSelected(ProjectWindowContentItem item)
    {
        item.IsSelected = false;
        selected.Remove(item);
    }
    public void RenameSelectedItem(TMP_InputField name)
    {
        if (selected == null)
        {
            return;
        }

        if (selected.Count > 1)
        {
            return;
        }
        
        selected[0].SetData(name.text, selected[1].Background);
    }
    public void SetMidAction(bool value)
    {
        isMidAction = value;
    }
    private void Update()
    {
        float mouseDistance = Vector3.Distance(Input.mousePosition, lastMousePosition);
        isMouseMoving = (mouseDistance > 5);

        HandleSelectedActions();
        HandleRightClickMenu();

        lastMousePosition = Input.mousePosition;
    }
    private void HandleSelectedActions()
    {
        HandleDragging();

        if (selected == null)
        {
            return;
        }

        bool clickMouseButton = Input.GetMouseButtonUp(0);
        bool clickMouseButtonHeld = Input.GetMouseButton(0);
        bool isClickingNowhere = (clickMouseButton && highlighted == null);

        bool pressAddSelectedKeys = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));

        // If there is a highlighted object and the LMB is being held, we can imagine
        // that the user may want to drag this item.
        if (highlighted != null && clickMouseButtonHeld)
        {
            // If the user is holding the LMB for longer than 0.35 seconds
            // we start dragging.
            if (Mouse.instance.LMBHeldTime() > 0.35f)
            {
                HandleDraggingEvents();
             
                // As we're dragging, we don't want to execute any
                // explorer type logic.
                return;
            }
            else
            {
                // Sets the position of the draggingItemGhost in advance so
                // the user doesn't see it being teleported when it's activated.
                draggingItemGhost.transform.position = Input.mousePosition;
            }
        }

        // If there is selected items and the user is not clicking on an item and is not
        // midaction (for example in the right click menu), deselect everything.
        if (selected.Count > 0 && isClickingNowhere && !isMidAction)
        {
            ResetSelected();

            // No need for any further logic as we just deselected everything.
            return;
        }

        // If there is no selected items yet, we can select one by just clicking on it.
        if (selected.Count == 0)
        {
            // If the user clicks on the highlighted, add it to the selected item list
            // just after clearing it to avoid weird scenario where there is a ghost
            // selected item in the list.
            if (clickMouseButton && highlighted != null)
            {
                selected.Clear();
                SetSelected(highlighted);

                return;
            }
        }
        // If there is only one item selected, we can perform simple operations like
        // rename the item or drag it to a weapon holder for example.
        else
        {
            // If the user clicks on another item while pressing control, it will add this item to the selected list. If the item was
            // already selected, we simply unselect it. If the user is not pressing control, just unselect the first one and select the
            // new highlighted one.
            if (highlighted != null)
            {
                if (clickMouseButton)
                {
                    if (pressAddSelectedKeys)
                    {
                        if (highlighted.IsSelected)
                        {
                            RemoveSelected(highlighted);
                        }
                        else
                        {
                            SetSelected(highlighted);
                        }

                        return;
                    }
                    // If the user is pressing shift and if there is only one object selected for now, do a row selection.
                    else if (selected.Count == 1 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                    {
                        // Get the sibling index of the currently selected item. This is where the selection will start.
                        int start = selected[0].transform.GetSiblingIndex();
                        // Get the sibling index of the highlighted item plus one. This is where selection will end.
                        // We add one to it so the for loop also includes the highlighted item itself.
                        int end = highlighted.transform.GetSiblingIndex() + 1;

                        // If the selection end is smaller than it's start, we need to swap the two.
                        if (end < start)
                        {
                            // Storing the last end value to assign it to the start one.
                            // We retrieve 2 from it so will include the item too.
                            int t = end - 2;

                            // Swapping the values.
                            end = start;
                            start = t;
                        }

                        // Setting the selected items.
                        for (int i = start + 1; i < end; i++)
                        {
                            SetSelected(items[i]);
                        }

                        return;
                    }
                    // If the user clicks on another item, select this one instead.
                    else if (!selected.Contains(highlighted))
                    {
                        ResetSelected();
                        SetSelected(highlighted);
                    }
                }
            }
            else if (clickMouseButton)
            {
                ResetSelected();
            }
        }

    }
    private void HandleDragging()
    {
        // If the user is dragging an item and let up the mouse button, then when can
        // stop dragging.
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            StopDragging();
        }
    }
    private void HandleDraggingEvents()
    {
        // Check if any of the selected are set as "not draggable". If so
        // do not drag the item.
        bool canBeDragged = true;

        for (int i = 0; i < selected.Count; i++)
        {
            if (selected[i].CanBeDragged == false)
            {
                canBeDragged = false;
                break;
            }
        }

        if (canBeDragged)
        {
            StartDragging();
        }
    }
    private void HandleRightClickMenu()
    { 
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
        dragging = selected;

        scrollRect.enabled = false;
        draggingItemGhost.gameObject.SetActive(true);
        draggingItemGhost.Init((dragging.Count > 1) ? DraggingItemGhost.GhostMode.Multiple : DraggingItemGhost.GhostMode.Single, dragging.Count);

        for (int i = 0; i < dragging.Count; i++)
        {
            dragging[i].Group.alpha = 0.5f;
            dragging[i].Group.interactable = false;
            dragging[i].Group.blocksRaycasts = false;
        }

        if (dragging.Count > 0)
        {
            onStartDragging?.Invoke(dragging.ToArray());
        }
    }
    public void StopDragging()
    {
        scrollRect.enabled = true;
        draggingItemGhost.gameObject.SetActive(false);

        pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        // 0 = texture holder; 1 = folder;
        int itemType = 0;
        ProjectWindowContentFolder folder = null;

        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                folder = result.gameObject.GetComponent<ProjectWindowContentFolder>();

                // if we hit a folder.
                if (folder != null)
                {
                    itemType = 1;
                    break;
                }
            }
        }

        if (itemType == 1)
        {
            foreach (ProjectWindowContentItem item in dragging)
            {
                item.SetDirectory(folder);

                if (folder.Path != currentPath)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (dragging.Count > 0)
            { 
                onStopDragging?.Invoke(dragging.ToArray());
            }
        }


        UpdateProject();
        isDragging = false;

        for (int i = 0; i < dragging.Count; i++)
        {
            dragging[i].Group.alpha = 1f;
            dragging[i].Group.interactable = true;
            dragging[i].Group.blocksRaycasts = true;

        }

        for (int c = 0; c < items.Count; c++)
        {
            if (items[c].IsSelected) RemoveSelected(items[c]);
        }

        dragging.Clear();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        this.isHighlighted = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.isHighlighted = true;
    }
    public void EditExternally(ProjectWindowContentItem[] items)
    {
        Debug.Log("hi?");
        foreach (ProjectWindowContentItem item in items)
        {
            if (item.GetType() == typeof(ProjectWindowContentWeapon) || item.GetType() == typeof(ProjectWindowContentFolder))
            {
                continue;
            }

            if (item.HeldTexture != null)
            {
                Application.OpenURL(item.HeldTexture.TexturePath);
            }
        }
    }
    public void EditExternally()
    {
        EditExternally(Selected);
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