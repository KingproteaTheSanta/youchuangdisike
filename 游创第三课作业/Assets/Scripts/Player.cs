using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("移动")]
    public float speed = 6f;

    public InputMode moveInputMode = InputMode.Axis;

    // Key 模式
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    // Axis 模式
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    private Vector2 input;

    [Header("冲刺")]
    public float dashSpeed = 15f;
    public float dashTime = 0.1f;
    private bool isDashing = false;
    private Vector2 dashDirection;

    public InputMode dashInputMode = InputMode.Key;
    public KeyCode dashKey = KeyCode.LeftShift;
    public string dashAxis = "Fire3";

    [Header("残影")]
    public GameObject afterImagePrefab;
    public float afterImageInterval = 0.02f;
    private float afterImageTimer;

    [Header("完美闪避")]
    public float dodgeTime = 2f;
    [HideInInspector] public bool isDodging = false;
    public GameObject dodgeEffectPrefab;

    [Header("特效")]
    public Sprite normalSprite;
    public Sprite dodgeSprite;

    private SpriteRenderer sr;
    private Color normalColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        normalColor = sr.color;

        if (normalSprite == null)
            normalSprite = sr.sprite;
    }

    void Update()
    {
        HandleInput();

        // 冲刺
        if (CheckDashInput() && !isDashing)
        {
            dashDirection = input != Vector2.zero ? input : Vector2.up;
            StartCoroutine(Dash());
        }

        // 残影
        afterImageTimer -= Time.deltaTime;
        if (afterImageTimer <= 0 && (input != Vector2.zero || isDashing))
        {
            SpawnAfterImage();
            afterImageTimer = afterImageInterval;
        }

        // 移动
        if (!isDashing)
        {
            transform.position += (Vector3)(input * speed * Time.deltaTime);
        }
    }

    // 输入
    void HandleInput()
    {
        input = Vector2.zero;

        if (moveInputMode == InputMode.Axis)
        {
            input.x = Input.GetAxisRaw(horizontalAxis);
            input.y = Input.GetAxisRaw(verticalAxis);
        }
        else if (moveInputMode == InputMode.Key)
        {
            if (Input.GetKey(upKey)) input.y += 1;
            if (Input.GetKey(downKey)) input.y -= 1;
            if (Input.GetKey(leftKey)) input.x -= 1;
            if (Input.GetKey(rightKey)) input.x += 1;
        }

        input = input.normalized;
    }

    bool CheckDashInput()
    {
        if (dashInputMode == InputMode.Key)
        {
            return Input.GetKeyDown(dashKey);
        }
        else if (dashInputMode == InputMode.Axis)
        {
            return Input.GetButtonDown(dashAxis);
        }

        return false;
    }

    // 残影
    void SpawnAfterImage()
    {
        if (afterImagePrefab)
        {
            GameObject img = Instantiate(afterImagePrefab, transform.position, Quaternion.identity);
            img.GetComponent<SpriteRenderer>().sprite = sr.sprite;
        }
    }

    //冲刺
    IEnumerator Dash()
    {
        isDashing = true;

        float timer = 0;
        while (timer < dashTime)
        {
            transform.position += (Vector3)(dashDirection * dashSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    //完美闪避
    public void PerfectDodge()
    {
        if (!isDodging)
            StartCoroutine(DodgeRoutine());
    }

    IEnumerator DodgeRoutine()
    {
        isDodging = true;

        sr.color = Color.cyan;

        if (dodgeSprite != null)
            sr.sprite = dodgeSprite;

        if (dodgeEffectPrefab)
            Instantiate(dodgeEffectPrefab, transform.position, Quaternion.identity);

        yield return new WaitForSecondsRealtime(0.2f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.04f;

        yield return new WaitForSeconds(dodgeTime);

        sr.color = normalColor;
        sr.sprite = normalSprite;

        isDodging = false;
    }

    //受击
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDodging && (other.CompareTag("Bullet") || other.GetComponent<Beam>() != null))
        {
            Debug.Log("死亡");
            GameObject.FindObjectOfType<GameManager>().GameOver();
            Destroy(gameObject);
        }
    }
}

// 输入
public enum InputMode
{
    Axis,
    Key
}