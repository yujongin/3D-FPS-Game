using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp = 10;
    public float maxHp = 10;

    float lastDamageTime;
    public float invincibleTime;

    public Image hpGauge;

    public AudioClip hurtSound;
    public AudioClip dieSound;

    IHealthListener healthListener;
    void Start()
    {
        healthListener = GetComponent<IHealthListener>();
    }

    public void Damage(float damage)
    {
        if (hp > 0 && lastDamageTime + invincibleTime < Time.time)
        {
            lastDamageTime = Time.time;
            hp -= damage;

            if (hpGauge != null)
            {
                hpGauge.fillAmount = hp / maxHp;
            }

            if (hp <= 0)
            {
                if(dieSound != null) 
                    GetComponent<AudioSource>().PlayOneShot(dieSound);
                //Á×À½
                if(healthListener != null)
                {
                    healthListener.OnDie();
                }
            }
            else
            {
                if (hurtSound != null)
                    GetComponent<AudioSource>().PlayOneShot(hurtSound);
            }
        }
    }

    public interface IHealthListener
    {
        void OnDie();
    }
}
