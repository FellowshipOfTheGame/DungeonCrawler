using UnityEngine;
using System.Collections;

public class ScenearyInv : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public string name;

    public float moveDistance = 3f; // Distancia que la puerta se moverá hacia arriba
    public float moveDuration = 3f;  // Tiempo que tardará en abrirse

    private Vector3 initialPosition; // Posición inicial de la puerta
    private Vector3 targetPosition;   // Posición objetivo de la puerta

    void Start()
    {
        initialPosition = transform.position; // Guardar la posición inicial
        targetPosition = initialPosition + Vector3.up * moveDistance; // Calcular la posición final
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el otro objeto tiene un tag
        if (!string.IsNullOrEmpty(other.tag))
        {
            // Obtiene el tag del objeto con el que colisionó
            string collidedTag = other.tag;

            var destroyable = other.GetComponent<Key>();

            // Aquí puedes realizar acciones adicionales con el tag
            Debug.Log("Player colisionó con un objeto de tag: " + collidedTag);

            // Ejemplo: Puedes usar el tag para verificar una condición
            if (collidedTag == "player")
            {
                var keys = other.GetComponent<MoveP>().keys;

                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i] == "Key1")
                    {
                        // Realiza la acción que necesitas aquí
                        Debug.Log("Se encontró 'key1' y se va a eliminar.");

                        OpenDoor();

                        // Elimina el elemento en la posición i
                        keys.RemoveAt(i);

                        // Como eliminamos un elemento, el índice actual i ahora apunta al siguiente elemento, 
                        // por lo que reducimos el índice para continuar recorriendo correctamente
                        i--;
                    }
                }
            }
        }
        else
        {
            Debug.Log("El objeto no tiene tag.");
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el otro objeto tiene un tag
        if (!string.IsNullOrEmpty(collision.gameObject.tag))
        {
            // Obtiene el tag del objeto con el que colisionó
            string collidedTag = collision.gameObject.tag;

            // Aquí puedes realizar acciones adicionales con el tag
            Debug.Log("Player colisionó con un objeto de tag: " + collidedTag);

            if (collidedTag == "player")
            {
                var keys = collision.gameObject.GetComponent<MoveP>().keys; // Asegúrate de que MoveP esté en el objeto jugador

                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i] == "Key1")
                    {
                        // Realiza la acción que necesitas aquí
                        Debug.Log("Se encontró 'Key1' y se va a eliminar.");

                        OpenDoor();

                        // Elimina el elemento en la posición i
                        keys.RemoveAt(i);

                        // Como eliminamos un elemento, el índice actual i ahora apunta al siguiente elemento, 
                        // por lo que reducimos el índice para continuar recorriendo correctamente
                        i--;
                    }
                }
            }
        }
        else
        {
            Debug.Log("El objeto no tiene tag.");
        }
    }


    public void OpenDoor()
    {
        StartCoroutine(OpenDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // Interpolación de la posición de la puerta
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime; // Incrementar el tiempo transcurrido
            yield return null; // Esperar el siguiente frame
        }

        // Asegurarse de que la puerta esté en la posición final
        transform.position = targetPosition;
    }
}
