/*==============================================================================
Copyright (c) 2012 QUALCOMM Austria Research Center GmbH.
All Rights Reserved.
Qualcomm Confidential and Proprietary
==============================================================================*/

using System;
using UnityEngine;

/// <summary>
/// This MonoBehaviour implements the Cloud Reco Event handling for this sample.
/// It registers itself at the CloudRecoBehaviour and is notified of new search results as well as error messages
/// The current state is visualized and new results are enabled using the TargetFinder API.
/// </summary>
public class CloudRecoEventHandler : MonoBehaviour, ICloudRecoEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES

    // CloudRecoBehaviour reference to avoid lookups
    private CloudRecoBehaviour mCloudRecoBehaviour;
    // ImageTracker reference to avoid lookups
    private ImageTracker mImageTracker;
    // reference to the cloud reco scene manager component:
    //private ContentManager mContentManager;

    //  array that all textures are loaded into on startup
    //private Texture mRequestingTexture;

    // the parent gameobject of the referenced ImageTargetTemplate - reused for all target search results
    private GameObject mParentOfImageTargetTemplate;

    #endregion // PRIVATE_MEMBER_VARIABLES



    #region EXPOSED_PUBLIC_VARIABLES

    /// <summary>
    /// can be set in the Unity inspector to reference a ImageTargetBehaviour that is used for augmentations of new cloud reco results.
    /// </summary>
    public ImageTargetBehaviour ImageTargetTemplate;

    #endregion



    #region ICloudRecoEventHandler_IMPLEMENTATION

    /// <summary>
    /// called when TargetFinder has been initialized successfully
    /// </summary>
    public void OnInitialized()
    {
        // get a reference to the Image Tracker, remember it
        mImageTracker = (ImageTracker)TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER);
        //mContentManager = (ContentManager)FindObjectOfType(typeof(ContentManager));
    }

    /// <summary>
    /// visualize initialization errors
    /// </summary>
    public void OnInitError(TargetFinder.InitState initError)
    {
        switch (initError)
        {
            case TargetFinder.InitState.INIT_ERROR_NO_NETWORK_CONNECTION:
                //ErrorMsg.New("Network Unavailable", "Failed to initialize CloudReco because the device has no network connection.", LoadAboutScene);
                break;
            case TargetFinder.InitState.INIT_ERROR_SERVICE_NOT_AVAILABLE:
               // ErrorMsg.New("Service Unavailable", "Failed to initialize CloudReco because the service is not available.", LoadAboutScene);
                break;
        }
    }

    /// <summary>
    /// visualize update errors
    /// </summary>
    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        /*switch (updateError)
        {
            case TargetFinder.UpdateState.UPDATE_ERROR_AUTHORIZATION_FAILED:
                ErrorMsg.New("Authorization Error","The cloud recognition service access keys are incorrect or have expired.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_BAD_FRAME_QUALITY:
                ErrorMsg.New("Poor Camera Image","The camera does not have enough detail, please try again later");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_NO_NETWORK_CONNECTION:
                ErrorMsg.New("Network Unavailable","Please check your internet connection and try again.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_PROJECT_SUSPENDED:
                ErrorMsg.New("Authorization Error","The cloud recognition service has been suspended.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_REQUEST_TIMEOUT:
                ErrorMsg.New("Request Timeout","The network request has timed out, please check your internet connection and try again.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_SERVICE_NOT_AVAILABLE:
                ErrorMsg.New("Service Unavailable","The service is unavailable, please try again later.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_TIMESTAMP_OUT_OF_RANGE:
                ErrorMsg.New("Clock Sync Error","Please update the date and time and try again.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_UPDATE_SDK:
                ErrorMsg.New("Unsupported Version","The application is using an unsupported version of Vuforia.");
                break;
        }*/
    }

    /// <summary>
    /// when we start scanning, unregister Trackable from the ImageTargetTemplate, then delete all trackables
    /// </summary>
    public void OnStateChanged(bool scanning)
    {
        if (scanning)
        {
            // clear all known trackables
            //mImageTracker.TargetFinder.ClearTrackables(false);

            // hide the ImageTargetTemplate
            //mContentManager.HideObject();
        }
    }

    /// <summary>
    /// Handles new search results
    /// </summary>
    /// <param name="targetSearchResult"></param>
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {
        // This code demonstrates how to reuse an ImageTargetBehaviour for new search results and modifying it according to the metadata
        // Depending on your application, it can make more sense to duplicate the ImageTargetBehaviour using Instantiate(), 
        // or to create a new ImageTargetBehaviour for each new result

        // Vuforia will return a new object with the right script automatically if you use
        // TargetFinder.EnableTracking(TargetSearchResult result, string gameObjectName)
        
        //Check if the metadata isn't null
        /*if(targetSearchResult.MetaData == null)
        {
            return;
        }*/
		Debug.Log("Found: " + targetSearchResult.TargetName);
        // enable the new result with the same ImageTargetBehaviour:
        ImageTargetBehaviour imageTargetBehaviour = mImageTracker.TargetFinder.EnableTracking(targetSearchResult, mParentOfImageTargetTemplate);

        if (imageTargetBehaviour != null)
        {
			AnimationInitializer AI = imageTargetBehaviour.GetComponentInChildren<AnimationInitializer>();
			
			if(AI)
				AI.Animate();
            // stop the target finder
            //mCloudRecoBehaviour.CloudRecoEnabled = false;
            
            // Calls the TargetCreated Method of the SceneManager object to start loading
            // the BookData from the JSON
            //mContentManager.TargetCreated(targetSearchResult.MetaData);
        }
    }

    #endregion // ICloudRecoEventHandler_IMPLEMENTATION



    #region UNTIY_MONOBEHAVIOUR_METHODS

    /// <summary>
    /// register for events at the CloudRecoBehaviour
    /// </summary>
    void Start()
    {
        // look up the gameobject containing the ImageTargetTemplate:
        mParentOfImageTargetTemplate = ImageTargetTemplate.gameObject;

        // intialize the ErrorMsg class
        //ErrorMsg.Init();

        // register this event handler at the cloud reco behaviour
        CloudRecoBehaviour cloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        if (cloudRecoBehaviour)
        {
            cloudRecoBehaviour.RegisterEventHandler(this);
        }

        // remember cloudRecoBehaviour for later
        mCloudRecoBehaviour = cloudRecoBehaviour;

        // load and remember all used textures:
        //mRequestingTexture = Resources.Load("UserInterface/TextureRequesting") as Texture;
    }

    /// <summary>
    /// draw the sample GUI and error messages
    /// </summary>
    void OnGUI()
    {
        if (mCloudRecoBehaviour.CloudRecoInitialized && mImageTracker.TargetFinder.IsRequesting())
        {
            // draw the requesting texture
           // DrawTexture(mRequestingTexture);
        }

        // draw error messages in case there were any
        //ErrorMsg.Draw();
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS



    #region PRIVATE_METHODS

    /// <summary>
    /// draws a textures using UnityGUI
    /// </summary>
    private void DrawTexture(Texture texture)
    {
        // scale texture up by device dependent factor
        int smallerScreenDimension = Screen.width < Screen.height ? Screen.width : Screen.height;
        float deviceDependentScale = smallerScreenDimension / 480f;

        // draw texture at center with given verticalPosition and scale:
        float width = texture.width * deviceDependentScale;
        float height = texture.height * deviceDependentScale;
        float left = (Screen.width * 0.5f) - (width * 0.5f);
        float top = (Screen.height * 0.9f) - (height * 0.5f);
        GUI.DrawTexture(new Rect(left, top, width, height), texture);
    }

    #endregion // PRIVATE_METHODS
}
