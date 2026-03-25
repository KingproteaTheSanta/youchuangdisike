using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour
{
    public float warningTime = 0.8f;        // 预警持续时间
    public float duration = 2f;             // 光束持续时间
    public bool horizontal = true;          // 光束方向
    public bool triggerPerfectDodge = true; // 是否触发残影闪避

    private SpriteRenderer sr;
    private Collider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (sr == null)
        {
            sr = gameObject.AddComponent<SpriteRenderer>();
            sr.color = Color.red; // 默认红色
        }

        if (col != null) col.enabled = false; // 先关闭碰撞
    }

    void Start()
    {
        // 开启协程处理预警
        StartCoroutine(BeamRoutine());
    }

    IEnumerator BeamRoutine()
    {
        // -------------------
        // 预警阶段
        // -------------------
        sr.color = new Color(1f, 0f, 0f, 0.5f); // 半透明红色
        yield return new WaitForSeconds(warningTime);

        // -------------------
        // 正式发射光束
        // -------------------
        sr.color = new Color(1f, 0f, 0f, 1f); // 实心红色
        if (col != null) col.enabled = true;

        yield return new WaitForSeconds(duration);

        // 销毁光束
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 玩家碰到光束 → 游戏结束
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && !player.isDodging)
            {
                Debug.Log("玩家被光束击中！游戏结束");
                Destroy(player.gameObject);
            }
        }

        // 残影触发完美闪避
        AfterImage after = other.GetComponent<AfterImage>();
        if (after != null && triggerPerfectDodge)
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            player.PerfectDodge();
        }
    }
}
