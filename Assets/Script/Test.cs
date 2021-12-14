using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateSphere());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator CreateSphere() {
        float r = 10f;

        int splitNum = 50;
        float d = Mathf.PI / splitNum;
        for (int i = 0; i <= splitNum; i++)
        {
            float sth = d * i;
            float x = r * Mathf.Cos(3 * sth) * Mathf.Cos(sth);
            float y = r * Mathf.Cos(3 * sth) * Mathf.Sin(sth);
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = new Vector3(x, y, 0);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
