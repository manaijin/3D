using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsAPI : MonoBehaviour
{
    public Texture texture;

    public Shader[] shaders;
    private List<Material> mats;

    private Rect[] uiRects;
    private Rect[] screenRects;

    void Start()
    {
        DataInit();
        // GraphicsStaticObject();
    }

    void Update()
    {

    }

    void OnGUI()
    {
        if (Event.current.type.Equals(EventType.Repaint))
        {
            // TextureToScreen();
            TextureCopy();
        }
    }

    void DataInit()
    {
        screenRects = new Rect[4];
        var center = new Vector2(Screen.width / 2, Screen.height / 2);
        screenRects[0] = new Rect(center.x - 300, center.y - 300, 250, 250);
        screenRects[1] = new Rect(center.x - 300, center.y + 50, 250, 250);
        screenRects[2] = new Rect(center.x + 50, center.y + 50, 250, 250);
        screenRects[3] = new Rect(center.x + 50, center.y - 300, 250, 250);


        uiRects = new Rect[4];
        uiRects[0] = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
        uiRects[1] = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
        uiRects[2] = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        uiRects[3] = new Rect(0.5f, 0.5f, 0.5f, 0.5f);

        if (shaders != null)
        {
            mats = new List<Material>();
            foreach (var shader in shaders)
            {
                mats.Add(new Material(shader));
            }
        }

    }

    void GraphicsStaticObject()
    {
        print(Graphics.activeColorBuffer);
        print(Graphics.activeDepthBuffer);
        print(Graphics.activeColorGamut);
        print(Graphics.minOpenGLESVersion);
    }

    /// <summary>
    /// ªÊ÷∆Œ∆¿ÌµΩ∆¡ƒª
    /// </summary>
    void TextureToScreen()
    {
        Graphics.DrawTexture(screenRects[0], texture, uiRects[0], 0, 0, 0, 0, null);
        Graphics.DrawTexture(screenRects[1], texture, uiRects[1], 0, 0, 0, 0, null);
        Graphics.DrawTexture(screenRects[2], texture, uiRects[2], 0, 0, 0, 0, null);
        Graphics.DrawTexture(screenRects[3], texture, uiRects[3], 0, 0, 0, 0, null);
    }

    private Rect defeatUVRect = new Rect(0, 0, 1, 1);
    /// <summary>
    /// Œ∆¿ÌøΩ±¥
    /// </summary>
    void TextureCopy()
    {
        if (mats == null) return;

        var rt = RenderTexture.GetTemporary(texture.width, texture.height, 0);
        var old = RenderTexture.active;
        Graphics.Blit(texture, rt, mats[0]);
        RenderTexture.active = old;

        Graphics.DrawTexture(screenRects[0], rt, uiRects[0], 0, 0, 0, 0, null);
        Graphics.DrawTexture(screenRects[1], rt, uiRects[1], 0, 0, 0, 0, null);
        Graphics.DrawTexture(screenRects[2], rt, uiRects[2], 0, 0, 0, 0, null);
        Graphics.DrawTexture(screenRects[3], rt, uiRects[3], 0, 0, 0, 0, null);

        RenderTexture.ReleaseTemporary(rt);
    }

    void PrintRT(RenderTexture rt)
    {

    }
}
