using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Planet : MonoBehaviour
{

    public string PlanetName;
    public string Character;
    public string Specialisation;
    public float Population;
    public float GDP;
    public float SectorTax;
    public float BaseCredits;
    public float MilitaryCapacity;
    public float Stability;
    public float NetCredits;
    public string TradeRoutes;
    public float ShipbuildingSlots;
    public float ArmySlots;
    //List of planetary modifiers
    public List<Modifier> PlanetaryModifiers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }




}
[System.Serializable]
public class Modifier
{
    public string Name;
    public string Description;
}
