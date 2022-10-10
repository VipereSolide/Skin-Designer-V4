using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectWindowContentFolder : ProjectWindowContentItem, IPointerClickHandler
{
    [Header("Folder Settings")]
    [SerializeField] private List<ProjectWindowContentItem> children = new List<ProjectWindowContentItem>();
    [SerializeField] private string m_path = string.Empty;
    [SerializeField] private ProjectWindowContentFolder returnFolder;

    public ProjectWindowContentFolder ReturnFolder { get => returnFolder; set => returnFolder = value; }

    public ProjectWindowContentItem[] Children
    {
        get { return children.ToArray(); }
    }

    public override void SetDirectory(ProjectWindowContentFolder folder)
    {
        base.SetDirectory(folder);
        returnFolder.SetChildrenPath(m_path);
        returnFolder.SetPath(childrenPath);
    }

    public void AddChild(ProjectWindowContentItem item)
    {
        children.Add(item);
    }

    public void RemoveChild(ProjectWindowContentItem item)
    {
        children.Remove(item);
    }

    public void SetName(string name)
    {
        this.name = name;
        UpdateItem();
    }

    public void SetPath(string path)
    {
        this.m_path = path;
        UpdateItem();
    }

    public void SetChildrenPath(string path)
    {
        this.childrenPath = path;
        UpdateItem();
    }

    public string GoBackInPath()
    {
        string[] paths = m_path.Split('/');
        string p = m_path.Replace(paths[paths.Length - 1], "");
        p = p.Remove(m_path.Length - 2, 1);
        
        return p;
    }

    public string Path
    {
        get { return m_path; }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSelected && eventData.clickCount > 1)
        {
            ProjectWindowManager.Instance.UpdatePath(this);
        }
    }

    private void Start()
    {
        StartItem();
    }

    void Update()
    {
        UpdateGraphics();
    }
}
