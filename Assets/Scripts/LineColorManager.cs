using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineColorManager : MonoBehaviour
{
    private LineRenderer[] lineRenderers;
    private Color[] originalColors;
    private Color whiteColor = Color.white;

    // Definir un rango cercano a cero
    public float zeroThreshold = 0.1f;

    void Start()
    {
        lineRenderers = FindObjectsOfType<LineRenderer>();
        originalColors = new Color[lineRenderers.Length];

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            if (lineRenderers[i] != null)
            {
                originalColors[i] = lineRenderers[i].startColor; // Asumiendo que los colores de inicio y fin son los mismos
            }
        }
    }

    void Update()
    {
        float rotation = Input.acceleration.x; // Ajustar según el eje deseado

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            if (Mathf.Abs(rotation) <= zeroThreshold)
            {
                lineRenderers[i].startColor = whiteColor;
                lineRenderers[i].endColor = whiteColor;
            }
            else
            {
                lineRenderers[i].startColor = originalColors[i];
                lineRenderers[i].endColor = originalColors[i];
            }
        }
    }
}
