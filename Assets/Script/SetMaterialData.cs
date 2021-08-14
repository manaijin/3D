using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetMaterialData : MonoBehaviour
{
    public Material stencil_mat;
    public Material standard_mat;

    private MeshRenderer mesh;
    private bool is_stencil = true;

    void Start()
    {
        TryGetComponent<MeshRenderer>(out mesh);
    }


    void Update()
    {
        if (!stencil_mat) return;
        if(is_stencil)
            stencil_mat.SetFloat("_Stencil", 125 - transform.position.z);
    }

    public void SetStencilMat()
    {
        if (!mesh) return;
        if (!stencil_mat) return;
        mesh.material = stencil_mat;
        is_stencil = true;
    }

    public void SetStandardMat()
    {
        if (!mesh) return;
        if (!standard_mat) return;
        mesh.material = standard_mat;
        is_stencil = false;
    }
}
