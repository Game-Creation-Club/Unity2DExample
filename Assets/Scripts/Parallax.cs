using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    
    private Vector3 initCameraPos;
    private Vector3 initPos;

    public Transform mainCamera;

    public float rate;

    void Start() {
        initCameraPos = mainCamera.position;
        initPos = transform.position;
    }

    void LateUpdate() {
        transform.position = initPos + (mainCamera.position - initCameraPos) * rate;
    }


}
