/*==============================================================================
            Copyright (c) 2012 QUALCOMM Austria Research Center GmbH.
            All Rights Reserved.
            Qualcomm Confidential and Proprietary
==============================================================================*/

using UnityEngine;

// A behaviour that implements the IVirtualButtonEventHandler interface and
// provides the soccer ball event logic.
public class VBSoccerballEventHandler :     MonoBehaviour,
                                            IVirtualButtonEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES

    private GameObject mSoccerball;
    private bool mIsRolling = false;
    private float mTimeRolling = 0.0f;
    private float mForce = 0.4f;

    #endregion // PRIVATE_MEMBER_VARIABLES



    #region PUBLIC_METHODS

    // Called when the virtual button has just been pressed:
    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonPressed");
        KickSoccerball();
    }


    // Called when the virtual button has just been released:
    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonReleased");
    }

    #endregion // PUBLIC_METHODS



    #region UNTIY_MONOBEHAVIOUR_METHODS

    void Start()
    {
        // Get handle to the soccerball object
        mSoccerball = transform.FindChild("soccerball").gameObject;

        // Register with the virtual buttons TrackableBehaviour
        VirtualButtonBehaviour vb =
                            GetComponentInChildren<VirtualButtonBehaviour>();
        if (vb)
        {
            vb.RegisterEventHandler(this);
        }

        // Scale the force by the target size
        mForce *= transform.localScale.x;
    }


    void Update()
    {
        mTimeRolling += Time.deltaTime;

        // Force the ball to sleep once it's slowed down enough
        if (mIsRolling && mTimeRolling > 1.0f &&
            mSoccerball.rigidbody.velocity.magnitude < 5)
        {
            mSoccerball.rigidbody.Sleep();
            mIsRolling = false;
        }
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS



    #region PRIVATE_METHODS

    // Kick the soccerball in a random direction
    private void KickSoccerball()
    {
        // Get the rectangle defining the boundaries of this target
        Bounds targetBounds = this.collider.bounds;
        Rect targetRect = new Rect( -targetBounds.extents.x,
                                    -targetBounds.extents.z,
                                    targetBounds.size.x,
                                    targetBounds.size.z);

        // Try to find a random direction to kick the ball in
        // such that the ball will stay inside the target boundaries
        // Give up and use any random direction if failed after 20 tries
        Vector2 randomDir;
        for (int i = 0; i < 20; i++)
        {
            randomDir = Random.insideUnitCircle.normalized;

            // Get the ball's current position inside the target rect
            Vector3 pos = mSoccerball.transform.localPosition *
                                        this.transform.localScale.x;

            // Estimate the ball's final position if kicked in the random
            // direction with the given force
            Vector2 finalPos = new Vector2(pos.x, pos.z) +
                                            randomDir * mForce * 1.5f;

            if (targetRect.Contains(finalPos))
            {
                break;
            }
        }

        // The direction of motion
        Vector3 kickDir = new Vector3(randomDir.x, 0, randomDir.y).normalized;

        // Torque to ensure the ball begins rolling right away
        Vector3 torqueDir = Vector3.Cross(Vector3.up, kickDir).normalized;

        // Add instantaneous forces using ForceMode.VelocityChange
        // This setting ignores the mass of the object
        mSoccerball.rigidbody.AddForce(kickDir * mForce,
                                        ForceMode.VelocityChange);
        mSoccerball.rigidbody.AddTorque(torqueDir * mForce,
                                        ForceMode.VelocityChange);

        mIsRolling = true;
        mTimeRolling = 0.0f;
    }

    #endregion // PRIVATE_METHODS
}
