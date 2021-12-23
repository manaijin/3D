using UnityEngine;
using UnityEngine.UI;

public class GLAPI : MonoBehaviour
{
    [Tooltip("画线材质，默认创建")]
    public Material lineMaterial;

    [Tooltip("面片材质1")]
    public Material lineMaterial1;

    [Tooltip("面片材质2")]
    public Material lineMaterial2;

    public int lineCount = 500;
    public float radius = 10.0f;

    public RawImage img;

    private Color startColor;
    private Color endColor;
    private Color dColor;
    private RenderTexture rt;

    private void OnEnable()
    {
        img.gameObject.SetActive(true);
    }

    private void Start()
    {
        ColorUtility.TryParseHtmlString("#FAD5A9", out startColor);
        ColorUtility.TryParseHtmlString("#ECE8E5", out endColor);
        dColor = (endColor - startColor) / lineCount;
        CreateLineMaterial();
        rt = RenderTexture.GetTemporary((int)img.rectTransform.rect.width, (int)img.rectTransform.rect.height, 0);
        img.texture = rt;
    }

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 0.2f);
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        lineMaterial.SetPass(0);        
        GL.PushMatrix();
        GL.LoadPixelMatrix();
        //GL.LoadPixelMatrix(0, Screen.width, Screen.height, 0);
        GL.Begin(GL.LINES);
        GL.Color(Color.black);
        GL.Vertex3(Screen.width / 2, 0, 0);
        GL.Vertex3(Screen.width / 2, Screen.height, 0);
        GL.Vertex3(0, Screen.height / 2, 0);
        GL.Vertex3(Screen.width, Screen.height / 2, 0);
        GL.End();
        GL.PopMatrix();

        SetViewPort();
        SetMatrix();
        RenderToTexture();
        //DrawCircle();
    }

    private void OnDestroy()
    {
        RenderTexture.ReleaseTemporary(rt);
    }

    private void OnDisable()
    {
        img.gameObject.SetActive(false);
    }

    void SetViewPort()
    {
        GL.Viewport(new Rect(0, Screen.height / 2, Screen.width / 2, Screen.height / 2));
        DrawCLine();
    }

    void SetMatrix()
    {
        // 世界空间
        lineMaterial1.SetPass(0);
        GL.Viewport(new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height / 2));
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);

        GL.Begin(GL.QUADS);
        GL.Color(Color.white);
        GL.TexCoord2(0, 0);
        GL.Vertex3(-quadSize, -quadSize, 0);
        GL.TexCoord2(0, 1);
        GL.Vertex3(-quadSize, quadSize, 0);
        GL.TexCoord2(1, 1);
        GL.Vertex3(quadSize, quadSize, 0);
        GL.TexCoord2(1, 0);
        GL.Vertex3(quadSize, -quadSize, 0);
        GL.End();

        // 背面
        GL.Begin(GL.QUADS);
        lineMaterial2.SetPass(0);
        GL.TexCoord2(1, 0);
        GL.Vertex3(-quadSize, -quadSize, 0);
        GL.TexCoord2(0, 0);
        GL.Vertex3(quadSize, -quadSize, 0);
        GL.TexCoord2(0, 1);
        GL.Vertex3(quadSize, quadSize, 0);
        GL.TexCoord2(1, 1);
        GL.Vertex3(-quadSize, quadSize, 0);

        GL.End();
        GL.PopMatrix();

        // 屏幕空间
        lineMaterial1.SetPass(0);
        GL.Viewport(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 2, Screen.height / 2));
        GL.PushMatrix();
        GL.LoadPixelMatrix();

        GL.Begin(GL.QUADS);

        float d = quadSize * 25;
        GL.Color(Color.white);
        GL.TexCoord2(0, 0);
        GL.Vertex3(Screen.width / 2 - d, Screen.height / 2 - d, 0);
        GL.TexCoord2(0, 1);
        GL.Vertex3(Screen.width / 2 - d, Screen.height / 2 + d, 0);
        GL.TexCoord2(1, 1);
        GL.Vertex3(Screen.width / 2 + d, Screen.height / 2 + d, 0);
        GL.TexCoord2(1, 0);
        GL.Vertex3(Screen.width / 2 + d, Screen.height / 2 - d, 0);
        GL.End();

        GL.PopMatrix();
    }

    void RenderToTexture()
    {
        var org = RenderTexture.active;
        RenderTexture.active = rt;

        GL.Clear(false, true, Color.clear);
        DrawCircle();

        RenderTexture.active = org;
    }

    private float quadSize = 5;

    private void DrawCLine()
    {
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);
        for (int i = 0; i < lineCount; ++i)
        {
            float a = i / (float)lineCount;
            float angle = a * Mathf.PI * 2;
            // Vertex colors change from red to green
            GL.Color(startColor + i * dColor);
            // One vertex at transform position
            GL.Vertex3(0, 0, 0);
            // Another vertex at edge of circle
            GL.Vertex3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        }
        GL.End();
        GL.PopMatrix();
    }

    private void DrawCircle()
    {
        lineMaterial1.SetPass(0);
        GL.PushMatrix();
        GL.LoadOrtho();

        GL.Begin(GL.TRIANGLES);
        GL.Color(Color.white);

        Vector2 centerPos = new Vector2(Screen.width / 2, Screen.height / 2);

        float angleDelta = 2 * Mathf.PI / lineCount;
        for (int i = 0; i <= lineCount; i++)
        {
            float angle = angleDelta * i;
            float angleNext = angle + angleDelta;

            float u = Mathf.Sin(angle) / 2 + 0.5f;
            float x1 = Mathf.Sin(angle) * radius + centerPos.x;
            float v = Mathf.Cos(angle) / 2 + 0.5f;
            float y1 = Mathf.Cos(angle) * radius + centerPos.y;

            float u2 = (Mathf.Sin(angleNext) / 2 + 0.5f);
            float x2 = Mathf.Sin(angleNext) * radius + centerPos.x;
            float v2 = (Mathf.Cos(angleNext) / 2 + 0.5f);
            float y2 = Mathf.Cos(angleNext) * radius + centerPos.y;

            GL.TexCoord2(0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, 0);

            GL.TexCoord2(u, v);
            GL.Vertex3(u, v, 0);

            GL.TexCoord2(u2, v2);
            GL.Vertex3(u2, v2, 0);
        }


        GL.End();
        GL.PopMatrix();
    }

    //private void DrawCircle()
    //{
    //    lineMaterial1.SetPass(0);
    //    GL.PushMatrix();
    //    GL.LoadPixelMatrix();

    //    GL.Begin(GL.TRIANGLES);
    //    GL.Color(Color.white);


    //    //// 矩形
    //    //GL.Vertex3(100, 100, 0);
    //    //GL.Vertex3(200, 200, 0);
    //    //GL.Vertex3(200, 100, 0);

    //    Vector2 centerPos = new Vector2(Screen.width / 2, Screen.height / 2);

    //    float angleDelta = 2 * Mathf.PI / lineCount;
    //    for (int i = 0; i <= lineCount; i++)
    //    {
    //        float angle = angleDelta * i;
    //        float angleNext = angle + angleDelta;

    //        float u = Mathf.Sin(angle) / 2 + 0.5f;
    //        float x1 = Mathf.Sin(angle) * radius + centerPos.x;
    //        float v = Mathf.Cos(angle) / 2 + 0.5f;
    //        float y1 = Mathf.Cos(angle) * radius + centerPos.y;

    //        float u2 = (Mathf.Sin(angleNext) / 2 + 0.5f);
    //        float x2 = Mathf.Sin(angleNext) * radius + centerPos.x;
    //        float v2 = (Mathf.Cos(angleNext) / 2 + 0.5f);
    //        float y2 = Mathf.Cos(angleNext) * radius + centerPos.y;

    //        GL.TexCoord2(0.5f, 0.5f);
    //        GL.Vertex3(centerPos.x, centerPos.y, 0);

    //        GL.TexCoord2(u, v);
    //        GL.Vertex3(x1, y1, 0);

    //        GL.TexCoord2(u2, v2);
    //        GL.Vertex3(x2, y2, 0);
    //    }


    //    GL.End();
    //    GL.PopMatrix();
    //}

    public void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            lineMaterial = Helper.CreateLineMaterial();
        }
    }
}
