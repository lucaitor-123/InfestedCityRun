using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager: Controlador central del juego "Infested City Run".
/// Se encarga de gestionar el estado de la partida (jugando o game over),
/// llevar el puntaje por distancia recorrida, contar las monedas recolectadas
/// y reiniciar la partida cuando el jugador pierde.
/// Implementa el patron Singleton para ser accesible desde cualquier script.
/// </summary>
public class GameManager : MonoBehaviour
{
    // --- Singleton ---
    // Instancia unica del GameManager accesible desde cualquier parte del codigo
    public static GameManager Instance;

    // --- Estado de la partida ---
    // Indica si el juego esta activo (true) o si el jugador perdio (false)
    private bool partidaActiva = false;

    // --- Puntaje ---
    // Puntaje actual basado en la distancia recorrida por el jugador
    private float puntaje = 0f;

    // Multiplicador para convertir la distancia recorrida en puntos
    [Header("Configuracion de Puntaje")]
    [Tooltip("Cuantos puntos se suman por cada unidad de distancia recorrida")]
    public float multiplicadorPuntaje = 10f;

    // --- Monedas ---
    // Cantidad de monedas recolectadas en la partida actual
    private int monedas = 0;

    // --- Referencias ---
    // Referencia al UIManager para actualizar los contadores en pantalla
    [Header("Referencias")]
    [Tooltip("Referencia al UIManager que muestra puntaje y monedas en pantalla")]
    public UIManager uiManager;

    /// <summary>
    /// Awake se ejecuta antes que Start.
    /// Configura el patron Singleton para asegurar una unica instancia.
    /// </summary>
    private void Awake()
    {
        // Si ya existe una instancia, destruir este duplicado
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Asignar esta instancia como la unica
        Instance = this;
    }

    /// <summary>
    /// Start se ejecuta al inicio de la escena.
    /// Inicia la partida.
    /// </summary>
    private void Start()
    {
        IniciarPartida();
    }

    /// <summary>
    /// Update se ejecuta una vez por frame.
    /// Si la partida esta activa, incrementa el puntaje segun la distancia.
    /// </summary>
    private void Update()
    {
        if (partidaActiva)
        {
            // Incrementar puntaje proporcionalmente al tiempo transcurrido
            puntaje += multiplicadorPuntaje * Time.deltaTime;

            // Actualizar el puntaje en la interfaz
            if (uiManager != null)
            {
                uiManager.ActualizarPuntaje((int)puntaje);
            }
        }
    }

    /// <summary>
    /// Inicia o reinicia la partida, reseteando puntaje y monedas.
    /// </summary>
    public void IniciarPartida()
    {
        partidaActiva = true;
        puntaje = 0f;
        monedas = 0;

        // Asegurar que el tiempo del juego esta corriendo
        Time.timeScale = 1f;

        // Actualizar la UI con los valores iniciales
        if (uiManager != null)
        {
            uiManager.ActualizarPuntaje(0);
            uiManager.ActualizarMonedas(0);
        }
    }

    /// <summary>
    /// Se llama cuando el jugador colisiona con un enemigo u obstaculo.
    /// Detiene la partida y muestra el menu de derrota.
    /// </summary>
    public void GameOver()
    {
        partidaActiva = false;

        // Pausar el juego
        Time.timeScale = 0f;

        // Mostrar el menu de derrota
        if (uiManager != null)
        {
            uiManager.MostrarMenuDerrota();
        }
    }

    /// <summary>
    /// Reinicia la escena actual para comenzar una nueva partida.
    /// Se llama desde el boton "Reiniciar" del menu de derrota.
    /// </summary>
    public void ReiniciarPartida()
    {
        // Restaurar el tiempo antes de recargar la escena
        Time.timeScale = 1f;

        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Suma una moneda al contador y actualiza la UI.
    /// Se llama desde el script Coin cuando el jugador la recolecta.
    /// </summary>
    public void SumarMoneda()
    {
        monedas++;

        if (uiManager != null)
        {
            uiManager.ActualizarMonedas(monedas);
        }
    }

    /// <summary>
    /// Devuelve si la partida esta activa o no.
    /// Otros scripts lo usan para saber si deben ejecutar su logica.
    /// </summary>
    /// <returns>True si la partida esta activa, false si es game over</returns>
    public bool EstaPartidaActiva()
    {
        return partidaActiva;
    }

    /// <summary>
    /// Devuelve el puntaje actual como entero.
    /// </summary>
    /// <returns>Puntaje actual</returns>
    public int ObtenerPuntaje()
    {
        return (int)puntaje;
    }

    /// <summary>
    /// Devuelve la cantidad de monedas recolectadas.
    /// </summary>
    /// <returns>Cantidad de monedas</returns>
    public int ObtenerMonedas()
    {
        return monedas;
    }
}