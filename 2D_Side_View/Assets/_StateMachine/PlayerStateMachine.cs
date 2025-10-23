using UnityEngine;

// ========== INTERFACE ==========
public interface IPlayerState
{
    void Enter();
    void Update();
    void Exit();
}

// ========== BASE STATE MACHINE ==========
public class PlayerStateMachine : MonoBehaviour
{
    private IPlayerState currentState;

    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public Vector2 moveInput;

    private void Start()
    {
        ChangeState(new PlayerIdleState(this));
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}

// ========== IDLE STATE ==========
public class PlayerIdleState : IPlayerState
{
    private readonly PlayerStateMachine player;

    public PlayerIdleState(PlayerStateMachine player) => this.player = player;

    public void Enter()
    {
        player.rb.velocity = Vector2.zero;
    }

    public void Update()
    {
        // Basit örnek: input varsa MoveState'e geç
        player.moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (player.moveInput.sqrMagnitude > 0.01f)
            player.ChangeState(new PlayerMoveState(player));
    }

    public void Exit() { }
}

// ========== MOVE STATE ==========
public class PlayerMoveState : IPlayerState
{
    private readonly PlayerStateMachine player;

    public PlayerMoveState(PlayerStateMachine player) => this.player = player;

    public void Enter() { }

    public void Update()
    {
        player.moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (player.moveInput.sqrMagnitude < 0.01f)
        {
            player.ChangeState(new PlayerIdleState(player));
            return;
        }

        player.rb.velocity = player.moveInput.normalized * player.moveSpeed;
    }

    public void Exit()
    {
        player.rb.velocity = Vector2.zero;
    }
}
