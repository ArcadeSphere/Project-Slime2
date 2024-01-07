using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointHandler : MonoBehaviour
{
    private readonly String playerPrefKey = "IsActive";
    private bool isActivated;
    [SerializeField] private GameObject checkedAnimation;
    [SerializeField] private GameObject textAnimation;
    [SerializeField] private String displayText;
    private Animator explodeAnim;
    private Animator textAnim;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        LoadValue();
        if (displayText != "")
            textAnimation.GetComponent<TextMeshPro>().text = displayText;
        else
            textAnimation.GetComponent<TextMeshPro>().text = "Default Text";
        if (checkedAnimation != null && textAnimation != null)
        {
            explodeAnim = checkedAnimation.GetComponent<Animator>();
            textAnim = textAnimation.GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
        {
            if (!isActivated)
            {
                isActivated = true;
                SaveValue();
                PlayerData.Instance.currentCheckpoint = this.transform;
                if (checkedAnimation != null && textAnimation != null)
                {
                    explodeAnim.SetTrigger("triggered");
                    textAnim.SetTrigger("textfade");
                }
            }
        }
    }

    void LoadValue()
    {
        isActivated = PlayerPrefs.GetInt(playerPrefKey, 0) == 1; // return value with the key and returns true if value is equal to 1
    }

    void SaveValue()
    {
        PlayerPrefs.SetInt(playerPrefKey, isActivated ? 1 : 0);
        PlayerPrefs.Save();
    }

    void OnApplicationQuit() {
        isActivated = false;
        SaveValue();
    }
}
