using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool is_grounded;
    private void OnTriggerStay(Collider collider)
    {
        is_grounded = collider != null;
    }
    private void OnTriggerExit(Collider collider)
    {
        is_grounded = false;
    }
}
