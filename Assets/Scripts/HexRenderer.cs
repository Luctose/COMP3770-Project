using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Properties of the Hex face
public struct Face{
	public List<Vector3> vertices;
	public List<int> triangles;
	public List<Vector2> uvs;
	
	public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs){
		this.vertices = vertices;
		this.triangles = triangles;
		this.uvs = uvs;
	}
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexRenderer : MonoBehaviour
{
	private Mesh m_mesh;
	private MeshFilter m_meshFilter;
	private MeshRenderer m_meshRenderer;
	
	private List<Face> m_faces;
	
	public Material material;
	
	public float height;
	public float outerSize;
	public float innerSize;
	
	// Prepare the mesh components
	private void Awake(){
		m_meshFilter = GetComponent<MeshFilter>();
		m_meshRenderer = GetComponent<MeshRenderer>();
		
		m_mesh = new Mesh();
		m_mesh.name = "Hex";
		
		m_meshFilter.mesh = m_mesh;
		m_meshRenderer.material = material;
	}
	
	private void OnEnable(){
		DrawMesh();
	}
	
	public void DrawMesh(){
		DrawFaces();
		CombineFaces();
	}
	
	private void DrawFaces(){
		m_faces = new List<Face>();
		
		for(int point = 0; point < 6; point++){
			m_faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, point));
		}
	}
	
	private void CombineFaces(){
		List<Vector3> vertices = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Vector2> uvs = new List<Vector2>();
		
		for(int i = 0; i < m_faces.Count; i++){
			// Add the vertices
			vertices.AddRange(m_faces[i].vertices);
			uvs.AddRange(m_faces[i].uvs);
			
			// Offset the triangles
			int offset = (i * 4);
			foreach(int triangle in m_faces[i].triangles){
				tris.Add(triangle + offset);
			}
		}
		
		m_mesh.vertices = vertices.ToArray();
		m_mesh.triangles = tris.ToArray();
		m_mesh.uv = uvs.ToArray();
		m_mesh.RecalculateNormals();
	}
	
	private Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false){
		Vector3 pointA = GetPoint(innerRad, heightB, point);
		Vector3 pointB = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
		Vector3 pointC = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
		Vector3 pointD = GetPoint(outerRad, heightA, point);
		
		List<Vector3> vertices = new List<Vector3>() {pointA, pointB, pointC, pointD };
		List<int> triangles = new List<int>() {0, 1, 2, 2, 3, 0 };
		List<Vector2> uvs = new List<Vector2>() {new Vector2(0,0), new Vector2(1,0), new Vector2(1,1), new Vector2(0,1)};
		if(reverse){
			vertices.Reverse();
		}
		
		return new Face(vertices, triangles, uvs);
	}
	
	protected Vector3 GetPoint(float size, float height, int index){
		float angle_deg = 60 * index;
		float angle_rad = Mathf.PI / 180f * angle_deg;
		return new Vector3((size * Mathf.Cos(angle_rad)), height, size * Mathf.Sin(angle_rad));
	}
	
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
