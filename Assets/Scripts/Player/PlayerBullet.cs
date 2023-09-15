using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public GameObject bulletSound;

    void OnEnable()
    {
        GameObject bulletSoundInstance = Instantiate(bulletSound);
        Destroy(bulletSoundInstance, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
