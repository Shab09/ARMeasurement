using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instObject : MonoBehaviour
{
    public GameObject distance;
    public GameObject Object;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            GameObject temp = Instantiate(Object);
            Vector3 pos = (GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,10)));
            //pos.z=distance.transform.position.z-transform.position.z;
            temp.transform.position= pos;
        }       
    }
}
