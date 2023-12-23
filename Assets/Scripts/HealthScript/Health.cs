using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class Health : MonoBehaviour
{
  
    [SerializeField] private float startinglives;
    public float currenthealth { get; private set; }
    private Animator anim;
    private bool isdead;
    [SerializeField] private float Framesduration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer sr;
    [SerializeField] private Behaviour[] behviourcomponents;
    private bool invulnerable;
    [SerializeField] private Flash flashing;
    [SerializeField] private Rigidbody2D rb;
    private void Awake()
    {
        currenthealth = startinglives;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currenthealth = Mathf.Clamp(currenthealth - _damage, 0, startinglives);

        if (currenthealth > 0)
        {
            flashing.flash_time();

            StartCoroutine(BeInvincible());
        }
        else
        {
            if (!isdead && rb != null)
            {

                foreach (Behaviour component in behviourcomponents)
                component.enabled = false;
                flashing.flash_time();
                if (this.gameObject.CompareTag("Player"))
                    anim.SetBool("grounded", true);
                anim.SetTrigger("dying");
                isdead = true;
                rb.sharedMaterial = null;

            }
        }
    }
    public void AddHealth(float _value)
    {
        currenthealth = Mathf.Clamp(currenthealth + _value, 0, startinglives);
    }
    private IEnumerator BeInvincible()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            sr.color = new Color(0, 1, 0, 0.5f);
            yield return new WaitForSeconds(Framesduration / (numberOfFlashes * 2));
            sr.color = Color.white;
            yield return new WaitForSeconds(Framesduration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }
    public void camera_shake_dead() {
        CameraShake.Instance.ShakeCamera(3f, 0.2f);
    }
    public void destroy_player()
    {

        Destroy(gameObject);
  

    }

}