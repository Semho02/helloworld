using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public string locationName;

    public static event System.Action<string> LocationVisited;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (LocationVisited != null)
            {
                LocationVisited.Invoke(locationName);
            }
        }
    }
}
