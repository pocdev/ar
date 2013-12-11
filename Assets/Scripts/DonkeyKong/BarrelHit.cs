using UnityEngine;
using System.Collections;

public class BarrelHit : MonoBehaviour {
	
	
	void Start()
	{
		GetComponent<UIButton3D>().SetValueChangedDelegate(onHit);
	}
	
	void onHit(IUIObject obj)
	{
		Debug.Log("Barrel Hit!");
		Vector3 randForce = Random.Range(0,1) > 0 ? Vector3.right : Vector3.left;
		randForce *= Random.Range(-500.0f,500.0f);
		
		obj.transform.rigidbody.AddForce((obj.transform.rigidbody.velocity + randForce) * -200);
		Destroy(this);
	}
}
