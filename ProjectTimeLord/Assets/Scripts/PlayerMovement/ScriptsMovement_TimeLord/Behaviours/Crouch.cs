using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : BaseBehaviour {

    public float scale = 0.5f;
    public bool crouching;
    public float centerOffsetY = 0f;

    private CircleCollider2D circleCollision;
    private Vector2 originalCenter;

    protected override void Awake()
    {
        base.Awake();
        circleCollision = GetComponent<CircleCollider2D>();
        originalCenter = circleCollision.offset;
    }

    protected virtual void OnCrouch(bool value)
    {
        crouching = value;
        ToggleScripts(!crouching);

        var size = circleCollision.radius;
        float newoffsetY;
        float sizeReciprocal;

        if(crouching)
        {
            sizeReciprocal = scale;
            newoffsetY = circleCollision.offset.y - size/2 + centerOffsetY;
        }
        else
        {
            sizeReciprocal = 1 / scale;
            newoffsetY = originalCenter.y;
        }

        size = size * sizeReciprocal;
        circleCollision.radius = size;
        circleCollision.offset = new Vector2(circleCollision.offset.x, newoffsetY);

    }

	// Update is called once per frame
	void Update () {

        bool canCrouch = (Input.GetAxis("Vertical") < 0);
        if(canCrouch && collisionState.idle && !crouching)
        {
            OnCrouch(true);
        }else if(crouching && !canCrouch)
        {
            OnCrouch(false);
        }
		
	}
}
