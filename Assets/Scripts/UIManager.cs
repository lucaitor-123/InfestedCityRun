using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIManager: Se encarga de mostrar y actualizar los elementos de la interfaz
/// como el puntaje, las monedas y el menu de derrota.
/// </summary>
public class UIManager : MonoBehaviour
{
    // --- Referencias a los textos de la UI ---
    [Header("Textos de la UI")]
    [Tooltip("Texto que muestra el puntaje en pantalla")]
    public Text textoPuntaje;

    [Tooltip("Texto que muestra la cantidad de monedas en pantalla")]
    public Text textoMonedas;

    // --- Menu de Derrota ---
    [Header("Menu de Derrota")]
    [Tooltip("Panel del menu de derrota que se muestra al perder")]
    public GameObject panelDerrota;

    /// <summary>
    /// Start: Oculta el menu de derrota al iniciar la partida.
    /// </summary>
    private void Start()
    {
        if (panelDerrota != null)
        {
            panelDerrota.SetActive(false);
        }
    }

    /// <summary>
    /// Actualiza el texto del puntaje en pantalla.
    /// </summary>
    /// <param name="puntaje">Puntaje actual del jugador</param>
    public void ActualizarPuntaje(int puntaje)
    {
        if (textoPuntaje != null)
        {
            textoPuntaje.text = "PUNTUACION: " + puntaje.ToString("D8");
        }
    }

    /// <summary>
    /// Actualiza el texto de monedas en pantalla.
    /// </summary>
    /// <param name="monedas">Cantidad de monedas recolectadas</param>
    public void ActualizarMonedas(int monedas)
    {
        if (textoMonedas != null)
        {
            textoMonedas.text = "MONEDAS: " + monedas.ToString("D2");
        }
    }

    /// <summary>
    /// Muestra el panel del menu de derrota y actualiza los textos
    /// con el puntaje y monedas finales.
    /// </summary>
    public void MostrarMenuDerrota()
    {
        if (panelDerrota != null)
        {
            panelDerrota.SetActive(true);

            // Llamar al DeathMenu para que actualice los textos finales
            DeathMenu deathMenu = panelDerrota.GetComponent<DeathMenu>();
            if (deathMenu != null)
            {
                deathMenu.Mostrar();
            }
        }
    }

    /// <summary>
    /// Oculta el panel del menu de derrota.
    /// </summary>
    public void OcultarMenuDerrota()
    {
        if (panelDerrota != null)
        {
            panelDerrota.SetActive(false);
        }
    }
}