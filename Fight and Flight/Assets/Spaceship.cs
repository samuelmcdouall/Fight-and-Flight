using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float lifetime;
    [SerializeField]
    float random_chosen_angle_arc;

    // Start is called before the first frame update
    void Start()
    {
        float chosen_angle_arc = Random.Range(-random_chosen_angle_arc, random_chosen_angle_arc);
        transform.Rotate(Vector3.up, chosen_angle_arc);
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.forward * Time.deltaTime * speed);
    }
}
