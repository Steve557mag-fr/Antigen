using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] internal UnityEvent eventsOnDeath;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        eventsOnDeath.Invoke();
        Destroy(collision.gameObject);
    }

}
