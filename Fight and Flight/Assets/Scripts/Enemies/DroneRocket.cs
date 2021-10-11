using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DroneRocket : MonoBehaviour
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
        else if (collider.gameObject.tag == "Player")
        {
            //Explode(); say GAME OVER and show score
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Explode()
    {
        AudioSource.PlayClipAtPoint(explosion_sfx, player.transform.position + audio_offset);
        Instantiate(explosion_fx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
