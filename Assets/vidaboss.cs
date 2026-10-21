
using UnityEngine;
using UnityEngine.UI;

public class BarraVidaBoss : MonoBehaviour
{
    [SerializeField] private boss bossObjectiu;
    [SerializeField] private Slider barraVida;

    private void Start()
    {
        if (barraVida == null)
            barraVida = GetComponent<Slider>();

        if (bossObjectiu == null || barraVida == null)
        {
            Debug.LogError("Falta assignar el boss o la barra de vida.");
            enabled = false;
            return;
        }

        barraVida.minValue = 0;
        barraVida.maxValue = bossObjectiu.VidaMaxima;
        barraVida.value = bossObjectiu.VidaActual;
    }

    private void Update()
    {
        if (bossObjectiu != null && barraVida != null)
        {
            barraVida.value = bossObjectiu.VidaActual;
        }
    }
}
