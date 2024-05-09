using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;

    [SerializeField] private GameObject attackArea;

    private bool isGrounded = true;
    private bool isAttack = false;
    private bool isJumping = false;

    private float horizontal;

    private int coin = 0;

    private Vector3 savePoint;

    private void Awake() {
        coin = PlayerPrefs.GetInt("coin", 0);
    }

    void Update() {
        if(IsDead) {
            return;
        }
        
        isGrounded = CheckGrounded();
        
        horizontal = Input.GetAxisRaw("Horizontal");

        if(isAttack) {
            rb.velocity = Vector2.zero;
            return;
        }

        if(isGrounded) {
            if(isJumping) {
                return;
            }
            
            if(Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }

            if(Mathf.Abs(horizontal) > 0.1f && !isJumping) {
                ChangeAnim("run"); 
            }

            if(Input.GetKeyDown(KeyCode.C)) {
                Attack();
            }

            if(Input.GetKeyDown(KeyCode.V)) {
                Throw();
            } 
        }

        if(!isGrounded && rb.velocity.y < 0f) {
            ChangeAnim("fall");
            isJumping = false;
        }

        if(Mathf.Abs(horizontal) > 0.1f) {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0,horizontal>0 ? 0 : 180, 0));
        } else if ( isGrounded) {
            rb.velocity = Vector2.zero;
            ChangeAnim("idle");
        }
    }

    internal void SavePoint() {
        savePoint = transform.position;
    }

    public override void OnInit() {
        base.OnInit();
        isAttack = false;

        transform.position = savePoint;

        ChangeAnim("idle");
        DeactiveAttackArea();
        SavePoint();
        UIManager.instance.SetCoin(coin);
    }

    public void SetMove(float horizontal) {
        this.horizontal = horizontal;
    }
    
    protected override void OnDespawn() {
        base.OnDespawn();
        OnInit();
    }

    protected override void OnDeath() {
        base.OnDeath();
    }

    private void ActiveAttackArea() {
        attackArea.SetActive(true);
    }
    private void DeactiveAttackArea() {
        attackArea.SetActive(false);
    }

    private bool CheckGrounded() {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.2f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);
        
        return hit.collider != null;
    }

    public void Attack() {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttackArea();
        Invoke(nameof(DeactiveAttackArea), 0.5f);
    }

    public void Throw() {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    private void ResetAttack() {
        ChangeAnim("idle");
        isAttack = false;
    }

    public void Jump() {
        isJumping = true;
        rb.AddForce(jumpForce * Vector2.up);
        ChangeAnim("jump");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Coin") {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoin(coin);
            Destroy(collision.gameObject);
        }

        if(collision.tag == "DeathZone") {
            ChangeAnim("die");

            Invoke(nameof(OnInit), 1f);
        }
    }
}
