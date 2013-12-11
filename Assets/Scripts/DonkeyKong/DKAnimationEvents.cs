using UnityEngine;
using System.Collections;

public class DKAnimationEvents : MonoBehaviour {
	
	public GameObject barrel;
	public GameObject rightHand;
	public GameObject leftHand;
	
	public int maxNumBarrelsToThrow = 2;
	private int _barrelsLeftToThrow;
	
	private Transform ARCamera;
	
	void Start()
	{
		_barrelsLeftToThrow = Random.Range(1,maxNumBarrelsToThrow+1);
		ARCamera = GameObject.Find("ARCamera").transform;
	}
	
	void idle1End()
	{
		animation.CrossFadeQueued("DonkeyKong_Idle2",0.3f,QueueMode.PlayNow);
	}
	
	void idle2End()
	{
		int i = Random.Range(1,5);
		
		if(i == 4)
		{
			animation.CrossFadeQueued("DonkeyKong_Idle1",0.3f,QueueMode.PlayNow);
		}
	}
	
	void ThrowBarrel()
	{
		barrel.GetComponent<Collider>().enabled = false;
		
		GameObject barrelClone = (GameObject)Instantiate(barrel);
		barrel.GetComponent<MeshRenderer>().enabled = false;
		
		barrelClone.transform.parent = transform;
		barrelClone.transform.position = barrel.transform.position;
		barrelClone.transform.rotation = barrel.transform.rotation;
		barrelClone.transform.localScale = barrel.transform.localScale;
		
		barrelClone.rigidbody.GetComponent<Collider>().enabled = true;
		barrelClone.rigidbody.isKinematic = false;
		barrelClone.rigidbody.useGravity = false;
		
		Vector3 forceSourcePosition = Random.Range(0,2) == 1 ? rightHand.transform.position : leftHand.transform.position;
		forceSourcePosition *= Random.Range(.5f,1.5f);
		
		Vector3 Vector2Camera = ARCamera.transform.position - barrelClone.transform.position;
		
		barrelClone.rigidbody.AddForceAtPosition(Vector2Camera * 100,forceSourcePosition);
		
		StartCoroutine(barrelTimer(barrelClone));
	}
	
	void barrelAnimStart()
	{
		barrel.GetComponent<MeshRenderer>().enabled = true;
	}
	
	void barrelAnimEnd()
	{
		_barrelsLeftToThrow--;
		
		if(_barrelsLeftToThrow == 0)
		{
			animation.CrossFadeQueued("DonkeyKong_Idle2",0f,QueueMode.PlayNow);
			_barrelsLeftToThrow = Random.Range(1,maxNumBarrelsToThrow+1);
		}
		
	}
	
	IEnumerator barrelTimer(GameObject barrelClone)
	{
		yield return new WaitForSeconds(2f);
		Destroy(barrelClone);
	}
	
	void onHit()
	{
		if(!animation.IsPlaying("DonkeyKong_Barrel"))
			animation.CrossFadeQueued("DonkeyKong_Barrel",0f,QueueMode.PlayNow,PlayMode.StopAll);
	}
	
	void Update()
	{
		GameObject temp = new GameObject();
		temp.transform.parent = transform;
		temp.transform.localPosition = Vector3.zero;
		temp.transform.LookAt(Camera.mainCamera.transform);
		
		Vector3 rot = transform.rotation.eulerAngles;
		rot.y = temp.transform.rotation.eulerAngles.y;
		
		transform.rotation = Quaternion.Euler(rot);
		Destroy(temp);
	}
}
