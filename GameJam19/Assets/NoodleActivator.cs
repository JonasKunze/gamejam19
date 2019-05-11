using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NoodleActivator : MonoBehaviour
{
    public GameObject noodleWeapon;
    public GameObject model;

    void Update()
    {
        model.SetActive(noodleWeapon.gameObject.GetActive());  
    }
}
