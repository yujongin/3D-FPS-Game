using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject trailPrefab;
    public Transform firingPosition;
    public GameObject particlePrefab;
    public TMP_Text bulletText;

    public int currentBullet = 8;
    public int totalBullet = 32;
    public int maxBulletMagazine = 8;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();  
    }

    void Update()
    {
        bulletText.text = currentBullet + "/" + totalBullet;
    }

    public void FireWeapon()
    {
        if (currentBullet > 0)
        {
            if (animator != null)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.SetTrigger("Fire");
                    currentBullet--;
                    Fire();
                    Debug.Log("22");
                }
            }
            else
            {
                currentBullet--;
                Fire();
            }
        }
    }
    protected virtual void Fire()
    {
        RayCastFire();
    }

    public void ReloadWeapon()
    {
        if (totalBullet > 0)
        {
            if (animator != null)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.SetTrigger("Reload");
                    Reload();
                }
            }
            else
            {
                Reload();
            }
        }
    }

    void Reload()
    {
        if(totalBullet >= maxBulletMagazine - currentBullet)
        {
            totalBullet -= maxBulletMagazine - currentBullet;
            currentBullet = maxBulletMagazine;
        }
        else
        {
            currentBullet += totalBullet;
            totalBullet = 0;
        }
    }

    public void RayCastFire()
    {
        Camera cam = Camera.main;

        RaycastHit hit;
        Ray r = cam.ViewportPointToRay(Vector3.one / 2);

        Vector3 hitPosition = r.origin + r.direction * 200;
        if (Physics.Raycast(r, out hit, 1000))//¾îµò°¡ ºÎµúÈ÷¸é true
        {
            hitPosition = hit.point;

            GameObject particle = Instantiate(particlePrefab);
            particle.transform.position = hitPosition; 
            particle.transform.forward = hit.normal;
        }

        GameObject go = Instantiate(trailPrefab);
        Vector3[] pos = new Vector3[] { firingPosition.position, hitPosition };
        go.GetComponent<LineRenderer>().SetPositions(pos);
        Destroy(go, 0.1f);
        //DestroyTrail(go);
    }

    //IEnumerator DestroyTrail(GameObject obj)
    //{
    //    yield return new WaitForSeconds(0.1f);

    //    Destroy(obj);
    //}
}
