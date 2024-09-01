using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediacionLineaSinusoide : MonoBehaviour
{
    public LineRenderer line1; // Primera línea visible al principio
    public LineRenderer line2; // Segunda línea visible al principio
    public LineRenderer hiddenLineRenderer; // Línea que empieza oculta

    // Parámetros para ajustar en función de la rotación
    public int pointsZero = 10; // Número de puntos en el estado 0
    public int pointsRotated = 20; // Número de puntos en el estado rotado
    public float amplitudeZero = 1.0f; // Amplitud en el estado 0
    public float amplitudeRotated = 2.0f; // Amplitud en el estado rotado
    public float frequencyZero = 1.0f; // Frecuencia en el estado 0
    public float frequencyRotated = 2.0f; // Frecuencia en el estado rotado
    public float speedZero = 1.0f; // Rapidez en el estado 0
    public float speedRotated = 2.0f; // Rapidez en el estado rotado

    // Definir un rango cercano a cero
    public float zeroThreshold = 0.1f;

    private Gyroscope gyro;
    private bool isRotated = false;

    void Start()
    {
        // Verifica si el giroscopio está disponible
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
        else
        {
            Debug.LogError("Giroscopio no soportado en este dispositivo.");
        }

        if (line1 == null || line2 == null || hiddenLineRenderer == null)
        {
            Debug.LogError("Faltan asignaciones en las variables públicas.");
            return;
        }

        hiddenLineRenderer.gameObject.SetActive(false); // Inicialmente está oculta
    }

    void Update()
    {
        if (gyro != null)
        {
            Vector3 rotationRate = gyro.rotationRate;

            if (Mathf.Abs(rotationRate.y) <= zeroThreshold)
            {
                // Estado 0 (dispositivo en posición vertical o cerca de ella)
                AdjustLines(pointsZero, amplitudeZero, frequencyZero, speedZero);

                // Detectar la presión de pantalla para mostrar la línea oculta
                if (Input.GetMouseButtonDown(0))
                {
                    line1.gameObject.SetActive(false); // Oculta la primera línea
                    line2.gameObject.SetActive(false); // Oculta la segunda línea
                    hiddenLineRenderer.gameObject.SetActive(true); // Muestra la línea oculta
                }
            }
            else
            {
                // Estado girado (dispositivo girado hacia la izquierda o derecha)
                AdjustLines(pointsRotated, amplitudeRotated, frequencyRotated, speedRotated);
                isRotated = true;
            }
        }
    }

    void AdjustLines(int points, float amplitude, float frequency, float speed)
    {
        // Ajusta los parámetros del LineRenderer
        AdjustLineRenderer(line1, points, amplitude, frequency, speed);
        AdjustLineRenderer(line2, points, amplitude, frequency, speed);
    }

    void AdjustLineRenderer(LineRenderer lineRenderer, int points, float amplitude, float frequency, float speed)
    {
        if (lineRenderer != null)
        {
            Vector3[] positions = new Vector3[points];
            for (int i = 0; i < points; i++)
            {
                float t = i / (float)(points - 1);
                float x = t * 2.0f * Mathf.PI * frequency;
                float y = amplitude * Mathf.Sin(x * speed);
                positions[i] = new Vector3(t * 10.0f, y, 0.0f); // Ajusta la escala según sea necesario
            }
            lineRenderer.positionCount = points;
            lineRenderer.SetPositions(positions);
        }
    }
}