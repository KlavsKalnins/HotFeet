using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{
    public static MeshGeneration instance;
    [SerializeField] private Material mat;
    [SerializeField] private Vector2[] vertices2D;
    public static int generatedMeshCount;
    [SerializeField] private float meshGenTime = 0.5f;
    public List<GameObject> generatedMeshes;

    private void OnEnable() => instance = this;
    

    public void MeshGen(Vector2[] vertices2D, Vector3 position)
    {
        StartCoroutine(BuildMesh(vertices2D, position));
    }

    IEnumerator BuildMesh(Vector2[] vertices2D, Vector3 position)
    {
        generatedMeshCount++;
        
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();
        
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = new Vector3(vertices2D[i].x, 0, vertices2D[i].y);
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        yield return new WaitForSeconds(meshGenTime);

        GameObject obj = new GameObject("Mesh-" + generatedMeshCount + " (" + vertices2D.Length + " )", 
            typeof(MeshFilter), 
            typeof(MeshRenderer), 
            typeof(MeshCollider), 
            typeof(Rigidbody));
        
        obj.layer = 8;
        obj.tag = "GeneratedMesh";
        obj.GetComponent<MeshFilter>().mesh = mesh;
        obj.GetComponent<MeshRenderer>().material = mat;
        var mc = obj.GetComponent<MeshCollider>();
        mc.sharedMesh = mesh;
        mc.convex = true;
        mc.isTrigger = true;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        
        obj.transform.position = position;
        generatedMeshes.Add(obj);
    }

    public void ClearMeshes()
    {
        foreach (var gm in generatedMeshes)
            Destroy(gm);
        generatedMeshes.Clear();
        generatedMeshCount = 0;
    }
}