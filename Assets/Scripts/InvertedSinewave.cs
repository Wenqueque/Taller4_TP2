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

    private SoundController soundController;

    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();

        // Añadir y configurar el EdgeCollider2D
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.edgeRadius = 0.1f;

        // Obtener el componente SoundController
        soundController = GetComponent<SoundController>();

        // Dibujar la línea inicialmente
        Draw();
    }

    void Update()
    {
        // Obtener la posición del mouse en el eje X
        float mousePositionX = Input.mousePosition.x / Screen.width; // Normaliza la posición entre 0 y 1

        // Ajustar los valores objetivo basados en la posición del mouse
        float targetAmplitude = Mathf.Lerp(0, maxAmplitude, mousePositionX);
        float targetFrequency = Mathf.Lerp(0, maxFrequency, mousePositionX);
        float targetMovementSpeed = Mathf.Lerp(0, maxMovementSpeed, mousePositionX);

        // Suavizar la transición a los valores objetivo
        amplitude = Mathf.Lerp(amplitude, targetAmplitude, Time.deltaTime * parameterChangeSpeed);
        frequency = Mathf.Lerp(frequency, targetFrequency, Time.deltaTime * parameterChangeSpeed);
        movementSpeed = Mathf.Lerp(movementSpeed, targetMovementSpeed, Time.deltaTime * parameterChangeSpeed);

        // Ajustar el sonido en función de la amplitud
        if (soundController != null)
        {
            soundController.SetAmplitude(amplitude);
        }

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
}
