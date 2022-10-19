using System.Collections;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    [SerializeField] private float m_movementSpeed = 5f;
    [SerializeField] private int m_direction = 1;
    [SerializeField] private Transform m_eyePos;
    [SerializeField] private float m_visionDistance = 1f;
    [SerializeField] private LayerMask m_wallLayer;

    private Vector2 _startingPos;

    private Coroutine _moveCoroutine;

    private void Awake()
    {
        _startingPos = transform.position;
    }

    public void StartMoving()
    {
        gameObject.SetActive(true);
        StopMoving();
        transform.position = _startingPos;
        _moveCoroutine = StartCoroutine(IE_Move());
    }

    private void StopMoving()
    {
        if(_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
    }

    private void Stop()
    {
        StopMoving();
        gameObject.SetActive(false);
    }

    private IEnumerator IE_Move()
    {
        while (true)
        {
            transform.Translate(Time.deltaTime * Vector3.right * m_movementSpeed * m_direction);
            Debug.DrawRay(m_eyePos.position, Vector2.right * m_direction * m_visionDistance, Color.green, 0.1f);
            var hit = Physics2D.Raycast(m_eyePos.position, Vector2.right * m_direction, m_visionDistance, m_wallLayer);
            if (hit)
            {
                Stop();
                yield break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<MakardwajController>();

        player?.Die();
        Stop();
    }
}
