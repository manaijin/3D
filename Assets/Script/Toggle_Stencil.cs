using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class Toggle_Stencil : MonoBehaviour
{
    public SetMaterialData[] datas;
    private Toggle toggle;
    
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out toggle);
        toggle.onValueChanged.AddListener(SetToggle);
    }

    void SetToggle(bool t) 
    {
        if (datas == null) return;
        foreach (var item in datas)
        {
            if (t)
                item.SetStencilMat();
            else
                item.SetStandardMat();
        }
    }
}
