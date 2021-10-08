using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float revolution_time = 1.0f;
    [SerializeField]
    bool rare = false;
    int rare_value = 5;
    int common_value = 1;
    public AudioClip collect_sfx;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * 360.0f / revolution_time);
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player") {
            if (rare)
            {
                Player.score += rare_value;
            }
            else
            {
                Player.score += common_value;
            }

            AudioSource.PlayClipAtPoint(collect_sfx, transform.position);
            Destroy(gameObject);
        }
    }
}
