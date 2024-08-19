using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;

    public AudioClip soundClip;
    public float maxAmplitude = 1;

    // Variables públicas para los parámetros a 0 grados
    [Header("0 Degrees Settings")]
    public float zeroDegreesPitch = 1.0f;
    public float zeroDegreesCutoffFrequency = 5000f;

    // Variables públicas para los parámetros a 90 grados
    [Header("90 Degrees Settings")]
    public float ninetyDegreesPitch = 1.5f;
    public float ninetyDegreesCutoffFrequency = 2000f;

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
        lowPassFilter.cutoffFrequency = zeroDegreesCutoffFrequency; // Valor inicial a 0 grados

        // Iniciar la reproducción del sonido
        audioSource.Play();

        // Habilitar el giroscopio
        Input.gyro.enabled = true;
    }

    void Update()
    {
        // Obtener los datos del giroscopio
        Vector3 rotationRate = Input.gyro.rotationRateUnbiased;
        Vector3 rotationEuler = Input.gyro.attitude.eulerAngles; // Obtener la orientación en grados

        // Calcular la amplitud basada en la magnitud de la rotación
        float rotationMagnitude = rotationRate.magnitude;
        float amplitude = Mathf.Clamp(rotationMagnitude, 0, maxAmplitude);

        // Ajustar el volumen del audio en función de la amplitud
        SetAmplitude(amplitude);

        // Ajustar parámetros basados en la orientación del dispositivo
        AdjustParametersBasedOnRotation(rotationEuler);
    }

    void AdjustParametersBasedOnRotation(Vector3 rotationEuler)
    {
        if (IsNearAngle(rotationEuler.z, 0f))
        {
            // Aplicar los parámetros específicos para 0 grados
            audioSource.pitch = zeroDegreesPitch;
            lowPassFilter.cutoffFrequency = zeroDegreesCutoffFrequency;
        }
        else if (IsNearAngle(rotationEuler.z, 90f))
        {
            // Aplicar los parámetros específicos para 90 grados
            audioSource.pitch = ninetyDegreesPitch;
            lowPassFilter.cutoffFrequency = ninetyDegreesCutoffFrequency;
        }
    }

    bool IsNearAngle(float angle, float targetAngle, float tolerance = 5f)
    {
        return Mathf.Abs(Mathf.DeltaAngle(angle, targetAngle)) <= tolerance;
    }

    public void SetAmplitude(float amplitude)
    {
        // Ajustar el volumen según la amplitud
        audioSource.volume = Mathf.Lerp(0.0f, 0.4f, amplitude / maxAmplitude); // Ajusta el rango según sea necesario
    }
}
