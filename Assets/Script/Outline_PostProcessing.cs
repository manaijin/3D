using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 1. 通过子摄像机拿到需要边缘处理的纯色纹理
/// 2. 对纯色纹理进行多个方向上的高斯模糊
/// 3. 将两个纹理进行相减，获得边缘纹理
/// 4. 将边缘纹理叠加到主摄像机的纹理中
/// </summary>
[ExecuteInEditMode]
public class Outline_PostProcessing : MonoBehaviour
{
    public Camera sub_cam;    
    public Material gaussian_mat;
    public Shader purecolor_shader;
    public float blurSpread = 0.6f;

    private RenderTexture rt;

    void Awake()
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
    }

    void OnPreRender()
    {
        if (!sub_cam && !sub_cam.enabled) return;
        sub_cam.targetTexture = rt;
        sub_cam.RenderWithShader(purecolor_shader, "");
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int rtW = source.width;
        int rtH = source.height;
        var temp1 = RenderTexture.GetTemporary(rtW, rtH, 0);
        var temp2 = RenderTexture.GetTemporary(rtW, rtH, 0);

        // 高斯模糊处理
        Graphics.Blit(rt, temp1);
        for (int i = 0; i < 4; i++)
        {
            gaussian_mat.SetFloat("_BlurSize", 1.0f + i * blurSpread);
            Graphics.Blit(temp1, temp2, gaussian_mat, 0);
            Graphics.Blit(temp2, temp1, gaussian_mat, 1);
        }

        //用模糊图和原始图计算出轮廓图
        gaussian_mat.SetColor("_OutlineColor", Color.red);
        gaussian_mat.SetTexture("_BlurTex", temp1);
        gaussian_mat.SetTexture("_SrcTex", rt);
        Graphics.Blit(source, destination, gaussian_mat, 2);

        RenderTexture.ReleaseTemporary(temp1);
        RenderTexture.ReleaseTemporary(temp2);
    }


    private void OnDestroy()
    {
        ClearRenderTexture();
    }
}
