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

    // Variables para ajustar la frecuencia con el arrastre
    public float minFrequency = 0.1f;
    public float maxFrequency = 5f;
    public float frequencyDragMultiplier = 10f;

    // Variables para ajustar la amplitud
    public float minAmplitude = 0.1f;
    public float maxAmplitude = 5f;
    public float amplitudeChangeSpeed = 1f; // Velocidad del cambio de amplitud

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
        }

        if (isCommonSineWave)
        {
            DrawSineWave();
        }
        else
        {
            DrawSquareWave();
        }

        // Ajustar frecuencia con arrastre
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Vector2 touchPosition = Input.mousePosition;
            // Convertir la posición del ratón o toque a la posición de la pantalla
            float normalizedX = Mathf.InverseLerp(0, Screen.width, touchPosition.x);
            float newFrequency = Mathf.Lerp(minFrequency, maxFrequency, normalizedX);

            // Aplicar el nuevo valor de frecuencia solo si es la onda sinusoidal
            if (isCommonSineWave)
            {
                frequency = newFrequency * frequencyDragMultiplier;
            }
        }

        // Leer valores del giroscopio para ajustar la amplitud
        float gyroRotationY = Input.gyro.rotationRateUnbiased.y; // Obtener la rotación del giroscopio en el eje Y

        // Convertir la rotación del giroscopio en un valor entre 0 y 1 (para interpolar entre min y max amplitud)
        float normalizedRotation = Mathf.InverseLerp(-0.5f, 0.5f, gyroRotationY); // Ajustar el rango según sea necesario
        amplitude = Mathf.Lerp(minAmplitude, maxAmplitude, normalizedRotation);
    }
}
