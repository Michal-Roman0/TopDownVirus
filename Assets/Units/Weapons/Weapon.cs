using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    //[SerializeField] Transform firePoint;
    [SerializeField] float bulletForce = 2f;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().rotation = angle;
        //bullet.GetComponent<Rigidbody2D>().AddForce(bulletForce * direction.normalized, ForceMode2D.Impulse);
        bullet.GetComponent<Rigidbody2D>().velocity = bulletForce * direction.normalized;
	}
}
