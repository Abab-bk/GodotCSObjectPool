# GodotCSObjectPool

An object pool for Godot .net

## Example :

To learn more, plz check source code comment.

```csharp
    public override void _Ready()
    {
        _bulletPool = new NodePool<Bullet>(
            () => GD.Load<PackedScene>("res://Bullet.tscn").Instantiate<Bullet>(),
            bullet =>
            {
                bullet.Show();
                bullet.SetProcessMode(ProcessModeEnum.Inherit);
            },
            bullet =>
            {
                bullet.Hide();
                bullet.GlobalPosition = _bullets.GlobalPosition;
                bullet.SetProcessMode(ProcessModeEnum.Disabled);
            },
            bullet =>
            {
                bullet.QueueFree();
            },
            false,
            2000,
            3000
        );
        AddChild(_bulletPool);
        _bulletPool.Init(bullet =>
        {
            bullet.Pool = _bulletPool;
        });
    }

    public void GetObj()
    {
        _bulletPool.Get();    
    }
```


