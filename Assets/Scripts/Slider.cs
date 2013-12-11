using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour {
	
	public delegate void OnSliderChangedEventHandler(float val);
	public event OnSliderChangedEventHandler onSliderChanged;
	
	public delegate void OnSliderPressedEventHandler();
	public event OnSliderPressedEventHandler onSliderPressed;
	
	public delegate void OnSliderReleasedEventHandler();
	public event OnSliderReleasedEventHandler onSliderReleased;
	
	private UISlider _slider;
	
	// Use this for initialization
	void Start() {
		_slider = GetComponent<UISlider>();
		_slider.SetInputDelegate(onInput);
	}
	
	void onInput(ref POINTER_INFO ptr)
	{		
		switch(ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.PRESS:
				Debug.Log("Slider Pressed");
				if(onSliderPressed != null)
					onSliderPressed();
				break;
			case POINTER_INFO.INPUT_EVENT.DRAG:
				Debug.Log("slider is now: " + _slider.Value);
				if(onSliderChanged != null)
					onSliderChanged(_slider.Value);
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				Debug.Log("Slider Released");
				if(onSliderReleased != null)
					onSliderReleased();
				break;
			default:
				break;
		}
	}
	
	public void setSliderValue(float val)
	{
		_slider.Value = val;
	}
}
