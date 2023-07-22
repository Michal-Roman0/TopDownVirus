using Assets.Units;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected float shootingDelay;
    [SerializeField] protected Weapon weapon;
    [SerializeField] protected GameObject target;

    [SerializeField] protected GameObject explosionEffect;
    protected Rigidbody2D rb;

	public int Health {
        get { return health; }
        set { health = value; } 
    }

	protected void Start()
    {
        Init();

        rb.velocity = Vector2.up * speed;
    }

    protected virtual void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player");

        StartCoroutine(ShootTimer());
    }
    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        
    }

    IEnumerator ShootTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootingDelay);
            Vector2 direction = target.transform.position - transform.position;
			gameObject.GetComponent<HitFlash>().Flash(5f);
			weapon.Shoot(direction);
        }


    }

	public void Damage(int damage)
	{
        Health--;
        if(Health < 1)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<HitFlash>().Flash(25f);
            Destroy(gameObject, 0.1f);
        }
	}
}
