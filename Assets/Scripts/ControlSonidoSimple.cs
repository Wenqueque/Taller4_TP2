using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSonidoSimple : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public string clipPath; // Ruta del clip en la carpeta Resources
    private float volumenBase = 0.5f;
    //private Vector2 startTouchPosition;

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
        float volume = Mathf.Lerp(0.5f, 1f, Mathf.Abs(accelX));
        audioSource.volume = volume;

        // Cambiar el color de la línea según la inclinación
        // Aquí va el código para cambiar el color de la línea si lo necesitas

        // Detectar el movimiento del dedo en la pantalla para cambiar el tono
        /*if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Guardar la posición inicial del toque
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // Calcular el cambio en la posición Y
                float deltaY = touch.position.y - startTouchPosition.y;

                // Ajustar el pitch en función del desplazamiento del dedo
                audioSource.pitch = Mathf.Clamp(1f + (deltaY / Screen.height), 0.5f, 2.0f);
            }
        }*/
    }
}
