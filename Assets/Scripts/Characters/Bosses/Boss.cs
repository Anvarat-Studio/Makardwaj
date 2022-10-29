using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using Makardwaj.Common;

public class Boss : Controller
{
    public bool IsDead { get; protected set; }

    public virtual void Activate() { }
    public virtual void SetInteracting() {}
    public virtual void TakeDamage() { }

    protected virtual void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (IsDead)
        {
            return;
        }
        var player = collision.collider.GetComponent<MakardwajController>();

        if (player)
        {
            player.Die();
        }
    }
}
