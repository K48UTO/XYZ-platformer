using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _speed = 1f;

   
    private Transform[] _objects;
    private float[] _angles;

    private void Start()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        _objects = new Transform[childCount];
        _angles = new float[childCount];

        float angleStep = 360f / childCount;

        for (int i = 0; i < childCount; i++)
        {
            _objects[i] = transform.GetChild(i);
            _angles[i] = i * angleStep;
        }
    }
    private void Update()
    {
        for (int i = 0; i < _objects.Length; i++)
        {

            if (_objects[i] == null) continue;
            _angles[i] = Mathf.Repeat(_angles[i] + _speed * Time.deltaTime, 360f);
       
            float radian = _angles[i] * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian) * _radius;
            float y = Mathf.Sin(radian) * _radius;

            _objects[i].localPosition = new Vector3(x, y, 0f);



        }
    }

   



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
