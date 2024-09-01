using System.Collections.Generic;
using UnityEngine;

public class SoberbiaArrastraryCambiarFrecuencia : MonoBehaviour

{
    public LineRenderer myLineRenderer;
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

    public float parameterChangeSpeed = 1;

    // Variables públicas para controlar el rango de la frecuencia al arrastrar
    public float maxFrequencyDrag = 5f;
    public float minFrequencyDrag = 0.5f;

    private EdgeCollider2D edgeCollider;
    private List<Vector2> linePoints = new List<Vector2>();

    private bool isDragging = false;
    private Vector3 lastMousePosition;

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
        float rotation = Mathf.Clamp01(Mathf.Abs(Input.acceleration.x)); // Ajustar según el eje deseado, clamping entre 0 y 1

        // Interpolar los valores objetivo entre los configurados para 0 grados y 90 grados
        float targetAmplitude = Mathf.Lerp(maxAmplitude0, maxAmplitude90, rotation);
        float targetMovementSpeed = Mathf.Lerp(maxMovementSpeed0, maxMovementSpeed90, rotation);
        int targetPoints = Mathf.RoundToInt(Mathf.Lerp(points0, points90, rotation));

        // Suavizar la transición a los valores objetivo
        amplitude = Mathf.Lerp(amplitude, targetAmplitude, Time.deltaTime * parameterChangeSpeed);
        movementSpeed = Mathf.Lerp(movementSpeed, targetMovementSpeed, Time.deltaTime * parameterChangeSpeed);
        points = Mathf.RoundToInt(Mathf.Lerp(points, targetPoints, Time.deltaTime * parameterChangeSpeed));

        // Detectar si se está arrastrando la línea
        if (Input.GetMouseButton(0))
        {
            if (!isDragging)
            {
                isDragging = true;
                lastMousePosition = Input.mousePosition;
            }

            // Calcular el cambio de frecuencia basado en el movimiento en el eje Y
            Vector3 currentMousePosition = Input.mousePosition;
            float mouseDeltaY = currentMousePosition.y - lastMousePosition.y;

            frequency += mouseDeltaY * Time.deltaTime * parameterChangeSpeed;
            frequency = Mathf.Clamp(frequency, minFrequencyDrag, maxFrequencyDrag);

            lastMousePosition = currentMousePosition;
        }
        else
        {
            isDragging = false;
        }

        Draw();
    }
}
