using UnityEngine;
using System.Collections;

public class Duck : Huntable {
	
	private bool _bHit = false;
	
	private float _timer = 10f;
	public override float Timer
	{
		get
		{
			return _timer;
		}
	}
	
	private int _pointvalue = 10;
	public override int PointValue
	{
		get
		{
			return _pointvalue;
		}
	}
	
	public override void OnHit()
	{
		if(_bHit)
			return;
		
		_bHit = true;
		Debug.Log("You hit " + gameObject.name);
		
		transform.parent.transform.position = transform.position;
		
		PathFollow PF = GetComponent<PathFollow>();
		PF.animate = false;
		PF.rot = false;
				
		transform.parent.animation.CrossFade("duck_hit");
	}
	
	public override void OnTimerExpire()
	{
		Debug.Log("Timer Expired on " + gameObject.name);
	}	
	
	public override void OnHitGround()
	{
		Destroy(transform.parent.gameObject);
		//TODO: Fire an event at a gamecontroller which will destroy this and instantiate a new duck.
	}
	
	public void OnGUI()
	{
		if(!_bHit && Application.platform == RuntimePlatform.OSXEditor)
		{
			if(GUI.Button(new Rect(0,200,50,50),"HIT DUCK"))
				OnHit();
		}
	}
}
