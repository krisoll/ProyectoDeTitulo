using UnityEngine;
using System.Collections;

public class BasicBullet : MonoBehaviour {
    public float impulse;
    public float gravityScale;
    public bool tracks;
    private Vector2 velocity;
    private Rigidbody2D rigid;
	// Use this for initialization
	void Start () {
        rigid = gameObject.AddComponent<Rigidbody2D>();
        rigid.gravityScale = gravityScale;
        if (gravityScale == 0) rigid.isKinematic = true;
        ImpulsoAngular(transform.eulerAngles.z);
	}
	
	/* Update is called once per frame
	void Update () {
	
	}
    */
    public void ImpulsoAngular(float angulo)
    {
        float angle = angulo * Mathf.Deg2Rad;
        velocity = new Vector2(impulse * Mathf.Cos(angle), impulse * Mathf.Sin(angle));
        rigid.velocity = velocity;
    }
}
