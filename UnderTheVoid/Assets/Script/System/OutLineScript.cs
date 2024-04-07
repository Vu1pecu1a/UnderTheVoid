using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutLineScript : MonoBehaviour
{
    /*
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineScaleFactor;
    [SerializeField] private Color outlineColor;
    private Renderer outlineRenderer;

    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
        outlineRenderer.enabled = true;
    }

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        Renderer rend = outlineObject.GetComponent<Renderer>();

        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("_Scale", scaleFactor);
        rend.shadowCastingMode = ShadowCastingMode.Off;

        outlineObject.GetComponent<OutLineScript>().enabled = false;
        outlineObject.GetComponent<Collider>().enabled = false;

        rend.enabled = false;

        return rend;
    }*/
    Material outline;

    Renderer renderers;
    List<Material> materialList = new List<Material>();

    private void OnMouseDown()
    {
        renderers = this.GetComponent<Renderer>();

        materialList.Clear();
        materialList.AddRange(renderers.sharedMaterials);
        materialList.Add(outline);

        renderers.materials = materialList.ToArray();
    }

    private void OnMouseUp()

    {
        Renderer renderer = this.GetComponent<Renderer>();

        materialList.Clear();
        materialList.AddRange(renderer.sharedMaterials);
        materialList.Remove(outline);

        renderer.materials = materialList.ToArray();
    }

    void Start()
    {
        outline = new Material(Shader.Find("Unlit/OutlineShader"));
    }
}