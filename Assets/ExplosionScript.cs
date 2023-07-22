using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    void Start()
    {
        HitFlash hf = gameObject.GetComponent<HitFlash>();
        if (hf != null) hf.Flash(25f);
        Destroy(gameObject, 1.5f);
    }
}
