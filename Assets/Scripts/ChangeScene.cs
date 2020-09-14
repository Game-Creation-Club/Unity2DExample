using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
    
    public string sceneTo;
    public string spawnTo;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            GameManager.instance.spawnTo = spawnTo;
            SceneManager.LoadScene(sceneTo);
        }
    }
}
