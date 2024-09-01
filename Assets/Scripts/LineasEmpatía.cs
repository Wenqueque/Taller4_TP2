using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineasEmpatía : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points = 100;
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    private EdgeCollider2D edgeCollider;
    private List<Vector2> linePoints = new List<Vector2>();
    private bool isCommonSineWave = false; // Para detectar si la línea ha cambiado

    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();

        // Añadir y configurar el EdgeCollider2D
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.edgeRadius = 0.1f; // Ajusta el tamaño del colisionador según sea necesario

        // Dibujar la línea inicialmente como una onda cuadrada
        DrawSquareWave();
    }

    void DrawSquareWave()
    {
        float xStart = xLimits.x;
        float xFinish = xLimits.y;

        myLineRenderer.positionCount = points;
        linePoints.Clear();

        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = amplitude * Mathf.Sign(Mathf.Sin(2 * Mathf.PI * frequency * x + (Time.timeSinceLevelLoad * movementSpeed)));

            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
            linePoints.Add(new Vector2(x, y));
        }

        // Actualizar el EdgeCollider2D
        edgeCollider.SetPoints(linePoints);
    }

    void DrawSineWave()
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
            float y = amplitude * Mathf.Sin(Tau * frequency * x);

            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
            linePoints.Add(new Vector2(x, y));
        }

        // Actualizar el EdgeCollider2D
        edgeCollider.SetPoints(linePoints);
    }

    void Update()
    {
        if (isCommonSineWave)
        {
            DrawSineWave();
        }
        else
        {
            DrawSquareWave();
        }

        // Cambiar a una línea sinusoidal común cuando se hace clic o se toca
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            isCommonSineWave = true;
        }
    }
}
