using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public int life = 2;

    public float speed;

    public float jumpForce;

    public GameObject bulletPrefab;

    public GameObject lifesPanel;

    private Rigidbody2D rigidBody2D;

    public float horizontal;

    public float cooldownTime = 1f;

    private bool isGrounded;

    private bool isInCooldown;

    private Animator animator;

    private Vector2 initialPosition;

    private float lastShoot;


    private GameObject destinyWarp;

    public Vector2 respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        respawnPoint = initialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0){
            /*if(isGrounded){
                respawnPoint = transform.position;
            }*/
            horizontal = Input.GetAxis("Horizontal") * speed;
            if (horizontal < 0.0f)
            {
                transform.localScale = new Vector2(-1.0f, 1.0f);
            }
            else if (horizontal > 0.0f)
            {
                transform.localScale = new Vector2(1.0f, 1.0f);
            }
            animator.SetBool("isRunning", horizontal != 0.0f);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            if(!isGrounded){
                animator.SetBool("isJumping", true);
            }else{
                animator.SetBool("isJumping", false);
            }

            if(Input.GetKeyDown(KeyCode.E) && Time.time > lastShoot + 1f){
                Shoot();
                lastShoot = Time.time;
            }

            if(Input.GetKeyDown(KeyCode.S) && destinyWarp){
                transform.position = destinyWarp.transform.position;
            }

            DeathOnFall();

        }
    }

    public void Death(){
        transform.position = initialPosition;
        respawnPoint = initialPosition;
        if(life <= 0){
          life = 2;
          for (int i = 0; i < lifesPanel.transform.childCount; i++)
          {
            lifesPanel.transform.GetChild(i).gameObject.SetActive(true);
          }
        }
  
    }

    public void Hit(float knockback, GameObject enemy)
    {
    if(!isInCooldown){

        StartCoroutine(cooldown());

        if(life > 0){
        lifesPanel.transform.GetChild(life).gameObject.SetActive(false);
        life -= 1;
        if(enemy){
            Vector2 difference = (transform.position - enemy.transform.position);
            float knockbackDirection = difference.x >= 0 ? 1 : -1;
            rigidBody2D.velocity = new Vector2(knockbackDirection * knockback , knockback/2);
        }
       }else{
        Death();
       }
    }
    }

    IEnumerator cooldown(){
        isInCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isInCooldown = false;
    }
    private void Jump()
    {
        rigidBody2D.AddForce(Vector2.up * jumpForce);
    }

    private void DeathOnFall()
    {
        if (transform.position.y < -10f)
        {
            transform.position = respawnPoint;
             Hit(0, null);
             
        }
    }

    public void Shoot(){
        Vector3 direction;
        if(transform.localScale.x > 0) 
            direction = Vector3.right;
        else 
            direction = Vector3.left;
        GameObject bullet = Instantiate(bulletPrefab,transform.position + direction * 0.8f, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }

    private void FixedUpdate()
    {
        if(!isInCooldown){
            rigidBody2D.velocity = new Vector2(horizontal, rigidBody2D.velocity.y);
        }
        
    }

    
    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.name == "Tilemap") isGrounded = true;
        if(collider.name == "PointA" || collider.name == "PointB"){
            GameObject warp = collider.transform.parent.gameObject;
        if(collider.name == "PointA"){
            destinyWarp = warp.transform.Find("PointB").gameObject;
        }else{
            destinyWarp = warp.transform.Find("PointA").gameObject;
        };
        }
        
    }
 
    private void OnTriggerExit2D(Collider2D collider) {
        if(collider.name == "Tilemap") isGrounded = false;
        if(collider.name == "PointA" || collider.name == "PointB"){
           
            destinyWarp = null;
        }
    }

  
}
