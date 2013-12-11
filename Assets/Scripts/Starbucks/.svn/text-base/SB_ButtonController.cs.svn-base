using UnityEngine;
using System.Collections;

public class SB_ButtonController : MonoBehaviour {

	public string iTunesURL;
	
	void Start()
	{
		GetComponent<UIButton>().SetValueChangedDelegate(onClick);
	}
	
	void onClick(IUIObject obj)
	{
		Application.OpenURL(iTunesURL);
	}
}
