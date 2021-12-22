using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingImg : MonoBehaviour
{
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        StartBlinking();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Blink()
    {
        while(true)
        {
            switch(image.color.a.ToString())
            {
                case "0":
                    image.color = new Color(image.color.r, image.color.g, image.color.g, 1);
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "1":
                    image.color = new Color(image.color.r, image.color.g, image.color.g, 0);
                    yield return new WaitForSeconds(0.5f);
                    break;    
            }
        }
    }

    void StartBlinking()
    {
        StopCoroutine(Blink());
        StartCoroutine(Blink());
    }

    void StopBlinking()
    {
        StopCoroutine(Blink());
    }
}
