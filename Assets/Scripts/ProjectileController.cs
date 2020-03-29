using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Shape, IKillable
{
    public Vector2 projectileDirection;
    public float projectileSpeed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Name = "Projectile";

        SetColor(0, 255, 0);
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        transform.Translate(projectileDirection * Time.deltaTime * projectileSpeed, Space.World);

        float top = transform.position.y + halfHeight;

        if (top >= gameSceneController.screenBounds.y)
        {
            gameSceneController.KillObject(this);
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public string GetName()
    {
        return Name;
    }
}
