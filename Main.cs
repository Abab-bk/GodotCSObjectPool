using Godot;
using System;
using AcidWallStudio.ObjectPool;

namespace CSharpObjectPool;

public partial class Main : Node2D
{
    [Export] private Label _infoLabel;
    [Export] private Label _fpsLabel;
    [Export] private CheckButton _modeButton;
    [Export] private Marker2D _bullets;

    private PackedScene _node = GD.Load<PackedScene>("res://Bullet.tscn");

    private NodePool<Bullet> _bulletPool;
    
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

    public override void _Process(double delta)
    {
        if (_modeButton.ButtonPressed)
        {
            _bulletPool.Get();
        }
        else
        {
            _bullets.AddChild(_node.Instantiate<Bullet>());
        }
        
        _fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";

        if (_bulletPool == null) return;
        if (!_modeButton.IsPressed())
        {
            _infoLabel.Text = "";
            return;
        }

        _infoLabel.Text = $"""
                           CurrentMode: Pool
                           InActiveCount: {_bulletPool.CountInactive} 
                           Active: {_bulletPool.CountActive}
                           TotalCount: {_bulletPool.CountAll}
                           """;
    }
}
