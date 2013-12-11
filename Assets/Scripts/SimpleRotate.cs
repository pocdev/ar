using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour {

	public Vector3 rotation = Vector3.zero;
	public float speed = 0.01f;
	public bool rotate = true;
	
	// Update is called once per frame
	void Update () {
		if(rotate)
			transform.Rotate(rotation, speed, Space.World);
	}
}
