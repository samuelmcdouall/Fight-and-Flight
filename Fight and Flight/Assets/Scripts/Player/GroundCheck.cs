using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool is_grounded;
    void OnTriggerStay(Collider collider)
    {
        is_grounded = collider != null;
    }
    void OnTriggerExit(Collider collider)
    {
        is_grounded = false;
    }
}
