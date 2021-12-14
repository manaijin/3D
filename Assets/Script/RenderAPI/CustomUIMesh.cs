using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CustomUIMesh : Image
{
    [SerializeField]
    public int splitNum = 100;
    [SerializeField]
    public int n = 3;


    private void FixedUpdate()
    {
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Color32 color32 = color;
        vh.Clear();

        Vector2 pCenter = rectTransform.anchoredPosition;
        Vector2 uvCenter = new Vector2(0.5f, 0.5f);
        float r = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height) / 2;
        float d = Mathf.PI * 2 / splitNum;

        vh.AddVert(pCenter, color32, uvCenter);
        for (int i = 0; i < splitNum; i++)
        {
            float sth = d * i;
            float u = Mathf.Cos(n * sth) * Mathf.Cos(sth);
            float v = Mathf.Cos(n * sth) * Mathf.Sin(sth);
            float u2 = Mathf.Cos(Time.unscaledTime) * u - Mathf.Sin(Time.unscaledTime) * v;
            float v2 = Mathf.Sin(Time.unscaledTime) * u + Mathf.Cos(Time.unscaledTime) * v;
            float x = r * u2;
            float y = r * v2;
            vh.AddVert(pCenter + new Vector2(x, y), color32, new Vector2(u2, v2) + uvCenter);
        }

        for (int i = 1; i < splitNum; i++)
        {
            vh.AddTriangle(0, i, i + 1);
        }
    }
}
