using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet: Controla el comportamiento de los proyectiles disparados por el jugador.
/// La bala avanza en linea recta hacia adelante sin ser afectada por la gravedad.
/// Se destruye al colisionar con un enemigo u obstaculo, y tambien se autodestruye
/// despues de un tiempo para no acumular balas infinitas en la escena.
/// </summary>
public class Bullet : MonoBehaviour
{
    // --- Configuracion ---
    [Header("Configuracion del Proyectil")]
    [Tooltip("Velocidad a la que la bala avanza hacia adelante")]
    public float velocidad = 30f;

    [Tooltip("Tiempo en segundos antes de que la bala se autodestruya si no golpea nada")]
    public float tiempoVida = 3f;

    /// <summary>
    /// Start: Programa la autodestruccion de la bala despues de un tiempo limite.
    /// Esto evita que se acumulen balas en la escena si no golpean nada.
    /// </summary>
    private void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    /// <summary>
    /// Update: Mueve la bala hacia adelante cada frame.
    /// Usa transform.Translate para un movimiento simple sin fisica,
    /// ya que la bala no debe ser afectada por la gravedad.
    /// </summary>
    private void Update()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    /// <summary>
    /// OnTriggerEnter: Se ejecuta cuando la bala entra en contacto
    /// con un collider marcado como Trigger.
    /// Destruye al enemigo u obstaculo y luego se destruye a si misma.
    /// </summary>
    /// <param name="other">El collider del objeto con el que colisiono la bala</param>
    private void OnTriggerEnter(Collider other)
    {
        // Si golpea a un enemigo
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        // Si golpea un obstaculo
        if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}