using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedSinewave2 : MonoBehaviour
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

    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;
    private float baseFrequency = 261.63f; // Frecuencia de la nota Do (C4) en Hz

    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();

        // A�adir y configurar el EdgeCollider2D
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.edgeRadius = 0.1f;

        // Crear y configurar el AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = true;
        audioSource.loop = true;

        // Asignar el clip de sonido
        audioSource.clip = Resources.Load<AudioClip>("sonido2"); // Aseg�rate de que el archivo est� en la carpeta Resources

        // A�adir y configurar el filtro de paso bajo
        lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        lowPassFilter.cutoffFrequency = 5000f; // Puedes ajustar este valor para suavizar m�s el sonido

        // Dibujar la l�nea inicialmente
        Draw();

        // Iniciar la reproducci�n del sonido
        audioSource.Play();
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
        // Obtener la posici�n del mouse en el eje X
        float mousePositionX = Input.mousePosition.x / Screen.width; // Normaliza la posici�n entre 0 y 1

        // Ajustar los valores objetivo basados en la posici�n del mouse
        float targetAmplitude = Mathf.Lerp(0, maxAmplitude, mousePositionX);

        // Suavizar la transici�n a los valores objetivo para la amplitud
        amplitude = Mathf.Lerp(amplitude, targetAmplitude, Time.deltaTime * parameterChangeSpeed);

        // Mantener los valores actuales para frecuencia y velocidad de movimiento
        // Por ahora no cambiamos `frequency` ni `movementSpeed`
        frequency = Mathf.Clamp(frequency, 0, maxFrequency);
        movementSpeed = Mathf.Clamp(movementSpeed, 0, maxMovementSpeed);

        // Ajustar el tono y volumen del sonido
        AdjustSound();

        Draw();
    }

    void AdjustSound()
    {
        // Ajustar el volumen seg�n la amplitud, con un comportamiento inverso
        audioSource.volume = Mathf.Lerp(0.0f, 1.0f, amplitude / maxAmplitude); // Ajusta el rango seg�n sea necesario
    }

    IEnumerator GenerateTone()
    {
        while (true)
        {
            float increment = baseFrequency * Mathf.Pow(2, audioSource.pitch - 1) / AudioSettings.outputSampleRate;
            float phase = 0f;

            float[] data = new float[1024]; // Aumentamos la resoluci�n
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Mathf.Sin(2 * Mathf.PI * phase);
                phase += increment;
                if (phase > 1f)
                    phase -= 1f;
            }

            audioSource.clip = AudioClip.Create("SineWave", data.Length, 1, AudioSettings.outputSampleRate, false);
            audioSource.clip.SetData(data, 0);
            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length);
        }
    }
}
