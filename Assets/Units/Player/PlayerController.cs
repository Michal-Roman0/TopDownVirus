using Assets.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
{
	//[SerializeField]
	//private float smoothInputSpeed=.2f;
	private CustomInput input = null;
	private Vector2 movementInput;
	private Vector2 smoothedMovementInput;
	private Vector2 movementInputSmoothVelocity;
	public float shipSpeed;
	public float responsiveness;
	public float shieldDistance;
	private bool floatUp = true;

	[SerializeField]
	private float playerSpeed = 0.5f;
	[SerializeField]
	private float maxPlayerSpeed = 50f;
	[SerializeField]
	private float idleFriction = 5f;
	[SerializeField]
	private int health = 25;
	[SerializeField]
	private Transform firePoint;
	[SerializeField] 
	private GameObject explosionEffect;
	[SerializeField]
	private ShieldController sc;
	[SerializeField]
	private float maxParryMeter;
	[SerializeField]
	private float parryMeterSpeed;
	[SerializeField]
	private float parryMeter;
	[SerializeField]
	private AudioSource audioSource;

	public float collisionOffset = 0.05f;

	public ContactFilter2D movementFilter;
	List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();


	bool canMove = true;
	bool canShoot = true;
	Rigidbody2D rigidBody;
	SpriteRenderer spriteRenderer;

	//Animator animator;
	public Camera cam;

	Vector2 mousePos;
	Vector3 shootDir;

	public int Health
	{
		get { return health; }
		set { health = value; }
	}

	private void Awake()
	{
		input = new CustomInput();
	}
	void Start()
	{
		rigidBody = gameObject.GetComponent<Rigidbody2D>();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		gameObject.GetComponent<UniversalBar>().maxValue = maxParryMeter;

		StartCoroutine(FloatTimer());
	}
	void Update()
	{
		if (parryMeter < maxParryMeter) parryMeter += parryMeterSpeed * Time.deltaTime;
		else parryMeter = maxParryMeter;

		gameObject.GetComponent<UniversalBar>().SetBarValue(parryMeter);

		if (floatUp)
		{
			transform.localScale = transform.localScale + new Vector3(0.07f, 0.07f, 0f) * Time.deltaTime;
		}
		else transform.localScale = transform.localScale - new Vector3(0.07f, 0.07f, 0f) * Time.deltaTime;
	}

	void FixedUpdate()
	{

		/*if (canMove && movementInput != Vector2.zero)
		{
			//rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, movementInput * maxPlayerSpeed, playerSpeed * Time.deltaTime);
			rigidBody.AddForce(movementInput * playerSpeed);

			if (rigidBody.velocity.magnitude > maxPlayerSpeed)
			{
				//we don't set velocity instantly to max, that would also cap the speed while being knockbacked
				float limitedSpeed = Mathf.Lerp(rigidBody.velocity.magnitude, maxPlayerSpeed, idleFriction);
				rigidBody.velocity = rigidBody.velocity.normalized * limitedSpeed;
			}
		}
		else
		{
			rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, Vector2.zero, idleFriction * Time.deltaTime);
		}*/

		smoothedMovementInput = Vector2.SmoothDamp(
		   smoothedMovementInput,
		   movementInput,
		   ref movementInputSmoothVelocity,
		   responsiveness
		   );

		rigidBody.velocity = smoothedMovementInput * shipSpeed;



		//---------------//
		//Shield steering with mouse control:
		mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		shootDir = (Vector3)mousePos - transform.position;

		shootDir = Vector3.ClampMagnitude(shootDir,4f);
		if (shootDir.sqrMagnitude < 1) shootDir = shootDir.normalized;

		float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg - 90f;
		firePoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		firePoint.position = transform.position + shieldDistance * shootDir;
		firePoint.transform.localScale = new Vector3(0.5f + shootDir.sqrMagnitude * 0.05f, 1f, 1f)*0.5f;

		// Intensywnosc zaleznie od odlegosci
		Color c = firePoint.GetComponent<SpriteRenderer>().color;
		c.a = 1f/(0.5f + shootDir.sqrMagnitude * 0.1f)-0.1f;
		firePoint.GetComponent<SpriteRenderer>().color = c;


	}
	void OnMove(InputValue movementValue)
	{
		movementInput = movementValue.Get<Vector2>();
	}
	private void OnEnable()
	{
		input.Enable();
		input.Player.Movement.performed += OnMovementPerformed;
		input.Player.Movement.canceled += OnMovementCancelled;
		input.Player.Fire.performed += OnFire;
	}
	private void OnDisable()
	{
		input.Disable();
		input.Player.Movement.performed -= OnMovementPerformed;
		input.Player.Movement.canceled -= OnMovementCancelled;
	}
	private void OnMovementPerformed(InputAction.CallbackContext value)
	{
		movementInput = value.ReadValue<Vector2>();
	}
	private void OnMovementCancelled(InputAction.CallbackContext value)
	{
		movementInput = Vector2.zero;
	}
	private void OnFire(InputAction.CallbackContext value)
	{
		//if (canShoot)
		//{
		//weapon.Shoot();
		//audioSource.Play();
		Parry();
		//}
	}

	private void Parry()
	{
		if(parryMeter == maxParryMeter)
		{
			sc.Parry();
			parryMeter = 0f;
		}
	}

	public void Damage(int damage)
	{
		Health--;
		if (Health < 1)
		{
			Instantiate(explosionEffect, transform.position, Quaternion.identity);
			//Destroy(gameObject, 0.5f);
			spriteRenderer.enabled = false;
			enabled = false;
		}
	}

	IEnumerator FloatTimer()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			floatUp = !floatUp;
		}
	}
}
