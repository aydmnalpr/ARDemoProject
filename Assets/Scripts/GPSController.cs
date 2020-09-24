using System;
using System.Collections;
using UnityEngine;

public class GPSController : MonoBehaviour
{
    public GameObject cameraCompass;
    public GameObject markerCompass;
    public static float heading = 0;
    
    private string message = "GPS is initialising...";

    private float thisLat;
    private float thisLon;
    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUI.Label(new Rect(30,30,1000,1000), message );
    }

    
    void Start()
    {
        StartCoroutine(StartGPS());
        
        heading = 
    }

    private void Update()
    {
        DateTime lastUpdate = epoch.AddSeconds(Input.location.lastData.timestamp);
        DateTime rightNow = DateTime.Now;

        thisLat = Input.location.lastData.latitude;
        thisLon = Input.location.lastData.longitude;
        message = "Current Lat" + thisLat +
                  "\nCurrent Long:" + thisLon +
                  "\nUpdate Time:" + lastUpdate.ToString("HH:mm:ss") +
                  "\nNow:" + rightNow.ToString("HH:mm:ss");

        heading = Input.compass.trueHeading;
        cameraCompass.transform.localRotation = Quaternion.Euler(90, 0, heading );
    }


    IEnumerator StartGPS()
    {
        message = "Starting";

        if (!Input.location.isEnabledByUser)
        {
            message = "Location Services Not Enabled.";
            yield break;
        }
        
        Input.location.Start(5,0);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            message = "Time out";
            yield break;
        }
        else
        {
            Input.compass.enabled = true;

            message = "Lat:" + Input.location.lastData.latitude +
                      "\nLong:" + Input.location.lastData.longitude +
                      "\nAlt:" + Input.location.lastData.altitude +
                      "\nHoriz Acc:" + Input.location.lastData.horizontalAccuracy +
                      "\nVert Acc:" + Input.location.lastData.verticalAccuracy +
                      "\n======" +
                      "\nHeading:" + Input.compass.trueHeading;
        }
        
        //Input.location.Stop();
    }
}
