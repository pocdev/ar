using UnityEngine;
using System.Collections;

public class HuntableController : MonoBehaviour {

	public GameObject duck1;
	public GameObject duck2;
	
	public MegaShape shape1;
	public MegaShape shape2;
	
	private Vector3 localScale;
	private Quaternion localRot;
	
	private float _animSpeed;
	
	void Awake()
	{
		localScale = new Vector3(20f,20f,20f);
		localRot = Quaternion.Euler(0f,0,0f);
		
		_animSpeed = 1;
	}
	
	void Update()
	{
		if(transform.childCount == 0)
		{
			_animSpeed *= 1.1f;
			
			GameObject temp = (GameObject)Instantiate(duck1);
			temp.animation["duck_fly"].speed = _animSpeed;
			temp.GetComponentInChildren<PathFollow>().target = shape1;
			temp.GetComponentInChildren<PathFollow>().speed *= _animSpeed;
			temp.transform.parent = transform;
			temp.transform.localScale = localScale;
			temp.transform.localRotation = localRot;
			
			temp = (GameObject)Instantiate(duck2);
			temp.animation["duck_fly"].speed = _animSpeed;
			temp.GetComponentInChildren<PathFollow>().speed *= _animSpeed;
			temp.GetComponentInChildren<PathFollow>().target = shape2;
			temp.transform.parent = transform;
			temp.transform.localScale = localScale;
			temp.transform.localRotation = localRot;
		}
	}
}
