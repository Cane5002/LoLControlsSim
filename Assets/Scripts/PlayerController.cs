using System.Collections;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float MovementSpeed;
    private bool CanMove;
    public float AttackDelay;
    public float AttackSpeed;
    public float AttackRange;
    public int AttackDamage;
    [SerializeField] private GameObject AttackRangeIndicator;
    public float AttackMoveSearchRadius;
    public GameObject AttackPrefab;

    [SerializeField] private Vector3 MoveTarget;
    public GameObject AttackTarget { get; private set; }
    private bool attacking;
    private bool attackB;

    [SerializeField] private GameObject sprite;

    void Awake() {
        MoveTarget = transform.position;
        AttackTarget = null;
        attacking = false;
        attackB = false;
        CanAttack = true;
        CanMove = true;
    }
    void FixedUpdate() {
        if (attacking && (AttackTarget != gameObject)) {
            if (TargetInRange()) {
                if (CanAttack) StartCoroutine(Attack());
            }
            else Move(AttackTarget.transform.position);
        }
        else if (transform.position != MoveTarget) Move(MoveTarget);
    }
    void Update() {
        AttackRangeIndicator.transform.localScale = new Vector3(AttackRange * 2, AttackRange * 2, 1);

        if (Input.GetButtonDown("Attack")) attackB = true;
        AttackRangeIndicator.SetActive(attackB);

        GameObject tar = CursorTarget();
        if (Input.GetButtonDown("Fire1")) { // Left click
            if (attackB) {
                AttackTarget = CursorClosestTarget();
                attacking = AttackTarget;
                MoveTarget = CursorPosition();
                attackB = false;
            }
            else AttackTarget = tar;
        }
        if (Input.GetButtonDown("Fire2")) {
            attackB = false;
            if (tar) AttackTarget = tar;
            attacking = tar && (tar != gameObject);
        }
        if (Input.GetButton("Fire2")) MoveTarget = CursorPosition();
    }

    private void LookAt(Vector3 position) {
        transform.up = new Vector2(position.x - transform.position.x, position.y - transform.position.y);
    }
    private void Move(Vector3 target) {
        if (!CanMove) return;
        LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target, MovementSpeed*Time.fixedDeltaTime);
    }
    private bool TargetInRange() {
        return (AttackTarget.transform.position - transform.position).magnitude < AttackRange;
    }
    private bool CanAttack;
    private IEnumerator Attack() {
        // Attack Stuffs
        Debug.Log("Attack");
        LookAt(AttackTarget.transform.position);
        Projectile attackProjectile = Instantiate(AttackPrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        attackProjectile?.SetProjectile(AttackTarget, AttackDamage); 

        CanAttack = false;
        CanMove = false;
        float attackTime = 1 / AttackSpeed;
        float animTime = attackTime * AttackDelay;

        Vector3 targetAngle = sprite.transform.eulerAngles + Vector3.up * 180f;
        float inc = 0.1f;
        for (float i = 0; i < animTime; i += inc) {
            sprite.transform.Rotate(Vector3.up * 180f * inc / animTime);
            yield return new WaitForSeconds(inc);
        }
        sprite.transform.localEulerAngles = Vector3.zero;
        CanMove = true;

        // Cooldown
        yield return new WaitForSeconds(attackTime-animTime);

        CanAttack = true;
    }

    // Mouse
    private GameObject CursorTarget() {
        Vector2 ray = CursorPosition();
        Debug.DrawLine(Camera.main.transform.position, ray);
        RaycastHit2D hit = Physics2D.Raycast(ray, ray);
        if (hit) {
            // Debug.Log("Selected " + hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }
        return null;
    }
    private GameObject CursorClosestTarget() {
        GameObject closest = null;
        float closestDistance = float.MaxValue;
        Vector3 origin = CursorPosition();
        Debug.DrawLine(origin - new Vector3(AttackMoveSearchRadius, 0, 0), origin + new Vector3(AttackMoveSearchRadius, 0, 0));
        Debug.DrawLine(origin - new Vector3(0, AttackMoveSearchRadius, 0), origin + new Vector3(0, AttackMoveSearchRadius, 0));
        GameObject[] hits = Array.ConvertAll(Physics2D.OverlapCircleAll(origin, AttackMoveSearchRadius), c => c.gameObject);
        foreach (GameObject hit in hits) {
            if (hit == gameObject) continue;
            Debug.Log("Search Hit");
            if (!closest) {
                closest = hit.gameObject;
                continue;
            }
            Vector3 pos = hit.transform.position;
            float distance = (pos - origin).magnitude;
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = hit;
            }
        }
        return closest;
    }
    private Vector3 CursorPosition() {
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v.z = 0;
        return v;
    }
}
