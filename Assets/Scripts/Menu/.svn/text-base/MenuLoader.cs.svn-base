using UnityEngine;
using System.Collections;

public class MenuLoader : MonoBehaviour 
{
	
	public string iPhone5SceneName;
	public string iPad3SceneName;
	
	public string sceneNameToLoadForEDITOR;
	
	void Awake() 
	{
		if(Application.platform == RuntimePlatform.OSXEditor)
		{
			Application.LoadLevelAdditiveAsync(sceneNameToLoadForEDITOR);
		}
		else
		{	
			switch(iPhone.generation) 
			{
				case iPhoneGeneration.iPhone5:
					Application.LoadLevelAdditiveAsync(iPhone5SceneName);
					break;
				default:
					Application.LoadLevelAdditiveAsync(iPad3SceneName);
					break;
			}
		}
	}
}
