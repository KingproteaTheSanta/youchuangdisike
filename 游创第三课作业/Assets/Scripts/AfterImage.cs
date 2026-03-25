using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float lifeTime = 0.4f;

    private float timer;
    private SpriteRenderer sr;

    private Color startColor;
    private Vector3 startScale;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        startColor = sr.color;
        startScale = transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / lifeTime;

   
        float alpha = Mathf.SmoothStep(1f, 0f, t);
        sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

  
        float scale = Mathf.Lerp(1f, 0.85f, t);
        transform.localScale = startScale * scale;

 
        transform.position += new Vector3(0, 0.1f * Time.deltaTime, 0);

        if (t >= 1f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 完美闪避触发
        if (other.CompareTag("Bullet") || other.GetComponent<Beam>() != null)
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            player.PerfectDodge();

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}