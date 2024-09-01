using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndaCompuestaMediacion : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points = 100;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    // Parámetros para las diferentes componentes de la onda
    public float amplitude1 = 1f;     // Amplitud de la onda sinusoidal principal
    public float frequency1 = 1f;     // Frecuencia de la onda sinusoidal principal

    public float amplitude2 = 0.5f;   // Amplitud de la onda cuadrada
    public float frequency2 = 5f;     // Frecuencia de la onda cuadrada
    public float squareWavePeriod = 0.25f; // Proporción de la onda que se asemeja a una onda cuadrada

    private EdgeCollider2D edgeCollider;
    private List<Vector2> linePoints = new List<Vector2>();

    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();

        // Añadir y configurar el EdgeCollider2D
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.edgeRadius = 0.1f;

        // Dibujar la línea inicialmente
        Draw();
    }

    void Draw()
    {
        float xStart = xLimits.x;
        float Tau = 2 * Mathf.PI;
        float xFinish = xLimits.y;

        myLineRenderer.positionCount = points;
        linePoints.Clear();

        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);

            // Onda sinusoidal principal (larga)
            float ySin = amplitude1 * Mathf.Sin(Tau * frequency1 * x + (Time.timeSinceLevelLoad * movementSpeed));

            // Patrón cuadrado
            float ySquare = 0f;
            if (progress % squareWavePeriod < squareWavePeriod / 2)
            {
                ySquare = amplitude2 * Mathf.Sin(Tau * frequency2 * x + (Time.timeSinceLevelLoad * movementSpeed));
            }

            // Combinación de la onda larga y la onda cuadrada
            float y = ySin + ySquare;

            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
            linePoints.Add(new Vector2(x, y));
        }

        // Actualizar el EdgeCollider2D
        edgeCollider.SetPoints(linePoints);
    }

    void Update()
    {
        Draw();
    }
}
