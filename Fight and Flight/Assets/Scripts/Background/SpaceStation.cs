using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 60.0f)]
    float revolution_time = 1.0f;

    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * 360.0f / revolution_time);
    }
}
