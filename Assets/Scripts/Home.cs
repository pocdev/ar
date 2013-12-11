using UnityEngine;
using System.Collections;
using System;

public class Home : MonoBehaviour 
{
	public static event Action onClickedHomeButton;
	
	void Start()
	{
		/*if(Application.loadedLevelName == "HolidayParty")
		{
			GetComponent<UIButton>().enabled = false;
			GameObject.Find("HolidayPartyBug").GetComponent<MeshRenderer>().enabled = true;
		}
		else
		{
			StartCoroutine(setupDelegate());
		}*/
		
		StartCoroutine(setupDelegate());
	}
	
	IEnumerator setupDelegate()
	{
		UIButton btn = null;
		
		while(btn == null)
		{
			btn = GetComponent<UIButton>();	
			yield return new WaitForEndOfFrame();
		}
		
		btn.SetValueChangedDelegate(onClick);
		
	}
	
	void onClick(IUIObject obj)
	{
		if(onClickedHomeButton != null)
			onClickedHomeButton();
			
		Application.LoadLevel(0);
	}
}
