using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUI : MonoBehaviour
{
    public TMPro.TMP_Text nameText;
    public TMPro.TMP_Text characterText;
    public TMPro.TMP_Text specialisationText;
    public TMPro.TMP_Text populationText;
    public TMPro.TMP_Text gdpText;
    public TMPro.TMP_Text sectorTaxText;
    public TMPro.TMP_Text baseCreditsText;
    public TMPro.TMP_Text militaryCapacityText;
    public TMPro.TMP_Text stabilityText;
    public TMPro.TMP_Text netCreditsText;
    public TMPro.TMP_Text tradeRoutesText;
    public TMPro.TMP_Text shipbuildingSlotsText;
    public TMPro.TMP_Text armySlotsText;
    public TMPro.TMP_Text planetaryModifierText;

    public void Setup(Planet planet)
    {
        nameText.text = planet.PlanetName;
        characterText.text = planet.Character;
        specialisationText.text = planet.Specialisation;
        populationText.text = planet.Population.ToString();
        gdpText.text = planet.GDP.ToString();
        sectorTaxText.text = planet.SectorTax.ToString();
        baseCreditsText.text = planet.BaseCredits.ToString();
        militaryCapacityText.text = planet.MilitaryCapacity.ToString();
        stabilityText.text = planet.Stability.ToString();
        netCreditsText.text = planet.NetCredits.ToString();
        tradeRoutesText.text = planet.TradeRoutes;
        shipbuildingSlotsText.text = planet.ShipbuildingSlots.ToString();
        armySlotsText.text = planet.ArmySlots.ToString();
        planetaryModifierText.text = string.Join(", ", planet.PlanetaryModifiers.ConvertAll(m => m.Name));
    }
}
