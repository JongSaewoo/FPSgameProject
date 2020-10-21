using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField]
    private float casingSpin = 1.0f;
    private AudioSource audioSource;
    private Rigidbody rigidbody3D;

    public void Setup(Vector3 direction)
    {
        audioSource = this.GetComponent<AudioSource>();
        rigidbody3D = this.GetComponent<Rigidbody>();

        rigidbody3D.velocity = new Vector3(direction.x, 1.0f, direction.z);
        rigidbody3D.angularVelocity = new Vector3(Random.Range(-casingSpin, casingSpin),
                                               Random.Range(-casingSpin, casingSpin),
                                               Random.Range(-casingSpin, casingSpin));
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Play();
    }
}
