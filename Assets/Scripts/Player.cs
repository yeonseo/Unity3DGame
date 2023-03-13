using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform groundCheck = null;
    [SerializeField] private LayerMask playerMask;

    private bool jumpKeyWasPress = false;
    private float horizontalInput;
    private Rigidbody rigidbody;
    private SceneLoader SceneLoader;
    private float jumpPower = 5.0f;
    private float velocityPower = 0.5f;

    private int score = 0;



    // new
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRot;

    [SerializeField] private Transform PlayerCamera;
    // [SerializeField] private Rigidbody rigidbody;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        SceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            jumpKeyWasPress = true;
        }
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(horizontalInput, rigidbody.velocity.y, 0);

        if(Physics.OverlapSphere(groundCheck.position, velocityPower, playerMask).Length == 0){
            return;
        }

        if ( velocityPower - score < 1.0f ) {
            velocityPower += 0.1f;
        }

        if (jumpKeyWasPress) {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            jumpKeyWasPress = false;
        }
            
        // Debug.Log(rigidbody.position.y);

        // Debug.Log(rigidbody.position.z);



        Debug.Log(rigidbody.position.y);
        if (rigidbody.position.y < -0.5) {
            Debug.Log(rigidbody.position.z);

            Debug.Log(rigidbody.position.y);

            SceneLoader.LoadNextScene();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 9) {
            score += 1;
            Destroy(other.gameObject);
        }
    }
}
