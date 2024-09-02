using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimidezLineaTimida : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points;
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    public float maxAmplitude0 = 2;
    public float maxAmplitude90 = 1;
    public float parameterChangeSpeed = 1;

    public float minFrequency = 1; // Frecuencia mínima
    public float maxFrequency = 10; // Frecuencia máxima
    public float dragSensitivity = 0.1f; // Sensibilidad del arrastre

    private EdgeCollider2D edgeCollider;
    private List<Vector2> linePoints = new List<Vector2>();
    private bool isDragging = false; // Para verificar si se está arrastrando la pantalla

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
        float xFinish = xLimits.y;

        myLineRenderer.positionCount = points;
        linePoints.Clear();

        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = amplitude * Mathf.Sin((x * frequency * 2 * Mathf.PI) + (Time.timeSinceLevelLoad * movementSpeed));

            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
            linePoints.Add(new Vector2(x, y));
        }

        // Actualizar el EdgeCollider2D
        edgeCollider.SetPoints(linePoints);
    }

    void Update()
    {
        // Ajustar la amplitud según la rotación del dispositivo solo si no se está arrastrando
        if (!isDragging)
        {
            float rotation = Mathf.Clamp01(Mathf.Abs(Input.acceleration.x)); // Ajustar según el eje deseado, clamping entre 0 y 1
            float targetAmplitude = Mathf.Lerp(maxAmplitude0, maxAmplitude90, rotation);
            amplitude = Mathf.Lerp(amplitude, targetAmplitude, Time.deltaTime * parameterChangeSpeed);
        }

        // Ajustar la frecuencia en función del arrastre
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                // Indicar que se está arrastrando
                isDragging = true;

                // Ajustar la frecuencia según el movimiento vertical del toque
                frequency += touch.deltaPosition.y * dragSensitivity * Time.deltaTime;
                frequency = Mathf.Clamp(frequency, minFrequency, maxFrequency); // Limitar la frecuencia entre min y max
            }

            if (touch.phase == TouchPhase.Ended)
            {
                // Indicar que el arrastre ha terminado
                isDragging = false;
            }
        }

        Draw();
    }
}
