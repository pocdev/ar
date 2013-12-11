using UnityEngine;
using System.Collections;

public class AttachUIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		GameObject manager = GameObject.Find("UI Manager");
		
		if(manager != null)
		{
			LayerMask mask = 1 << LayerMask.NameToLayer("GUI");
			//Debug.Log("mask: " + mask.value.ToString());
			manager.GetComponent<UIManager>().AddCamera(GetComponent<Camera>(),mask,Mathf.Infinity,1);
		}			
	}
}
