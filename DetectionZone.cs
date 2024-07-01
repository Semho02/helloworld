using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectionObjects = new List<Collider2D>();
    public Collider2D collider;
    public string tagTarget = "Player";
    // Start is called before the first frame update
    void Start()
    {
        collider.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == tagTarget)
        {
            detectionObjects.Add(collider);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == tagTarget)
        {
            detectionObjects.Remove(collider);
        }
    }
}
