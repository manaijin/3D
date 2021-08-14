using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 使用卷积算子，提取边缘纹理
/// </summary>
[ExecuteInEditMode]
public class Outline_Sobel : MonoBehaviour
{
    public Material sobel_mat;

    public Material blend_img_mat;


    public Color edge_color = Color.black;

    public Camera sub_cam;

    public RenderTexture T;

    private RenderTexture rt;

    void Start()
    {
        if (!sub_cam) return;
        CreatePurecolorRenderTexture();
    }

    private void ClearRenderTexture()
    {
        if (rt)
        {
            RenderTexture.ReleaseTemporary(rt);
            rt = null;
        }
    }

    private void CreatePurecolorRenderTexture()
    {
        sub_cam.cullingMask = 1 << LayerMask.NameToLayer("Player");
        int width = sub_cam.pixelWidth;
        int height = sub_cam.pixelHeight;
        ClearRenderTexture();
        rt = RenderTexture.GetTemporary(width, height, 0);
        rt.filterMode = FilterMode.Bilinear;
        rt.antiAliasing = 4;
    }

    // void OnPreRender()
    // {
    //     if (!sub_cam && !sub_cam.enabled) return;
    //     sub_cam.targetTexture = rt;
    //     sub_cam.Render();
    // }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //if (!sub_cam) return;
        if (!blend_img_mat) return;
        sobel_mat.SetColor("_EdgeColor", edge_color);
        Graphics.Blit(source, destination, sobel_mat);

        //var temp1 = RenderTexture.GetTemporary(rt.width, rt.height, 0);
        //Graphics.Blit(rt, destination, sobel_mat, 0);
        // blend_img_mat.SetTexture("_BgTex", source);
        // Graphics.Blit(T, destination, blend_img_mat, 0);
        //RenderTexture.ReleaseTemporary(temp1);
    }

    private void OnDestroy()
    {
        ClearRenderTexture();
    }
}
