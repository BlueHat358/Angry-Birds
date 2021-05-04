using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : Bird{

    [SerializeField]
    private float _boostForce = 50;
    private bool _hasBoost = false;

    public void Boost() {
        if(State == BirdState.Thrown &&
            !_hasBoost) {
            Rigidbody.AddForce(Rigidbody.velocity * _boostForce);
            _hasBoost = true;
        }
    }

    public override void OnTap() {
        Boost();
    }
}
