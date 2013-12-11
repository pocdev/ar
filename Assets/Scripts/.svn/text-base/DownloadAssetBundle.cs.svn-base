using UnityEngine;
using System.Collections;
using System;

public class DownloadAssetBundle : MonoBehaviour {
	
	private string AssetName = "";
	private string _bundleLocation;
	private int _version;
	
	void Start()
	{
		_bundleLocation = "https://s3-us-west-1.amazonaws.com/ar-assets/assets/AssetBundles/" + Application.loadedLevelName + "AB.unity3d";
		_version = 1;
		
		Caching.CleanCache();
		StartCoroutine(doDownload());
	}
	
	IEnumerator doDownload()
	{
		using(WWW www = WWW.LoadFromCacheOrDownload(_bundleLocation,_version))
		{		
			yield return www;
	
			Debug.Log("Download Complete! - " + _bundleLocation);
			
			if (www.error != null)
				throw new Exception("WWW download had an error:" + www.error);
		
			Debug.Log("going to load bundle");
			
			AssetBundle bundle;
			
			try
			{
				bundle = www.assetBundle;
				Debug.Log("name: " + bundle.name);
				Debug.Log("main asset Name: " + bundle.mainAsset.name);
				
				if (AssetName == "")
					Instantiate(bundle.mainAsset);
				else
					Instantiate(bundle.Load(AssetName));
                
				// Unload the AssetBundles compressed contents to conserve memory
				bundle.Unload(false);
			}
			catch(Exception e)
			{
				Debug.Log(e.ToString() + " " + e.InnerException.ToString());
			}
		}
		
		//LoadDataSet();
	}
	
	/*private bool LoadDataSet()
	{		
	    // Check if the data set exists at the given path.
   	 
 	  	if (!DataSet.Exists("ConcordAR"))
    	{
        	return false;
	    }

	    // Request an ImageTracker instance from the TrackerManager.
    
    	ImageTracker imageTracker =
        	(ImageTracker)TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER);
		
		imageTracker.DeactivateDataSet(imageTracker.GetActiveDataSet());

  		// Create a new empty data set.
    
    	DataSet dataSet = imageTracker.CreateDataSet();

    	// Load the data set from the given path.
    
    	if (!dataSet.Load("ConcordAR"))
    	{
        	return false;
    	}

    	// (Optional) Activate the data set.
    
    	imageTracker.ActivateDataSet(dataSet);

    	return true;
	}*/
}
