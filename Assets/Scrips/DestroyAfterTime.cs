using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time;
    void start()
    {
        Destroy(gameObject, time);
    }
}
