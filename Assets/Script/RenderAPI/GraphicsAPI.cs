using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsAPI : MonoBehaviour
{
    public RawImage[] imgs;
    void Start()
    {
        GraphicsStaticObject();
    }

    void Update()
    {

    }

    void GraphicsStaticObject()
    {
        print(Graphics.activeColorBuffer);
        print(Graphics.activeDepthBuffer);
        print(Graphics.activeColorGamut);        
        print(Graphics.minOpenGLESVersion);
    }
}
