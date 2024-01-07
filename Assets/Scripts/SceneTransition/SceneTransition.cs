using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }
    [SerializeField] private Canvas canvasUi;
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    private void Start() {
        StartCoroutine(ToggleCanvasVisibility());
    }
    
    public void TransitionToActiveScene() {
        StartCoroutine(Transition());
    }

    private IEnumerator ToggleCanvasVisibility(){
        canvasUi.gameObject.SetActive(false);
        yield return new WaitForSeconds(transitionTime);
        canvasUi.gameObject.SetActive(true);
    }

    IEnumerator Transition() {
        canvasUi.gameObject.SetActive(false);
        transition.SetTrigger("end");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        transition.SetTrigger("start");
    }
}
