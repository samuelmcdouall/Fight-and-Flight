using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBolt : MonoBehaviour
{
    [SerializeField]
    float lifetime = 10.0f;
    public GameObject explosion_fx;
    public AudioClip explosion_sfx;
    GameObject player;
    Vector3 audio_offset = new Vector3(0.0f, 0.0f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Platform")
        {
            Explode();
        }
        else if (collider.gameObject.tag == "Target")
        {
            Destroy(collider.gameObject);
            Explode();
        }
        else if (collider.gameObject.tag == "Drone Hit Box")
        {
            Destroy(collider.gameObject.transform.parent.gameObject);
            Explode();
        }
    }

    private void Explode()
    {
        AudioSource.PlayClipAtPoint(explosion_sfx, player.transform.position + audio_offset);
        Instantiate(explosion_fx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
