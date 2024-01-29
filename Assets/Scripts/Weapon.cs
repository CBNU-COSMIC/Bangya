using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float sensitivity = 2.0f; // 마우스 감도
    //public Transform playerBody; // 플레이어 캐릭터의 Transform

    float rotationX = 0f;

    void RotateGunAndPlayer()
    {
        // 마우스 입력 감지
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;

        // 플레이어 캐릭터를 수평으로 회전
        //playerBody.Rotate(Vector3.up * mouseX);

        // 마우스 입력 감지
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // 수직 회전 값 갱신
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // 총구를 수직으로 회전
        shootPoint.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    /// <summary>
    /// ///////////////
    /// </summary>

    // Weapon Specification
    public string weaponName;
    public int bulletsPerMag;
    public int bulletsTotal;
    public int currentBullets;
    public float range;
    public float fireRate;

    // Parameters
    private float fireTimer = 0;

    // References
    public Transform shootPoint;
    public ParticleSystem muzzleFlash;
    private Animator anim;

    // Sound
    public AudioClip shootSound;
    public AudioSource audioSource;

    // Use this for initialization
    private void Start()
    {
        currentBullets = bulletsPerMag;
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        // 마우스 움직임을 감지하여 총구와 플레이어를 회전
        RotateGunAndPlayer();

        Debug.DrawRay(shootPoint.position, transform.forward*100, Color.blue);

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Monster"))
            {
                Debug.Log("나이스샷");
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (currentBullets > 0)
                Fire();
        }

        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }

    private void Fire()
    {
        if (fireTimer < fireRate)
        {
            return;
        }
        Debug.Log("Shot Fired!");
        Debug.DrawRay(shootPoint.position, Vector3.forward, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
            Debug.Log("Hit!");
        }
        currentBullets--;
        fireTimer = 0.0f;
        //audioSource.PlayOneShot(shootSound); // shoot sound
        //anim.CrossFadeInFixedTime("Fire", 0.01f); // fire animation
        //muzzleFlash.Play();
    }
}