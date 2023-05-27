using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpFoce = 10.0f;
    public ForceMode forcemode = ForceMode.Impulse;
    public GameManager gameManager;

    float h;
    float v;

    bool isRun;
    bool eDown;
    bool isInteraction = false;

    Vector3 moveVec3;

    GameObject scanObject;

    [SerializeField]
    GameObject nearObject;

    Rigidbody rigid;
    Animator anim;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Interaction();
        Jump();
    }

    void GetInput()
    {
        h = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        v = gameManager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        eDown = Input.GetButtonDown("Interaction");
    }
    void Jump()
    {
        if (isInteraction) return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector3.up * jumpFoce, forcemode);

            Debug.Log("Jump!");
        }
    }
    void Move()
    {
        if (isInteraction) return;

        isRun = Input.GetButton("Run");

        if (isRun) moveSpeed = 7.0f;
        else moveSpeed = 5.0f;

        moveVec3 = new Vector3(h, 0, v).normalized * moveSpeed * Time.deltaTime;
        transform.position += moveVec3;

        transform.LookAt(transform.position + moveVec3);
        // ´ë¾È: rigidbody.velocity = moveVec3

        anim.SetBool("isWolk", moveVec3 != Vector3.zero);
        anim.SetBool("isRun", isRun);        
    }
    void Interaction()
    {
        if (eDown)
        {
            gameManager.TalkAction(scanObject);
        }
    }
   void ChangeInteraction()
    {
        isInteraction = !isInteraction;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            nearObject = other.gameObject;

            if (Input.GetButtonDown("Interaction")) anim.SetTrigger("Interaction");
        }
        else
            scanObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Item")
            nearObject = null;
    }

}
