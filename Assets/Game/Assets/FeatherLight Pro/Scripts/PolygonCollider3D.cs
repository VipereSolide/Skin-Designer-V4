using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

namespace FeatherLight.Pro
{
    public class PolygonCollider3D : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private string path;

        // Private references.
        private PolygonCollider2D m_PolygonCollider2D;
        private SpriteRenderer m_SpriteRenderer;
        private MeshCollider m_MeshCollider;

        public Mesh m_Mesh;

        private void Start()
        {
            // Assign the private references.
            m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
            m_PolygonCollider2D = transform.GetComponent<PolygonCollider2D>();

            List<Mesh> meshes = new List<Mesh>();

            for (int i = 0; i < sprites.Length; i++)
            {
                // Creating the mesh collider.
                m_SpriteRenderer.sprite = sprites[i];
                Destroy(m_PolygonCollider2D);

                m_PolygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
                m_Mesh = m_PolygonCollider2D.CreateMesh(false, false);

                MeshUtility.Optimize(m_Mesh);
                meshes.Add(m_Mesh);
            }
            MeshSaverEditor.SaveMeshNewInstanceItem(meshes.ToArray(), sprites[0].name, path);
        }
    }
}
#endif
