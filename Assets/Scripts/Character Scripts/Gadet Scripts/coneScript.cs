using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coneScript : MonoBehaviour
{
    public float coneAngle = 135f;
    public float coneDistance = 3f;
    public float fixZ = 5f;
    public float coneHeight = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 startPosition = transform.position;
        startPosition.z = fixZ;
        transform.localScale = new Vector3(coneDistance * Mathf.Tan(coneAngle*Mathf.Deg2Rad/2) * 2, coneHeight, coneDistance);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 0.3f);
        //Destroy(gameObject, 2.0f);
    }
}
