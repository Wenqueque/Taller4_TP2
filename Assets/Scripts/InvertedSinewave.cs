using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedSinewave : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points;
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    public float maxAmplitude = 2;
    public float maxFrequency = 2;
    public float maxMovementSpeed = 2;
    public float parameterChangeSpeed = 1;

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
            float y = amplitude * Mathf.Sin((Tau * frequency * x) + (Time.timeSinceLevelLoad * movementSpeed));
            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
            linePoints.Add(new Vector2(x, y));
        }

        // Actualizar el EdgeCollider2D
        edgeCollider.SetPoints(linePoints);
    }

    void Update()
    {
        // Obtener la rotación del dispositivo
        float rotation = Input.acceleration.x; // Ajustar según el eje deseado

        // Invertir los valores objetivo (disminuir con la rotación)
        float targetAmplitude = Mathf.Lerp(maxAmplitude, 0, Mathf.Abs(rotation));
        float targetFrequency = Mathf.Lerp(maxFrequency, 0, Mathf.Abs(rotation));
        float targetMovementSpeed = Mathf.Abs(rotation) * maxMovementSpeed; // Mantén el movimiento normal

        // Suavizar la transición a los valores objetivo
        amplitude = Mathf.Lerp(amplitude, targetAmplitude, Time.deltaTime * parameterChangeSpeed);
        frequency = Mathf.Lerp(frequency, targetFrequency, Time.deltaTime * parameterChangeSpeed);
        //movementSpeed = Mathf.Lerp(movementSpeed, targetMovementSpeed, Time.deltaTime * parameterChangeSpeed);

        Draw();
    }
}