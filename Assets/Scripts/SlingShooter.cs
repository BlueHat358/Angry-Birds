using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShooter : MonoBehaviour{

    public CircleCollider2D collider;
    public LineRenderer Trajectory;

    private Vector2 _startPos;
    private Bird _bird;

    [SerializeField]
    private float _radius = 0.75f;

    [SerializeField]
    private float _throwSpeed = 30f;

    // Start is called before the first frame update
    void Start(){
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseUp() {
        collider.enabled = false;
        Vector2 vel = _startPos - (Vector2)transform.position;
        float distance = Vector2.Distance(_startPos, transform.position);

        _bird.Shoot(vel, distance, _throwSpeed);

        // Kembalikan ketapel ke posisi awal
        gameObject.transform.position = _startPos;
        Trajectory.enabled = false;
    }

    private void OnMouseDrag() {
        // Mengubah posisi mouse ke world position
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Menghitung supaya 'karet' ketapel berada dalam radius yang ditentukan
        Vector2 dir = p - _startPos;
        if(dir.sqrMagnitude > _radius) {
            dir = dir.normalized * _radius;
        }
        transform.position = _startPos + dir;

        float distance = Vector2.Distance(_startPos, transform.position);

        if (!Trajectory.enabled) {
            Trajectory.enabled = true;
        }

        DisplayTrajectory(distance);
    }

    public void InitiateBird(Bird bird) {
        _bird = bird;
        _bird.MoveTo(gameObject.transform.position, gameObject);
        collider.enabled = true;
    }

    void DisplayTrajectory(float distance) {
        if(_bird == null) {
            return;
        }

        Vector2 vel = _startPos - (Vector2)transform.position;
        int segmentCount = 5;
        Vector2[] segments = new Vector2[segmentCount];

        // Posisi awal trjectory merupakan posisi mouse dari player saat ini
        segments[0] = transform.position;

        // Vellocity awal
        Vector2 segVel = vel * _throwSpeed * distance;

        for(int i = 1; i < segmentCount; i++) {
            float elapsedTime = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVel * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
        }

        Trajectory.positionCount = segmentCount;
        for(int i = 0; i < segmentCount; i++) {
            Trajectory.SetPosition(i, segments[i]);
        }
    }
}
