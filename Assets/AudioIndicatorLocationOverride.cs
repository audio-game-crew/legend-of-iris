using UnityEngine;
using System.Collections;

public class AudioIndicatorLocationOverride : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white.seta(0.5f);
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
