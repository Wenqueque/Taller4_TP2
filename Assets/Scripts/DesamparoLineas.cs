using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesamparoLineas : MonoBehaviour
{
    public LineRenderer visibleLine;  // La línea visible al inicio
    public LineRenderer hiddenLine;   // La línea oculta al inicio

    public float maxAmplitude = 2f;   // Máxima amplitud para la línea visible cuando se escala
    public float noiseFrequency = 20f; // Frecuencia alta para simular ruido
    public float noiseSpeed = 10f;    // Velocidad alta para simular ruido
    public float scalingSpeed = 2f;   // Velocidad de escalado

    private float initialAmplitude;
    private float initialFrequency;
    private bool isNoiseActive = false;

    void Start()
    {
        if (visibleLine == null || hiddenLine == null)
        {
            Debug.LogError("Las líneas deben ser asignadas en el Inspector.");
            return;
        }

        // Ocultar la línea al inicio
        hiddenLine.gameObject.SetActive(false);

        // Guardar valores iniciales
        initialAmplitude = 1f; // Amplitud inicial estándar para la sinusoide
        initialFrequency = 1f; // Frecuencia inicial estándar para la sinusoide

        // Configurar la línea visible como una sinusoide normal
        StartCoroutine(SinewaveMovement());
    }

    void Update()
    {
        // Detectar cuando se presiona la pantalla
        if (Input.GetMouseButtonDown(0) && !isNoiseActive)
        {
            hiddenLine.gameObject.SetActive(true); // Mostrar la línea oculta
            isNoiseActive = true; // Activar el estado de ruido
            StopCoroutine(SinewaveMovement()); // Detener el movimiento sinusoidal
            StartCoroutine(NoiseMovement()); // Iniciar el movimiento de ruido
        }
    }

    IEnumerator SinewaveMovement()
    {
        while (!isNoiseActive)
        {
            float rotation = Mathf.Clamp01(Mathf.Abs(Input.acceleration.x)); // Ajustar según el eje deseado, clamping entre 0 y 1
            float amplitude = Mathf.Lerp(0.5f, initialAmplitude, rotation);  // Escalando según la rotación del giroscopio
            float frequency = initialFrequency;

            DrawSinewave(visibleLine, amplitude, frequency, Time.time);

            yield return null;
        }
    }

    IEnumerator NoiseMovement()
    {
        while (isNoiseActive)
        {
            DrawNoise(visibleLine, maxAmplitude);
            yield return null;
        }
    }

    void DrawSinewave(LineRenderer lineRenderer, float amplitude, float frequency, float time)
    {
        int points = 100; // Número de puntos en la línea
        float xStart = 0;
        float xEnd = 12;
        float Tau = 2 * Mathf.PI;

        lineRenderer.positionCount = points;
        for (int i = 0; i < points; i++)
        {
            float progress = (float)i / (points - 1);
            float x = Mathf.Lerp(xStart, xEnd, progress);
            float y = amplitude * Mathf.Sin(Tau * frequency * x + time);
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    void DrawNoise(LineRenderer lineRenderer, float amplitude)
    {
        int points = 100; // Número de puntos en la línea
        float xStart = 0;
        float xEnd = 12;

        lineRenderer.positionCount = points;
        for (int i = 0; i < points; i++)
        {
            float progress = (float)i / (points - 1);
            float x = Mathf.Lerp(xStart, xEnd, progress);
            float y = Random.Range(-amplitude, amplitude); // Generar ruido aleatorio dentro del rango de amplitud
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
