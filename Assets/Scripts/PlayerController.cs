using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerController: Controla al jugador del juego "Infested City Run".
/// Gestiona el movimiento horizontal entre 3 carriles, el salto,
/// el avance automatico hacia adelante, el disparo de proyectiles
/// y las colisiones con enemigos, obstaculos y monedas.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // --- Movimiento hacia adelante ---
    [Header("Movimiento hacia adelante")]
    [Tooltip("Velocidad constante a la que el jugador avanza hacia adelante")]
    public float velocidadAvance = 10f;

    // --- Movimiento horizontal (carriles) ---
    [Header("Movimiento horizontal")]
    [Tooltip("Distancia entre cada carril (centro a centro)")]
    public float distanciaEntreCarriles = 3f;

    [Tooltip("Velocidad a la que el jugador se mueve entre carriles")]
    public float velocidadLateral = 10f;

    // Carril actual del jugador: 0 = izquierda, 1 = centro, 2 = derecha
    private int carrilActual = 1;

    // Posicion horizontal objetivo hacia donde se mueve el jugador
    private float posicionObjetivoX;

    // --- Salto ---
    [Header("Salto")]
    [Tooltip("Fuerza del salto del jugador")]
    public float fuerzaSalto = 8f;

    [Tooltip("Gravedad personalizada para que el salto se sienta bien")]
    public float gravedad = -20f;

    // Velocidad vertical actual del jugador
    private float velocidadVertical = 0f;

    // Indica si el jugador esta en el suelo
    private bool enSuelo = true;

    // Referencia al CharacterController para mover al jugador
    private CharacterController controlador;

    // --- Disparo ---
    [Header("Disparo")]
    [Tooltip("Prefab del proyectil (bala) que dispara el jugador")]
    public GameObject prefabBala;

    [Tooltip("Punto de origen desde donde se dispara la bala")]
    public Transform puntoDisparo;

    [Tooltip("Tiempo minimo entre cada disparo en segundos")]
    public float cooldownDisparo = 0.5f;

    // Momento del ultimo disparo para controlar el cooldown
    private float ultimoDisparo = 0f;

    // --- Audio ---
    [Header("Audio")]
    [Tooltip("Sonido que se reproduce al disparar")]
    public AudioClip sonidoDisparo;

    // Componente de audio para reproducir efectos de sonido
    private AudioSource audioSource;

    // --- Animacion ---
    // Referencia al Animator para controlar las animaciones del jugador
    private Animator animator;

    /// <summary>
    /// Start: Se ejecuta al inicio. Obtiene las referencias necesarias
    /// y calcula la posicion inicial del jugador en el carril central.
    /// </summary>
    private void Start()
    {
        controlador = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();

        // Posicion inicial en el carril central
        posicionObjetivoX = 0f;
    }

    /// <summary>
    /// Update: Se ejecuta cada frame.
    /// Gestiona las entradas del jugador y aplica el movimiento.
    /// </summary>
    private void Update()
    {
        // No hacer nada si la partida no esta activa
        if (GameManager.Instance != null && !GameManager.Instance.EstaPartidaActiva())
        {
            return;
        }

        // Procesar las entradas del jugador
        ProcesarMovimientoLateral();
        ProcesarSalto();
        ProcesarDisparo();

        // Aplicar el movimiento final
        AplicarMovimiento();
    }

    /// <summary>
    /// Detecta las teclas A y D para cambiar de carril.
    /// El jugador se mueve entre 3 carriles: izquierda (0), centro (1), derecha (2).
    /// </summary>
    private void ProcesarMovimientoLateral()
    {
        // Mover a la izquierda con la tecla A
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (carrilActual > 0)
            {
                carrilActual--;
            }
        }

        // Mover a la derecha con la tecla D
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (carrilActual < 2)
            {
                carrilActual++;
            }
        }

        // Calcular la posicion X objetivo segun el carril
        // Carril 0 = -distancia, Carril 1 = 0, Carril 2 = +distancia
        posicionObjetivoX = (carrilActual - 1) * distanciaEntreCarriles;
    }

    /// <summary>
    /// Detecta la tecla Espacio para saltar.
    /// Solo permite saltar si el jugador esta en el suelo.
    /// </summary>
    private void ProcesarSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            velocidadVertical = fuerzaSalto;
            enSuelo = false;
        }
    }

    /// <summary>
    /// Detecta la tecla F para disparar un proyectil hacia adelante.
    /// Respeta un cooldown minimo entre disparos.
    /// </summary>
    private void ProcesarDisparo()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Verificar si ya paso el tiempo de cooldown
            if (Time.time - ultimoDisparo >= cooldownDisparo)
            {
                Disparar();
                ultimoDisparo = Time.time;
            }
        }
    }

    /// <summary>
    /// Instancia un proyectil en el punto de disparo y lo lanza hacia adelante.
    /// Reproduce el sonido de disparo si esta configurado.
    /// </summary>
    private void Disparar()
    {
        if (prefabBala != null && puntoDisparo != null)
        {
            Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);

            // Reproducir sonido de disparo
            if (audioSource != null && sonidoDisparo != null)
            {
                audioSource.PlayOneShot(sonidoDisparo);
            }
        }
    }

    /// <summary>
    /// Aplica el movimiento final al jugador combinando:
    /// - Avance automatico hacia adelante (eje Z)
    /// - Movimiento lateral suave entre carriles (eje X)
    /// - Gravedad y salto (eje Y)
    /// </summary>
    private void AplicarMovimiento()
    {
        // Movimiento hacia adelante constante
        float movimientoZ = velocidadAvance * Time.deltaTime;

        // Movimiento lateral suave hacia el carril objetivo
        float posicionActualX = transform.position.x;
        float movimientoX = (posicionObjetivoX - posicionActualX) * velocidadLateral * Time.deltaTime;

        // Aplicar gravedad
        if (enSuelo && velocidadVertical < 0)
        {
            velocidadVertical = -1f;
        }
        velocidadVertical += gravedad * Time.deltaTime;
        float movimientoY = velocidadVertical * Time.deltaTime;

        // Combinar todo en un solo vector de movimiento
        Vector3 movimiento = new Vector3(movimientoX, movimientoY, movimientoZ);

        // Mover al jugador usando el CharacterController
        controlador.Move(movimiento);

        // Verificar si el jugador toco el suelo despues del movimiento
        if (controlador.isGrounded)
        {
            enSuelo = true;
            velocidadVertical = -1f;
        }
    }

    /// <summary>
    /// OnTriggerEnter: Se ejecuta cuando el jugador entra en contacto
    /// con un Trigger (collider marcado como "Is Trigger").
    /// Detecta monedas para recolectarlas.
    /// </summary>
    /// <param name="other">El collider del objeto con el que se hizo contacto</param>
    private void OnTriggerEnter(Collider other)
    {
        
    }

    /// <summary>
    /// OnControllerColliderHit: Se ejecuta cuando el CharacterController
    /// colisiona fisicamente con otro collider (no trigger).
    /// Detecta enemigos y obstaculos para terminar la partida.
    /// </summary>
    /// <param name="hit">Informacion de la colision</param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Si choca con un enemigo u obstaculo, es Game Over
        if (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Obstacle"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}