using UnityEngine;
using System.Collections;

[RequireComponent (typeof(UIButton))]
public class GoToWebsite : MonoBehaviour {
	
	public string URL;
	
	// Use this for initialization
	void Start () {
		GetComponent<UIButton>().SetValueChangedDelegate(onPressed);
	}
	
	void onPressed(IUIObject obj)
	{
		Application.OpenURL(URL);
	}
}
