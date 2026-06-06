using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlatformSpawner: Se encarga de generar y gestionar las plataformas
/// que forman el camino infinito del juego "Infested City Run".
/// Genera plataformas iniciales al comenzar la partida, destruye las que
/// quedan detras del jugador y crea nuevas al final del camino.
/// Cada plataforma generada recibe un elemento hijo aleatorio
/// (enemigo, obstaculo o moneda) en uno de los 3 carriles.
/// </summary>
public class PlatformSpawner : MonoBehaviour
{
    // --- Configuracion de plataformas ---
    [Header("Configuracion de Plataformas")]
    [Tooltip("Prefab de la plataforma que se usara como suelo")]
    public GameObject prefabPlataforma;

    [Tooltip("Cantidad de plataformas que se generan al inicio de la partida")]
    public int cantidadInicial = 20;

    [Tooltip("Largo de cada plataforma en el eje Z (debe coincidir con el tamano real del prefab)")]
    public float largoPlataforma = 10f;

    // --- Referencia al jugador ---
    [Header("Referencia al Jugador")]
    [Tooltip("Transform del jugador para saber su posicion y destruir plataformas detras de el")]
    public Transform jugador;

    [Tooltip("Distancia detras del jugador a la que se destruyen las plataformas")]
    public float distanciaDestruccion = 20f;

    // --- Prefabs de elementos hijos ---
    [Header("Enemigos")]
    [Tooltip("Prefab del zombie que aparece en las plataformas")]
    public GameObject prefabZombie;

    [Header("Obstaculos (Vehiculos)")]
    [Tooltip("Array con los prefabs de los diferentes tipos de vehiculos (minimo 3)")]
    public GameObject[] prefabsVehiculos;

    [Header("Coleccionables")]
    [Tooltip("Prefab de la moneda que aparece en las plataformas")]
    public GameObject prefabMoneda;

    // --- Configuracion de spawn de hijos ---
    [Header("Probabilidades de Spawn")]
    [Tooltip("Probabilidad de que aparezca un zombie (0 a 1)")]
    [Range(0f, 1f)]
    public float probabilidadZombie = 0.3f;

    [Tooltip("Probabilidad de que aparezca un vehiculo (0 a 1)")]
    [Range(0f, 1f)]
    public float probabilidadVehiculo = 0.3f;

    [Tooltip("Probabilidad de que aparezca una moneda (0 a 1)")]
    [Range(0f, 1f)]
    public float probabilidadMoneda = 0.3f;

    // --- Posiciones de los 3 carriles ---
    [Header("Posiciones de Carriles")]
    [Tooltip("Distancia entre carriles en el eje X (debe coincidir con PlayerController)")]
    public float distanciaEntreCarriles = 3f;

    // --- Variables internas ---
    // Posicion en Z donde se generara la siguiente plataforma
    private float siguientePosicionZ = 0f;

    // Lista que almacena todas las plataformas activas en la escena
    private List<GameObject> plataformasActivas = new List<GameObject>();

    /// <summary>
    /// Start: Genera las plataformas iniciales al comenzar la partida.
    /// Las primeras 3 plataformas se generan sin obstaculos para dar
    /// espacio al jugador al inicio.
    /// </summary>
    private void Start()
    {
        siguientePosicionZ = 0f;

        for (int i = 0; i < cantidadInicial; i++)
        {
            // Las primeras 3 plataformas no tienen obstaculos para que el jugador arranque seguro
            bool conElementos = (i >= 3);
            GenerarPlataforma(conElementos);
        }
    }

    /// <summary>
    /// Update: Revisa cada frame si alguna plataforma quedo detras
    /// del jugador para destruirla y generar una nueva al final.
    /// </summary>
    private void Update()
    {
        if (jugador == null) return;

        // No hacer nada si la partida no esta activa
        if (GameManager.Instance != null && !GameManager.Instance.EstaPartidaActiva())
        {
            return;
        }

        // Revisar si la primera plataforma de la lista quedo detras del jugador
        if (plataformasActivas.Count > 0 && plataformasActivas[0] != null)
        {
            float posicionTraseraPlataforma = plataformasActivas[0].transform.position.z + largoPlataforma;

            // Si la plataforma ya quedo lejos detras del jugador, destruirla
            if (posicionTraseraPlataforma < jugador.position.z - distanciaDestruccion)
            {
                DestruirPlataforma();
            }
        }
    }

