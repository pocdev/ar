using UnityEngine;
using System.Collections;

public class ToggleButtonController : MonoBehaviour {
	
	private static ToggleButtonController _instance = null;
	private static object _padlock = new object();
	
	public static ToggleButtonController Instance
	{
		get
		{
			if(_instance == null)
			{
				lock(_padlock)
				{
					if(_instance == null)
					{
						_instance = GameObject.Find("Toggle").GetComponent<ToggleButtonController>();
					}
				}
			}
			
			return _instance;
		}
	}
	
	private UIStateToggleBtn _toggle;
	private SpriteText _cameraText;
	private SpriteText _targetText;
	
	void Start()
	{
		_toggle = GetComponent<UIStateToggleBtn>();
		_cameraText = GameObject.Find("\"Camera\"").GetComponent<SpriteText>();
		_targetText = GameObject.Find("\"Target\"").GetComponent<SpriteText>();
		
		_toggle.SetValueChangedDelegate(onClick);
	}
	
	void onClick(IUIObject obj)
	{
		Color tempCameraColor = _cameraText.color;
		_cameraText.SetColor(_targetText.color);
		_targetText.SetColor(tempCameraColor);
	}
	
	public string StateName
	{
		get { return _toggle.StateName; }
	}
}
