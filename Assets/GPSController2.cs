using System;
using System.Collections;
using UnityEngine;
using Vuforia;

public class GPSController2 : MonoBehaviour
{
    public GameObject markerCompass;
    public TrackableBehaviour imageTarget;
    public static float heading = 0;
    
    private string message = "GPS is initialising...";
    private string message2 = "";

    private float thisLat;
    private float thisLon;
    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUI.Label(new Rect(130,110,1000,1000), message );
        GUI.Label(new Rect(300,500,1000,1000), message2 );

    }

    
    void Start()
    {
        
        StartCoroutine(StartGPS());
        
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
        //Quaternion directionToNorth = Quaternion.Euler(90, 0, heading );
        Vector3 dir = (new Vector3(0,heading,0) - imageTarget.gameObject.transform.position).normalized;
        float dirY = (new Vector3(0, heading, 0).y - imageTarget.gameObject.transform.rotation.y);
        //Vector3 dir = (directionToNorth.eulerAngles - transform.rotation.eulerAngles).normalized;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        message2 = $"{imageTarget.gameObject.transform.rotation.y}";
        //markerCompass.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
        markerCompass.transform.localRotation = Quaternion.Euler(90, 0, dirY );
        markerCompass.transform.position = imageTarget.gameObject.transform.position + new Vector3(0,2,0);



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

            // message = "Lat:" + Input.location.lastData.latitude +
            //           "\nLong:" + Input.location.lastData.longitude +
            //           "\nAlt:" + Input.location.lastData.altitude +
            //           "\nHoriz Acc:" + Input.location.lastData.horizontalAccuracy +
            //           "\nVert Acc:" + Input.location.lastData.verticalAccuracy +
            //           "\n======" +
            //           "\nHeading:" + Input.compass.trueHeading;
        }
        
        //Input.location.Stop();
    }
}
