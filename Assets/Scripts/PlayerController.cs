using UnityEngine.InputSystem;


using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //체력바
    public float maxhp=100;
    public float Hp = 100;
    Animator ani;
    int hashAttackCount = Animator.StringToHash("AttackCount");

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip footstep;

    public PlayerCamera PlayerCam;

    public Collider WeaponColider;
    public Collider HitCoilder;


    public CharacterController characterController;
    //이동
    [SerializeField]
    Vector2 inputvec;
    [SerializeField]
    Vector2 camerainput;
    public float camV;
    public float camH;
    float v;
    float h;
    [SerializeField]
    float moveAmount;
    [SerializeField]
    float walkingspeed = 2;
    [SerializeField]
    float runningspeed = 5;
    [SerializeField]
    float rotationSpeed = 15;

    public bool isAttack;
    public bool isDodge;
    public bool isDie;
    public bool isHit;

    public enum State{
        Idle,
        Attack,
        Dodge,
        Hit,
        Move,
        Die
    }
    public State Currentstate;
    
    
    [SerializeField]
    private Vector3 moveDir;
    private Vector3 targetRotationDir;

    public int AttackCount
    {
        get => ani.GetInteger(hashAttackCount);
        set => ani.SetInteger(hashAttackCount, 0);
    }

    void Start()
    {
        Currentstate = State.Idle;
        TryGetComponent(out ani);
        characterController = transform.GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        if(Currentstate == State.Die){return;}
        if(inputvec != Vector2.zero){Currentstate = State.Move;}
        if(Currentstate == State.Move){Move();}
        //죽으면 움직이지말기
        HandleCameraMovementInput();
        HandleRotation();
    }

    void Move()
    {
        //쳐맞는중이면 하지말고 리턴
        if(Currentstate == State.Die){return;}

        v = inputvec.y;
        h = inputvec.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(v) + Mathf.Abs(h));

        if (moveAmount > 0 && moveAmount <= 0.5f)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5f)
        {
            moveAmount = 1f;
        }

        moveDir = Camera.main.transform.forward * v + Camera.main.transform.right * h;
        moveDir.y = 0;
        moveDir.Normalize();

        if (moveAmount == 1f)//달리기
        {
            characterController.Move(moveDir * runningspeed * Time.deltaTime);
        }
        else if (moveAmount == 0.5f)//걷기
        {
            characterController.Move(moveDir * walkingspeed * Time.deltaTime);
        }
        ani.SetInteger("Run", (int)moveAmount);
        

        if(!audioSource.isPlaying && moveAmount!=0){
            audioSource.clip= footstep;
            audioSource.Play();
        }
    }

    //playerSystem 조작들 
    void OnAttack()
    {
        if(Currentstate == State.Die){return;}
        Currentstate = State.Attack;
        ani.SetTrigger("Attack");
        // onHit();// 애니메이션 자체로 수정할예정
    }
    void OnChargeAttack(){
        if(Currentstate == State.Die){return;}
        Currentstate = State.Attack;
        ani.SetTrigger("chargedAttack");
        // onHit();// 애니메이션 자체로 수정할예정
    }
    //닺지
    void OnSprint(){
          if(Currentstate == State.Die){return;}
        ani.SetTrigger("Roll");
        Currentstate = State.Dodge;
    }
    void OnMove(InputValue value)
    {
        if(Currentstate == State.Die){return;}
        Debug.Log("버튼한번 클릭");
        inputvec = value.Get<Vector2>();
    }
    void OnCamControl(InputValue value)
    {
        if(Currentstate == State.Die){return;}
        camerainput = value.Get<Vector2>();
    }


    //카메라 이동
    void HandleRotation()
    {
        targetRotationDir = Vector3.zero;
        targetRotationDir = PlayerCam.camobj.transform.forward * v + PlayerCam.camobj.transform.right * h;
        targetRotationDir.Normalize();
        targetRotationDir.y = 0;

        if (targetRotationDir == Vector3.zero)
        {
            targetRotationDir = transform.forward;
        }
        Quaternion newRotation = Quaternion.LookRotation(targetRotationDir);
        Quaternion targetRotaition = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotaition;
    }

    void HandleCameraMovementInput()
    {
        camV = camerainput.y;
        camH = camerainput.x;
    }
    // 공격 맞을 시 행동
    private void OnTriggerEnter(Collider other) {
        if(other.tag !="EnemyWeapon"){return;}

        if(other.tag=="EnemyWeapon")
        { 
            if(Hp > 0){
                if(isDodge){return;}
                isHit = true;
                ani.SetTrigger("Hit");
            }else if(Hp <= 0){
                onDie();
            }
        }
    }

    void onDie(){
            Currentstate = State.Die;
            ani.SetTrigger("Die");
            Debug.Log("죽음");
    }
    
    

    //공격 시 공격 확인 
    void onHit(){
        WeaponColider.enabled = true;
    }


    //아래부터 애니메이션에 들어가 있는 함수들
    //공격 애니메이션 끝날 때 마다 콜라이더 해제
    public void WeaponColiderfalse(){
        WeaponColider.enabled=false;
    }
    public void SoundPlay(AudioClip soundName){
        audioSource.clip = soundName;
        audioSource.Play();
    }
}
