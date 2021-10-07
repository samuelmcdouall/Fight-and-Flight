using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    bool deteriorating = false;
    bool started_deteriorating = false;
    public float max_life_time = 10.0f;
    float current_life_time;
    public float platform_fall_speed = 3.0f;
    Material platform_m;
    public Color color_max_life;
    public Color color_min_life;
    float color_lerp_time = 0;
    //GameObject platform_spawner;
    // Start is called before the first frame update

    void Start()
    {
        current_life_time = max_life_time;
        platform_m = GetComponent<Renderer>().material;
        //platform_spawner = GameObject.FindGameObjectWithTag("Platform Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        if (deteriorating)
        {   
            if (!started_deteriorating)
            {
                
                //started_deteriorating = true;
            }

            if (current_life_time > 0.0f)
            {
                current_life_time -= Time.deltaTime;
                platform_m.SetColor("_Color", Color.Lerp(color_max_life, color_min_life, color_lerp_time/max_life_time));
                color_lerp_time += Time.deltaTime;
            }

            else if (current_life_time < 0.0f)
            {
                current_life_time = 0.0f;
            }
            
            else
            {
                transform.Translate(-Vector3.up * Time.deltaTime * platform_fall_speed);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            deteriorating = true;
        }
        else if (collision.gameObject.tag == "Floor")
        {
            //platform_spawner.GetComponent<PlatformSpawner>().AttemptToSpawnPlatform();
            Destroy(gameObject);
        }
    }
}
