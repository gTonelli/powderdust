using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject bulletSound;

    void OnEnable()
    {
        GameObject bulletSoundInstance = Instantiate(bulletSound);
        Destroy(bulletSoundInstance, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Destroying enemy bullet");
        Debug.Log("Collided with" + collision.gameObject.name);
        Destroy(gameObject);
    }
}
