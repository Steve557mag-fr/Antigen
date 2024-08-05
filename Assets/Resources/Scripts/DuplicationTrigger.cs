using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicationTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bacteria>())
        {
            collision.GetComponent<Bacteria>().Duplicate();
        }
    }



}
