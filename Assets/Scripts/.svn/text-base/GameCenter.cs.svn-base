using UnityEngine;
using System.Collections;

public class GameCenter : MonoBehaviour 
{

	private static GameCenter _instance = null;
	private static Object _padlock = new Object();
	
	public static GameCenter Instance
	{
		get
		{
			if(_instance == null)
			{
				lock(_padlock)
				{
					if(_instance == null)
						_instance = GameObject.Find("GameCenter").GetComponent<GameCenter>();
				}
			}
			
			return _instance;
		}
	} 
	
	void Awake()
	{		
		if(GameCenterBinding.isGameCenterAvailable() && !GameCenterBinding.isPlayerAuthenticated())
		{
			GameCenterManager.playerAuthenticated += playerAuthenticated;
			GameCenterBinding.authenticateLocalPlayer();
		}
	}	
	
	void playerAuthenticated()
	{
		GameCenterBinding.showCompletionBannerForAchievements();
	}
}
