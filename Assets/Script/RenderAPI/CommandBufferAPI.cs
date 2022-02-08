using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CommandBufferAPI : MonoBehaviour
{
    public Material MRT;
    public Mesh mesh;

    private CommandBuffer buffer1;
    private CommandBuffer buffer2;
    private CommandBuffer buffer3;
    private CommandBuffer buffer4;

    private RenderTexture r1;
    private RenderTexture r2;
    private RenderTargetIdentifier[] mrt;

    private RenderTexture r3;
    private RenderTexture r4;
    private RenderTargetIdentifier[] mrt2;

    void Start()
    {
        r1 = RenderTexture.GetTemporary((int)Screen.width, (int)Screen.height);
        r2 = RenderTexture.GetTemporary((int)Screen.width, (int)Screen.height);
        mrt = new RenderTargetIdentifier[] { r1.colorBuffer, r2.colorBuffer };

        r3 = RenderTexture.GetTemporary((int)Screen.width, (int)Screen.height);
        r4 = RenderTexture.GetTemporary((int)Screen.width, (int)Screen.height);
        mrt2 = new RenderTargetIdentifier[] { r3.colorBuffer, r4.colorBuffer };

        Shader.SetGlobalTexture("AfterOpaqueTexture", r1);
        Shader.SetGlobalTexture("AfterAllTexture", r2);

        Shader.SetGlobalTexture("AfterOpaqueTexture2", r3);
        Shader.SetGlobalTexture("AfterAllTexture2", r4);

        var mt = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 10);

        buffer1 = new CommandBuffer() { name = "AfterSkybox" };        
        buffer1.Blit(BuiltinRenderTextureType.CameraTarget, r1);
        buffer1.Blit(BuiltinRenderTextureType.CameraTarget, r2);
        buffer1.SetRenderTarget(mrt, r1.depthBuffer);
        buffer1.DrawMesh(mesh, mt, MRT);
        Camera.main.AddCommandBuffer(CameraEvent.AfterSkybox, buffer1);

        buffer2 = new CommandBuffer() { name = "AfterImageEffects" };
        buffer2.Blit(BuiltinRenderTextureType.CameraTarget, r3);
        buffer2.Blit(BuiltinRenderTextureType.CameraTarget, r4);
        buffer2.SetRenderTarget(mrt2, r3.depthBuffer);
        buffer2.DrawMesh(mesh, mt, MRT);
        Camera.main.AddCommandBuffer(CameraEvent.AfterImageEffects, buffer2);
    }


    //void OnRenderImage(RenderTexture source, RenderTexture destination)
    //{
    //    Graphics.Blit(source, destination);
    //    Graphics.Blit(source, r1, MRT);
    //}

    private void OnDisable()
    {
        Clear();
    }

    private void OnDestroy()
    {
        Clear();
    }

    void Clear()
    {
        if (buffer1 != null && Camera.main)
        {
            buffer1.Clear();
            Camera.main.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, buffer1);
            buffer1 = null;
        }

        if (buffer2 != null && Camera.main)
        {
            buffer2.Clear();
            Camera.main.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, buffer2);
            buffer2 = null;
        }

        if (r1)
        {
            RenderTexture.ReleaseTemporary(r1);
            r1 = null;
        }

        if (r2)
        {
            RenderTexture.ReleaseTemporary(r2);
            r2 = null;
        }

        if (r3)
        {
            RenderTexture.ReleaseTemporary(r3);
            r3 = null;
        }

        if (r4)
        {
            RenderTexture.ReleaseTemporary(r4);
            r4 = null;
        }
    }
}
