using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStatModifier : PickupItem
{
    [SerializeField] private List<CharacterStat> statsModifier;

    protected override void OnPickUp(GameObject receiver)
    {
        CharacterStatHandler statHandler = receiver.GetComponent<CharacterStatHandler>();

        foreach(CharacterStat stat in statsModifier)
        {
            statHandler.AddStatModifire(stat);
        }

        //체력바 새로고침
        HealthSystem healthSystem = receiver.GetComponent<HealthSystem>();
        healthSystem.ChangeHealth(0);
    }
}
