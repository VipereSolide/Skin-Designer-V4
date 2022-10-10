#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MeshSaverEditor
{

#if UNITY_EDITOR
    [MenuItem("CONTEXT/MeshFilter/Save Mesh...")]
    public static void SaveMeshInPlace(MenuCommand menuCommand)
    {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMesh(m, m.name, false, true);
    }

    [MenuItem("CONTEXT/MeshFilter/Save Mesh As New Instance...")]
    public static void SaveMeshNewInstanceItem(MenuCommand menuCommand)
    {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMesh(m, m.name, true, true);
    }

    public static void SaveMeshNewInstanceItem(Mesh[] meshes, string name, string path)
    {
        SaveMeshes(meshes, name, true, true);
    }

	public static void SaveMeshNewInstanceItem(Mesh mesh, string name, string path)
    {
        SaveMesh(mesh, name, true, true);
    }

    public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }

    public static void SaveMeshes(Mesh[] meshes, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFolderPanel("Save Separate Mesh Asset", "Assets/", name);
        if (string.IsNullOrEmpty(path)) return;

		int index = 0;

        foreach (Mesh m in meshes)
        {
			Debug.Log(path + "/Mesh " + index + ".asset");
        	string path2 = FileUtil.GetProjectRelativePath(path + "/Mesh " + index + ".asset");
            Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(m) as Mesh : m;

            if (optimizeMesh)
                MeshUtility.Optimize(meshToSave);

            AssetDatabase.CreateAsset(meshToSave, path2);
            AssetDatabase.SaveAssets();

			index++;
        }
    }
#endif
}