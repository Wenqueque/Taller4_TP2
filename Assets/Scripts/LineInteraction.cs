using UnityEngine;

public class LineInteraction : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("Línea presionada: " + gameObject.name);
    }
}
