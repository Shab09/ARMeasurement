using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateDistance : MonoBehaviour
{
    public GameObject imageTarget;
    public GameObject cube1;
    public GameObject cube2;
    private Text answer;
    // Start is called before the first frame update
    void Start()
    {
        answer = GameObject.Find("answerText").GetComponent<Text>();    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 localscale = imageTarget.transform.localScale;
        float scale = localscale.z;
        answer.text = "Camera: " + transform.position + "\nTarget: " + imageTarget.transform.position + "\nScale: " + scale;
        //Get distance between camera and traget
        //float distance = (Vector3.Distance(transform.position,imageTarget.transform.position))/10;
        float distance = (transform.position.z - imageTarget.transform.position.z)/10;
        //float temp = imageTarget.GetComponent<MeshRenderer>();
        answer.text += "\nDistance: " + distance;
        float distanceBetweenTwoPoints = (Vector3.Distance(cube1.transform.position,cube2.transform.position));
        answer.text += "\nDistance between two points: " + distanceBetweenTwoPoints;
    }
}
