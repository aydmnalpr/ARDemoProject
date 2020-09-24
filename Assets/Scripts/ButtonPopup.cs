using UnityEngine;
using System.Collections;
using Vuforia;
using static Vuforia.TrackableBehaviour;


public class ButtonPopup : MonoBehaviour {
	
    private TrackableBehaviour mTrackableBehaviour;
	
    private bool mShowGUIButton = false;
    private Rect mButtonRect = new Rect(50,50,120,60);
	
    void Start () {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStateChanged);
        }
    }
	
    public void OnTrackableStateChanged(
        TrackableBehaviour.StatusChangeResult obj)
    {
        if (obj.NewStatus == Status.DETECTED ||
            obj.NewStatus == Status.TRACKED)
        {
            mShowGUIButton = true;
        }
        else
        {
            mShowGUIButton = false;
        }
    }
	
    void OnGUI() {
        if (mShowGUIButton) {
            // draw the GUI button
            if (GUI.Button(mButtonRect, "Hello")) {
                // do something on button click	
            }
        }
    }
}
