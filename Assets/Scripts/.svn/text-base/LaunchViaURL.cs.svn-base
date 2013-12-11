using UnityEngine;
using System.Collections;

public class LaunchViaURL : MonoBehaviour {

	public void launchViaURL(string URL)
	{
		try
		{
			Application.LoadLevel(URL.Replace("pocar://",""));
		}
		catch(System.Exception)
		{
			Debug.Log("Error trying to load Level: " + URL);
		}
	}
}
