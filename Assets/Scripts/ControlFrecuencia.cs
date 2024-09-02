using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlFrecuencia : MonoBehaviour
{
    private AudioSource audioSource;

    public float frecuenciaBase = 220f; // Frecuencia base del sonido (Hz)
    public float frecuenciaMinima = 100f; // Frecuencia mínima (Hz)
    private float pitchMinimo; // Pitch mínimo para mantener la frecuencia mínima

    private bool isTouching = false;
    private Vector2 startTouchPosition;

    public AudioClip clip1; // Puedes asignar el clip desde el Inspector

    void Start()
    {
        // Obtener el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("No se encontró un AudioSource. Se ha añadido uno nuevo.");
        }

        // Configurar el AudioSource con el clip de sonido deseado
        if (clip1 != null)
        {
            audioSource.clip = clip1;
            audioSource.loop = true; // Opcional: si deseas que el sonido se repita continuamente
            audioSource.playOnAwake = true; // Opcional: si deseas que el sonido comience a reproducirse al iniciar
            audioSource.Play();
        }
        else
        {
            Debug.LogError("El clip de audio no está asignado en el Inspector.");
        }

        // Calcular el pitch mínimo necesario para no bajar de la frecuencia mínima
        pitchMinimo = frecuenciaMinima / frecuenciaBase;
        audioSource.pitch = 1f; // Pitch inicial
    }

    void Update()
    {
        // Detectar el movimiento del dedo en la pantalla para ajustar el tono
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Guardar la posición inicial del toque
                startTouchPosition = touch.position;
                isTouching = true;
            }
            else if (touch.phase == TouchPhase.Moved && isTouching)
            {
                // Calcular el cambio en la posición Y del dedo
                float deltaY = touch.position.y - startTouchPosition.y;

                // Ajustar el pitch en función del desplazamiento del dedo
                // Limitar el pitch para que no baje la frecuencia por debajo de la mínima
                float nuevoPitch = Mathf.Clamp(1f + (deltaY / Screen.height), pitchMinimo, 2.0f);
                audioSource.pitch = nuevoPitch;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Reiniciar el estado de toque
                isTouching = false;
            }
        }
    }
}