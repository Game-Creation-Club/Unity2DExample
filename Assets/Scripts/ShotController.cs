using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour {

    public float speed = 5f;
    public GameObject explodeParticles;
    public bool right = true;
    public bool moving = true;

    private AudioSource audioSource;
    public AudioClip shotSound;
    public AudioClip explodeSound;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(shotSound);
    }

    void Update() {
        if (moving)
            transform.position += ((right ? transform.right : -1 * transform.right) * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (moving && other.tag != "Player" && other.tag != "Shot" && other.tag != "IgnoreCollision") {
            moving = false;
            audioSource.PlayOneShot(explodeSound);
            GetComponent<Animator>().SetTrigger("Explode");
        }
    }

    void Destroy() {
        Destroy(gameObject);
    }
}
