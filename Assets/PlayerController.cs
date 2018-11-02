using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float jumpforce;
	public Rigidbody2D fruta;

	private Rigidbody2D rb;
	private bool facingRight = true;
	private bool jump = false;
	private Animator anim; 
	private bool noChao = false;
	private Transform groundCheck;	
	private int collectedItens = 0;	

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();
		anim = gameObject.GetComponent<Animator>();
		groundCheck = gameObject.transform.Find("GroundCheck");
	}
	
	// Update is called once per frame
	void Update () {
		noChao = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));		

		if(Input.GetButtonDown("Jump") && noChao){
			jump = true; 
			anim.SetTrigger("Pulou");
		}

		if(Input.GetKeyUp(KeyCode.H)){
			Attack();
		}
		
	}
	void FixedUpdate(){
		float h = Input.GetAxisRaw("Horizontal");
		anim.SetFloat("Velocidade", Mathf.Abs(h));
		Debug.Log(h);
		rb.velocity = new Vector2( h * speed , rb.velocity.y);
	
		if(h > 0 && !facingRight){
			flip();
		} 	
		else if(h < 0 && facingRight){
			flip();
		}
		if( jump ) {
			rb.AddForce(new Vector2(0, jumpforce));
			jump = false;
		}
		if(Input.GetKeyDown(KeyCode.V)){
			Run();
		}
		if(Input.GetKeyUp(KeyCode.V)){
			NormalRun();
		}
	}
	void fall(){
		
		// anim.SetTrigger("Morreu");
	}

	void Attack(){
		Rigidbody2D clone;
		clone = Instantiate(fruta, transform.position, transform.rotation );
		clone.velocity = transform.TransformDirection(Vector3.forward * 10);
	}

	void flip(){
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;  
	}

	void CollectItem(GameObject gameObject)	{
		Debug.Log("Item coletado! total de itens:"+collectedItens);
		this.collectedItens++;
		Destroy(gameObject);	
	}

	void Run(){
		speed = speed * 2;
	}
	void NormalRun(){
		speed = speed / 2;
	}
	void OnCollisionEnter2D (Collision2D col){		
		if(col.gameObject.tag == "collectable"){
			CollectItem(col.gameObject);
		}
	}
}
