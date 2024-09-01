using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AparicionLineaMediacion : MonoBehaviour
{
    public LineRenderer line1; // Primera línea visible al principio
    public LineRenderer line2; // Segunda línea visible al principio
    public LineRenderer hiddenLineRenderer; // Línea que empieza oculta

    private Color originalColor1;
    private Color originalColor2;
    private Color whiteColor = Color.white;

    // Definir un rango cercano a cero
    public float zeroThreshold = 0.1f;

    void Start()
    {
        if (line1 == null || line2 == null || hiddenLineRenderer == null)
        {
            Debug.LogError("Faltan asignaciones en las variables públicas.");
            return;
        }

        // Guardar los colores originales
        originalColor1 = line1.startColor;
        originalColor2 = line2.startColor;

        hiddenLineRenderer.gameObject.SetActive(false); // Inicialmente está oculta
    }

    void Update()
    {
        float rotation = Input.acceleration.x; // Ajustar según el eje deseado

        if (Mathf.Abs(rotation) <= zeroThreshold)
        {
            // Si estamos en el eje 0, cambiar color y manejar la visibilidad
            line1.startColor = whiteColor;
            line1.endColor = whiteColor;
            line2.startColor = whiteColor;
            line2.endColor = whiteColor;

            // Detectar la presión de pantalla para mostrar la línea oculta
            if (Input.GetMouseButtonDown(0))
            {
                line1.gameObject.SetActive(false); // Oculta la primera línea
                line2.gameObject.SetActive(false); // Oculta la segunda línea
                hiddenLineRenderer.gameObject.SetActive(true); // Muestra la línea oculta
            }
        }
        else
        {
            // Restaurar colores si no estamos en el eje 0
            line1.startColor = originalColor1;
            line1.endColor = originalColor1;
            line2.startColor = originalColor2;
            line2.endColor = originalColor2;
        }
    }
}