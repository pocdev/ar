using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System;
using System.Text;

public class PhotoManager : MonoBehaviour 
{
	
	private string accessKey = "510e759b9d9d59b19e19b323e4275bf2e4ddfa87";
	private string secretKey = "81e6bbbea9d84da4574cb8527f37e82776e86f46";
	
	void Start()
	{
		EtceteraManager.imagePickerChoseImage += photoSelected;
		EtceteraBinding.promptForPhoto(0.5f,PhotoPromptType.CameraAndAlbum);
	}
	
	void photoSelected(string photoPath)
	{
		Debug.Log("Photo Taken at: " + photoPath);
		byte[] photoFileContents = File.ReadAllBytes(photoPath);
		string base64FileContents = System.Convert.ToBase64String(photoFileContents);
		Debug.Log("photo file base64 string is: " + base64FileContents);	
	
		buildWebRequest();
				
		EtceteraManager.imagePickerChoseImage -= photoSelected;
	}
	
	void buildWebRequest()
	{
		WebRequest request = WebRequest.Create("https://vws.vuforia.com/targets");
		request.Method = "POST";
		
		request.Headers.Add("Date", DateTime.UtcNow.ToString());
		StringBuilder SB = new StringBuilder();
		
		request.Headers.Add("Authorization","1234");
		
		
		request.Headers.Add("Content-Type", "application/json");
		
	}
}
