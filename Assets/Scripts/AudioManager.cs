using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AudioManager: Gestiona toda la musica y efectos de sonido del juego.
/// Controla la musica de fondo durante la partida y la musica del menu
/// de derrota, cambiando entre ellas segun el estado del juego.
/// Implementa el patron Singleton para ser accesible desde cualquier script.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // --- Singleton ---
    // Instancia unica del AudioManager accesible desde cualquier parte del codigo
    public static AudioManager Instance;

    // --- Musica ---
    [Header("Musica")]
    [Tooltip("Musica de fondo que suena durante la partida")]
    public AudioClip musicaJuego;

    [Tooltip("Musica que suena al perder la partida (menu de derrota)")]
    public AudioClip musicaDerrota;

    [Tooltip("Volumen de la musica (0 a 1)")]
    [Range(0f, 1f)]
    public float volumenMusica = 0.5f;

    // --- Efectos de sonido ---
    [Header("Efectos de Sonido")]
    [Tooltip("Volumen de los efectos de sonido (0 a 1)")]
    [Range(0f, 1f)]
    public float volumenEfectos = 1f;

    // --- Componentes de audio ---
    // AudioSource dedicado a la musica de fondo
    private AudioSource musicaSource;

    // AudioSource dedicado a los efectos de sonido
    private AudioSource efectosSource;

    /// <summary>
    /// Awake: Configura el patron Singleton y crea los AudioSource necesarios.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Crear AudioSource para la musica
        musicaSource = gameObject.AddComponent<AudioSource>();
        musicaSource.loop = true;
        musicaSource.playOnAwake = false;
        musicaSource.volume = volumenMusica;

        // Crear AudioSource para los efectos
        efectosSource = gameObject.AddComponent<AudioSource>();
        efectosSource.loop = false;
        efectosSource.playOnAwake = false;
        efectosSource.volume = volumenEfectos;
    }

    /// <summary>
    /// Start: Inicia la musica de fondo del juego al comenzar la partida.
    /// </summary>
    private void Start()
    {
        ReproducirMusicaJuego();
    }

    /// <summary>
    /// Reproduce la musica de fondo de la partida.
    /// </summary>
    public void ReproducirMusicaJuego()
    {
        if (musicaJuego != null)
        {
            musicaSource.clip = musicaJuego;
            musicaSource.volume = volumenMusica;
            musicaSource.Play();
        }
    }

    /// <summary>
    /// Reproduce la musica del menu de derrota y detiene la musica del juego.
    /// </summary>
    public void ReproducirMusicaDerrota()
    {
        if (musicaDerrota != null)
        {
            musicaSource.clip = musicaDerrota;
            musicaSource.volume = volumenMusica;
            musicaSource.Play();
        }
        else
        {
            musicaSource.Stop();
        }
    }

    /// <summary>
    /// Detiene toda la musica que este sonando.
    /// </summary>
    public void DetenerMusica()
    {
        musicaSource.Stop();
    }

    /// <summary>
    /// Reproduce un efecto de sonido una sola vez.
    /// Se puede llamar desde cualquier script con AudioManager.Instance.ReproducirEfecto()
    /// </summary>
    /// <param name="clip">El AudioClip del efecto a reproducir</param>
    public void ReproducirEfecto(AudioClip clip)
    {
        if (clip != null)
        {
            efectosSource.PlayOneShot(clip, volumenEfectos);
        }
    }
}