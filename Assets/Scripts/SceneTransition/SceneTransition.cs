using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }
    [SerializeField] private Animator transition;
    [SerializeField] private int transitionWaitTime = 1;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
       
    }

    public void SetStartTrigger(){
        transition.SetTrigger("start");
    }

    public void TransitionToActiveScene() {
        StartCoroutine(Transition());
    } 
    IEnumerator Transition() {
        transition.SetTrigger("end");
        yield return new WaitForSeconds(transitionWaitTime);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        transition.SetTrigger("start");
    }
}
