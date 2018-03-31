using UnityEngine;
using Cinemachine;

/// <summary>
/// Steuert das Spielerverhalten, z.B. die Steuerung, physikalisches Verhalten, etc.
/// </summary>
public class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// Laufgeschwindigkeit des Spielers.
    /// </summary>
    [SerializeField] float speed = 0.05f;

    /// <summary>
    /// Das grafische Modell, um unter anderem die Drehung in Laufrichtung 
    /// des Modells zu steuern.
    /// </summary>
    [SerializeField] GameObject model;
    [SerializeField] float radiusDuringMoving = 0.5f;
    [SerializeField] GameObject cameraLookAtObject;
    private CapsuleCollider capsule;
    private float oldRadius;
    private bool isPlayerAlive = true;

    /// <summary>
    /// Bezeichner der Horizonal-Axis.
    /// </summary>
    private string horizontalAxis = "Horizontal";

    /// <summary>
    /// Winkel um die sich die Spielerfigur um die eigene Achse (=Y)
    /// drehen soll.
    /// </summary>
    private float towardsY = 0f;

    /// <summary>
    /// Referenz auf die Rigidbody-Componente.
    /// </summary>
    private Rigidbody rigidBody;

    /// <summary>
    /// Referenz auf die Animation Componente.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Geschwindigkeit während eines Sprungs.
    /// </summary>
    private float jumpSpeed = 8f;

    /// <summary>
    /// Verändert die Geschwindigkeit der Gravitation, damit die Spielfigur schneller
    /// fällt.
    /// </summary>
    private float gravitySpeed = 10f;

    /// <summary>
    /// Überprüft, ob die Spielfigur auf dem Boden ist, oder nicht.
    /// False, die Spielfigur fällt oder springt gerade.
    /// </summary>
    private bool isGrounded = true;

    /// <summary>
    /// Überprüft, ob die Taste zum Springen gedrückt ist.
    /// Verbessert das Sprungverhalten.
    /// </summary>
    private bool isJumpTriggered = false;

    private void Awake()
    {
        SaveGameData.OnSave += Saveme;
        InitiateCinemachine();
    }

    private void InitiateCinemachine()
    {
        CinemachineVirtualCamera cvc = FindObjectOfType<CinemachineVirtualCamera>();
        if (cvc != null)
        {
            cvc.Follow = transform;
            cvc.LookAt = cameraLookAtObject.transform;
        }
    }

    private void OnDestroy()
    {
        SaveGameData.OnSave -= Saveme;
    }

    private void Saveme(SaveGameData savegame)
    {
        savegame.playerPosition = transform.position;
    }

    private void Loadme(SaveGameData savegame)
    {
        if (savegame != null &&
            gameObject.scene.buildIndex == savegame.currentLevel)
        {
            transform.position = savegame.playerPosition;
        }
    }

    // Use this for initialization
    private void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        capsule = GetComponent<CapsuleCollider>();
        oldRadius = capsule.radius;
        Loadme(SaveGameData.GetCurrentSaveGameData());
    }

    // Update is called once per frame
    private void Update ()
    {
        if (GameIsPaused() || PlayerIsDead()) { return; }
        float axisValue = Input.GetAxis(horizontalAxis);
        float vertical = rigidBody.velocity.y;
        HandleMove(horizontalAxis, axisValue);
        HandleJump(vertical);
    }

    private bool GameIsPaused()
    {
        return Time.timeScale == 0f;
    }

    private bool PlayerIsDead()
    {
        if (transform.position.y > -3f)
        {
            return false;
        }
        isPlayerAlive = false;
        return true;
    }

    private void OnCollisionStay(Collision collision)
    {
        RaycastHit hitInfo;
        isGrounded = Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.12f);
    }

    /// <summary>
    /// Behandelt das Move-Event, so dass Animation und Bewegung im 
    /// Raum gesteuert werden.
    /// </summary>
    /// <param name="axisInput"></param>
    /// <param name="axisValue"></param>
    private void HandleMove(string axisInput, float axisValue)
    {
        Move(axisInput, axisValue);
        PlayMoveAnimation(axisValue);
    }

    /// <summary>
    /// Behandelt das Jump-Event, so dass Animation und Bewegung im 
    /// Raum gesteuert werden.
    /// </summary>
    /// <param name="vertical"></param>
    private void HandleJump(float vertical)
    {
        Jump();
        PlayJumpAnimation(vertical);
    }

    /// <summary>
    /// Behandelt das Springen im Raum.
    /// </summary>
    private void Jump()
    {
        if (Input.GetAxis("Jump") > 0f && isGrounded)
        {
            if (!isJumpTriggered)
            {
                rigidBody.AddRelativeForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            }
            isJumpTriggered = true;
        }
        else
        {
            isJumpTriggered = false;
        } 
        rigidBody.AddRelativeForce(Vector3.down * gravitySpeed);
    }

    /// <summary>
    /// Bewegt den Spieler anhand der Input-Axis.
    /// </summary>
    /// <param name="axisInput">Horizontaler oder Vertikaler Input</param>
    private void Move(string axisInput, float axisValue)
    {
        OnMovingSetGreaterRadius(axisValue);
        SetModelPosition(axisInput, axisValue, RotateModel(axisValue));
    }

    /// <summary>
    /// Setzt während der Bewegung des Spielers, den Radius der Kapsel um den Spieler höher, 
    /// um ein realistisches Kollisionsverhalten bei Wänden zu realisieren.
    /// </summary>
    /// <param name="axisValue"></param>
    private void OnMovingSetGreaterRadius(float axisValue)
    {
        if (axisValue != 0f)
        {
            capsule.radius = radiusDuringMoving;
        }
        else
        {
            capsule.radius = oldRadius;
        }
    }

    /// <summary>
    /// Setzt die Spielerposition anhand des Inputs und den daraus resulierenden
    /// Input-Wertes in der Spielewelt.
    /// </summary>
    /// <param name="axisInput"></param>
    /// <param name="axisValue"></param>
    private void SetModelPosition(string axisInput, float axisValue, float rotationDirection)
    {
        Vector3 direction = (rotationDirection * transform.forward);
        if (horizontalAxis.Equals(axisInput))
        {
            float oldRadius = capsule.radius;
            CalculatePosition(axisValue, direction);
        }
    }

    /// <summary>
    /// Berechnet die neue Position des Spielers, anhand des Axis-Wertes 
    /// und der Axis-Richtungen.
    /// </summary>
    /// <param name="axisValue"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private Vector3 CalculatePosition(float axisValue, Vector3 direction)
    {
        return transform.position += axisValue * speed * direction;
    }

    /// <summary>
    /// Rotiert das Spielermodell, so dass das Modell in Richtung der Laufrichtung
    /// schaut.
    /// </summary>
    /// <param name="axisValue"></param>
    private float RotateModel(float axisValue)
    {
        if (axisValue > 0)
        {
            towardsY = 0f;
        }
        else if (axisValue < 0)
        {
            towardsY = 180f;
        }

        float rotationVelocity = Time.deltaTime * 20f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, -towardsY, 0f), rotationVelocity);
        return (Mathf.Round(transform.rotation.eulerAngles.y) > 170f) ? -1f : 1f;
    }

    /// <summary>
    /// Spielt die Animation während des Springens ab.
    /// </summary>
    private void PlayJumpAnimation(float vertical)
    {
        animator.SetBool("grounded", isGrounded);
        animator.SetFloat("jumpSpeed", vertical);
    }

    /// <summary>
    /// Spielt die Animation auf dem Boden ab. 
    /// </summary>
    /// <param name="axisValue"></param>
    private void PlayMoveAnimation(float axisValue)
    {
        animator.SetFloat("forward", Mathf.Abs(axisValue));
    }

    public bool IsPlayerAlive()
    {
        return isPlayerAlive;
    }
}
