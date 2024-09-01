using System.Collections.Generic;
using UnityEngine;

public class GrosorLineaProteccion : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public float increaseAmount = 0.1f; // Cantidad que aumentará el grosor

    public int points;
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    public float maxAmplitude0 = 2;
    public float maxFrequency0 = 2;
    public float maxMovementSpeed0 = 2;
    public int points0 = 50;

    public float maxAmplitude90 = 1;
    public float maxFrequency90 = 1;
    public float maxMovementSpeed90 = 1;
    public int points90 = 100;

    // Nuevos parámetros para cuando está en 90 grados y hasIncreased es verdadero
    public float additionalMaxAmplitude90 = 3;
    public float additionalMaxFrequency90 = 3;
    public float additionalMaxMovementSpeed90 = 3;
    public int additionalPoints90 = 150;

    public float parameterChangeSpeed = 1;

    private EdgeCollider2D edgeCollider;
    private List<Vector2> linePoints = new List<Vector2>();

    private bool hasIncreased = false; // Para asegurarnos de que solo aumente una vez
    private bool isDragging = false; // Para saber si se está arrastrando la línea

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
            float y = amplitude * Mathf.Sin((Tau * frequency * x) + (Time.timeSinceLevelLoad * movementSpeed));
            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
            linePoints.Add(new Vector2(x, y));
        }

        // Actualizar el EdgeCollider2D
        edgeCollider.SetPoints(linePoints);
    }

    void Update()
    {
        // Si se presiona el botón del ratón y no ha aumentado antes
        if (Input.GetMouseButton(0) && !hasIncreased)
        {
            // Aumentar el grosor de la línea
            myLineRenderer.startWidth += increaseAmount;
            myLineRenderer.endWidth += increaseAmount;

            // Marcar que el grosor ya ha sido aumentado
            hasIncreased = true;
        }

        // Detectar si se está arrastrando la línea
        isDragging = Input.GetMouseButton(0); // Verifica si el botón del mouse está presionado

        // Obtener la rotación del dispositivo
        float rotation = Mathf.Clamp01(Mathf.Abs(Input.acceleration.x)); // Ajustar según el eje deseado, clamping entre 0 y 1

        // Si el grosor ha sido aumentado y se está arrastrando la línea
        if (hasIncreased && isDragging)
        {
            if (Mathf.Approximately(rotation, 1.0f)) // Casi igual a 90 grados
            {
                amplitude = Mathf.Lerp(amplitude, additionalMaxAmplitude90, Time.deltaTime * parameterChangeSpeed);
                frequency = Mathf.Lerp(frequency, additionalMaxFrequency90, Time.deltaTime * parameterChangeSpeed);
                movementSpeed = Mathf.Lerp(movementSpeed, additionalMaxMovementSpeed90, Time.deltaTime * parameterChangeSpeed);
                points = Mathf.RoundToInt(Mathf.Lerp(points, additionalPoints90, Time.deltaTime * parameterChangeSpeed));
            }
        }
        else if (!hasIncreased)
        {
            // Interpolar los valores objetivo entre los configurados para 0 grados y 90 grados
            float targetAmplitude = Mathf.Lerp(maxAmplitude0, maxAmplitude90, rotation);
            float targetFrequency = Mathf.Lerp(maxFrequency0, maxFrequency90, rotation);
            float targetMovementSpeed = Mathf.Lerp(maxMovementSpeed0, maxMovementSpeed90, rotation);
            int targetPoints = Mathf.RoundToInt(Mathf.Lerp(points0, points90, rotation));

            // Suavizar la transición a los valores objetivo
            amplitude = Mathf.Lerp(amplitude, targetAmplitude, Time.deltaTime * parameterChangeSpeed);
            frequency = Mathf.Lerp(frequency, targetFrequency, Time.deltaTime * parameterChangeSpeed);
            movementSpeed = Mathf.Lerp(movementSpeed, targetMovementSpeed, Time.deltaTime * parameterChangeSpeed);
            points = Mathf.RoundToInt(Mathf.Lerp(points, targetPoints, Time.deltaTime * parameterChangeSpeed));
        }

        Draw();
    }
}
