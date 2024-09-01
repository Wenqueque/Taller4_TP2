using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndaCompuesta : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points = 100;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    // Parámetros para las diferentes componentes de la onda
    public float amplitude1 = 1;   // Amplitud de la onda sinusoidal
    public float frequency1 = 2;   // Frecuencia de la onda sinusoidal
    public float phase1 = 0;       // Fase de la onda sinusoidal

    public float amplitude2 = 0.5f;   // Amplitud de la onda cuadrada
    public float frequency2 = 5;      // Frecuencia de la onda cuadrada
    public float phase2 = 0;          // Fase de la onda cuadrada

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

            // Crear una onda sinusoidal
            float ySin = amplitude1 * Mathf.Sin(Tau * frequency1 * x + phase1 + (Time.timeSinceLevelLoad * movementSpeed));

            // Crear una onda cuadrada
            float ySquare = amplitude2 * Mathf.Sign(Mathf.Sin(Tau * frequency2 * x + phase2 + (Time.timeSinceLevelLoad * movementSpeed)));

            // Combinar la onda sinusoidal y la onda cuadrada
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
