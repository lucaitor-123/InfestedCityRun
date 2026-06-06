using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CameraFollow: Controla la camara principal del juego.
/// Sigue al jugador posicionandose por arriba y detras de el,
/// moviendose solo hacia adelante (eje Z) junto con el jugador.
/// No se mueve de forma horizontal (eje X), tal como pide el enunciado.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // --- Referencias ---
    [Header("Referencia al Jugador")]
    [Tooltip("Transform del jugador al que la camara debe seguir")]
    public Transform jugador;

    // --- Configuracion de posicion ---
    [Header("Configuracion de Camara")]
    [Tooltip("Distancia vertical (altura) de la camara respecto al jugador")]
    public float altura = 7f;

    [Tooltip("Distancia hacia atras de la camara respecto al jugador")]
    public float distanciaAtras = 8f;

    [Tooltip("Angulo de inclinacion de la camara mirando hacia abajo (en grados)")]
    public float anguloInclinacion = 20f;

    [Tooltip("Suavizado del movimiento de la camara (menor = mas suave)")]
    public float suavizado = 5f;

    // Offset calculado a partir de la altura y distancia configuradas
    private Vector3 offset;

    /// <summary>
    /// Start: Calcula el offset inicial de la camara respecto al jugador.
    /// </summary>
    private void Start()
    {
        offset = new Vector3(0f, altura, -distanciaAtras);

        // Aplicar la rotacion de la camara para que mire hacia abajo
        transform.rotation = Quaternion.Euler(anguloInclinacion, 0f, 0f);
    }

    /// <summary>
    /// LateUpdate: Se ejecuta despues de todos los Update.
    /// Se usa LateUpdate para que la camara se mueva despues del jugador,
    /// evitando vibraciones o movimientos desincronizados.
    /// La camara solo sigue al jugador en el eje Z (adelante),
    /// manteniendo su posicion X fija en el centro del camino.
    /// </summary>
    private void LateUpdate()
    {
        if (jugador == null) return;

        // Calcular la posicion objetivo: solo sigue en Z, X siempre en 0
        Vector3 posicionObjetivo = new Vector3(0f, jugador.position.y, jugador.position.z) + offset;

        // Mover la camara suavemente hacia la posicion objetivo
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, suavizado * Time.deltaTime);
    }
}