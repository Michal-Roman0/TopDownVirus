using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShieldController : MonoBehaviour
{
    private Collider2D col;
    private AudioSource[] audio;
    public bool parryActive;
    public RaycastHit2D[] rayHit = new RaycastHit2D[10];
	[SerializeField] GameObject parryBulletPrefab;

	void Start()
    {
        col = GetComponent<Collider2D>();
        audio = GetComponents<AudioSource>();
    }

    void Update()
    {

    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
        gameObject.GetComponent<HitFlash>().Flash(8f);
        
    }
    public void Parry()
    {
		int collisions = col.Cast(transform.up, new ContactFilter2D(), rayHit, 0.8f);
		if (collisions > 0)
		{
            for(int i = 0; i < collisions; i++)
            {
                if (rayHit[i].transform.CompareTag("Bullet"))
                {
					Debug.Log(rayHit.Length);
					Debug.DrawRay(rayHit[i].point, transform.up * 0.05f, Color.green, 10f);
                    //rayHit[i].transform.GetComponent<SpriteRenderer>().color = Color.red;

                    //rayHit[i].rigidbody.velocity *= 2;
                    Destroy(rayHit[i].transform.gameObject);
				}

			}
            gameObject.GetComponent<HitFlash>().Flash(40f);
            parryActive = true;

			audio[1].Play();

			StartCoroutine(ParryAttackTimer());

            audio[0].Play();
		}
		Debug.DrawRay(transform.position, transform.up * 0.5f, Color.yellow, 10f);
	}


    IEnumerator ParryAttackTimer()
    {
		yield return new WaitForSeconds(1.5f);
        audio[1].Stop();

        GameObject parryBull = Instantiate(parryBulletPrefab, transform.position, Quaternion.identity);
        parryBull.GetComponent<Rigidbody2D>().velocity = transform.up * 10f;
		gameObject.GetComponent<HitFlash>().Flash(40f);
    }

}
