using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineasDesinteres : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points;
    public float amplitude = 1;
    public float frequency = 1;
    public float frequencyStep = 0.1f; // Paso para aumentar o disminuir la frecuencia
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    public float maxAmplitude0 = 2;
    public float maxAmplitude90 = 5;
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
        // Obtener la rotación del dispositivo (giroscopio)
        float rotation = Mathf.Clamp01(Mathf.Abs(Input.acceleration.x)); // Ajustar según el eje deseado, clamping entre 0 y 1

        // Calcular la amplitud máxima en función de la rotación del dispositivo
        float targetAmplitude = Mathf.Lerp(maxAmplitude0, maxAmplitude90, rotation);

        // Suavizar la transición a la nueva amplitud
        amplitude = Mathf.Lerp(amplitude, targetAmplitude, Time.deltaTime * parameterChangeSpeed);

        // Aumentar la frecuencia al presionar el botón izquierdo del ratón
        if (Input.GetMouseButton(0))
        {
            frequency += frequencyStep * Time.deltaTime;
        }
        // Disminuir la frecuencia al presionar el botón derecho del ratón
        else if (Input.GetMouseButton(1))
        {
            frequency -= frequencyStep * Time.deltaTime;
        }

        // Dibujar la línea en cada frame
        Draw();
    }
}
