using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _ammoText;

    public void UpdateAmmo(int ammo){
        _ammoText.text = ammo + "/" + 50;
    }
}
