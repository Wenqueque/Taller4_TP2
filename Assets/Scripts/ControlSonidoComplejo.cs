using UnityEngine;

public class ControlSonidoComplejo : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public Color colorIzquierda = Color.blue; // Color cuando hay inclinación a la izquierda
    public Color colorOriginal = Color.white; // Color cuando no hay inclinación
    public Color colorDerecha = Color.yellow; // Color cuando hay inclinación a la derecha

    private float volumenBase = 0.5f;
    private Vector2 startTouchPosition;
    private bool isTouching = false;

    public AudioClip clip1; // Clip de sonido inicial
    public AudioClip clip2; // Nuevo clip de sonido para cambiar

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

        // Configurar el AudioSource con el clip de sonido inicial
        audioSource.clip = clip1; // Asegúrate de asignar el clip1 en el Inspector
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

        // Cambiar el color de la línea según la inclinación
        if (accelX > 0.1f)
        {
            // Si se inclina hacia la derecha, cambiar a color derecho
            lineRenderer.startColor = colorDerecha;
            lineRenderer.endColor = colorDerecha;
        }
        else if (accelX < -0.1f)
        {
            // Si se inclina hacia la izquierda, cambiar a color izquierdo
            lineRenderer.startColor = colorIzquierda;
            lineRenderer.endColor = colorIzquierda;
        }
        else
        {
            // Si no hay inclinación significativa, establecer color original
            lineRenderer.startColor = colorOriginal;
            lineRenderer.endColor = colorOriginal;
        }

        // Ajustar la amplitud del sonido según la inclinación
        float volume = Mathf.Lerp(0.5f, 1f, Mathf.Abs(accelX));
        audioSource.volume = volume;

        // Detectar el movimiento del dedo en la pantalla para cambiar el tono y el clip
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

                // Cambiar el clip de audio si el dedo se mueve significativamente
                if (Mathf.Abs(deltaY) > 50) // Puedes ajustar este umbral según sea necesario
                {
                    audioSource.clip = clip2; // Cambiar al nuevo clip
                    audioSource.Play(); // Reproducir el nuevo clip
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Reiniciar el estado de toque
                isTouching = false;
            }
        }
    }
}
