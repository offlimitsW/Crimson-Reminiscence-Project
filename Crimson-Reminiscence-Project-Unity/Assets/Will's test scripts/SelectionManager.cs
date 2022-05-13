using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    Material selectedMAT;
    [SerializeField]
    string selectableTag;

    private Material tempMAT;
    private Transform _selection;

    public float interactableRange = 10f;

    void Update()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = tempMAT;
            _selection = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactableRange))
        {
            var selection = hit.transform;
            // Has tag
            if (selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    tempMAT = selectionRenderer.material;
                    selectionRenderer.material = selectedMAT;
                }
                _selection = selection;
            }
            // Or has component
            if (selection.GetComponent<Selectable>() != null)
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    tempMAT = selectionRenderer.material;
                    selectionRenderer.material = selectedMAT;
                }
                _selection = selection;
            }
        }
    }
}
