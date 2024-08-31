using UnityEngine;

public class GrosorLineaProteccion : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public float increaseAmount = 0.1f; // Cantidad que aumentará el grosor
    public KeyCode increaseKey = KeyCode.Space; // Tecla para aumentar el grosor

    private bool hasIncreased = false; // Para asegurarnos de que solo aumente una vez

    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // Si se presiona la tecla de aumento y no ha aumentado antes
        if (Input.GetMouseButton(0) && !hasIncreased)
        {
            // Aumentar el grosor de la línea
            myLineRenderer.startWidth += increaseAmount;
            myLineRenderer.endWidth += increaseAmount;

            // Marcar que el grosor ya ha sido aumentado
            hasIncreased = true;
        }
    }
}