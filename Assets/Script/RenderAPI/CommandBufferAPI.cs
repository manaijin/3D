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

    void OnEnable()
    {
        buffer1 = new CommandBuffer();
        buffer1.name = "111111";
        Camera.main.AddCommandBuffer(CameraEvent.AfterForwardOpaque, buffer1);
    }


    void Update()
    {
        porary1 = Shader.PropertyToID("tempProperty1");
        porary2 = Shader.PropertyToID("tempProperty2");
        if (buffer1 == null) return;
        buffer1.Clear();
        buffer1.GetTemporaryRT(porary1, -1, -1, 0, FilterMode.Bilinear);
        buffer1.Blit(BuiltinRenderTextureType.CurrentActive, porary1);

        int blurredID = Shader.PropertyToID("_Temp1");
        buffer1.GetTemporaryRT(blurredID, -2, -2, 0, FilterMode.Bilinear);
        buffer1.Blit(porary1, blurredID);
        buffer1.ReleaseTemporaryRT(porary1);
        buffer1.SetGlobalTexture("AfterOpaqueTexture", blurredID);
    }

    public void OnDisable()
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
            Camera.main.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, buffer1);
            buffer1.Clear();
            buffer1 = null;
        }

        if (buffer2 != null && Camera.main)
        {            
            Camera.main.RemoveCommandBuffer(CameraEvent.AfterImageEffects, buffer2);
            buffer2.Clear();
            buffer2 = null;
        }
    }
}
