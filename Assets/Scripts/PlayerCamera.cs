using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public PlayerController player;
    public Camera camobj;
    [SerializeField]
    Transform cameraPivotTransform;

    [Header("Camera Setting")]
    private float cameraSmoothSpeed = 1;
    [SerializeField]
    float UpAndDownRotationSpeed = 220;
    [SerializeField]
    float leftAndRightRotationSpeed = 220;
    [SerializeField]
    float minimumPivot = -30; //아래 보기 최대
    [SerializeField]
    float maxmumPivot = 60;  //위보기 최대 
    [SerializeField]
    float cameraCollisionRaduis = 0.5f;
    [SerializeField]
    LayerMask colliderWithLayers;

    [Header("Camera Values")]
    private Vector3 cameraVeloctiy;
    private Vector3 camObjPos;
    [SerializeField]
    float leftAndRightLookAngle;
    [SerializeField] 
    float UpAndDownLookAngle;
    [SerializeField]
    float camZPos;
    [SerializeField]
    float targetCamZPos;
    [SerializeField]


    private void Start() {
        //최초 카메라 위치값 초기화
        camZPos = camobj.transform.localPosition.z;
    }
    private void LateUpdate()
    {
        HandleAllCameraAction();
    }
    public void HandleAllCameraAction()
    {
        if (player != null)
        {
            FollowTarget();
            HandleRoatations();
            HandleCollisons();
        }
    }

    void FollowTarget()
    {
        Vector3 targetCameraPos = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVeloctiy, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPos;
    }

    void HandleRoatations()
    {
       leftAndRightLookAngle +=player.camH *leftAndRightRotationSpeed *Time.deltaTime;
       
       UpAndDownLookAngle -= player.camV * UpAndDownRotationSpeed *Time.deltaTime;

       UpAndDownLookAngle =Mathf.Clamp(UpAndDownLookAngle,minimumPivot,maxmumPivot);

       Quaternion targetRot;
       Vector3 camRot = Vector3.zero;

       camRot.y = leftAndRightLookAngle;
       targetRot = Quaternion.Euler(camRot);
       transform.rotation = targetRot;

       camRot = Vector3.zero;
       camRot.x =UpAndDownLookAngle;
       targetRot = Quaternion.Euler(camRot);
       cameraPivotTransform.localRotation =targetRot;

    }

    void HandleCollisons(){
        //카메라 최초 위치값 넣고  그 기반으로 시작
        targetCamZPos = camZPos;

        RaycastHit hit;
        Vector3 direction  = camobj.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        //레이케스트 스피어를 쏴서  출발점(cameraPivotTransform.position)  direction방향으로  맞은 물체(hit) 저장  최대 인식 거리 Mathf.Abs(targetCamZPos) ,  colliderWithLayers 만
        if(Physics.Raycast(cameraPivotTransform.position, direction, out hit , Mathf.Abs(targetCamZPos), colliderWithLayers))
        {
            float distanceFromHitobj = Vector3.Distance(cameraPivotTransform.position , hit.point);
            targetCamZPos = -(distanceFromHitobj - cameraCollisionRaduis);
        }

        //카메라의 최소거리를 제한 
        if(Mathf.Abs(targetCamZPos) < cameraCollisionRaduis)
        {
            targetCamZPos = -cameraCollisionRaduis;
            
        }
        //camobj.transform.localPosition.z 현재위치에서 targetCamZPos까지 거리중 시작점에서 20퍼센트 더 간 거리를 부드럽게 이동
        camObjPos.z = Mathf.Lerp(camobj.transform.localPosition.z, targetCamZPos, 0.2f);
    

        camobj.transform.localPosition= camObjPos;
    }
}
