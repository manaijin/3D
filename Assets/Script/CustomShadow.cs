using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomShadow : MonoBehaviour
{
    [Tooltip("“ı”∞Õ∂…‰Shader")]
    public Shader shadowCaster;

    // Start is called before the first frame update
    void Start()
    {
        CreateDirLightCamera();
    }

    private void OnEnable()
    {
        CreateDirLightCamera();
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 projectionMatrix = GL.GetGPUProjectionMatrix(LightCamera.projectionMatrix, false);
        Shader.SetGlobalMatrix("_gWorldToShadow", projectionMatrix * LightCamera.worldToCameraMatrix);
        LightCamera.RenderWithShader(shadowCaster, "");
    }

    private GameObject goLightCamera;
    private Camera LightCamera;
    public Camera CreateDirLightCamera()
    {
        if (goLightCamera != null) return LightCamera;
        goLightCamera = new GameObject("Directional Light Camera");
        goLightCamera.transform.SetParent(transform);
        goLightCamera.transform.localPosition = Vector3.zero;
        goLightCamera.transform.forward = transform.forward;

        LightCamera = goLightCamera.AddComponent<Camera>();
        LightCamera.backgroundColor = Color.white;
        LightCamera.clearFlags = CameraClearFlags.SolidColor;
        LightCamera.orthographic = true;
        LightCamera.orthographicSize = 6f;
        LightCamera.nearClipPlane = 0.3f;
        LightCamera.farClipPlane = 20;

        LightCamera.enabled = false;
        
        if (!LightCamera.targetTexture)
            LightCamera.targetTexture = Create2DTextureFor(LightCamera);

        Shader.SetGlobalFloat("_gShadowBias", 0.005f);
        Shader.SetGlobalFloat("_gShadowStrength", 0.5f);
        Shader.SetGlobalTexture("_gShadowMapTexture", rt_2d);
        return LightCamera;
    }

    RenderTexture rt_2d;

    private RenderTexture Create2DTextureFor(Camera cam)
    {
        RenderTextureFormat rtFormat = RenderTextureFormat.Default;
        if (!SystemInfo.SupportsRenderTextureFormat(rtFormat))
            rtFormat = RenderTextureFormat.Default;

        rt_2d = new RenderTexture(512 , 512 , 24, rtFormat);
        rt_2d.hideFlags = HideFlags.DontSave;

        return rt_2d;
    }
}
