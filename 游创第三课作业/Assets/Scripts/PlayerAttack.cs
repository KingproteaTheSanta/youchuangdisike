using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject beamPrefab;

    public KeyCode attackKey = KeyCode.J;

    private Transform targetPlayer;

    private bool hasSpread = false;
    private bool hasBeam = false;
    private bool hasHoming = false;

    void Start()
    {
        FindTarget();
    }

    void Update()
    {
        if (targetPlayer == null)
            FindTarget();

        if (Input.GetKeyDown(attackKey))
        {
            if (hasSpread) Spread();
            if (hasBeam) Beam();
            if (hasHoming) Homing();
        }
    }

    void FindTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var p in players)
        {
            if (p != gameObject)
            {
                targetPlayer = p.transform;
                break;
            }
        }
    }

    //解锁能力
    public void UnlockSpread() => hasSpread = true;
    public void UnlockBeam() => hasBeam = true;
    public void UnlockHoming() => hasHoming = true;

    // 散射
    void Spread()
    {
        int count = 8;

        Vector2 baseDir = (targetPlayer.position - transform.position).normalized;

        for (int i = 0; i < count; i++)
        {
            float angle = i * (360f / count);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * baseDir;

            GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            b.tag = "Bullet";
            b.GetComponent<Bullet>().direction = dir;
        }
    }

    //光束（Boss同款）
    void Beam()
    {
        Vector2 dir = (targetPlayer.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        GameObject beam = Instantiate(beamPrefab, transform.position, Quaternion.identity);
        beam.tag = "Bullet";

        beam.transform.rotation = Quaternion.Euler(0, 0, angle);

        float length = GetBeamLength();
        beam.transform.localScale = new Vector3(length, 0.3f, 1f);
    }

    //追踪
    void Homing()
    {
        int count = 4;

        for (int i = 0; i < count; i++)
        {
            GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            b.tag = "Bullet";

            Vector2 dir = (targetPlayer.position - transform.position).normalized;
            b.GetComponent<Bullet>().direction = dir;
        }
    }

    float GetBeamLength()
    {
        Camera cam = Camera.main;

        float h = cam.orthographicSize * 2f;
        float w = h * cam.aspect;

        return Mathf.Max(w, h) * 1.5f;
    }
}
