
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;

#if !UNITY_FLASH
public class MegaCopyObjects : MonoBehaviour
{
	[MenuItem("GameObject/Mega Instance Object")]
	static void InstanceModifiedMesh()
	{
		GameObject from = Selection.activeGameObject;
		MegaCopyObject.InstanceObject(from);
	}

	[MenuItem("GameObject/Mega Copy Object")]
	static void DoCopyObjects()
	{
		GameObject from = Selection.activeGameObject;
		MegaCopyObject.DoCopyObjects(from);
	}

	[MenuItem("GameObject/Mega Copy Hier")]
	static void DoCopyObjectsHier()
	{
		GameObject from = Selection.activeGameObject;
		MegaCopyObject.DoCopyObjectsChildren(from);
	}

	[MenuItem("GameObject/Create Mega Prefab")]
	static void DoCreateSimplePrefab()
	{
#if true
		if ( Selection.activeGameObject != null )
		{
			if ( !Directory.Exists("Assets/MegaPrefabs") )
			{
				AssetDatabase.CreateFolder("Assets", "MegaPrefabs");
				//string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
				//Debug.Log("folder path " + newFolderPath);
			}

			GameObject obj = Selection.activeGameObject;

			GameObject prefab = PrefabUtility.CreatePrefab("Assets/MegaPrefabs/" + Selection.activeGameObject.name + ".prefab", obj);	//Selection.activeGameObject);

			MeshFilter mf = obj.GetComponent<MeshFilter>();
			if ( mf )
			{
				MeshFilter newmf = prefab.GetComponent<MeshFilter>();
				//newmf.sharedMesh = CloneMesh(mf.sharedMesh);

				Mesh mesh = CloneMesh(mf.sharedMesh);

				mesh.name = obj.name + " copy";
				AssetDatabase.AddObjectToAsset(mesh, prefab);
				//AssetDatabase.CreateAsset(mesh, "Assets/MegaPrefabs/" + Selection.activeGameObject.name + ".asset");
				newmf.sharedMesh = mesh;

				MeshCollider mc = prefab.GetComponent<MeshCollider>();
				if ( mc )
				{
					mc.sharedMesh = null;
					mc.sharedMesh = mesh;
				}
			}
			//PrefabUtility.DisconnectPrefabInstance(prefab);
		}
#else
		Transform[] transforms = Selection.transforms;
		foreach ( Transform t in transforms )
		{
			//Object prefab = EditorUtility.CreateEmptyPrefab("Assets/Temporary/" + t.gameObject.name + ".prefab");
			Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Temporary/" + t.gameObject.name + ".prefab");

			MeshFilter mf = t.gameObject.GetComponent<MeshFilter>();
			Mesh ms = mf.sharedMesh;
			Debug.Log("Mesh " + ms);
			GameObject newgo = EditorUtility.ReplacePrefab(t.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);

			MeshFilter newmf = newgo.GetComponent<MeshFilter>();
			//Mesh newms = newmf.sharedMesh;
			//Debug.Log("Mesh " + newms);
			newmf.sharedMesh = CloneMesh(ms);
		}
#endif
	}


#if false
		[MenuItem("GameObject/Create MegaShape Prefab")]
	static void DoCreateSimplePrefabNew()
	{
#if true
		if ( Selection.activeGameObject != null )
		{
			if ( !Directory.Exists("Assets/MegaPrefabs") )
			{
				AssetDatabase.CreateFolder("Assets", "MegaPrefabs");
				//string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
				//Debug.Log("folder path " + newFolderPath);
			}

			GameObject obj = Selection.activeGameObject;

			GameObject prefab = PrefabUtility.CreatePrefab("Assets/MegaPrefabs/" + Selection.activeGameObject.name + ".prefab", Selection.activeGameObject);

			MeshFilter mf = obj.GetComponent<MeshFilter>();

			if ( mf )
			{
				MeshFilter newmf = prefab.GetComponent<MeshFilter>();

				Mesh mesh = CloneMesh(mf.sharedMesh);

				mesh.name = obj.name + " copy";
				AssetDatabase.AddObjectToAsset(mesh, prefab);
				//AssetDatabase.CreateAsset(mesh, "Assets/MegaPrefabs/" + Selection.activeGameObject.name + ".asset");
				newmf.sharedMesh = mesh;

				MeshCollider mc = prefab.GetComponent<MeshCollider>();
				if ( mc )
				{
					mc.sharedMesh = null;
					mc.sharedMesh = mesh;
				}
			}


			//PrefabUtility.DisconnectPrefabInstance(prefab);
		}
#else
		Transform[] transforms = Selection.transforms;
		foreach ( Transform t in transforms )
		{
			//Object prefab = EditorUtility.CreateEmptyPrefab("Assets/Temporary/" + t.gameObject.name + ".prefab");
			Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Temporary/" + t.gameObject.name + ".prefab");

			MeshFilter mf = t.gameObject.GetComponent<MeshFilter>();
			Mesh ms = mf.sharedMesh;
			Debug.Log("Mesh " + ms);
			GameObject newgo = EditorUtility.ReplacePrefab(t.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);

			MeshFilter newmf = newgo.GetComponent<MeshFilter>();
			//Mesh newms = newmf.sharedMesh;
			//Debug.Log("Mesh " + newms);
			newmf.sharedMesh = CloneMesh(ms);
		}
#endif
	}
#endif


	static Mesh CloneMesh(Mesh mesh)
	{
		Mesh clonemesh = new Mesh();
		clonemesh.vertices = mesh.vertices;
		clonemesh.uv1 = mesh.uv1;
		clonemesh.uv2 = mesh.uv2;
		clonemesh.uv = mesh.uv;
		clonemesh.normals = mesh.normals;
		clonemesh.tangents = mesh.tangents;
		clonemesh.colors = mesh.colors;

		clonemesh.subMeshCount = mesh.subMeshCount;

		for ( int s = 0; s < mesh.subMeshCount; s++ )
		{
			clonemesh.SetTriangles(mesh.GetTriangles(s), s);
		}

		//clonemesh.triangles = mesh.triangles;

		clonemesh.boneWeights = mesh.boneWeights;
		clonemesh.bindposes = mesh.bindposes;
		clonemesh.name = mesh.name + "_copy";
		clonemesh.RecalculateBounds();

		return clonemesh;
	}
}
#endif