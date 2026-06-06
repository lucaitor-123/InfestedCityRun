using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// DeathMenu: Controla el menu de derrota que aparece cuando el jugador pierde.
/// Muestra el puntaje final, las monedas recolectadas y un boton para reiniciar.
/// El boton tiene animaciones al pasar el raton por encima y al presionarlo,
/// tal como solicita el enunciado.
/// </summary>
public class DeathMenu : MonoBehaviour
{
    // --- Referencias UI ---
    [Header("Textos del Menu")]
    [Tooltip("Texto que muestra el mensaje de derrota")]
    public Text textoDerrota;

    [Tooltip("Texto que muestra el puntaje final obtenido")]
    public Text textoPuntajeFinal;

    [Tooltip("Texto que muestra las monedas recolectadas")]
    public Text textoMonedasFinal;

    [Header("Boton de Reinicio")]
    [Tooltip("Boton para reiniciar la partida")]
    public Button botonReiniciar;

    // --- Configuracion visual ---
    [Header("Animacion del Boton")]
    [Tooltip("Color normal del boton")]
    public Color colorNormal = new Color(0.8f, 0.2f, 0.2f, 1f);

    [Tooltip("Color del boton al pasar el raton por encima")]
    public Color colorHover = new Color(1f, 0.4f, 0.4f, 1f);

    [Tooltip("Color del boton al presionarlo")]
    public Color colorPresionado = new Color(0.6f, 0.1f, 0.1f, 1f);

    [Tooltip("Escala del boton al pasar el raton por encima")]
    public float escalaHover = 1.1f;

    // Escala original del boton para restaurarla
    private Vector3 escalaOriginal;

    /// <summary>
    /// Start: Configura el boton de reinicio con sus colores de animacion
    /// y asigna la funcion de reiniciar al evento onClick.
    /// </summary>
    private void Start()
    {
        if (botonReiniciar != null)
        {
            // Guardar escala original
            escalaOriginal = botonReiniciar.transform.localScale;

            // Configurar las transiciones de color del boton
            ColorBlock colores = botonReiniciar.colors;
            colores.normalColor = colorNormal;
            colores.highlightedColor = colorHover;
            colores.pressedColor = colorPresionado;
            colores.selectedColor = colorNormal;
            colores.fadeDuration = 0.1f;
            botonReiniciar.colors = colores;

            // Asignar la funcion de reiniciar al boton
            botonReiniciar.onClick.AddListener(Reiniciar);

            // Agregar eventos de hover para la animacion de escala
            EventTrigger trigger = botonReiniciar.gameObject.AddComponent<EventTrigger>();

            // Evento al entrar el raton
            EventTrigger.Entry entradaRaton = new EventTrigger.Entry();
            entradaRaton.eventID = EventTriggerType.PointerEnter;
            entradaRaton.callback.AddListener((data) => { AlPasarRaton(); });
            trigger.triggers.Add(entradaRaton);

            // Evento al salir el raton
            EventTrigger.Entry salidaRaton = new EventTrigger.Entry();
            salidaRaton.eventID = EventTriggerType.PointerExit;
            salidaRaton.callback.AddListener((data) => { AlSalirRaton(); });
            trigger.triggers.Add(salidaRaton);
        }
    }

    /// <summary>
    /// Muestra el menu de derrota con el puntaje y monedas finales.
    /// Cambia la musica a la de derrota.
    /// </summary>
    public void Mostrar()
    {
        gameObject.SetActive(true);

        // Mostrar puntaje final
        if (textoPuntajeFinal != null && GameManager.Instance != null)
        {
            textoPuntajeFinal.text = "PUNTUACION: " + GameManager.Instance.ObtenerPuntaje().ToString("D8");
        }

        // Mostrar monedas finales
        if (textoMonedasFinal != null && GameManager.Instance != null)
        {
            textoMonedasFinal.text = "MONEDAS: " + GameManager.Instance.ObtenerMonedas().ToString("D2");
        }

        // Cambiar a musica de derrota
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirMusicaDerrota();
        }
    }

    /// <summary>
    /// Reiniciar: Se ejecuta al presionar el boton de reinicio.
    /// Llama al GameManager para recargar la escena.
    /// </summary>
    private void Reiniciar()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReiniciarPartida();
        }
    }

    /// <summary>
    /// AlPasarRaton: Agranda el boton cuando el raton pasa por encima.
    /// </summary>
    private void AlPasarRaton()
    {
        if (botonReiniciar != null)
        {
            botonReiniciar.transform.localScale = escalaOriginal * escalaHover;
        }
    }

    /// <summary>
    /// AlSalirRaton: Restaura el tamano original del boton cuando el raton sale.
    /// </summary>
    private void AlSalirRaton()
    {
        if (botonReiniciar != null)
        {
            botonReiniciar.transform.localScale = escalaOriginal;
        }
    }
}