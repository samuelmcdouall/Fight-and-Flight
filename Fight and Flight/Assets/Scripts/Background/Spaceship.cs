using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float lifetime;
    [SerializeField]
    float random_chosen_angle_arc;
    public bool reverse_spawner = false;

    void Start()
    {
        DetermineFlightPath();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(-Vector3.forward * Time.deltaTime * speed);
    }

    void DetermineFlightPath()
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
    }


}
