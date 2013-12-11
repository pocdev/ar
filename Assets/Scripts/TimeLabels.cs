using UnityEngine;
using System.Collections;
using System;

public class TimeLabels : MonoBehaviour {
	
	private SpriteText _currentTime;
	private SpriteText _duration;
	
	void Start () 
	{
		_currentTime = GameObject.Find("CurrentTime").GetComponent<SpriteText>();
		_duration = GameObject.Find("Duration").GetComponent<SpriteText>();
		
		_currentTime.text = "00:00";
		_duration.text = "00:00";
	}
	
	public void setDuration(float duration)
	{
		_duration.Text = duration.ToString("n2");
	}
	
	public void setCurrentTime(float currentTime)
	{
		_currentTime.Text = currentTime.ToString("n2");
	}
}
