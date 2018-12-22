using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectBehavior : MonoBehaviour {

    public GameObject SelectItems;
    public GameObject SelectionRectangle;
    public int Speed = 100;

    private int selectedIndex = 1;
    private Transform[] SelectItemChildren;

	// Use this for initialization
	void Start () {
        SelectItemChildren = new Transform[SelectItems.transform.childCount];

        var i = 0;

        foreach (var child in SelectItems.transform)
        {
            SelectItemChildren[i] = child as Transform;
            i++;
        }
	}
	
	// Update is called once per frame
	void Update () {
        var selectedItem = SelectItemChildren[selectedIndex];
        if (selectedItem != null)
        {
            //SelectionRectangle.transform.SetPositionAndRotation(selectedItem.transform.position, selectedItem.transform.rotation);
            SelectionRectangle.transform.position = Vector3.Lerp(SelectionRectangle.transform.position, selectedItem.transform.position, Speed * Time.deltaTime);
        }

        if (Input.anyKeyDown)
        {
            selectedIndex = (selectedIndex + 1) % SelectItemChildren.Length; 
        }
		
	}

    /*
    IEnumerable<Transform> GetItems(Transform transform)
    {
        foreach (var child in transform)
        {
            Debug.Log(child);
        }
        return Enumerable.Empty<Transform>();
    }
    */
}
