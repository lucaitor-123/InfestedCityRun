using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vehicle: Obstaculo estatico que aparece en uno de los 3 carriles del camino.
/// A diferencia del zombie, el vehiculo no se mueve y permanece fijo
/// en la plataforma donde aparecio. El jugador puede saltarlo por encima.
/// Se destruye al recibir un proyectil del jugador.
/// Mata al jugador al colisionar con el (reinicia la partida).
/// Existen minimo 3 tipos de vehiculos diferentes (cambian color o modelo).
/// </summary>
public class Vehicle : MonoBehaviour
{
    // --- Audio ---
    [Header("Audio")]
    [Tooltip("Sonido que se reproduce cuando el vehiculo es destruido por una bala")]
    public AudioClip sonidoDestruccion;

    /// <summary>
    /// OnTriggerEnter: Detecta cuando una bala del jugador golpea al vehiculo.
    /// La bala se destruye en su propio script, aqui solo se destruye el vehiculo.
    /// </summary>
    /// <param name="other">El collider del objeto que entro en contacto</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destruir();
        }
    }

    /// <summary>
    /// Destruir: Reproduce el sonido de destruccion y elimina el vehiculo.
    /// Usa PlayClipAtPoint para que el sonido se escuche incluso
    /// despues de que el objeto sea destruido.
    /// </summary>
    private void Destruir()
    {
        if (sonidoDestruccion != null)
        {
            AudioSource.PlayClipAtPoint(sonidoDestruccion, transform.position);
        }

        Destroy(gameObject);
    }
}
