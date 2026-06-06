using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coin: Moneda recolectable que flota sobre el camino y rota visualmente.
/// Al colisionar con el jugador, se suma al contador de monedas
/// a traves del GameManager y se destruye.
/// Es hija de una plataforma, por lo que tambien se destruye
/// cuando la plataforma desaparece.
/// </summary>
public class Coin : MonoBehaviour
{
    // --- Rotacion visual ---
    [Header("Rotacion Visual")]
    [Tooltip("Velocidad a la que la moneda rota sobre si misma en grados por segundo")]
    public float velocidadRotacion = 180f;

    // --- Movimiento flotante ---
    [Header("Movimiento Flotante")]
    [Tooltip("Altura del movimiento flotante de la moneda")]
    public float alturaFlotacion = 0.3f;

    [Tooltip("Velocidad del movimiento flotante")]
    public float velocidadFlotacion = 2f;

    // Posicion inicial en Y para calcular la flotacion
    private float posicionInicialY;

    // --- Audio ---
    [Header("Audio")]
    [Tooltip("Sonido que se reproduce al recolectar la moneda")]
    public AudioClip sonidoRecoleccion;

    /// <summary>
    /// Start: Guarda la posicion inicial en Y para el efecto de flotacion.
    /// </summary>
    private void Start()
    {
        posicionInicialY = transform.position.y;
    }

    /// <summary>
    /// Update: Rota la moneda sobre su eje Y y la hace flotar
    /// con un movimiento senoidal suave para darle vida visual.
    /// </summary>
    private void Update()
    {
        // Rotar la moneda sobre el eje Y
        transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime);

        // Movimiento flotante usando seno
        float nuevaY = posicionInicialY + Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion;
        transform.position = new Vector3(transform.position.x, nuevaY, transform.position.z);
    }

    /// <summary>
    /// OnTriggerEnter: Detecta cuando el jugador toca la moneda.
    /// Suma una moneda al GameManager, reproduce el sonido y se destruye.
    /// </summary>
    /// <param name="other">El collider del objeto que entro en contacto</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Sumar moneda al contador
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SumarMoneda();
            }

            // Reproducir sonido de recoleccion
            if (sonidoRecoleccion != null)
            {
                AudioSource.PlayClipAtPoint(sonidoRecoleccion, transform.position);
            }

            Destroy(gameObject);
        }
    }
}