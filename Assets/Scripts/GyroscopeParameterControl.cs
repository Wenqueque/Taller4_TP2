using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeParameterControl : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points;
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    public float maxAmplitudeLeft = 2;
    public float maxFrequencyLeft = 2;
    public float maxMovementSpeedLeft = 2;
    public int pointsLeft = 50;

    public float maxAmplitudeRight = 1;
    public float maxFrequencyRight = 1;
    public float maxMovementSpeedRight = 1;
    public int pointsRight = 100;

    public float parameterChangeSpeed = 1;
    public float gyroSensitivityThreshold = 0.05f; // Umbral para detectar la rotación

    private EdgeCollider2D edgeCollider;
    private List<Vector2> linePoints = new List<Vector2>();
    private Vector3 previousGyroRotation;

    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();

        // Añadir y configurar el EdgeCollider2D
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.edgeRadius = 0.1f;

        // Habilitar el giroscopio
        Input.gyro.enabled = true;

        // Inicializar la rotación anterior del giroscopio
        previousGyroRotation = Input.gyro.rotationRateUnbiased;

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
            float y = amplitude * Mathf.Sign(Mathf.Sin((Tau * frequency * x) + (Time.timeSinceLevelLoad * movementSpeed)));
            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
            linePoints.Add(new Vector2(x, y));
        }

        // Actualizar el EdgeCollider2D
        edgeCollider.SetPoints(linePoints);
    }

    void Update()
    {
        // Obtener la rotación actual del giroscopio
        Vector3 currentGyroRotation = Input.gyro.rotationRateUnbiased;

        // Calcular la diferencia de rotación
        Vector3 rotationDifference = currentGyroRotation - previousGyroRotation;

        // Verificar si la diferencia de rotación supera el umbral de sensibilidad
        if (Mathf.Abs(rotationDifference.y) > gyroSensitivityThreshold)
        {
            // Determinar la dirección de rotación (izquierda o derecha)
            bool isRotatingRight = rotationDifference.y > 0;

            // Interpolar los valores objetivo según la dirección de rotación
            float targetAmplitude = isRotatingRight ? maxAmplitudeRight : maxAmplitudeLeft;
            float targetFrequency = isRotatingRight ? maxFrequencyRight : maxFrequencyLeft;
            float targetMovementSpeed = isRotatingRight ? maxMovementSpeedRight : maxMovementSpeedLeft;
            int targetPoints = isRotatingRight ? pointsRight : pointsLeft;

            // Suavizar la transición a los valores objetivo
            amplitude = Mathf.Lerp(amplitude, targetAmplitude, Time.deltaTime * parameterChangeSpeed);
            frequency = Mathf.Lerp(frequency, targetFrequency, Time.deltaTime * parameterChangeSpeed);
            movementSpeed = Mathf.Lerp(movementSpeed, targetMovementSpeed, Time.deltaTime * parameterChangeSpeed);
            points = Mathf.RoundToInt(Mathf.Lerp(points, targetPoints, Time.deltaTime * parameterChangeSpeed));

            // Dibujar la línea con los nuevos parámetros
            Draw();
        }

        // Actualizar la rotación anterior
        previousGyroRotation = currentGyroRotation;
    }
}
