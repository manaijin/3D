using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CommandBufferAPI : MonoBehaviour
{
    public Material mat;

    private CommandBuffer buffer1;
    private CommandBuffer buffer2;
    private int porary1;
    private int porary2;

    void Start()
    {
        porary1 = Shader.PropertyToID("tempProperty1");
        porary2 = Shader.PropertyToID("tempProperty2");
    }


    void Update()
    {
        if (buffer1 == null)
        {
            buffer1 = new CommandBuffer() { name = "AfterOpaqueTexture" };
            Camera.main.AddCommandBuffer(CameraEvent.AfterForwardOpaque, buffer1);
        }

        buffer1.Clear();
        buffer1.GetTemporaryRT(porary1, -1, -1, 0, FilterMode.Bilinear);
        buffer1.Blit(BuiltinRenderTextureType.CurrentActive, porary1);
        buffer1.SetGlobalTexture("AfterOpaqueTexture", porary1);
        //buffer1.ReleaseTemporaryRT(porary1);


        if (buffer2 == null)
        {
            buffer2 = new CommandBuffer() { name = "AfterAllTexture" };
            Camera.main.AddCommandBuffer(CameraEvent.AfterImageEffects, buffer2);
        }
        buffer2.Clear();
        buffer2.GetTemporaryRT(porary2, -1, -1, 0, FilterMode.Bilinear);
        buffer2.Blit(BuiltinRenderTextureType.CurrentActive, porary2);
        buffer2.SetGlobalTexture("AfterAllTexture", porary2);
        //buffer2.ReleaseTemporaryRT(porary2);
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
        if (buffer1 != null)
        {
            buffer1.Clear();
            Camera.main.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, buffer1);
            buffer1 = null;
        }

        if (buffer2 != null)
        {
            buffer2.Clear();
            Camera.main.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, buffer2);
            buffer2 = null;
        }
    }
}
