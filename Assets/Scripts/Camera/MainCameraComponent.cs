using Game.Components;
using Game.Events;
using Game.Features.Player.Components;
using ME.ECS;
using UnityEngine;

public class MainCameraComponent : MonoBehaviour
{
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    public Vector3 offset;
    public float moveSpeed;
    public float rotateSpeed;

    public GlobalEvent playerCreatedEvent;
    public GameOverGlobalEvent gameOverEvent;

    private Entity _playerEntity;

    private void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;

        playerCreatedEvent.Subscribe(OnPlayerCreated);
        gameOverEvent.Subscribe(OnGameOver);
    }

    private void OnDestroy()
    {
        playerCreatedEvent.Unsubscribe(OnPlayerCreated);
        gameOverEvent.Unsubscribe(OnGameOver);
    }

    private void OnGameOver(in Entity entity)
    {
        _playerEntity = Entity.Empty;
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
    }

    private void OnPlayerCreated(in Entity entity)
    {
        _playerEntity = entity;
    }

    private void Update()
    {
        if (Worlds.currentWorld == null)
            return;

        if (_playerEntity.IsEmpty())
            return;

        var playerPosition = _playerEntity.Read<PositionComponent>().value;
        var playerDirection = _playerEntity.Read<MovementDirection>().value;

        var desiredPosition = playerPosition + Quaternion.LookRotation(playerDirection, Vector3.up) * offset;

        if ((desiredPosition - transform.position).sqrMagnitude > 1f)
        {
            transform.position = desiredPosition;
            transform.rotation = Quaternion.LookRotation(playerPosition - transform.position, Vector3.up);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerPosition - transform.position, Vector3.up), rotateSpeed * Time.deltaTime);
        }
    }
}
