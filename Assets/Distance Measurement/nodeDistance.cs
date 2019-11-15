using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class nodeDistance : MonoBehaviour
{
    public List<GameObject> nodes = new List<GameObject>();
    public int currnodelistposition=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addNode(GameObject newNode){
        nodes.Add(newNode);
    }
    public void drawMeasurement(){
    }
}
