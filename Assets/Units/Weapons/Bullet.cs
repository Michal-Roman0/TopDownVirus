using Assets.Units;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting.APIUpdating;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float reflectCooldown = 0.1f;
    private Rigidbody2D rb;
	private AudioSource[] audio;
	public Light2D glow;
	public GameObject endEffect;
    public bool playerReflected = false;
    float reflectCooldownEnd;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
		audio = gameObject.GetComponents<AudioSource>();
        reflectCooldownEnd = Time.time + reflectCooldown;
		Destroy(gameObject, 4f);
    }

    void Update()
    {
        //gameObject.transform.localScale = new Vector3(0.1f, rb.velocity.magnitude * 0.2f, 0.1f);
    }
	private void OnTriggerEnter2D(Collider2D other)
	{
		IDamagable hit = other.GetComponent<IDamagable>();
        if (hit != null)
        {
            if(other.CompareTag("Enemy") && playerReflected)
            {
                hit.Damage(1);
                DespawnBullet();
            }
            else if (other.CompareTag("Player") && !playerReflected)
            {
				hit.Damage(1);
				DespawnBullet();
			}
        }
		//change to Reflectable?
		Debug.Log("Siema" + other.tag);
        if (other.CompareTag("Shield") && Time.time > reflectCooldownEnd)
        {

			//rb.velocity = -rb.velocity;
			//Vector2 bullet_vel_norm = rb.velocity.normalized;
			//Vector2 player_pos = GameObject.FindWithTag("Player").transform.position;
			//Vector2 player_to_shield_vector = (Vector2)transform.position - player_pos;
			//Vector2 player_to_shield_vector = (Vector2)transform.position - player_pos;
			//rb.velocity = Vector2.Reflect(rb.velocity, player_to_shield_vector);

			/* // start
            float p1 = GameObject.FindWithTag("Player").transform.position.x;
            float p2 = GameObject.FindWithTag("Player").transform.position.y;
            float c1 = transform.position.x;
            float c2 = transform.position.y;
            float v1 = rb.velocity.x;
            float v2 = rb.velocity.y;
            float s1 = p1 - c1;
            float s2 = p2 - c2;
            float a1 = s1 / s2;
            float a2 = s2 / s1;
            float x1 = ((a1 * c1 - v2 + a2 * (c1 + v1)) / (a1 + a2));
            float V1 = 2 * x1 - c1 - v1;
            float V2 = 2 * (a2 * x1 + c2 + v2 - a2*(c1 + v1)-c2 - v2);
            rb.velocity = new Vector2(V1, V2);
			// end */

			playerReflected = !playerReflected;

        }
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		IDamagable hit = collision.gameObject.GetComponent<IDamagable>();
		if (hit != null)
		{
			if (collision.gameObject.CompareTag("Enemy") && playerReflected)
			{
				hit.Damage(1);
				DespawnBullet();
			}
			else if (collision.gameObject.CompareTag("Player") && !playerReflected)
			{
				hit.Damage(1);
				DespawnBullet();
			}
		}
		if (collision.gameObject.CompareTag("Shield") && Time.time > reflectCooldownEnd)
        {
			//reflection
			Reflect(collision);
			playerReflected = true;
			gameObject.layer = LayerMask.NameToLayer("PlayerBullets");
			gameObject.GetComponentInChildren<Light2D>().color = Color.cyan;
		}
		else if(collision.gameObject.CompareTag("Wall") && Time.time > reflectCooldownEnd)
		{
			Reflect(collision);
		}
	}
	private void Reflect(Collision2D collision)
	{
		Vector3 reflectVector = collision.contacts[0].normal;
		Vector3 vel = Vector3.Reflect(rb.velocity, -reflectVector);
		rb.velocity = vel;

		reflectCooldownEnd = Time.time + reflectCooldown;

		audio[1].Play();
	}
	private void DespawnBullet()
    {
		gameObject.GetComponent<Collider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponentInChildren<Light2D>().enabled = false;
		Instantiate(endEffect, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.5f);
    }
	public void ChangeGlowColor(Color color)
	{
		glow.color = color;
	}
}
