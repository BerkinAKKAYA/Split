using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void OnBecameInvisible() => gameObject.SetActive(false);
}