using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slam : BaseBehaviour {

    public GameObject dustEffectPrefab;
    [SerializeField]
    private float speed = 50f;
    public float scale = 0.5f;
    public float centerOffsetY = 0f;
    public bool slamming;
    private CircleCollider2D circleCol;
    private Vector2 origin;

    protected override void Awake()
    {
        base.Awake();
        circleCol = GetComponent<CircleCollider2D>();
        origin = circleCol.offset;
    }

    protected virtual void OnSlam(bool value)
    {
        slamming = value;
        ToggleScripts(!slamming);

        var size = circleCol.radius;
        float newoffsetY;
        float sizeReciprocal;

        if(slamming)
        {
            sizeReciprocal = scale;
            newoffsetY = circleCol.offset.y - size/2 + centerOffsetY;
        }
        else
        {
            sizeReciprocal = 1 / scale;
            newoffsetY = origin.y;
        }

        size = size * sizeReciprocal;
        circleCol.radius = size;
        circleCol.offset = new Vector2(circleCol.offset.x, newoffsetY);

    }

	// Update is called once per frame
	void FixedUpdate () {

        bool canSlam = false;
        if (body2d.velocity.y < -0.1)
        {
            canSlam = (Input.GetAxis("Vertical") < 0);
        }
       
        if(canSlam && !collisionState.idle && !slamming)
        {
            OnSlam(true);
            float tSpeed = speed;
            GameObject clone = Instantiate(dustEffectPrefab);
            clone.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
            body2d.velocity = new Vector2(body2d.velocity.x, body2d.velocity.y * speed);
        }
        else if(slamming && !canSlam)
        {
            OnSlam(false);
        }

	}
}
