using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] Transform prefab = null;

    [Header("Fall Speed")]
    [SerializeField] float initialFallSpeed = 5f;
    [SerializeField] float fallAcceleration = 0.05f;

    private Transform[] objectPool;
    private float nextCreate;
    private float fallSpeed;
    private int frameCount;

    #region Awake
    void Awake()
    {
        PopulatePool();
        Reset();
        fallSpeed = initialFallSpeed;
    }
    #endregion

    #region Update
    void Update()
    {
        if (frameCount++ > 60) {
            frameCount = 0;
            fallSpeed += fallAcceleration * 60 * Time.deltaTime;
        }

        Create();
        FallAndRotate();
    }
    #endregion

    #region Fall & Rotate
    void FallAndRotate()
    {
        foreach (Transform child in transform)
        {
            var velocity = fallSpeed * Time.deltaTime;
            child.Translate(0, -velocity, 0, Space.World);
            child.Rotate(0, 0, 360 * Time.deltaTime, Space.World);
        }
    }
    #endregion

    #region Create
    void Create()
    {
        if (Time.time > nextCreate)
        {
            Create(Random.value < .5f);
            nextCreate = Time.time + (1 / fallSpeed) * 2;
        }
    }

    void Create(bool side)
    {
        if (side)
        {
            Create(Vector2.left  * 2);
            Create(Vector2.right * 2);
        }
        else Create(Vector2.zero);
    }

    void Create(Vector2 pos)
    {
        foreach (Transform obj in transform)
        {
            var go = obj.gameObject;
            if(!go.activeSelf)
            {
                go.SetActive(true);
                obj.localPosition = pos;
                return;
            }
        }
    }
    #endregion

    #region Reset
    public void ResetLocal()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        fallSpeed = initialFallSpeed;
    }
    
    public static void Reset()
        => FindObjectOfType<Obstacles>().ResetLocal();
    #endregion

    #region Populate Pool
    void PopulatePool(int size=20)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        objectPool = new Transform[size];

        for (int i=0; i<size; i++)
        {
            var obj = Instantiate(prefab, transform);
            obj.localScale = Vector2.one * .3f;
            obj.localEulerAngles = new Vector3(0, 0, 45);
        }
    }
    #endregion
}