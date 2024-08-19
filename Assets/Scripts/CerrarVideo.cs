using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CerrarVideo : MonoBehaviour
{
    public VideoPlayer video;  // El VideoPlayer que reproducirá el video
    public string sceneName;   // El nombre de la escena a la que deseas cambiar

    // Awake es llamado antes de que empiece la escena
    void Awake()
    {
        // Intentar obtener el VideoPlayer, si no se asignó desde el Inspector
        if (video == null)
        {
            video = GetComponent<VideoPlayer>();
        }

        // Si no se encuentra un VideoPlayer, mostrar un error y detener la ejecución
        if (video == null)
        {
            Debug.LogError("No hay un VideoPlayer adjunto al objeto. Por favor, añade uno.");
            return;
        }

        video.Play();  // Reproduce el video
        video.loopPointReached += CheckOver;  // Suscríbete al evento que se dispara al finalizar el video
    }

    // Este método se llama cuando el video termina de reproducirse
    void CheckOver(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneName);  // Cambia a la escena especificada
    }
}
