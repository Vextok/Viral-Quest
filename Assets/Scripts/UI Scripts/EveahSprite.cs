using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EveahSprite : MonoBehaviour
{
    private Sprite sprite1;
    public Sprite sprite2;
    private Image sR;
    System.Random rand = new();
    private bool isFlickering = false;

    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<Image>();
        sprite1 = sR.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFlickering)
            StartCoroutine(Flicker());
    }
    IEnumerator Flicker()
    {
        var num = rand.Next(0, 100);
        if (num >= 99)
        {
            isFlickering = true;
            sR.sprite = sprite2;
            yield return new WaitForSeconds(.01f);
            sR.sprite = sprite1;
            yield return new WaitForSeconds(.01f);
            sR.sprite = sprite2;
            yield return new WaitForSeconds(.01f);
            sR.sprite = sprite1;
            isFlickering = false;
        }
    }
}
