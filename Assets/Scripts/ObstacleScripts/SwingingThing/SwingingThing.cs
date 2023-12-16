using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingThing : MonoBehaviour
{
    private Rigidbody2D swtrb;
    [SerializeField] private float swtspeed;
    [SerializeField] private float leftangle;
    [SerializeField] private float rightangle;
    void Start()
    {
        swtrb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
