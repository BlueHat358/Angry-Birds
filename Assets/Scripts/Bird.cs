using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour{

    public enum BirdState { Idle, Thrown, HitSomething}
    public BirdState State { get { return _state; } }
    public GameObject Parent;
    public Rigidbody2D Rigidbody;
    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    protected BirdState _state;
    private float _minVel = 0.05f;
    private bool _flagDestroy = false;

    // Start is called before the first frame update
    void Start(){
        Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;
    }

    // Update is called once per frame
    void Update(){
        Debug.Log("State: " + _state);
    }

    private void FixedUpdate() {
        if(_state == BirdState.Idle && 
            Rigidbody.velocity.sqrMagnitude >= _minVel) {
            _state = BirdState.Thrown;
        }

        if((_state == BirdState.Thrown || _state == BirdState.HitSomething) && 
            Rigidbody.velocity.sqrMagnitude < _minVel && 
            !_flagDestroy) {
            // Hancurkan gameobject setelah 2 detik
            // jike kecepatannya sudah kurang dari batas minimum
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    private IEnumerator DestroyAfter(float second) {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent) {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 vel, float distance, float speed) {
        Collider.enabled = true;
        Rigidbody.bodyType = RigidbodyType2D.Dynamic;
        Rigidbody.velocity = vel * speed * distance;
        OnBirdShot(this);
    }

    private void OnDestroy() {
        if(_state == BirdState.Thrown || _state == BirdState.HitSomething) {
            OnBirdDestroyed();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        _state = BirdState.HitSomething;
    }

    public virtual void OnTap() {
        // Do Nothing
    }

    public virtual void Explosion() {
        // Do Nothing

        Debug.Log("Success");
    }
}
