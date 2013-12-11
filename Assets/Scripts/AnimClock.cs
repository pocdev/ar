using UnityEngine;
using System.Collections;
using System;

public class AnimClock : MonoBehaviour {
	
	public float clockHandRadius = 10f;
	
	private Scrubber _scrubber;
	private LineRenderer _clockHand;
	
	// Use this for initialization
	void Start () {
		_scrubber = GameObject.FindGameObjectWithTag("Scrubber").GetComponent<Scrubber>();
		_scrubber.OnScrubValueChanged += OnScrubberChanged;		
		
		_clockHand = GetComponentInChildren<LineRenderer>();
		_clockHand.SetVertexCount(2);
		_clockHand.SetPosition(1,new Vector3(0f,10f,0f));
	}	
	
	void OnScrubberChanged(float scrubPercentage)
	{	
		Debug.Log("scrubpercentage: " + scrubPercentage);
		
		float angle = ((360f * scrubPercentage) + 90) * Mathf.Deg2Rad;
		
		float x = clockHandRadius * (Mathf.Cos(angle));
		float y = clockHandRadius * (Mathf.Sin(angle));
		
		//Debug.Log(string.Format("{0},{1}",x,y));
		
		_clockHand.SetPosition(1,new Vector3(-x,y,0f)); //flip x to make the anim clock progress clockwise instead of counter-clockwise.
	}
}
