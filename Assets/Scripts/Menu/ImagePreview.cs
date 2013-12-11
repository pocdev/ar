using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImagePreview : MonoBehaviour {
	
	private static ImagePreview _instance = null;
	private static Object _padlock = new Object();
	
	public static ImagePreview Instance
	{
		get
		{
			if(_instance == null)
			{
				lock(_padlock)
				{
					if(_instance == null)
						_instance = GameObject.Find("ImagePreview").GetComponent<ImagePreview>();
				}
			}
			
			return _instance;
		}
	}
	
	public List<Texture2D> textureList = new List<Texture2D>();
	private SimpleSprite _imgPreview;
	private MeshRenderer _noPreviewAvailable;
	
	void Start()
	{
		_imgPreview = GetComponent<SimpleSprite>();
		_noPreviewAvailable = GameObject.Find("\"No Preview Available\"").GetComponent<MeshRenderer>();
	}
	
	public void Show()
	{
		UIRadioBtn btn = (UIRadioBtn)RadioBtnGroup.GetSelected(0);
		
		if(btn != null)
		{
			int index = (int)btn.transform.parent.GetComponent<UIListItemContainer>().Data;
			Texture2D tex = textureList[index];
			
			if(tex != null)
			{
				_imgPreview.SetTexture(tex);
				_imgPreview.SetPixelDimensions(tex.width, tex.height);
				_imgPreview.SetLowerLeftPixel(new Vector2(0,tex.height));
				
				float scalingConstant = 2.0f;//(iPhone.generation == iPhoneGeneration.iPhone5) ? 1.25f : 1.5f;
				
				_imgPreview.SetSize(tex.width*scalingConstant,tex.height*scalingConstant);
				
				GetComponent<MeshRenderer>().enabled = true;
				_noPreviewAvailable.enabled = false;
			}
			else
			{
				GetComponent<MeshRenderer>().enabled = false;
				_noPreviewAvailable.enabled = true;
			}	
		}
	}
	
	public void Hide()
	{
		GetComponent<MeshRenderer>().enabled = false;
	}
}
