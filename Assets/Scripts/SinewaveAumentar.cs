using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinewaveAumentar : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points = 50;
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    public float maxAmplitude = 2;
    public float maxFrequency = 2;
    public float maxMovementSpeed = 2;
    public float parameterChangeSpeed = 1;

    public int minPoints = 10;  // Número mínimo de puntos
    public int maxPoints = 100; // Número máximo de puntos

    private EdgeCollider2D edgeCollider;
    private List<Vector2> linePoints = new List<Vector2>();

    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();

        // Añadir y configurar el EdgeCollider2D
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.edgeRadius = 0.1f; // Ajusta el tamaño del colisionador según sea necesario

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

        // Ajustar parámetros basados en la rotación y la velocidad de cambio
        float targetAmplitude = Mathf.Abs(rotation) * maxAmplitude;
        //float targetFrequency = Mathf.Abs(rotation) * maxFrequency;
        float targetMovementSpeed = Mathf.Abs(rotation) * maxMovementSpeed;

        amplitude = Mathf.Lerp(amplitude, targetAmplitude, Time.deltaTime * parameterChangeSpeed);
        //frequency = Mathf.Lerp(frequency, targetFrequency, Time.deltaTime * parameterChangeSpeed);
        movementSpeed = Mathf.Lerp(movementSpeed, targetMovementSpeed, Time.deltaTime * parameterChangeSpeed);

        // Ajustar la cantidad de puntos según la rotación del dispositivo
        points = Mathf.RoundToInt(Mathf.Lerp(minPoints, maxPoints, Mathf.Abs(rotation)));

        Draw();
    }
}
