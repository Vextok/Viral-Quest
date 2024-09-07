using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coneScript : MonoBehaviour
{
    private float coneAngle = 126f;
    private float coneDistance = 1f;
    public float fixZ = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 startPosition = transform.position;
        startPosition.z = fixZ;
        transform.localScale = new Vector3(coneDistance * Mathf.Tan(coneAngle*Mathf.Deg2Rad/2) * 2, 4, coneDistance);
        Debug.Log("Cone Scale: " + transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 0.3f);
    }
}
