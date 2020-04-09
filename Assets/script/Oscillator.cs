using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;

    [SerializeField] float speed = 0.01f;
    float movementFactor;

    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        updateFactor();
        Vector3 offset = movementFactor*movementVector;
        transform.position = startPosition+offset;
    }

    void updateFactor() {
        if (movementFactor+speed > 1 || movementFactor+speed < 0){
            speed = -1f*speed;
        } else {
            movementFactor = movementFactor+speed;
        }

    }
}
