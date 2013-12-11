using UnityEngine;
using System.Collections;

public class IconController : MonoBehaviour {
	
	public string linkToOpen;
	
	// Use this for initialization
	void Start () 
	{
		GetComponent<UIButton3D>().SetValueChangedDelegate(onClick);
	}
	
	void onClick(IUIObject obj)
	{
		if(linkToOpen != "" && linkToOpen != null)
			Application.OpenURL(linkToOpen);
	}
}
