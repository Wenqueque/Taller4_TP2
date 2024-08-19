using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;

    public AudioClip soundClip;
    public float maxAmplitude = 1;

    void Start()
    {
        // Crear y configurar el AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = true;
        audioSource.loop = true;

        // Asignar el clip de sonido
        audioSource.clip = soundClip;

        // Añadir y configurar el filtro de paso bajo
        lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        lowPassFilter.cutoffFrequency = 5000f; // Puedes ajustar este valor para suavizar más el sonido

        // Iniciar la reproducción del sonido
        audioSource.Play();
    }

    public void SetAmplitude(float amplitude)
    {
        // Ajustar el volumen según la amplitud, con un comportamiento inverso
        audioSource.volume = Mathf.Lerp(0.0f, 0.4f, amplitude / maxAmplitude); // Ajusta el rango según sea necesario
    }
}
