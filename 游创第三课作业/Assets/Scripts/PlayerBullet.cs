using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 6f;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("»÷ÖÐÍæ¼Ò");
            Destroy(gameObject);
        }
    }
}
