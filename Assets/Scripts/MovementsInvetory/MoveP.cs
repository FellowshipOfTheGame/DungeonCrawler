using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveP : MonoBehaviour
{

    public float speed = 5f; // Velocidad de movimiento
    public List<string> keys;



    private void Start()
    {
       keys = new List<string>();
    }

    void Update()
    {
        // Obtener entrada para mover el objeto en el eje X e Y
        float moveHorizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        // Aplicar el movimiento al objeto
        transform.Translate(moveHorizontal, 0, moveVertical);
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
            if (collidedTag == "Keys")
            {
                // Ejecuta alguna acción específica cuando el objeto tiene el tag "Key"
                Debug.Log("¡Player ha recogido la llave!");

                keys.Add(other.name);
                destroyable.destroy();
            }
        }
        else
        {
            Debug.Log("El objeto no tiene tag.");
        }
    }

}
