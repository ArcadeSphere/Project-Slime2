using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    private bool isActivated = false;
    [SerializeField] private GameObject explode;
    [SerializeField] private GameObject text;
    [SerializeField] private String displayText;
    private Animator explodeAnim;
    private Animator textAnim;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        if (displayText != "")
            text.GetComponent<TextMeshPro>().text = displayText;
        else
            text.GetComponent<TextMeshPro>().text = "Default Text";
        if (explode != null && text != null)
        {
            explodeAnim = explode.GetComponent<Animator>();
            textAnim = text.GetComponent<Animator>();
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
        {
            if (!isActivated)
            {
                isActivated = true;
                PlayerData.Instance.currentCheckpoint = this.transform;
                if (explode != null && text != null)
                {
                    explodeAnim.SetTrigger("triggered");
                    textAnim.SetTrigger("textfade");
                }
                Debug.Log("Checkpoint Set");
            }
        }
    }
}
