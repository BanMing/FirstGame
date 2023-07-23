using Godot;
using System;

public partial class Main : Node
{
    [Export]
    public PackedScene MobScene { get; set; }

    private int _score;
    public override void _Ready()
    {
        //NewGame();
    }

    public void GameOver()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
        GetNode<HUD>("HUD").ShowGameOver();
        GetNode<AudioStreamPlayer2D>("DeathSound").Play();
    }

    public void NewGame()
    {
        _score = 0;

        Player player = GetNode<Player>("Player");
        Marker2D startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);
        player.Show();

        GetNode<Timer>("StartTimer").Start();

        HUD hud = GetNode<HUD>("HUD");
        hud.UpdateScore(_score);
        hud.ShowMessage("Get Ready!");

        // Note that for calling Godot-provided methods with strings,
        // we have to use the original Godot snake_case name
        GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
        
        GetNode<AudioStreamPlayer2D>("Music").Play();
    }

    private void OnScoreTimerTimeout()
    {
        _score++;
        GetNode<HUD>("HUD").UpdateScore(_score);
    }

    private void OnStartTimerTimeout()
    {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    private void OnMobTimerTimeout()
    {
        // Create a new instance of the Mob scene
        Mob mob = MobScene.Instantiate<Mob>();

        // Choose a random location on Path2D.
        PathFollow2D mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");

        mobSpawnLocation.ProgressRatio += 0.1f;

        // Set the mob's direction perpendicular to the path direction
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        mob.Position = mobSpawnLocation.Position;

        // Add some randomness to the direction
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        // Choose the velocity
        Vector2 velocity = new Vector2((float)GD.RandRange(150.0f, 250.0f), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        // Spawn the mob by adding it to the Main scene
        AddChild(mob);
    }
}
