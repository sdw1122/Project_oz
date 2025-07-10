using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hammer : MonoBehaviour
{
    Rigidbody rb;

    public GameObject weapon;    
    public float damage = 40f;
    public float skill1;
    private float skill1HoldTime = 0;
    public float skill1CoolDown; //10초
    public float skill2 = 60f;

    private InputSystem_Actions controls;
    private bool skill1Pressed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new InputSystem_Actions();
        skill1CoolDown = 10f;

        // Skill1 시작(누름)
        controls.Player.Skill1.started += ctx =>
        {
            if (skill1CoolDown >= 10f)
            {
                skill1Pressed = true;
            }
            
        };

        // Skill1 해제(뗌)
        controls.Player.Skill1.canceled += ctx =>
        {
            if (skill1Pressed)
            {
                skill1Pressed = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                skill1 = 0;
                if (skill1HoldTime < 1)
                    skill1 = 0;
                else if (skill1HoldTime < 2)
                    skill1 = damage * 2;
                else if (skill1HoldTime < 3)
                    skill1 = damage * 4;
                else
                    skill1 = damage * 8;

                Skill1(skill1);
                skill1HoldTime = 0;
            }

        };

        // Skill2 (Q키)
        controls.Player.Skill2.performed += ctx =>
        {
            Skill2();
        };
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
    void Update()
    {
        if (skill1Pressed)
        {
            skill1HoldTime += Time.deltaTime;
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY
                | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }

        if (skill1CoolDown != 10f)
        {
            skill1CoolDown += Time.deltaTime;
        }
        else if (skill1CoolDown >= 10f)
        {
            skill1CoolDown = 10f;
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Skill2();
        }
    }


    private void OnMouseDown()
    {
        
    }

    void Skill1(float damage)
    {
        // 박스 중심: 플레이어 앞쪽 2.5만큼 (원하는 거리로 조정)
        Vector3 boxCenter = transform.position + transform.forward * 2.5f;
        // 박스의 반 크기: 가로 2.5, 높이 1, 깊이 2.5 (전체 크기: 5x2x5)
        Vector3 boxHalfExtents = new Vector3(2.5f, 1f, 2.5f);
        // 박스 회전: 플레이어의 회전과 동일
        Quaternion boxOrientation = transform.rotation;
        // "Enemy" 레이어만 감지 (레이어를 사용하지 않는다면 생략 가능)
        int layerMask = LayerMask.GetMask("Enemy");

        // 박스 범위 내의 "Enemy"레이어만 감지
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxHalfExtents, boxOrientation, layerMask);

        foreach (var hit in hitColliders)
        {
            // 적에게 데미지 주기 (Enemy 스크립트에 TakeDamage 함수가 있다고 가정)
            //Enemy enemy = hit.GetComponent<Enemy>();
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(attackDamage);
            //}
            Debug.Log("맞음");
        }
        skill1CoolDown = 0;
    }

    void Skill2()
    {
        float range = 5f;         // 공격 거리(반경)
        float angle = 90f;        // 부채꼴 각도(도 단위)
        float attackDamage = skill2;

        // 1. 전방 구 범위 내 적 감지
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Enemy"));

        foreach (var hit in hitColliders)
        {
            // 2. 플레이어 → 적 방향 벡터
            Vector3 dirToTarget = (hit.transform.position - transform.position).normalized;
            // 3. 전방 벡터와 각도 비교
            float dot = Vector3.Dot(transform.forward, dirToTarget);
            float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (theta <= angle / 2f)
            {
                // 4. 부채꼴 안에 들어온 적에게만 데미지
                //Enemy enemy = hit.GetComponent<Enemy>();
                //if (enemy != null)
                //{
                //    enemy.TakeDamage(attackDamage);
                //}
                Debug.Log("Skill2 맞음");
            }
        }
    }

    void DrawSkill2ConeGizmo()
    {
        float range = 5f;      // Skill2 범위
        float angle = 90f;     // Skill2 각도

        int segments = 30;     // 부채꼴을 나눌 선 개수
        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;

        Gizmos.color = new Color(0, 0.5f, 1, 0.3f);

        // 중심선
        Gizmos.DrawLine(origin, origin + forward * range);

        // 부채꼴의 양 끝 방향 벡터
        Quaternion leftRot = Quaternion.AngleAxis(-angle / 2, Vector3.up);
        Quaternion rightRot = Quaternion.AngleAxis(angle / 2, Vector3.up);
        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        Gizmos.DrawLine(origin, origin + leftDir * range);
        Gizmos.DrawLine(origin, origin + rightDir * range);

        // 부채꼴 호 그리기
        Vector3 prevPoint = origin + leftDir * range;
        for (int i = 1; i <= segments; i++)
        {
            float lerp = (float)i / segments;
            Quaternion rot = Quaternion.AngleAxis(Mathf.Lerp(-angle / 2, angle / 2, lerp), Vector3.up);
            Vector3 point = origin + (rot * forward) * range;
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }

    // skill 범위 시각화
    void OnDrawGizmosSelected()
    {
        if (skill1Pressed)
        {
            Vector3 boxCenter = transform.position + transform.forward * 2.5f;
            Vector3 boxHalfExtents = new Vector3(2.5f, 1f, 2.5f); //이 범위 조절
            Quaternion boxOrientation = transform.rotation;

            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxCenter, boxOrientation, Vector3.one);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawCube(Vector3.zero, boxHalfExtents * 2);
        }
        DrawSkill2ConeGizmo();
    }
}
