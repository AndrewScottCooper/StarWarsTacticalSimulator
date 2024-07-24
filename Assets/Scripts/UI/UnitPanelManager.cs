using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;

public class UnitPanelManager : MonoBehaviour
{
    public Button groundUnitsButton;
    public Button spaceUnitsButton;
    public TMP_Dropdown factionDropdown;
    public Transform contentPanel;
    public GameObject unitButtonPrefab;

    // Lists of GameObjects representing units
    public List<GameObject> groundUnits;
    public List<GameObject> spaceUnits;

    void Start()
    {
        groundUnitsButton.onClick.AddListener(() => ShowUnits("Ground"));
        spaceUnitsButton.onClick.AddListener(() => ShowUnits("Space"));
        factionDropdown.onValueChanged.AddListener(delegate { ShowUnits(factionDropdown.options[factionDropdown.value].text); });

        ShowUnits("Space"); // Default view
    }

    private GameObject currentUnit; // Reference to the currently spawned unit
    void Update()
    {
        // If a unit is currently being placed, follow the mouse
        if (currentUnit != null)
        {
            FollowMouse();
        }
    }

    void ShowUnits(string type)
    {
        // Clear previous units
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Determine which units to show based on type
        List<GameObject> unitsToShow = type == "Ground" ? groundUnits : spaceUnits;

        // Get the selected faction as an index
        int selectedFactionIndex = factionDropdown.value;
        UnitFaction.Faction selectedFaction = (UnitFaction.Faction)selectedFactionIndex;

        // Filter units based on the selected faction
        List<GameObject> filteredUnits = new List<GameObject>();
        foreach (var unit in unitsToShow)
        {
            UnitFaction unitFaction = unit.GetComponent<UnitFaction>();
            if (unitFaction.faction == selectedFaction)
            {
                filteredUnits.Add(unit);
            }
        }

        // Instantiate new unit items
        foreach (var unitPrefab in filteredUnits)
        {
            GameObject newButton = Instantiate(unitButtonPrefab, contentPanel);
            UnitButton unitButton = newButton.GetComponent<UnitButton>();

            // Get the sprite from the SpriteRenderer component
            Sprite unitSprite = unitPrefab.GetComponent<SpriteRenderer>().sprite;

            // Setup the button with the unit's image and an example onClick action
            unitButton.Setup(unitSprite, () => StartPlacingUnit(unitPrefab));
        }
    }

    void StartPlacingUnit(GameObject unitPrefab)
    {
        if (currentUnit != null)
        {
            Destroy(currentUnit);
        }
        currentUnit = Instantiate(unitPrefab);
        currentUnit.GetComponent<Collider2D>().enabled = false; // Disable collider while placing
    }

    void FollowMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the z-axis is always set to zero
        currentUnit.transform.position = mousePosition;

        // Place the unit when left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            currentUnit.GetComponent<Collider2D>().enabled = true; // Re-enable collider
            currentUnit = null; // Stop following the mouse
        }
    }

}