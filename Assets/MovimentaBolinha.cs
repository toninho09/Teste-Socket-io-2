using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class MovimentaBolinha : MonoBehaviour
{

    private Rigidbody rb;
    public float velocidade;

    // Use this for initialization
    void Start()
    {
        CancelInvoke();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 move = new Vector3((Input.GetAxis("Horizontal") + CrossPlatformInputManager.GetAxis("Horizontal")), 0, (Input.GetAxis("Vertical") + CrossPlatformInputManager.GetAxis("Vertical")));
        rb.AddForce(move * velocidade);
    }


    void OnTriggerEnter(Collider outro)
    {
        if (outro.gameObject.CompareTag("item"))
        {
        }
    }
}
