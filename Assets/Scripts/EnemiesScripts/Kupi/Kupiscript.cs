using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kupiscript : PlayerDetector
{
    [Header("Shooting parameters")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootCooldown = 2f;
    private bool canShoot = true;
    [SerializeField] private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            if (playerDetected && IsPlayerOnDesiredLayer() && canShoot)
            {
                // Trigger the shooting animation and shooting logic
                TriggerShootingAnimation();

                canShoot = false;
                yield return new WaitForSeconds(shootCooldown);
                canShoot = true;
            }
            yield return null;
        }
    }
    

    void TriggerShootingAnimation()
    {
        anim.SetTrigger("shootatplayer");
    }

    bool IsPlayerOnDesiredLayer()
    {
        // Check if the detected player is on the desired layer
        if (Target != null)
        {
            return Target.layer == LayerMask.NameToLayer("Player");
        }

        return false;
    }

    // Method to be called from the animation event
    public void Shoot()
    {
        Debug.Log("shooting");
        // Instantiate the projectile at the shoot point position and rotation
        //GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // Assuming the projectile has a script named ProjectileScript
        //ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();

        // Set the direction the projectile will move (e.g., to the right)
       // projectileScript.SetDirection(Vector2.right);
    }
}