using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1.5f);
    }
}
