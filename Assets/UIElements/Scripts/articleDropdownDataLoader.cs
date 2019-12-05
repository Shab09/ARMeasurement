using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class articleDropdownDataLoader : MonoBehaviour
{
    [System.Serializable]
    public struct OptionItem
    {
        public string label;
        public Sprite image;
    }
    public Dropdown articleDropDown;
    public List<OptionItem> optionItems;
    private List<Dropdown.OptionData> options;


    // Start is called before the first frame update
    void Start()
    {
        articleDropDown.ClearOptions();
        options = new List <Dropdown.OptionData>();
        foreach (OptionItem item in optionItems){
            Dropdown.OptionData temp = new Dropdown.OptionData(item.label,item.image);
            Debug.Log(temp.image);
            options.Add(temp);
        }
        articleDropDown.options = options;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
