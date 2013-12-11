using UnityEngine;
using System.Collections;

public class LaunchButtonController : MonoBehaviour {
	
	private static LaunchButtonController _instance = null;
	private static Object _padlock = new Object();
	
	public static LaunchButtonController Instance
	{
		get
		{
			if(_instance == null)
			{
				lock(_padlock)
				{
					if(_instance == null)
					{
						_instance = GameObject.Find("Launch Button").GetComponent<LaunchButtonController>();
					}
				}
			}
			
			return _instance;
		}
	}
	
	private UIButton launchButton;
	private UIPanelManager _panelManager;
	
	private UIStateToggleBtn _toggle;
	
	private MeshRenderer _loadingLogo;
	private MeshRenderer _noPreviewAvailable;
	
	// Use this for initialization
	void Start () {
		launchButton = GetComponent<UIButton>();
		launchButton.SetValueChangedDelegate(onClick);
		
		_panelManager = GameObject.Find("Panel Manager").GetComponent<UIPanelManager>();
		_loadingLogo = 	GameObject.Find("LoadingLogo").GetComponent<MeshRenderer>();
		_noPreviewAvailable = GameObject.Find("\"No Preview Available\"").GetComponent<MeshRenderer>();
		
		_toggle = GameObject.Find("Toggle").GetComponent<UIStateToggleBtn>();
	}

	public void onClick(IUIObject obj)
	{
		UIRadioBtn selectedRadioButton = (UIRadioBtn)RadioBtnGroup.GetSelected(0);
		
		if(selectedRadioButton == null)
			return;
		
		string levelName = selectedRadioButton.transform.parent.name;
		levelName = levelName.Substring(0,levelName.IndexOf('('));
		
		Debug.Log(string.Format("levelname is {0}",levelName.ToLower()));
				
		if(_toggle.StateName == "Camera")
		{
			_loadingLogo.enabled = true;
			_noPreviewAvailable.enabled = false;
			ImagePreview.Instance.Hide();
			
			if(levelName.ToLower() == "fandango")
			{
				Handheld.PlayFullScreenMovie("http://poc.bentlight.com/_content/fandango/Fandango/Fandango.mp4");
			}
			else if(levelName.ToLower() == "standalone")
			{
				string URL = (Application.platform == RuntimePlatform.OSXEditor) ? "http://www.google.com" : "POCARStandalone://";
				Application.OpenURL(URL);
			}
			else
			{
				StartCoroutine(doLevelLoad(levelName));
			}
		}
		else
		{
			_loadingLogo.enabled = false;
			ImagePreview.Instance.Show();
			_panelManager.MoveForward();
		}
		
	}
	
	IEnumerator doLevelLoad(string levelName)
	{	
		_panelManager.MoveForward();
		
		while(_panelManager.TransitioningPanelCount != 0)
			yield return new WaitForEndOfFrame();
		
		Application.LoadLevelAsync(levelName);
	}
}
