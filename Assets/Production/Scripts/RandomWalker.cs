using UnityEngine;
using System.Collections;

public class RandomWalker : MonoBehaviour
{
    public float moveRadius = 2f;
    public float moveSpeed = 1.5f;
    public float waitTime = 1f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (!isMoving)
            {
                Vector2 random2D = Random.insideUnitCircle * moveRadius;
                targetPosition = new Vector3(
                    startPosition.x + random2D.x,
                    transform.position.y,
                    startPosition.z + random2D.y
                );

                isMoving = true;
            }

            while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;

                if (direction != Vector3.zero)
                {
                    transform.forward = direction;
                }

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            isMoving = false;
            yield return new WaitForSeconds(waitTime);
        }
    }
}