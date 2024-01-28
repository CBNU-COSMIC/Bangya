using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster1 : MonoBehaviour
{

Transform target;
    NavMeshAgent nmAgent;
    Animator anim;

    float HP = 0;
    public float lostDistance;

    enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        KILLED
    }

    State state;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();

        HP = 10;
        state = State.IDLE;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (HP > 0)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator IDLE()
    {
        // 현재 animator 상태정보 얻기
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // 애니메이션 이름이 IdleNormal 이 아니면 Play
        if (curAnimStateInfo.IsName("IdleNoral") == false)
            anim.Play("IdleNormal", 0, 0);

        // 몬스터가 Idle 상태일 때 두리번 거리게 하는 코드
        // 50% 확률로 좌/우로 돌아 보기
        int dir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;

        // 회전 속도 설정
        float lookSpeed = Random.Range(25f, 40f);

        // IdleNormal 재생 시간 동안 돌아보기
        for (float i = 0; i < curAnimStateInfo.length; i += Time.deltaTime)
        {
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + (dir) * Time.deltaTime * lookSpeed, 0f);

            yield return null;
        }
    }

    IEnumerator CHASE()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnimStateInfo.IsName("WalkFWD") == false)
        {
            anim.Play("WalkFWD", 0, 0);
            // SetDestination 을 위해 한 frame을 넘기기위한 코드
            yield return null;
        }

        // 목표까지의 남은 거리가 멈추는 지점보다 작거나 같으면
        if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
        {
            // StateMachine 을 공격으로 변경
            ChangeState(State.ATTACK);
        }
        // 목표와의 거리가 멀어진 경우
        else if (nmAgent.remainingDistance > lostDistance)
        {
            target = null;
            nmAgent.SetDestination(transform.position);
            yield return null;
            // StateMachine 을 대기로 변경
            ChangeState(State.IDLE);
        }
        else
        {
            // WalkFWD 애니메이션의 한 사이클 동안 대기
            yield return new WaitForSeconds(curAnimStateInfo.length);
        }
    }

    IEnumerator ATTACK()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // 공격 애니메이션은 공격 후 Idle Battle 로 이동하기 때문에
        // 코드가 이 지점에 오면 무조건 Attack01 을 Play
        anim.Play("Attack01", 0, 0);

        // 거리가 멀어지면
        if (nmAgent.remainingDistance > nmAgent.stoppingDistance)
        {
            // StateMachine을 추적으로 변경
            ChangeState(State.CHASE);
        }
        else
            // 공격 animation 의 두 배만큼 대기
            // 이 대기 시간을 이용해 공격 간격을 조절할 수 있음.
            yield return new WaitForSeconds(curAnimStateInfo.length * 2f);
    }

    IEnumerator KILLED()
    {
        yield return null;
    }

    void ChangeState(State newState)
    {
        state = newState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Player") return;
        // Sphere Collider 가 Player 를 감지하면
        target = other.transform;
        // NavMeshAgent의 목표를 Player 로 설정
        nmAgent.SetDestination(target.position);
        // StateMachine을 추적으로 변경
        ChangeState(State.CHASE);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        // target 이 null 이 아니면 target 을 계속 추적
        nmAgent.SetDestination(target.position);
    }

}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster1 : MonoBehaviour
{
    Transform target;
    NavMeshAgent nmAgent;
    Animator anim;

    float HP = 0;
    public float lostDistance;

    enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        KILLED,
        PlayerDead
    }

    State state;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();

        HP = 10;
        state = State.IDLE;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (HP > 0)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator IDLE()
    {
        // 현재 animator 상태정보 얻기
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // 애니메이션 이름이 IdleNormal 이 아니면 Play
        if (curAnimStateInfo.IsName("IdleNormal") == false)
            anim.Play("IdleNormal", 0, 0);

        // 몬스터가 Idle 상태일 때 두리번 거리게 하는 코드
        // 50% 확률로 좌/우로 돌아 보기
        int dir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;

        // 회전 속도 설정
        float lookSpeed = Random.Range(25f, 40f);

        // IdleNormal 재생 시간 동안 돌아보기
        for (float i = 0; i < curAnimStateInfo.length; i += Time.deltaTime)
        {
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + (dir) * Time.deltaTime * lookSpeed, 0f);

            yield return null;
        }

        ChangeState(State.CHASE);
    }

    IEnumerator CHASE()
    {
        AnimatorStateInfo curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // 목표까지의 남은 거리가 멈추는 지점보다 작거나 같으면
        if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
        {
            anim.SetTrigger("ATTACK");
        }
        // 목표와의 거리가 멀어진 경우
        else if (nmAgent.remainingDistance > lostDistance)
        {
            target = null;
            nmAgent.SetDestination(transform.position);

            // Attack 트리거를 해제
            anim.SetBool("CHASE", false);

            // StateMachine 을 대기로 변경
            ChangeState(State.IDLE);
            yield break;
        }
        else
        {
            // WalkFWD 애니메이션의 한 사이클 동안 대기
            yield return new WaitForSeconds(curAnimStateInfo.length);
        }
    }


    IEnumerator ATTACK()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // 거리가 멀어지면
        if (nmAgent.remainingDistance > nmAgent.stoppingDistance)
        {
            // StateMachine을 추적으로 변경
            ChangeState(State.CHASE);
        }
        else
            // 공격 animation 의 두 배만큼 대기
            // 이 대기 시간을 이용해 공격 간격을 조절할 수 있음.
            yield return new WaitForSeconds(curAnimStateInfo.length * 2f);
    }



    void ChangeState(State newState)
    {
        state = newState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Player") return;
        // Sphere Collider 가 Player 를 감지하면      
        target = other.transform;
        // NavMeshAgent의 목표를 Player 로 설정
        nmAgent.SetDestination(target.position);
        // StateMachine을 추적으로 변경
        ChangeState(State.CHASE);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        // target 이 null 이 아니면 target 을 계속 추적
        nmAgent.SetDestination(target.position);
    }
}
*/
//죽는모션, 티배깅은 플레이어 기능 더 구현되고 만들 예정