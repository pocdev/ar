using UnityEngine;
using System.Collections;

[RequireComponent (typeof(UIStateToggleBtn))]
public class AnimToggleButton : MonoBehaviour {
	
	public delegate void OnToggleChangedEventHandler(bool autoPlay);
	public OnToggleChangedEventHandler onToggleChanged;
	
	UIStateToggleBtn _toggle;
	
	// Use this for initialization
	void Start () {
		_toggle = GetComponent<UIStateToggleBtn>();
		_toggle.SetValueChangedDelegate(onInput);
	}
	
	void onInput(IUIObject obj)
	{
		Debug.Log(string.Format("The Toggle is in State: {0}, aka {1}",_toggle.StateName,_toggle.StateNum));
		if(onToggleChanged != null)
			onToggleChanged(_toggle.StateNum == 1);
	}
	
	public void pauseAnimation()
	{
		_toggle.SetToggleState(0);
		if(onToggleChanged != null)
			onToggleChanged(false);
	}
}
