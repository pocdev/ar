using UnityEngine;
using System.Collections;

public sealed class SocialNetworking : MonoBehaviour {
	
	private static volatile SocialNetworking _instance = null;
	private static Object _padlock = new Object();
		
	public static SocialNetworking Instance
	{
		get
		{
			if(_instance == null)
			{
				lock(_padlock)
				{
					if(_instance == null)
						_instance = GameObject.Find("SocialNetworking").GetComponent<SocialNetworking>();
				}
			}
			
			return _instance;
		}
	}
	
	void Awake()
	{
		SocialNetworkingManager.facebookSessionInvalidatedEvent += FacebookLogin;
		FacebookBinding.init("196000460505838");
	}
	
	void Start()
	{
		if(FacebookBinding.isLoggedIn())
		{
			Debug.Log("Extending Token");
			FacebookBinding.extendAccessToken();
		}
	}
	
	void btnFacebook_OnClick()
	{
		Application.OpenURL("https://www.facebook.com/POC.QED?ref=ts&fref=ts");
		//FacebookLogin();
	}
	
	void FacebookLogin()
	{
		string[] perms = new string[3];
		perms[0] = "publish_actions";
		perms[1] = "publish_stream";
		perms[2] = "email";
			
		Debug.Log("Logging into FB");
		FacebookBinding.loginWithRequestedPermissions(perms);
	}
}
