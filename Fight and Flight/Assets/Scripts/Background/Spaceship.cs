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
    [SerializeField]
    public bool reverse_spawner = false;

    // Start is called before the first frame update
    void Start()
    {
        float chosen_angle_arc;
        if (!reverse_spawner)
        {
            chosen_angle_arc = Random.Range(-random_chosen_angle_arc, random_chosen_angle_arc);
        }
        else
        {
            chosen_angle_arc = Random.Range(-random_chosen_angle_arc + 180, random_chosen_angle_arc + 180);
        }
        transform.Rotate(Vector3.up, chosen_angle_arc);
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!reverse_spawner)
        //{
           transform.Translate(-Vector3.forward * Time.deltaTime * speed);
        //}
        //else
        //{
        //    transform.Translate(Vector3.forward * Time.deltaTime * speed);
        //}
        
    }
}
