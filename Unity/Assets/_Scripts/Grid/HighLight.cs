using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighLight : MonoBehaviour {
    private List<Material> defaultMaterials = new List<Material>();
	public Material highlightMaterial, highlightMouseOver;
    public bool highlight = false;
    public bool mousover  = false;
    public Grid grid;
	public GameObject child1, child2;

	private List<Material> 	replaceHighlights = new List<Material>(),
							replaceHighLightOvers = new List<Material>();

    void Start()
    {
		List<MeshRenderer> renderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
        grid = GameObject.Find("GridCreator").GetComponent<Grid>();
        if (grid.editing == false)
        {
            foreach (MeshRenderer meshie in renderers)
            {
                defaultMaterials.Add(meshie.materials[0]);
            }

            if (highlightMaterial != null)
            {
                foreach (MeshRenderer meshie in renderers)
                {
                    Material replaceHighlight = new Material(highlightMaterial);
                    replaceHighlight.SetTexture("_MainTex", defaultMaterials[renderers.IndexOf(meshie)].GetTexture("_MainTex"));
                    replaceHighlights.Add(replaceHighlight);

                    Material replaceHightlightOver = new Material(highlightMouseOver);
                    replaceHightlightOver.SetTexture("_MainTex", defaultMaterials[renderers.IndexOf(meshie)].GetTexture("_MainTex"));
                    replaceHighLightOvers.Add(replaceHightlightOver);
                }
            }
        }
    }
    void Update()
    {
        if (grid.editing == false)
        {
            if (!highlight)
            {
                
                if (gameObject.name != "EmptyHex(Clone)" && defaultMaterials.Count > 1)
                {
                    if (defaultMaterials[0] != null && defaultMaterials[1] != null)
                    {
                        if (child1 != null)
                        {
                            child1.renderer.material = defaultMaterials[0];
                        }
                        if (child2 != null)
                        {
                            child2.renderer.material = defaultMaterials[1];
                        }
                    }                
                   
                }
                else if (gameObject.name != "EmptyHex(Clone)" && defaultMaterials.Count == 1)
                {
                    if (child1 != null)
                    {
                        child1.renderer.material = defaultMaterials[0];
                    }
                }
             

            }
            else
            {
                if (!mousover)
                {
                    if (child1 != null)
                    {
                        child1.renderer.material = replaceHighlights[0];
                    }
                    if (child2 != null)
                    {
                        child2.renderer.material = replaceHighlights[1];
                    }
                }
                else
                {
                    if (child1 != null)
                    {
                        child1.renderer.material = replaceHighLightOvers[0];

                    }
                    if (child2 != null)
                    {
                        child2.renderer.material = replaceHighLightOvers[1];
                    }
                }
            }
        }
    }
}
