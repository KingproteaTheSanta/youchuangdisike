using UnityEngine;

public class Item : MonoBehaviour
{
    public float fallSpeed = 2f;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAttack pa = other.GetComponent<PlayerAttack>();

            if (pa != null)
            {
                if (CompareTag("item1")) pa.UnlockSpread();
                if (CompareTag("item2")) pa.UnlockBeam();
                if (CompareTag("item3")) pa.UnlockHoming();
            }

            Destroy(gameObject);
        }
    }
}
