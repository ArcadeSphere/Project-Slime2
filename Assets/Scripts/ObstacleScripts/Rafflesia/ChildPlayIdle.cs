using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPlayIdle : MonoBehaviour
{
     [SerializeField] private GameObject gameObjectRef;
    private Animator objectAnimator;


    private void Start() {
        objectAnimator = gameObjectRef.GetComponent<Animator>();
    }

    void PlayChildIdle() {
        objectAnimator.SetBool("shoot", false);
    } 
}
