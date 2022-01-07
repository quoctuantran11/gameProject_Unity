using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnComplete : MonoBehaviour
{
    void OnComplete()
    {
        Destroy(gameObject);
    }
}
