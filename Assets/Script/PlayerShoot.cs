using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject prefab;
    public Vector2 spawnPoint;

    float cooldownCurrent;
    public float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldownCurrent -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (cooldownCurrent < 0) {
                cooldownCurrent = cooldown;
                GameObject shot = Instantiate(prefab, (Vector2)transform.position + spawnPoint, new Quaternion(0, 0, 0, 0));
                shot.GetComponent<Rigidbody2D>().velocity = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - ((Vector2) transform.position + spawnPoint)).normalized * 6.66f;
            }
        }
    }
}
