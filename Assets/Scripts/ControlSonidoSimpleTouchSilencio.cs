using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSonidoSimpleTouchSilencio : MonoBehaviour
{
     private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public string clipPath; // Ruta del clip en la carpeta Resources
    private float volumenBase = 0.5f;
    //private Vector2 startTouchPosition;

    private Vector2 startTouchPosition;
    private bool isTouching = false;

    void Start()
    {
        // Obtener el componente LineRenderer
        lineRenderer = GetComponent<LineRenderer>();

        // Agregar un componente AudioSource si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Cargar el AudioClip desde la ruta especificada en Resources
        AudioClip clip = Resources.Load<AudioClip>(clipPath);

        if (clip != null)
        {
            audioSource.clip = clip;
        }
        else
        {
            Debug.LogError("No se pudo cargar el clip de audio desde la ruta: " + clipPath);
            return;
        }

        audioSource.loop = true; // Repetir continuamente
        audioSource.playOnAwake = true; // Comenzar a reproducirse al iniciar
        audioSource.volume = volumenBase; // Establecer volumen inicial
        audioSource.pitch = 1f; // Tono inicial
        audioSource.Play(); // Reproducir el audio
    }

    void Update()
    {
        // Leer la aceleración en el eje X
        float accelX = Input.acceleration.x;

        // Ajustar la amplitud del sonido según la inclinación
        float volume = Mathf.Lerp(0.3f, 0f, Mathf.Abs(accelX));
        audioSource.volume = volume;

        // Detectar el movimiento del dedo en la pantalla para cambiar el tono
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
                // Calcular el cambio en la posición Y
                float deltaY = touch.position.y - startTouchPosition.y;

                // Ajustar el pitch en función del desplazamiento del dedo
                // Aquí el pitch cambia en un rango de 0.5 a 2.0 según el desplazamiento en el eje Y
                audioSource.pitch = Mathf.Clamp(1f + (deltaY / Screen.height), 0.5f, 2.0f);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Reiniciar el estado de toque
                isTouching = false;
            }
        }

    }
}
