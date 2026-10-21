
using UnityEngine;
using UnityEngine.SceneManagement;

public class boss : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int vidaMaxima = 100;
    private int vidaActual;

    [Header("Cooldown de mal")]
    [SerializeField] private float cooldownMal = 1f;
    private float tempsUltimMal = -999f;

    [Header("Moviment")]
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private int direction = 1;

    [Header("Velocitat aleatoria")]
    [SerializeField] private float speedMinima = 2f;
    [SerializeField] private float speedMaxima = 7f;
    [SerializeField] private float tempsCanviVelocitat = 1.5f;

    private float temporitzadorVelocitat;

    [Header("Escena final")]
    [SerializeField] private string nomEscenaCredits = "credits";

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool mort = false;

    public int VidaActual => vidaActual;
    public int VidaMaxima => vidaMaxima;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void Start()
    {
        vidaMaxima = Mathf.Max(1, vidaMaxima);
        vidaActual = vidaMaxima;

        if (minX > maxX)
        {
            float auxiliar = minX;
            minX = maxX;
            maxX = auxiliar;
        }

        direction = direction >= 0 ? 1 : -1;

        speedMinima = Mathf.Max(0f, speedMinima);
        speedMaxima = Mathf.Max(speedMinima, speedMaxima);
        speed = Mathf.Clamp(speed, speedMinima, speedMaxima);

        tempsCanviVelocitat = Mathf.Max(0.1f, tempsCanviVelocitat);
        temporitzadorVelocitat = tempsCanviVelocitat;

        ActualitzarOrientacio();
    }

    private void Update()
    {
        if (mort)
            return;

        CanviarVelocitat();
        MoureBoss();
    }

    private void CanviarVelocitat()
    {
        temporitzadorVelocitat -= Time.deltaTime;

        if (temporitzadorVelocitat <= 0f)
        {
            speed = Random.Range(speedMinima, speedMaxima);
            temporitzadorVelocitat = tempsCanviVelocitat;
        }
    }

    private void MoureBoss()
    {
        Vector3 posicio = transform.position;

        posicio.x += direction * speed * Time.deltaTime;

        if (posicio.x >= maxX)
        {
            posicio.x = maxX;
            direction = -1;
            ActualitzarOrientacio();
        }
        else if (posicio.x <= minX)
        {
            posicio.x = minX;
            direction = 1;
            ActualitzarOrientacio();
        }

        transform.position = posicio;
    }

    private void ActualitzarOrientacio()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            // Orientació corregida
            spriteRenderer.flipX = direction > 0;
        }
    }

    public void TakeDamage(int damage)
    {
        if (mort || damage <= 0)
            return;

        if (Time.time < tempsUltimMal + cooldownMal)
            return;

        tempsUltimMal = Time.time;

        vidaActual -= damage;
        vidaActual = Mathf.Max(0, vidaActual);

        Debug.Log("Vida del boss: " + vidaActual + "/" + vidaMaxima);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        if (mort)
            return;

        mort = true;

        Debug.Log("Boss derrotat!");

        if (Application.CanStreamedLevelBeLoaded(nomEscenaCredits))
        {
            SceneManager.LoadScene(nomEscenaCredits);
        }
        else
        {
            Debug.LogError(
                "No es pot carregar l'escena '" +
                nomEscenaCredits +
                "'. Comprova el nom a Build Profiles."
            );
        }
    }
}
