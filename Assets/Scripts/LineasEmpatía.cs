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

    // Parámetros para el ajuste basado en la rotación
    public float maxAmplitude0 = 1;
    public float maxFrequency0 = 1;
    public float maxMovementSpeed0 = 1;
    public int points0 = 50;

    public float maxAmplitude90 = 5;
    public float maxFrequency90 = 5;
    public float maxMovementSpeed90 = 5;
    public int points90 = 100;

    public float parameterChangeSpeed = 1;
    public float minFrequency = 0.1f; // Frecuencia mínima para el arrastre
    public float maxFrequency = 10f; // Frecuencia máxima para el arrastre
    public float frequencyDragMultiplier = 10f; // Multiplicador para ajustar la frecuencia con el arrastre

    private EdgeCollider2D edgeCollider;
    private List<Vector2> linePoints = new List<Vector2>();
    private bool isCommonSineWave = false; // Para detectar si la línea ha cambiado
    private bool isDragging = false; // Para detectar si se está arrastrando

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
            float y = amplitude * Mathf.Sin(Tau * frequency * x + (Time.timeSinceLevelLoad * movementSpeed));

            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
            linePoints.Add(new Vector2(x, y));
        }

        // Actualizar el EdgeCollider2D
        edgeCollider.SetPoints(linePoints);
    }

    void Update()
    {
        // Cambiar a una línea sinusoidal común cuando se hace clic
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            isCommonSineWave = true;
            isDragging = true;
        }

        // Ajustar frecuencia con arrastre
        if (isDragging)
        {
            Vector2 touchPosition = Input.mousePosition;
            float normalizedX = Mathf.InverseLerp(0, Screen.width, touchPosition.x);
            float newFrequency = Mathf.Lerp(minFrequency, maxFrequency, normalizedX);

            // Aplicar el nuevo valor de frecuencia solo si es la onda sinusoidal
            if (isCommonSineWave)
            {
                frequency = newFrequency * frequencyDragMultiplier;
            }
        }

        // Cambiar el estado de arrastre cuando se deja de tocar la pantalla
        if (Input.GetMouseButtonUp(0) || Input.touchCount == 0)
        {
            isDragging = false;
        }

        // Obtener la rotación del dispositivo
        float rotation = Mathf.Clamp01(Mathf.Abs(Input.acceleration.x)); // Ajustar según el eje deseado, clamping entre 0 y 1

        // Interpolar los valores objetivo entre los configurados para 0 grados y 90 grados
        float targetAmplitude = Mathf.Lerp(maxAmplitude0, maxAmplitude90, rotation);
        float targetFrequency = Mathf.Lerp(maxFrequency0, maxFrequency90, rotation);
        float targetMovementSpeed = Mathf.Lerp(maxMovementSpeed0, maxMovementSpeed90, rotation);
        int targetPoints = Mathf.RoundToInt(Mathf.Lerp(points0, points90, rotation));

        // Suavizar la transición a los valores objetivo
        amplitude = Mathf.Lerp(amplitude, targetAmplitude, Time.deltaTime * parameterChangeSpeed);
        movementSpeed = Mathf.Lerp(movementSpeed, targetMovementSpeed, Time.deltaTime * parameterChangeSpeed);
        points = Mathf.RoundToInt(Mathf.Lerp(points, targetPoints, Time.deltaTime * parameterChangeSpeed));

        // Dibujar la línea
        if (isCommonSineWave)
        {
            DrawSineWave();
        }
        else
        {
            DrawSquareWave();
        }
    }
}