    /// <summary>
    /// Genera una nueva plataforma al final del camino.
    /// Opcionalmente le agrega un elemento hijo (enemigo, obstaculo o moneda).
    /// </summary>
    /// <param name="conElementos">Si es true, agrega un hijo aleatorio a la plataforma</param>
    private void GenerarPlataforma(bool conElementos)
    {
        // Crear la plataforma en la siguiente posicion disponible
        Vector3 posicion = new Vector3(0f, 0f, siguientePosicionZ);
        GameObject nuevaPlataforma = Instantiate(prefabPlataforma, posicion, Quaternion.identity);

        // Agregar un elemento hijo si corresponde
        if (conElementos)
        {
            GenerarElementoHijo(nuevaPlataforma);
        }

        // Registrar la plataforma y actualizar la posicion para la siguiente
        plataformasActivas.Add(nuevaPlataforma);
        siguientePosicionZ += largoPlataforma;
    }

    /// <summary>
    /// Destruye la plataforma mas antigua (la primera de la lista)
    /// y genera una nueva al final del camino para mantener el ciclo infinito.
    /// Al destruir la plataforma, sus hijos (enemigos, obstaculos, monedas)
    /// se destruyen automaticamente con ella.
    /// </summary>
    private void DestruirPlataforma()
    {
        // Destruir la plataforma mas antigua
        GameObject plataformaVieja = plataformasActivas[0];
        plataformasActivas.RemoveAt(0);
        Destroy(plataformaVieja);

        // Generar una nueva plataforma al final del camino
        GenerarPlataforma(true);
    }

    /// <summary>
    /// Genera un elemento hijo aleatorio (zombie, vehiculo o moneda)
    /// en una de las 3 posiciones de carril de la plataforma.
    /// El elemento se hace hijo de la plataforma para que se destruya con ella.
    /// </summary>
    /// <param name="plataforma">La plataforma padre donde aparecera el elemento</param>
    private void GenerarElementoHijo(GameObject plataforma)
    {
        // Elegir un carril aleatorio (0 = izquierda, 1 = centro, 2 = derecha)
        int carril = Random.Range(0, 3);
        float posicionX = (carril - 1) * distanciaEntreCarriles;

        // Posicion del elemento: centrado en la plataforma sobre el carril elegido
        Vector3 posicionHijo = plataforma.transform.position + new Vector3(posicionX, 0f, largoPlataforma / 2f);

        // Decidir que tipo de elemento generar con probabilidades
        float dado = Random.Range(0f, 1f);
        GameObject elementoGenerado = null;

        if (dado < probabilidadZombie)
        {
            // Generar un zombie
            if (prefabZombie != null)
            {
                elementoGenerado = Instantiate(prefabZombie, posicionHijo, Quaternion.identity);
            }
        }
        else if (dado < probabilidadZombie + probabilidadVehiculo)
        {
            // Generar un vehiculo aleatorio de los disponibles
            if (prefabsVehiculos != null && prefabsVehiculos.Length > 0)
            {
                int indiceAuto = Random.Range(0, prefabsVehiculos.Length);
                elementoGenerado = Instantiate(prefabsVehiculos[indiceAuto], posicionHijo, Quaternion.identity);
            }
        }
        else if (dado < probabilidadZombie + probabilidadVehiculo + probabilidadMoneda)
        {
            // Generar una moneda
            if (prefabMoneda != null)
            {
                // La moneda aparece un poco elevada para que flote sobre el suelo
                Vector3 posicionMoneda = posicionHijo + new Vector3(0f, 1f, 0f);
                elementoGenerado = Instantiate(prefabMoneda, posicionMoneda, Quaternion.identity);
            }
        }

        // Hacer el elemento hijo de la plataforma para que se destruya con ella
        if (elementoGenerado != null)
        {
            elementoGenerado.transform.SetParent(plataforma.transform);
        }
    }
}