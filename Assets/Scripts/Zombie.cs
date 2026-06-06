using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zombie: Enemigo que se mueve en linea recta en direccion contraria al jugador
/// (hacia la pantalla) sin detenerse. Aparece en uno de los 3 carriles de forma
/// aleatoria y se mantiene siempre en ese carril sin cambiarse a otro.
/// El jugador no puede saltarlo por encima.
/// Se destruye al recibir un proyectil del jugador.
/// Mata al jugador al colisionar con el (reinicia la partida).
/// </summary>
public class Zombie : MonoBehaviour
{
    // --- Movimiento ---
    [Header("Movimiento")]
    [Tooltip("Velocidad a la que el zombie avanza hacia el jugador")]
    public float velocidad = 5f;

    // --- Audio ---
    [Header("Audio")]
    [Tooltip("Sonido que se reproduce cuando el zombie muere")]
    public AudioClip sonidoMuerte;

    // Referencia al Animator para la animacion de correr
    private Animator animator;

    /// <summary>
    /// Start: Obtiene la referencia al Animator y rota al zombie
    /// para que mire hacia el jugador (direccion contraria al avance).
    /// </summary>
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        // Rotar 180 grados en Y para que el zombie mire hacia el jugador
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    /// <summary>
    /// Update: Mueve al zombie en linea recta hacia el jugador cada frame.
    /// El zombie avanza en el eje Z negativo (hacia la camara) sin parar.
    /// </summary>
    private void Update()
    {
        // No moverse si la partida no esta activa
        if (GameManager.Instance != null && !GameManager.Instance.EstaPartidaActiva())
        {
            return;
        }

        // Avanzar en linea recta hacia el jugador (eje Z negativo)
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    /// <summary>
    /// OnTriggerEnter: Detecta cuando una bala del jugador golpea al zombie.
    /// La bala se destruye en su propio script, aqui solo se destruye el zombie.
    /// </summary>
    /// <param name="other">El collider del objeto que entro en contacto</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Morir();
        }
    }

    /// <summary>
    /// Morir: Reproduce el sonido de muerte y destruye al zombie.
    /// El sonido se reproduce en la posicion del zombie usando PlayClipAtPoint
    /// para que se escuche incluso despues de que el objeto se destruya.
    /// </summary>
    private void Morir()
    {
        // Reproducir sonido de muerte desde la posicion de la camara
        // para que siempre se escuche sin importar la distancia
        if (sonidoMuerte != null)
        {
            AudioSource.PlayClipAtPoint(sonidoMuerte, Camera.main.transform.position, 1f);
        }

        Destroy(gameObject);
    }
}