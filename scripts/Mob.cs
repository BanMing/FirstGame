using Godot;
using System;

public partial class Mob : RigidBody2D
{
    public override void _Ready()
    {
        AnimatedSprite2D animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        string[] mobTypes = animatedSprite.SpriteFrames.GetAnimationNames();
        animatedSprite.Play(mobTypes[GD.Randi() % mobTypes.Length]);
    }

    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
        QueueFree();
    }
}
