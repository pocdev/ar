using UnityEngine;
using System.Collections;

public class LaunchHandler : MonoBehaviour 
{
	public bool enabledInEditor = false;
	
	void Awake()
	{
		string launchString = PlayerPrefs.GetString("LaunchURL");
		
		if(Application.platform == RuntimePlatform.OSXEditor && enabledInEditor)
		{
			Debug.Log("IN EDITOR MODE, DOING LAUNCHHANDLER STUFF!");
			launchString = "DespicableMe";
		}
		
		Debug.Log("*************  launchString is " + launchString);
		
		if(launchString != "0")
		{
			PlayerPrefs.SetString("LaunchURL","0");
			
			launchString = launchString.Replace("pocar://","");
			try
			{
				Application.LoadLevel(launchString);
			}
			catch(System.Exception)
			{
				Debug.Log("Error trying to launch level: " + launchString);
			}
		}
	}
}
