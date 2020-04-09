using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    float mass;
    enum State
    {
        Alive,
        Dying,
        Transcending    
    }

    State state = State.Alive;

    [SerializeField]  float thrustMultiplier = 1000f;
    [SerializeField] float rotateMultiplier = 100f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip Finish;
    [SerializeField] AudioClip Death;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem FinishParticle;
    [SerializeField] ParticleSystem DeathParticle;

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
        if (state == State.Alive) {
            Thrust();
            Rotate();
        }

    }

    private void Thrust()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }

            rigidBody.AddRelativeForce(thrustMultiplier * this.mass* Time.deltaTime * Vector3.up);
            mainEngineParticle.Play();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticle.Stop();
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
        if (state != State.Alive) {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(Finish);
                FinishParticle.Play();
                Invoke("LoadNextScene",3f);
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(Death);
                mainEngineParticle.Stop();
                DeathParticle.Play();
                Invoke("resetScene",3f);
                break;
        }
    }

    private void LoadNextScene() {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex+1 < SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene(buildIndex+1);
        }

    }

    private void resetScene() {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(buildIndex);
    }
}
