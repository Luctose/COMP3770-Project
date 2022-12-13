using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridMaster{
	public class NodeReferences : MonoBehaviour{
		public MeshRenderer tileRenderer;
		public Material[] tileMaterials;	// Should have 2 materials minimum
		public Material defaultMaterial;
		// Material[0] is the walkable material
		// Material[1] is the unwalkable material
		
		void start(){
			tileRenderer = GetComponent<MeshRenderer>();
			tileRenderer.material = defaultMaterial;
		}
		
		// GridBase tells this class which material to render
		public void ChangeTileMaterial(int index){
			tileRenderer.material = tileMaterials[index];
		}
	}
}