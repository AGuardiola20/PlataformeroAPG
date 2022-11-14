using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{

    public float speed = 5;
    public float direction = 1;
    public float directionTimeChange = 4f;
    private Rigidbody2D rigidbody2D;
    private GameObject wallL;
    private GameObject wallR;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
       // StartCoroutine(DirectionChange());
        wallL = transform.parent.Find("wallL").gameObject;
        wallR = transform.parent.Find("wallR").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //Fixed Udpate
    private void FixedUpdate() {
        rigidbody2D.velocity = new Vector2(direction * speed, rigidbody2D.velocity.y);  
    }

    //Corrutina
    /*IEnumerator DirectionChange(){
        while (true)
        {
        yield return new WaitForSeconds(directionTimeChange);
        Turn();
        }
    }*/

     private new void OnTriggerEnter2D(Collider2D collider) {

        base.OnTriggerEnter2D(collider);

       if(collider.gameObject == wallL || collider.gameObject == wallR ){
            Turn();
       }
    }

    private void Turn(){
        direction = direction * -1;
        transform.localScale = new Vector2(-transform.localScale.x,transform.localScale.y);
    }
}
