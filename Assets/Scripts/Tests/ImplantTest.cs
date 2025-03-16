using UnityEngine;
using System.Collections.Generic;

public class ImplantTest : MonoBehaviour
{
    private void Start()
    {
        List<Implant> implants = ImplantDatabase.Instance.GetAllImplants();

        foreach (Implant implant in implants)
        {
            Debug.Log($"Имплант: {implant.Name} | Слот: {implant.Slot}");
        }
    }
}
