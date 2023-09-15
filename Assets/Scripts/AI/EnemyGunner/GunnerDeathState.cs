using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GunnerDeathState : GunnerBaseState
{
    public SpriteRenderer spriteRenderer;
    private GunnerFSM gunnerFsm;

    public override void EnterState(GunnerFSM fsm)
    {
        gunnerFsm = fsm;
        spriteRenderer = fsm.GetComponent<SpriteRenderer>();
        Rigidbody2D rigidbody = fsm.GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector2.zero;
        fsm.GetComponent<CapsuleCollider2D>().enabled = false;
        fsm.StartCoroutine(Fade());
    }

    public override void RunState(GunnerFSM fsm) { }

    private IEnumerator Fade()
    {
        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(0.3f);
            Color color = spriteRenderer.color;
            if (color.a == 1)
            {
                color.a = 0;
            }
            else
            {
                color.a = 1;
            }
            spriteRenderer.color = color;
        }
        gunnerFsm.DestroyThis();
    }
}
