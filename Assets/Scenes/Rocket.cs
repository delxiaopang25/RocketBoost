using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    float mass;
    [SerializeField]  float thrustMultiplier = 1000f;
    [SerializeField] float rotateMultiplier = 100f;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        mass = rigidBody.mass;
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            rigidBody.AddRelativeForce(thrustMultiplier * this.mass* Time.deltaTime * Vector3.up);
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {

        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(rotateMultiplier * this.mass*Time.deltaTime  * Vector3.forward);
        } else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-rotateMultiplier * this.mass* Time.deltaTime * Vector3.forward);
        }
        rigidBody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Friendly");
                break;
            default:
                print("Dead");
                break;
        }
    }
}
