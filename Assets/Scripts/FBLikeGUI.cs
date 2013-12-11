using UnityEngine;
using System.Collections;

public sealed class FBLikeGUI : MonoBehaviour {
	
	public event System.EventHandler onClickedLikeButton = null;
	
	private static FBLikeGUI _instance = null;
	private static Object _padlock = new Object();
	
	public Texture FBTexture = null;
	
	public static FBLikeGUI Instance
	{
		get
		{
			if(_instance == null)
			{
				lock(_padlock)
				{
					if(_instance == null)
						_instance = GameObject.Find("ARCamera").GetComponent<FBLikeGUI>();
				}
			}
			
			return _instance;
		}
	}
	
	void OnGUI()
	{
		if(onClickedLikeButton != null)
		{
			if(GUI.Button(new Rect(0,0,Screen.width * 0.1f, Screen.height * 0.1f), FBTexture))
			{
				onClickedLikeButton(this, new System.EventArgs());
				onClickedLikeButton = null;
			}
		}
	}
}
