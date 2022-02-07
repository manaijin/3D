using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CommandBufferAPI : MonoBehaviour
{
    public Material pureColor;

    private CommandBuffer buffer1;
    private CommandBuffer buffer2;
    private CommandBuffer buffer3;
    private CommandBuffer buffer4;
    private int pro1;
    private int pro2;
    private int pro3;

    private RenderTexture r1;
    private RenderTexture r2;

    void Start()
    {
        pro1 = Shader.PropertyToID("tempProperty1");
        pro2 = Shader.PropertyToID("tempProperty20");
        pro3 = Shader.PropertyToID("tempProperty30");

        buffer1 = new CommandBuffer() { name = "AfterSkybox" };
        Camera.main.AddCommandBuffer(CameraEvent.AfterSkybox, buffer1);

        buffer2 = new CommandBuffer() { name = "AfterImageEffects" };
        Camera.main.AddCommandBuffer(CameraEvent.AfterImageEffects, buffer2);

        //buffer3 = new CommandBuffer() { name = "BeforeSkybox" };
        //Camera.main.AddCommandBuffer(CameraEvent.BeforeSkybox, buffer3);

        //buffer4 = new CommandBuffer() { name = "BeforeImageEffects" };
        //Camera.main.AddCommandBuffer(CameraEvent.BeforeImageEffects, buffer4);

        r1 = RenderTexture.GetTemporary((int)Screen.width, (int)Screen.height);
        r2 = RenderTexture.GetTemporary((int)Screen.width, (int)Screen.height);

        Shader.SetGlobalTexture("AfterOpaqueTexture", r1);
        Shader.SetGlobalTexture("AfterAllTexture", r2);
    }

    void Update()
    {
        buffer1.Clear();
        buffer2.Clear();
        //buffer3.Clear();
        //buffer4.Clear();

        //buffer3.Blit(null, r1, pureColor);
        buffer1.Blit(BuiltinRenderTextureType.CurrentActive, r1);

        //buffer4.Blit(null, r2, pureColor);
        buffer2.Blit(BuiltinRenderTextureType.CurrentActive, r2);
    }


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
    }
}
