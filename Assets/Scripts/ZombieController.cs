using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private enum ControlMode
    {
        /// <summary>
        /// Up moves the character forward, left and right turn the character gradually and down moves the character backwards
        /// </summary>
        Tank,
        /// <summary>
        /// Character freely moves in the chosen direction from the perspective of the camera
        /// </summary>
        Direct,
        Target
    }

    public Transform target;
    NavMeshAgent nav;


    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;

    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Tank;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;

    private Vector3 m_currentDirection = Vector3.zero;

    private void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); }
        nav = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        switch (m_controlMode)
        {
            case ControlMode.Direct:
                DirectUpdate();
                break;

            case ControlMode.Tank:
                TankUpdate();
                break;
            case ControlMode.Target:
                Targeting();
                break;
            default:
                Debug.LogError("Unsupported state");
                break;
        }

        //nav.SetDestination(target.position);
        //nav.speed = m_moveSpeed;
    }

    private void TankUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        m_animator.SetFloat("MoveSpeed", m_currentV);
    }

    private void DirectUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }
    }

    /*
     * <개조한 코드>
     * target(플레이어) 와의 거리를 구한다.
     * 일정 거리 (게임상에서 몬스터가 플레이어 인식) 미만에서
     * 타겟까지의 방향을 구한다.
     * 그 후에 타겟을 바라보게 회전하고 그 방향으로 계속 움직인다.
     * 마지막으로는 움직이는 애니메이션을 재생한다.
     */
    private void Targeting()
    {
        float rotationSpeed = 3.0f;
        float minDistance = 3.0f;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= minDistance)
        {
            Vector3 directionToTarget = target.position - transform.position;
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationSpeed * Time.deltaTime);
            if (directionToTarget != Vector3.zero)
            {
                m_currentDirection = Vector3.Slerp(m_currentDirection, directionToTarget, Time.deltaTime * m_interpolation);

                transform.rotation = Quaternion.LookRotation(m_currentDirection);
                transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

                m_animator.SetFloat("MoveSpeed", directionToTarget.magnitude);
            }

        }
    }
}
