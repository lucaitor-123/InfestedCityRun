using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Platform: Script asociado a cada plataforma individual del camino.
/// Define las 3 posiciones de carril donde pueden aparecer elementos hijos
/// (enemigos, obstaculos o monedas) y gestiona la decoracion lateral
/// de la plataforma (edificios, muros, etc).
/// La destruccion de la plataforma es gestionada por PlatformSpawner.
/// Al destruirse, todos sus hijos se destruyen automaticamente con ella.
/// </summary>
public class Platform : MonoBehaviour
{
    // --- Posiciones de spawn ---
    [Header("Posiciones de Carriles")]
    [Tooltip("Posicion del carril izquierdo donde pueden aparecer elementos")]
    public Transform puntoCarrilIzquierdo;

    [Tooltip("Posicion del carril central donde pueden aparecer elementos")]
    public Transform puntoCarrilCentro;

    [Tooltip("Posicion del carril derecho donde pueden aparecer elementos")]
    public Transform puntoCarrilDerecho;

    // --- Decoracion ---
    [Header("Decoracion Lateral")]
    [Tooltip("Array de prefabs de decoracion para el lado izquierdo (edificios, muros, etc)")]
    public GameObject[] decoracionIzquierda;

    [Tooltip("Array de prefabs de decoracion para el lado derecho")]
    public GameObject[] decoracionDerecha;

    [Tooltip("Posicion donde aparece la decoracion del lado izquierdo")]
    public Transform puntoDecoIzquierda;

    [Tooltip("Posicion donde aparece la decoracion del lado derecho")]
    public Transform puntoDecoDerecha;

    /// <summary>
    /// Start: Al generarse la plataforma, instancia decoracion aleatoria
    /// a los lados del camino si hay prefabs configurados.
    /// </summary>
    private void Start()
    {
        GenerarDecoracion();
    }

    /// <summary>
    /// Devuelve la posicion en el mundo de uno de los 3 carriles.
    /// </summary>
    /// <param name="carril">Indice del carril: 0 = izquierda, 1 = centro, 2 = derecha</param>
    /// <returns>Posicion del carril solicitado</returns>
    public Vector3 ObtenerPosicionCarril(int carril)
    {
        switch (carril)
        {
            case 0:
                if (puntoCarrilIzquierdo != null)
                    return puntoCarrilIzquierdo.position;
                break;
            case 1:
                if (puntoCarrilCentro != null)
                    return puntoCarrilCentro.position;
                break;
            case 2:
                if (puntoCarrilDerecho != null)
                    return puntoCarrilDerecho.position;
                break;
        }

        // Si no hay punto asignado, calcular la posicion manualmente
        float posX = (carril - 1) * 3f;
        return transform.position + new Vector3(posX, 0f, 0f);
    }

    /// <summary>
    /// Genera decoracion aleatoria a ambos lados de la plataforma.
    /// Los objetos de decoracion se hacen hijos de la plataforma
    /// para que se destruyan junto con ella.
    /// </summary>
    private void GenerarDecoracion()
    {
        // Decoracion lado izquierdo
        if (decoracionIzquierda != null && decoracionIzquierda.Length > 0 && puntoDecoIzquierda != null)
        {
            int indice = Random.Range(0, decoracionIzquierda.Length);
            GameObject deco = Instantiate(decoracionIzquierda[indice], puntoDecoIzquierda.position, puntoDecoIzquierda.rotation);
            deco.transform.SetParent(transform);
        }

        // Decoracion lado derecho
        if (decoracionDerecha != null && decoracionDerecha.Length > 0 && puntoDecoDerecha != null)
        {
            int indice = Random.Range(0, decoracionDerecha.Length);
            GameObject deco = Instantiate(decoracionDerecha[indice], puntoDecoDerecha.position, puntoDecoDerecha.rotation);
            deco.transform.SetParent(transform);
        }
    }
}
