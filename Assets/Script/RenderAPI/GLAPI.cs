using UnityEngine;
using UnityEngine.UI;

public class GLAPI : MonoBehaviour
{
    [Tooltip("画线材质，默认创建")]
    public Material lineMaterial;

    public int lineCount = 100;
    public float radius = .0f;

    public RawImage img;

    private Color startColor;
    private Color endColor;
    private Color dColor;
    private RenderTexture rt;

    private void Start()
    {
        ColorUtility.TryParseHtmlString("#FAD5A9", out startColor);
        ColorUtility.TryParseHtmlString("#ECE8E5", out endColor);
        dColor = (endColor - startColor) / lineCount;
        CreateLineMaterial();
        rt = RenderTexture.GetTemporary((int)img.rectTransform.rect.width, (int)img.rectTransform.rect.height, 0);
        img.texture = rt;
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        var org = RenderTexture.active;
        RenderTexture.active = rt;
        lineMaterial.SetPass(0);

        GL.Clear(true, true, Color.clear);
        GL.PushMatrix();
        //GL.MultMatrix(transform.localToWorldMatrix);        

        // DrawCLine();
        // DrawCircle();
        // DrawTriangle();
        // DrawQuad();
        // DrawCircleSurface();
        // DrawCube();

        GL.LoadOrtho();
        GL.Color(Color.red);
        GL.Begin(GL.TRIANGLES);
        GL.Vertex3(0f, 0f, 0);
        GL.Vertex3(0.5f, 0.5f, 0);
        GL.Vertex3(0.5f, 0f, 0);
        GL.End();

        GL.PopMatrix();
        RenderTexture.active = org;
    }

    private void OnDestroy()
    {
        RenderTexture.ReleaseTemporary(rt);
    }

    private void DrawCube()
    {
        GL.Begin(GL.QUADS);

        GL.Color(Color.blue);
        GL.Vertex3(-quadSize, -quadSize, -quadSize);
        GL.Vertex3(quadSize, -quadSize, -quadSize);
        GL.Vertex3(quadSize, quadSize, -quadSize);
        GL.Vertex3(-quadSize, quadSize, -quadSize);

        GL.Color(Color.yellow);
        GL.Vertex3(-quadSize, -quadSize, quadSize);
        GL.Vertex3(quadSize, -quadSize, quadSize);
        GL.Vertex3(quadSize, quadSize, quadSize);
        GL.Vertex3(-quadSize, quadSize, quadSize);

        GL.Color(Color.red);
        GL.Vertex3(quadSize, -quadSize, -quadSize);
        GL.Vertex3(quadSize, -quadSize, quadSize);
        GL.Vertex3(quadSize, quadSize, quadSize);
        GL.Vertex3(quadSize, quadSize, -quadSize);

        GL.Color(Color.green);
        GL.Vertex3(-quadSize, -quadSize, -quadSize);
        GL.Vertex3(-quadSize, -quadSize, quadSize);
        GL.Vertex3(-quadSize, quadSize, quadSize);
        GL.Vertex3(-quadSize, quadSize, -quadSize);

        GL.Color(Color.cyan);
        GL.Vertex3(-quadSize, -quadSize, -quadSize);
        GL.Vertex3(quadSize, -quadSize, quadSize);
        GL.Vertex3(quadSize, -quadSize, quadSize);
        GL.Vertex3(-quadSize, -quadSize, quadSize);

        GL.Color(Color.magenta);
        GL.Vertex3(-quadSize, quadSize, -quadSize);
        GL.Vertex3(quadSize, quadSize, -quadSize);
        GL.Vertex3(quadSize, quadSize, quadSize);
        GL.Vertex3(-quadSize, quadSize, quadSize);

        GL.End();
    }

    private void DrawCircleSurface()
    {
        float angleDelta = 2 * Mathf.PI / lineCount;

        GL.Begin(GL.TRIANGLES);
        GL.Color(Color.yellow);

        for (int i = 0; i < lineCount; i++)
        {
            float angle = angleDelta * i;
            float angleNext = angle + angleDelta;

            GL.Vertex3(0, 0, 0);
            GL.Vertex3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            GL.Vertex3(Mathf.Cos(angleNext) * radius, Mathf.Sin(angleNext) * radius, 0);
        }

        GL.End();
    }

    private float quadSize = 10;
    private void DrawQuad()
    {
        GL.Begin(GL.QUADS);

        GL.Color(Color.blue);
        GL.Vertex3(-quadSize, -quadSize, 0);
        GL.Color(Color.yellow);
        GL.Vertex3(quadSize, -quadSize, 0);
        GL.Color(Color.red);
        GL.Vertex3(quadSize, quadSize, 0);
        GL.Color(Color.cyan);
        GL.Vertex3(-quadSize, quadSize, 0);

        GL.End();
    }

    private float triangleSize = 20;
    private void DrawTriangle()
    {
        GL.Begin(GL.TRIANGLES);
        GL.Color(Color.green);

        GL.Vertex3(-triangleSize, -triangleSize, 0);
        GL.Color(Color.black);
        GL.Vertex3(triangleSize, -triangleSize, 0);
        GL.Color(Color.red);
        GL.Vertex3(0, triangleSize, 0);

        GL.End();
    }

    private void DrawCLine()
    {
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
            GL.Vertex3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        }
        GL.End();
    }

    private void DrawCircle()
    {
        float angleDelta = 2 * Mathf.PI / lineCount;

        GL.Begin(GL.LINES);
        GL.Color(Color.red);

        for (int i = 0; i < lineCount; i++)
        {
            float angle = angleDelta * i;
            float angleNext = angle + angleDelta;

            GL.Vertex3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            GL.Vertex3(Mathf.Cos(angleNext) * radius, Mathf.Sin(angleNext) * radius, 0);
        }

        GL.End();
    }

    public void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            lineMaterial = Helper.CreateLineMaterial();
        }
    }
}
