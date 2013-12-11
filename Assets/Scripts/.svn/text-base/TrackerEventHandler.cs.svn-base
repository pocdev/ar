/*==============================================================================
            Copyright (c) 2012 QUALCOMM Austria Research Center GmbH.
            All Rights Reserved.
            Qualcomm Confidential and Proprietary
==============================================================================*/

using UnityEngine;

// A custom handler that implements the ITrackerEventHandler interface.
public class TrackerEventHandler : MonoBehaviour,
                                   ITrackerEventHandler
{
    #region UNTIY_MONOBEHAVIOUR_METHODS

    void Start()
    {
        QCARBehaviour qcarBehaviour = GetComponent<QCARBehaviour>();
        if (qcarBehaviour)
        {
            qcarBehaviour.RegisterTrackerEventHandler(this);
        }
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS



    #region PUBLIC_METHODS

    // Implementation of the ITrackerEventHandler function called after all
    // trackables have changed.
    public void OnTrackablesUpdated()
    {
        //Debug.Log("trackables updated");
    }
	
	public void OnInitialized()
	{
	}

    #endregion // PUBLIC_METHODS
}
