using UnityEngine;
using System.Collections;

public class CameraAngleSync : MonoBehaviour {
	
	Camera _mainCamera;
	
	// Use this for initialization
	void Start () {
		_mainCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.localRotation = _mainCamera.transform.rotation;
		transform.Rotate(0f,180f,0f);
	}
}
