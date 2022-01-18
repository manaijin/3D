using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GraphicsAPI : MonoBehaviour
{
    public Texture texture;

    public Shader[] shaders;
    private List<Material> mats;

    private Rect[] uiRects;
    private Rect[] screenRects;

    public Mesh mesh;
    public Material[] meshMat;
    private Vector3[] worldPos = new Vector3[100];
    private Matrix4x4[] worldMats = new Matrix4x4[100];
    public bool useGPU = false;

    public GameObject UINode;

    void Start()
    {
        DataInit();
        // GraphicsStaticObject();
    }

    private void OnEnable()
    {
        if (UINode) UINode.SetActive(true);
    }

    void Update()
    {       
        if (useGPU)
        {
            Graphics.DrawMeshInstanced(mesh, 0, meshMat[0], worldMats, worldMats.Length);
            Graphics.DrawMeshInstanced(mesh, 1, meshMat[1], worldMats, worldMats.Length);
            Graphics.DrawMeshInstanced(mesh, 2, meshMat[2], worldMats, worldMats.Length);
        }
        else
        {
            foreach (var pos in worldPos)
            {
                Graphics.DrawMesh(mesh, pos, Quaternion.identity, meshMat[0], 0, Camera.main, 0);
                Graphics.DrawMesh(mesh, pos, Quaternion.identity, meshMat[1], 0, Camera.main, 1);
                Graphics.DrawMesh(mesh, pos, Quaternion.identity, meshMat[2], 0, Camera.main, 2);
            }
        }
    }

    void OnGUI()
    {
        if (Event.current.type.Equals(EventType.Repaint))
        {
            // TextureToScreen();
            // TextureCopy();
            ShowDifferentResolution();
        }
    }

    private void OnDisable()
    {
        if (UINode != null)
            UINode.SetActive(false);
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

        for (int i = 0; i < worldPos.Length; i++)
        {
            worldPos[i] = new Vector3(UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f));
            worldMats[i] = Matrix4x4.identity;
            worldMats[i][0, 3] = worldPos[i].x;
            worldMats[i][1, 3] = worldPos[i].y;
            worldMats[i][2, 3] = worldPos[i].z;
            worldMats[i][3, 3] = 1;
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
    /// 渲染纹理到屏幕
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
    /// 纹理拷贝
    /// </summary>
    void TextureCopy()
    {
        if (mats == null) return;

        var rt = RenderTexture.GetTemporary(texture.width, texture.height, 0);

        Graphics.Blit(texture, rt, mats[0]);
        RenderTexture.active = null;
        Graphics.DrawTexture(screenRects[0], rt, uiRects[0], 0, 0, 0, 0, null);

        Graphics.Blit(texture, rt, mats[1]);
        RenderTexture.active = null;
        Graphics.DrawTexture(screenRects[1], rt, uiRects[1], 0, 0, 0, 0, null);

        Graphics.Blit(texture, rt, mats[2]);
        RenderTexture.active = null;
        Graphics.DrawTexture(screenRects[2], rt, uiRects[2], 0, 0, 0, 0, null);

        Graphics.Blit(texture, rt, mats[3]);
        RenderTexture.active = null;
        Graphics.DrawTexture(screenRects[3], rt, uiRects[3], 0, 0, 0, 0, null);

        RenderTexture.ReleaseTemporary(rt);
    }

    private Texture2D tempTexture2D;
    private Texture2D tempTexture2D2;
    void ShowDifferentResolution()
    {
        if (tempTexture2D == null || tempTexture2D2 == null) return;
        Graphics.DrawTexture(screenRects[0], tempTexture2D, defeatUVRect, 0, 0, 0, 0, null);
        Graphics.DrawTexture(screenRects[1], tempTexture2D2, defeatUVRect, 0, 0, 0, 0, null);
    }

    public void ScreenShot()
    {
        var rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        tempTexture2D = new Texture2D(Screen.width, Screen.height);
        tempTexture2D2 = new Texture2D(100, 100);

        // Cmaera渲染RT
        Camera.main.targetTexture = rt;
        Camera.main.Render();

        // 读取RT数据
        RenderTexture.active = rt;
        tempTexture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tempTexture2D.Apply();
        ChangeTextureSize(rt, tempTexture2D2);

        RenderTexture.active = null;
        Camera.main.targetTexture = null;

        // 保存纹理数据为png
        var imageData = tempTexture2D.EncodeToPNG();
        var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        FileStream file = File.Create(path + "\\1.png");
        file.Write(imageData, 0, imageData.Length);
        file.Close();

        var imageData2 = tempTexture2D2.EncodeToPNG();
        FileStream file2 = File.Create(path + "\\2.png");
        file2.Write(imageData2, 0, imageData2.Length);
        file2.Close();

        RenderTexture.ReleaseTemporary(rt);
    }

    private void ChangeTextureSize(RenderTexture source, Texture2D target)
    {
        var rt = RenderTexture.GetTemporary(target.width, target.height);
        var old = RenderTexture.active;
        Graphics.Blit(source, rt);
        target.ReadPixels(new Rect(0, 0, target.width, target.height), 0, 0);
        target.Apply();
        RenderTexture.active = old;
        RenderTexture.ReleaseTemporary(rt);
    }
}
