using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {
	
	public float animationSpeed = 1f;
	
	private ZoomPinch _zoomPinch;
	private SJScrubListener _scrubListener;
	private bool _isPlaying = false;
	
	// Use this for initialization
	void Start () {
		GetComponent<UIButton>().SetValueChangedDelegate(onPressed);
		_scrubListener = GameObject.FindGameObjectWithTag("SJScrubListener").GetComponent<SJScrubListener>();
		_zoomPinch = GameObject.Find("sail_null").GetComponent<ZoomPinch>();
	}
	
	void onPressed(IUIObject obj)
	{
		_isPlaying = !_isPlaying;
		
		if(_isPlaying)
		{
			Debug.Log("PLAYING ANIMS");
			_scrubListener.startAnim(animationSpeed);
			_zoomPinch.resetScale();
		}
		else
		{
			Debug.Log("PAUSING ANIMS");			
			_scrubListener.stopAnim();
		}
	}
	
	public void manualOverride()
	{
		if(_isPlaying)
		{
			_isPlaying = false;
			Debug.Log("MANUAL OVERRIDE, PAUSING ANIMS");
			_scrubListener.stopAnim();
		}
	}
}
