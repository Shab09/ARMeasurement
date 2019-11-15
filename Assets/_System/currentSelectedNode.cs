using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class currentSelectedNode : MonoBehaviour {
    public static currentSelectedNode selectedNodeSystem;
    
    public GameObject SelectedNode;
    private nodeDistance SelectedNodeDistance;
    
    public LineRenderer line;
    public GameObject NodeReference;
    public GameObject SelectedNodeHighLight;
    

    public GameObject NodeSelectedPanel;
    public GameObject SaveNodePanel;
    public GameObject SavePrompt;
    public InputField nameofdistance;
    

    public float k=0.00507f;
    // Start is called before the first frame update
    void Start () {
        if (selectedNodeSystem == null) {
            selectedNodeSystem = this;
        } else if (selectedNodeSystem != this) {
            Destroy (gameObject);
        }
    }
    void Update () {
        if (Input.GetMouseButtonDown (0)) {
            if (!EventSystem.current.currentSelectedGameObject) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                if (Physics.Raycast (ray, out hit) && hit.transform.tag == "Node") {

                    SelectedNode = hit.transform.gameObject;
                    SelectedNodeDistance = SelectedNode.GetComponent<nodeDistance> ();

                    if (SelectedNodeDistance.nodes.Count > 0) {
                        line.positionCount = 2;
                        line.SetPosition (1, SelectedNodeDistance.nodes[SelectedNodeDistance.currnodelistposition].transform.position);
                        SaveNodePanel.SetActive(true);
                    }
                    else{
                        SaveNodePanel.SetActive(false);
                    }
                    line.SetPosition (0, SelectedNode.transform.position);

                } else {

                    Vector3 position = Input.mousePosition;
                    position.z = 100;

                    GameObject newNode = Instantiate (NodeReference);

                    if (SelectedNode)
                        SelectedNode.GetComponent<nodeDistance> ().addNode (newNode);

                    SelectedNode = newNode;

                    Vector3 newPosition = Camera.main.ScreenToWorldPoint (position);
                    newPosition.z = 100;
                    SelectedNode.transform.position = newPosition;
                    SelectedNodeDistance = SelectedNode.GetComponent<nodeDistance> ();
                    line.SetPosition (0, SelectedNode.transform.position);
                    line.positionCount = 1;
                    SaveNodePanel.SetActive(false);
                }

                SelectedNodeHighLight.transform.position = SelectedNode.transform.position;
                SelectedNodeHighLight.SetActive (true);
                NodeSelectedPanel.SetActive(true);
            }
        }
    }

    public void nextNode () {
        if (SelectedNodeDistance && SelectedNodeDistance.nodes.Count > 0) {
            line.positionCount = 2;
            SelectedNodeDistance.currnodelistposition++;
            if (SelectedNodeDistance.currnodelistposition > SelectedNodeDistance.nodes.Count - 1) {
                SelectedNodeDistance.currnodelistposition = 0;
            }
            line.SetPosition (1, SelectedNodeDistance.nodes[SelectedNodeDistance.currnodelistposition].transform.position);
        }
    }
    public void previousNode () {
        if (SelectedNodeDistance && SelectedNodeDistance.nodes.Count > 0) {
            line.positionCount = 2;
            SelectedNodeDistance.currnodelistposition--;
            if (SelectedNodeDistance.currnodelistposition < 0) {
                SelectedNodeDistance.currnodelistposition = SelectedNodeDistance.nodes.Count - 1;
            }
            line.SetPosition (1, SelectedNodeDistance.nodes[SelectedNodeDistance.currnodelistposition].transform.position);
        }
    }
    public void done () {
        SelectedNodeDistance = null;
        SelectedNode = null;
        SelectedNodeHighLight.SetActive (false);
        NodeSelectedPanel.SetActive(false);
    }

    public void promptSave(){
        SavePrompt.SetActive(true);
    }

    public void savePoints () {

        GridMapping.point firstpoint = new GridMapping.point ();
        Vector3 firstposition = SelectedNode.transform.position;
        firstpoint.x = firstposition.x;
        firstpoint.y = firstposition.y;
        firstpoint.z = firstposition.z;

        GridMapping.point secondpoint = new GridMapping.point ();
        Vector3 secondposition = SelectedNodeDistance.nodes[SelectedNodeDistance.currnodelistposition].transform.position;
        secondpoint.x = secondposition.x;
        secondpoint.y = secondposition.y;
        secondpoint.z = secondposition.z;

        GridMapping.distance newdistance = new GridMapping.distance();
        newdistance.one = firstpoint;
        newdistance.two = secondpoint;
        newdistance.nameofdistance = nameofdistance.text;

        float z = distMeasure.measurement.imagePosition();
        float x = Vector3.Distance(firstposition,secondposition);
        float realWorldDist= x*z*(2*k)*10 ;
        
        Debug.Log("Real World Distance" + realWorldDist);
        newdistance.distanceBWpoints =  realWorldDist;

        Tamplates.tamplates.CurrentGrid.storage.Add(newdistance);
        SavePrompt.SetActive(false);
    }
    public void CancleSave(){
        SavePrompt.SetActive(false);
    }
}