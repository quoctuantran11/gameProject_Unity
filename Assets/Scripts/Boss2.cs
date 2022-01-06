using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Boss
{
    public GameObject boomerang;
    public float minTime, maxTime;

    void Awake() {
        Invoke("ThrowBoomerang", Random.Range(minTime, maxTime));
    }

    void ThrowBoomerang()
    {
        animator.SetTrigger("Boomerang");
        GameObject tempBoomerang = Instantiate(boomerang, transform.position, transform.rotation);
        if(isFlipped)
        {
            tempBoomerang.GetComponent<Boomerang>().direction = 1;
        }
        else
        {
            tempBoomerang.GetComponent<Boomerang>().direction = -1;
        }
        Invoke("ThrowBoomerang", Random.Range(minTime, maxTime));
    }
}
