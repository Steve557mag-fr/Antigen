using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] internal UnityEvent<Collider2D> eventsOnDeath;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        eventsOnDeath.Invoke(collision);
        Destroy(collision.gameObject);
    }

}
