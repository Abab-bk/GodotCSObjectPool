using Godot;
using System;
using AcidWallStudio.ObjectPool;

namespace CSharpObjectPool;

public partial class Bullet : Node2D
{
    [Export] private VisibleOnScreenNotifier2D _notifier2D;
    
    public NodePool<Bullet> Pool = null;

    public override void _Ready()
    {
        _notifier2D.ScreenExited += () =>
        {
            if (Pool == null)
            {
                QueueFree();
                return;
            }
            Pool.Release(this);
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += Vector2.Right * 300f * (float)delta;
    }
}
