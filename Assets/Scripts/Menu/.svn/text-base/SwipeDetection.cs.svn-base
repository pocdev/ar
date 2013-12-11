//	SwipeDetection.cs
//	Detects a swipe on an EZ GUI UI Object
//	Attach to the same GameObject as the UI Object you need to have swipes detected upon

using UnityEngine;
using System.Collections;


public class SwipeDetection : MonoBehaviour {
	public string title;
	public float swipeThreshold;
	public bool swipeOnYAxis = false;
	private bool swiped = false;
	
	private UIStateToggleBtn _toggle;
	private SpriteText _cameraText;
	private SpriteText _targetText;
	
	void Start()
	{
		_toggle = GetComponent<UIStateToggleBtn>();
		_toggle.SetInputDelegate(DetectSwipe);
		_toggle.SetValueChangedDelegate(onClick);
		
		_cameraText = GameObject.Find("\"Camera\"").GetComponent<SpriteText>();
		_targetText = GameObject.Find("\"Target\"").GetComponent<SpriteText>();
	}
	
	void onClick(IUIObject obj)
	{
		Debug.Log("TOGGLE CLICKED");
		Color tempCameraColor = _cameraText.color;
		_cameraText.SetColor(_targetText.color);
		_targetText.SetColor(tempCameraColor);
	}
	
	void DetectSwipe (ref POINTER_INFO ptr) {
		if (ptr.active && !swiped) {
			Debug.Log (title + ": ptr.active && !swiped");
			if (swipeOnYAxis) {
				if (ptr.origPos.y - ptr.devicePos.y < -swipeThreshold) {
					Debug.Log (title + ": Swipe Detected! UP");

					swiped = true;
				}
				if (ptr.origPos.y - ptr.devicePos.y > swipeThreshold) {
					Debug.Log (title + ": Swipe Detected! DOWN");

					swiped = true;
				}
			} else {
				if (ptr.origPos.x - ptr.devicePos.x > swipeThreshold) {
					Debug.Log (title + ": Swipe Detected! LEFT");
					_toggle.SetToggleState(0);
					_toggle.soundToPlay.Play();
					swiped = true;
				}
				if (ptr.origPos.x - ptr.devicePos.x < -swipeThreshold) {
					Debug.Log (title + ": Swipe Detected! RIGHT");
					_toggle.SetToggleState(1);
					_toggle.soundToPlay.Play();
					swiped = true;
				}
			}
		}
		if (ptr.evt == POINTER_INFO.INPUT_EVENT.RELEASE || ptr.evt == POINTER_INFO.INPUT_EVENT.RELEASE_OFF) {
			swiped = false;
//			Debug.Log (title + ": Swiped = " + swiped);
		}
	}
}


// © 2010 Adam Buckner (aka: Little Angel) and theantranch.com (mailto: adam@theantranch.com)
// Not for reuse or resale without permission