using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class distMeasure : MonoBehaviour, ITrackableEventHandler
{
    public static distMeasure measurement;
    public GameObject c1, c2;
    public float realWorldDist = 6.5f;
    public float k = 0.00507f;
    private float z;
    public GameObject imageTarget;
    public GameObject measurementNode;
    private Camera origin;
    private TrackableBehaviour mTrackableBehaviour;
    private bool trackOk;
    private Text answer;
    // Start is called before the first frame update
    void Start()
    {
        if(measurement==null){
            measurement=this;
        }
        answer = GameObject.Find("answerText").GetComponent<Text>();
        origin = GetComponent<Camera>();
        if (imageTarget.GetComponent<TrackableBehaviour>())
        {
            mTrackableBehaviour = imageTarget.GetComponent<TrackableBehaviour>();
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(trackOk){
            //Distance bertween cameera and image Target
            z = Mathf.Abs(transform.position.z - imageTarget.transform.position.z);

            //Formula to calculate distance betwwen cubes
            float x = (realWorldDist / (z * (2 * k)*10));
            answer.text = x.ToString();
            //Assign new cube positions
            Vector3 tempDist = c1.transform.position;
            tempDist.x = (x / 2);
            c1.transform.position = tempDist;

            tempDist = c2.transform.position;
            tempDist.x = -(x / 2);
            c2.transform.position = tempDist;
        }
        if(Input.GetMouseButtonDown(0)){

        }
    }
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            trackOk=true;
        }
        else {
            trackOk=false;
        }
    }
    public float imagePosition(){
        if(trackOk)
            return z;
        return 0;
    }
}
