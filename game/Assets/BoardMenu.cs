using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardMenu : MonoBehaviour {

    public Text energyField;

	void Start () {
        SetEnergy();
        Events.OnEnergyWon += OnEnergyWon;
	}
    void OnDestroy()
    {
        Events.OnEnergyWon -= OnEnergyWon;
    }
    void OnEnergyWon()
    {
        SetEnergy();
    }
    void SetEnergy()
    {
        int totalEnergy = Data.Instance.energyManager.ENERGY_TO_START;
        int energy = Data.Instance.energyManager.energy;
        energyField.text = energy + "/" + totalEnergy;
    }
}
