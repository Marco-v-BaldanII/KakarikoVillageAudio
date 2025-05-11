using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))] /*Can't add script to object without a collider */
public class Surface : MonoBehaviour
{
    [SerializeField] private Surfaces surface;

    private bool on_trigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false && on_trigger) { return; }
        FootsetpCollection.surface = surface; // static field
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FootsetpCollection.surface = surface;
        }
        if (collision.gameObject.CompareTag("Player") == false && on_trigger) { return; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") == false && on_trigger == false) { return; }
        FootsetpCollection.surface = surface; // static field
    }

}
