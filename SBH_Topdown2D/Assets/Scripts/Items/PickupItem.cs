using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupItem : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnPickUp(other.gameObject);

        if(pickupSound)
        {
            SoundManager.PlayClip(pickupSound);
        }

        Destroy(gameObject);
    }

    protected abstract void OnPickUp(GameObject receiver);
}
