using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEffect : MonoBehaviour
{
    void Kill() {
        Destroy(gameObject);
    }
}
