using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string spawnTo;

    void Start() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded  += OnSceneLoad;
        } else {
            Destroy(gameObject);
        }
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        if (spawnTo != null) {
            SpawnPoint target = Object.FindObjectsOfType<SpawnPoint>().Where(x => x.spawnId == spawnTo).First();
            PlayerController.instance.transform.position = target.transform.position;
            spawnTo = null;
        }
        foreach (CinemachineVirtualCamera cam in Object.FindObjectsOfType<CinemachineVirtualCamera>()) {
            cam.Follow = PlayerController.instance.transform;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
