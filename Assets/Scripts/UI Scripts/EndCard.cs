using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EndCard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;
        TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
        List<string> textLine = text.text.Split("\n\n").ToList();
        int[] scores = DataCollector.GetScores();
        for (int i = 0; i < scores.Length; i++)
            textLine[i] += scores[i];
        text.text = string.Join("\n\n", textLine);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return))
            transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Loading...";
    }
}
