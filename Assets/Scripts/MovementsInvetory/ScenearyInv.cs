using UnityEngine;
using System.Collections;

public class ScenearyInv : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public string name;

    public float moveDistance = 3f; // Distancia que la puerta se mover� hacia arriba
    public float moveDuration = 3f;  // Tiempo que tardar� en abrirse

    private Vector3 initialPosition; // Posici�n inicial de la puerta
    private Vector3 targetPosition;   // Posici�n objetivo de la puerta

    void Start()
    {
        initialPosition = transform.position; // Guardar la posici�n inicial
        targetPosition = initialPosition + Vector3.up * moveDistance; // Calcular la posici�n final
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
            // Obtiene el tag del objeto con el que colision�
            string collidedTag = other.tag;

            var destroyable = other.GetComponent<Key>();

            // Aqu� puedes realizar acciones adicionales con el tag
            Debug.Log("Player colision� con un objeto de tag: " + collidedTag);

            // Ejemplo: Puedes usar el tag para verificar una condici�n
            if (collidedTag == "player")
            {
                var keys = other.GetComponent<MoveP>().keys;

                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i] == "Key1")
                    {
                        // Realiza la acci�n que necesitas aqu�
                        Debug.Log("Se encontr� 'key1' y se va a eliminar.");

                        OpenDoor();

                        // Elimina el elemento en la posici�n i
                        keys.RemoveAt(i);

                        // Como eliminamos un elemento, el �ndice actual i ahora apunta al siguiente elemento, 
                        // por lo que reducimos el �ndice para continuar recorriendo correctamente
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
            // Obtiene el tag del objeto con el que colision�
            string collidedTag = collision.gameObject.tag;

            // Aqu� puedes realizar acciones adicionales con el tag
            Debug.Log("Player colision� con un objeto de tag: " + collidedTag);

            if (collidedTag == "player")
            {
                var keys = collision.gameObject.GetComponent<MoveP>().keys; // Aseg�rate de que MoveP est� en el objeto jugador

                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i] == "Key1")
                    {
                        // Realiza la acci�n que necesitas aqu�
                        Debug.Log("Se encontr� 'Key1' y se va a eliminar.");

                        OpenDoor();

                        // Elimina el elemento en la posici�n i
                        keys.RemoveAt(i);

                        // Como eliminamos un elemento, el �ndice actual i ahora apunta al siguiente elemento, 
                        // por lo que reducimos el �ndice para continuar recorriendo correctamente
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
            // Interpolaci�n de la posici�n de la puerta
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime; // Incrementar el tiempo transcurrido
            yield return null; // Esperar el siguiente frame
        }

        // Asegurarse de que la puerta est� en la posici�n final
        transform.position = targetPosition;
    }
}
