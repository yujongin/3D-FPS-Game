using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject trailPrefab;
    public Transform firingPosition;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();  
    }


    public void FireWeapon()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetTrigger("Fire");
            RayCastFire();
        }
    }

    public void ReloadWeapon()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetTrigger("Reload");
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
        }

        GameObject go = Instantiate(trailPrefab);
        Vector3[] pos = new Vector3[] { firingPosition.position, hitPosition };
        go.GetComponent<LineRenderer>().SetPositions(pos);
    }
}
