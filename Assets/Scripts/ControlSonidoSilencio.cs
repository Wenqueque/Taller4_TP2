using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSonidoSilencio : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Obtener el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("No se encontró un AudioSource. Se ha añadido uno nuevo.");
        }
    }

    void Update()
    {
        // Detectar si hay contacto con la pantalla
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Silenciar el audio al tocar la pantalla
                audioSource.volume = 0f;
            }
        }
        else
        {
            // Si no hay contacto, restablecer el volumen a un valor predeterminado (opcional)
            audioSource.volume = 0.5f; // Puedes ajustar este valor según sea necesario
        }
    }
}