using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public float maxhp = 100;
    public float Hp = 100;
    public NavMeshAgent nav;
    [SerializeField]
    Transform target;
    public int RandomNumber;
    public bool isMove;
    public Collider WeaponCoilder;

    public Collider hitcoilder;

    public bool isDie = false;
    public enum MoveType
    {
        Left,
        Right,      // 1
        JumpB,       // 2
        WalkB,      // 3
        WalkF     // 4
    }

    public enum AttackType
    {
        Attackhorizontal,
        Attackvertical,      // 1
        AttackRun,       // 2
    }
    Animator ani;

    private void Awake()
    {
        TryGetComponent(out ani);
        TryGetComponent(out nav);
    }
    void Start()
    {
        RandomNumber = Random.Range(1, 201);
        if (!isDie && RandomNumber <= 100)
        {
            Move();
        }
        else if (!isDie && RandomNumber > 101)
        {
            Attack();
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool PlayerDie = target.GetComponent<PlayerController>().isDie;
        if(PlayerDie){
            nav.SetDestination(transform.position);
        }
        //죽지 않았으면
        if (!isDie)
        {
            nav.SetDestination(target.position);
            if (!nav.isStopped)
            {
                if (nav.velocity.magnitude > 0.1f)
                {
                    Debug.Log("사용하고있음");
                    // 이동 방향을 기준으로 회전
                    Vector3 direction = nav.velocity.normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                }
            }
            else
            {
                if (!isMove)
                {
                    //좀 더 부드럽게 하려먼 수정필요 
                    Rotate();
                }
            }
        }
        if(Hp <=0 && !isDie){
            onDie();
        }
    }

    void Rotate()
    {
        Vector3 targetDirection = target.position - transform.position; // 목표 방향 계산
        targetDirection.y = 0; // 수평 회전만 고려
        if (targetDirection.magnitude > 0) // 목표 거리  무한대 
        {
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void reStartNumber()
    {
        RandomNumber = Random.Range(1, 201);
    }

    public void Move()
    {
        isMove = true;
        ani.SetTrigger("MOVE");
        switch (RandomNumber)
        {
            case int n when (n <= 20):
                ani.SetInteger("MOVETYPE", (int)MoveType.Left);
                nav.isStopped = true;
                break;
            case int n when (n > 20 && n <= 40):
                ani.SetInteger("MOVETYPE", (int)MoveType.Right);
                nav.isStopped = true;
                break;
            case int n when (n > 40 && n <= 60):
                ani.SetInteger("MOVETYPE", (int)MoveType.JumpB);
                nav.isStopped = true;
                break;
            case int n when (n > 60 && n <= 80):
                nav.isStopped = false;
                ani.SetInteger("MOVETYPE", (int)MoveType.WalkB);
                break;
            case int n when (n > 80 && n <= 100):
                nav.isStopped = false;
                ani.SetInteger("MOVETYPE", (int)MoveType.WalkF);
                break;
            default:
                Debug.Log("무브에서 ㅈ버그 발생");
                return;
        }
    }

    public void Attack()
    {
        isMove = false;
        Rotate();
        ani.SetTrigger("ATTACK");
        switch (RandomNumber)
        {
            case int n when (n > 100 && n <= 120):
                nav.isStopped = true;
            
                ani.SetInteger("ATTACKTYPE", (int)AttackType.Attackhorizontal);
                break;
            case int n when (n > 120 && n <= 150):
                nav.isStopped = true;
                
                ani.SetInteger("ATTACKTYPE", (int)AttackType.Attackvertical);
                break;
            case int n when (n > 150 && n <= 200):
                nav.isStopped = true;
                
                ani.SetInteger("ATTACKTYPE", (int)AttackType.AttackRun);
                break;
            default:
                Debug.Log("어택에서 ㅈ버그 발생");
                return;
        }
    }
    void onDie()
    {
        
        Debug.Log("죽었어요");
        ani.SetTrigger("Die");
        isDie = true;
        //시체매너
        // hitcoilder.enabled = false;
    }

    void AttackColiderEnable(){
        WeaponCoilder.enabled = true;
    }
    void AttackColiderDisable(){
        WeaponCoilder.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Weapon") { return; }
        if (other.tag == "Weapon")
        {
            if(Hp >0){
                ani.SetTrigger("Hit");
            }
        }
    }
}
