using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float speed = 1.0f;
    private Rigidbody2D rb;
    private Vector2 nextPosition;

    [SerializeField] private bool _isActivated = false;

    private List<Rigidbody2D> objectsOnPlatform = new List<Rigidbody2D>();

    public List<Rigidbody2D> ObjectsOnPlatform { get => objectsOnPlatform; set => objectsOnPlatform = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextPosition = startPosition;
    }

    private void Update()
    {
        if (_isActivated)
        {
            if (Vector2.Distance(rb.position, nextPosition) < speed * Time.deltaTime)
            {
                nextPosition = nextPosition == startPosition ? endPosition : startPosition;
            }
            Vector2 velocity = (nextPosition - rb.position).normalized * speed;
            rb.velocity = velocity;

            foreach (var obj in ObjectsOnPlatform)
            {
                obj.transform.position += new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime;
            }
        }
        else
        {
            // Установка скорости платформы равной нулю
            rb.velocity = Vector2.zero;

            // Установка скорости всех объектов на платформе равной нулю
            //foreach (var obj in objectsOnPlatform)
            //{
            //    obj.velocity = Vector2.zero;
            //}
        }
       
    }

    
    public void AddObjectOnPlatform(GameObject obj)
    {
        var rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null && !ObjectsOnPlatform.Contains(rb))
        {
            ObjectsOnPlatform.Add(rb);
        }
    }

    public void RemoveObjectFromPlatform(GameObject obj)
    {
        var rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null && ObjectsOnPlatform.Contains(rb))
        {
            ObjectsOnPlatform.Remove(rb);
        }
    }

    [ContextMenu("ActivatePlatform")]
    public void ActivatePlatform()
    {
        _isActivated = true;
    }
}
