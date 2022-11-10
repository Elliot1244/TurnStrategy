using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;


    //Fonctions qui montrent ou cachent les zones de déplacement possible
    public void Show(Material material)
    {
        _meshRenderer.enabled = true;
        _meshRenderer.material = material;
    }

    public void Hide()
    {
        _meshRenderer.enabled = false;
    }
}
