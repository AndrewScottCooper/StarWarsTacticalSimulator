using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

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

    void ShowUnits(string type)
    {
        // Clear previous units
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Determine which units to show based on type
        List<GameObject> unitsToShow = type == "Ground" ? groundUnits : spaceUnits;

        // Instantiate new unit items
        foreach (var unitPrefab in unitsToShow)
        {
            GameObject newButton = Instantiate(unitButtonPrefab, contentPanel);
            UnitButton unitButton = newButton.GetComponent<UnitButton>();

            // Get the sprite from the SpriteRenderer component
            Sprite unitSprite = unitPrefab.GetComponent<SpriteRenderer>().sprite;

            // Setup the button with the unit's image and an example onClick action
            unitButton.Setup(unitSprite, () => Debug.Log("Unit Selected: " + unitPrefab.name));
        }
    }
}