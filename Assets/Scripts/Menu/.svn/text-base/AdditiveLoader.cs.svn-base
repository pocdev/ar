using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelLoadDetails
{
	public string levelName;
	public int weight;
}

public class AdditiveLoader : MonoBehaviour {
	
	private string _platform;
	
	public LevelLoadDetails[] levelInfo = new LevelLoadDetails[0];
	
	void Start () 
	{	
		if(Application.platform == RuntimePlatform.OSXEditor)
		{
			_platform = "-iPad3";
		}
		else
		{
			switch(iPhone.generation)
			{
				case iPhoneGeneration.iPhone5:	
					_platform = "-iPhone5";
					break;
				default:
					_platform = "-iPad3";
					break;
			}
		}
		
		foreach(LevelLoadDetails LLD in levelInfo)
		{
			Application.LoadLevelAdditiveAsync(LLD.levelName + _platform);
		}
	}
}
