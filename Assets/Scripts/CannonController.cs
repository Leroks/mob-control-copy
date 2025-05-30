using UnityEngine;
using DG.Tweening;

public class CannonController : MonoBehaviour
{
    [Header("Cannon Settings")]
    public float moveSpeed = 5f;
    public float projectileSpeed = 15f;
    public float fireRate = 0.1f;
    public Transform firePoint;
    
    private float nextFireTime;
    private bool isMouseHeld;
    private Camera mainCamera;
    private Tweener currentWobbleTween;
    private WobbleEffect wobbleEffect;
    
    private void Start()
    {
        mainCamera = Camera.main;
        DOTween.SetTweensCapacity(500, 50);
        wobbleEffect = GetComponent<WobbleEffect>();
    }
    
    private void Update()
    {
        HandleMouseInput();
        FollowMouseX();
    }
    
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseHeld = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseHeld = false;
        }
        
        if (isMouseHeld && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    [Header("Movement Bounds")]
    [SerializeField] private float minX = -3.5f;
    [SerializeField] private float maxX = 3.5f;
    
    private void FollowMouseX()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCamera.WorldToScreenPoint(transform.position).z;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        
        // Clamp the x position to the bounds
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Lerp(newPosition.x, worldPos.x, moveSpeed * Time.deltaTime);
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        
        transform.position = newPosition;
    }

    private void Fire()
    {
        ProjectileBehavior projectile = ProjectilePool.Instance.GetProjectile();
        if (projectile != null)
        {
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = Quaternion.identity;
            projectile.Initialize(projectileSpeed);
            
            wobbleEffect.PlayWobbleAnimation();
        }
    }
}
