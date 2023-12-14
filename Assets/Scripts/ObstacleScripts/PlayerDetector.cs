using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
   [field: SerializeField]
   public bool playerDetected {get; private set;}
   public Vector2 directionToTarget => target.transform.position - detectorOrigin.position;

   [Header("OverlapBox parameters")]
   [SerializeField] private Transform detectorOrigin;
   [SerializeField] private Vector2 detectorSize = Vector2.one;
   [SerializeField] private Vector2 detectorOriginOffset = Vector2.zero;
   public float detectionDelay = 0.3f;
   public LayerMask detectorLayerMask;

   [Header("Gizmo parameters")]
   public Color gizmoIdleColor = new Color(0f, 1f, 0f, 0.4f);
   public Color gizmoDetectedColor = new Color(1f, 0f, 0f, 0.4f);
   public bool showGizmo = true;


   private GameObject target;

   public GameObject Target 
    {
        get => target;
        private set 
        {
            target = value;
            playerDetected = target != null;
        }
    }

   private void Start() 
    {
        StartCoroutine(DetectionCoroutine());
    }

    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionDelay);
        PerformDetection();
        StartCoroutine(DetectionCoroutine());
    }

    public void PerformDetection()
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize, 0, detectorLayerMask);
        if (collider != null)
        {
            Target = collider.gameObject;
        }
        else
        {
            Target = null;
        }
    }

    private void OnDrawGizmos() {
        if (showGizmo && detectorOrigin != null)
        {
            Gizmos.color = gizmoIdleColor;
            if (playerDetected)
            {
                Gizmos.color = gizmoDetectedColor;
            }
            Gizmos.DrawCube((Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize);
        }
    }

}
