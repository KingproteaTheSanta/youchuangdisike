using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour
{
    [Header("引用")]
    public GameObject bulletPrefab;
    public GameObject beamPrefab;
    public Transform player;
    public Transform bossCenter;

    [Header("全局参数")]
    public float startDelay = 1f;

    [Header("雨弹参数")]
    public float rainSpawnHeight = 4f;
    public Vector2 rainXRange = new Vector2(-5f, 5f);
    public float rainInterval = 0.4f;

    [Header("追踪弹参数")]
    public float trackingInterval = 1f;

    [Header("环形弹参数")]
    public int circleCount = 16;
    public float circleInterval = 4f;

    [Header("光束")]
    public float beamLoopInterval = 2f;

    [Header("光束 - 通用")]
    public float beamThickness = 0.3f;

    [Header("光束 - 扇形")]
    public int fanCount = 5;
    public float fanStartAngle = -60f;
    public float fanStep = 30f;

    [Header("光束 - 十字")]
    public float[] crossAngles = { 0f, 90f, 45f, -45f };

    [Header("光束 - 随机")]
    public int randomBeamCount = 6;

    [Header("光束 - 旋转")]
    public int spinBeamCount = 4;
    public float spinDuration = 2f;
    public float spinSpeed = 180f;

    void Start()
    {
        StartCoroutine(StartAttackRoutine());
    }

    //开局
    IEnumerator StartAttackRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        StartCoroutine(RainAttack());
        yield return new WaitForSeconds(1f);

        StartCoroutine(TrackingAttack());
        yield return new WaitForSeconds(1f);

        StartCoroutine(CircleAttack());
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(BeamAttackLoop());
    }

    // 弹幕
    IEnumerator RainAttack()
    {
        while (true)
        {
            Vector2 pos = new Vector2(
                Random.Range(rainXRange.x, rainXRange.y),
                rainSpawnHeight
            );

            GameObject b = Instantiate(bulletPrefab, pos, Quaternion.identity);
            b.GetComponent<Bullet>().direction = Vector2.down;

            yield return new WaitForSeconds(rainInterval);
        }
    }

    //追踪
    IEnumerator TrackingAttack()
    {
        while (true)
        {
            Vector2 pos = new Vector2(
                Random.Range(rainXRange.x, rainXRange.y),
                rainSpawnHeight
            );

            GameObject b = Instantiate(bulletPrefab, pos, Quaternion.identity);

            Vector2 dir = ((Vector2)player.position - pos).normalized;
            b.GetComponent<Bullet>().direction = dir;

            yield return new WaitForSeconds(trackingInterval);
        }
    }

    IEnumerator CircleAttack()
    {
        while (true)
        {
            for (int i = 0; i < circleCount; i++)
            {
                float angle = i * Mathf.PI * 2 / circleCount;
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                GameObject b = Instantiate(bulletPrefab, bossCenter.position, Quaternion.identity);
                b.GetComponent<Bullet>().direction = dir;
            }

            yield return new WaitForSeconds(circleInterval);
        }
    }


    IEnumerator BeamAttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(beamLoopInterval);

            int mode = Random.Range(0, 4);

            if (mode == 0) SpawnFanBeams();
            else if (mode == 1) SpawnCrossBeams();
            else if (mode == 2) SpawnRandomBeams();
            else StartCoroutine(SpinBeams());
        }
    }


    float GetBeamLength()
    {
        Camera cam = Camera.main;

        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;

        return Mathf.Max(width, height) * 1.5f;
    }


    void SpawnFanBeams()
    {
        float length = GetBeamLength();

        for (int i = 0; i < fanCount; i++)
        {
            float angle = fanStartAngle + i * fanStep;

            GameObject beam = Instantiate(beamPrefab, bossCenter.position, Quaternion.identity);
            beam.transform.rotation = Quaternion.Euler(0, 0, angle);
            beam.transform.localScale = new Vector3(length, beamThickness, 1f);
        }
    }


    void SpawnCrossBeams()
    {
        float length = GetBeamLength();

        foreach (float angle in crossAngles)
        {
            GameObject beam = Instantiate(beamPrefab, bossCenter.position, Quaternion.identity);
            beam.transform.rotation = Quaternion.Euler(0, 0, angle);
            beam.transform.localScale = new Vector3(length, beamThickness, 1f);
        }
    }


    void SpawnRandomBeams()
    {
        float length = GetBeamLength();

        for (int i = 0; i < randomBeamCount; i++)
        {
            float angle = Random.Range(0f, 360f);

            GameObject beam = Instantiate(beamPrefab, bossCenter.position, Quaternion.identity);
            beam.transform.rotation = Quaternion.Euler(0, 0, angle);
            beam.transform.localScale = new Vector3(length, beamThickness, 1f);
        }
    }



    IEnumerator SpinBeams()
    {
        float length = GetBeamLength();

        GameObject[] beams = new GameObject[spinBeamCount];

        for (int i = 0; i < spinBeamCount; i++)
        {
            float angle = i * (360f / spinBeamCount);

            beams[i] = Instantiate(beamPrefab, bossCenter.position, Quaternion.identity);
            beams[i].transform.rotation = Quaternion.Euler(0, 0, angle);
            beams[i].transform.localScale = new Vector3(length, beamThickness, 1f);
        }

        float timer = 0;

        while (timer < spinDuration)
        {
            foreach (GameObject b in beams)
            {
                if (b != null)
                    b.transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        foreach (GameObject b in beams)
        {
            if (b != null) Destroy(b);
        }
    }
}
