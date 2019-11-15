using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tamplates : MonoBehaviour
{
    public static Tamplates tamplates;
    public struct Grids{
        public GridMapping grid;
        public string GridName;
    }
    public List<Grids> GridList = new List<Grids>();
    public GridMapping CurrentGrid;
    // Start is called before the first frame update
    void Start()
    {
        if(tamplates==null){
            tamplates=this;
        }
        Grids newGrid = new Grids();

        CurrentGrid = new GridMapping();

        newGrid.grid = CurrentGrid;
        GridList.Add(newGrid);

        Debug.Log(Tamplates.tamplates.CurrentGrid.storage.Count);     
    }
    public void newGrid(){
        Grids newGrid = new Grids();

        CurrentGrid = new GridMapping();
        
        newGrid.grid = CurrentGrid;
        GridList.Add(newGrid);
    }
}
