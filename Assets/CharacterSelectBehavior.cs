using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectBehavior : MonoBehaviour {

    public GameObject SelectItems;
    public GameObject SelectionRectangle;
    public GameObject SelectedText;
    public int Speed = 100;

    private int selectedIndex = 1;
    private CharacterSelectItem[] SelectItemChildren;

    private Quaternion initialRotation;

	// Use this for initialization
	void Start () {
        SelectItemChildren = GetComponentsInChildren<CharacterSelectItem>();
        initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        CharacterSelectItem selectedItem = SelectItemChildren[selectedIndex];
        if (selectedItem != null)
        {
            //SelectionRectangle.transform.SetPositionAndRotation(selectedItem.transform.position, selectedItem.transform.rotation);
            SelectionRectangle.transform.position = Vector3.Lerp(SelectionRectangle.transform.position, selectedItem.gameObject.transform.position, Speed * Time.deltaTime);
            SelectionRectangle.transform.localScale = Vector3.Lerp(SelectionRectangle.transform.localScale, selectedItem.gameObject.transform.localScale, Speed * Time.deltaTime);
            SelectedText.GetComponent<TextMesh>().text = selectedItem.Name;
        }

        if (Input.anyKeyDown)
        {
            selectedIndex = (selectedIndex + 1) % SelectItemChildren.Length; 
        }

        transform.rotation = initialRotation * Quaternion.Euler(20 * new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")));

		
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
