using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    public bool isSelected = false;
    private Vector3 offset;

    void OnMouseDown()
    {
        isSelected = !isSelected;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePosition;
        offset.z = 0;
        Debug.Log("Unit Selected: " + isSelected);
    }

    void Update()
    {
        if (isSelected)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            mousePosition.z = 0; // Ensure z position remains 0
            transform.position = mousePosition;
            Debug.Log("Moving to: " + mousePosition);
        }
    }
}
