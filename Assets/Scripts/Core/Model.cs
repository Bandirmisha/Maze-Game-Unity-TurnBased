using MazeGame;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Model
{
    public static Model inst { get; private set; }

    private Maze maze { get; set; }

    private Player player { get; set; }
    private List<Enemy> zombies { get; set; }
    private List<Enemy> skeletons { get; set; }
    private List<Arrow> arrows { get; set; }

    public Action<Vector3> onSkeletonShoot { get; set; }

    public Model()
    {
        inst = this;

        maze = new Maze();

        player = new Player();
        CreateZombies();
        CreateSkeletons();

        arrows = new List<Arrow>();
        onSkeletonShoot += CreateArrow;
    }

    #region Инициализация объектов
    private void CreateZombies()
    {
        zombies = new List<Enemy>();
        int zombieCount = UnityEngine.Random.Range(5, 10);
        for (int i = 0; i < zombieCount; i++)
        {
            zombies.Add(new Zombie(GetEnemyStartPos()));
        }
    }

    private void CreateSkeletons()
    {
        skeletons = new List<Enemy>();
        int skeletonCount = UnityEngine.Random.Range(3, 6);
        for (int i = 0; i < skeletonCount; i++)
        {
            skeletons.Add(new Skeleton(GetEnemyStartPos()));
        }
    }

    private Vector3 GetEnemyStartPos()
    {
        Vector3 vec;

        do
        {
            vec = new Vector3(UnityEngine.Random.Range(3, mazeWidth), 0, -UnityEngine.Random.Range(3, mazeHeight));
        }
        while (Field[(int)vec.x, -(int)vec.z].type == CellType.Wall);

        return vec;
    }

    private void CreateArrow(Vector3 startPos)
    {
        arrows.Add(new Arrow(startPos));
    }
    #endregion

    /// <summary>
    /// Метод, выполняющийся каждый кадр
    /// </summary>
    public void Tick()
    {
        if (player.isMoving)
            player.Move();

        foreach (var enemy in zombies.Concat(skeletons))
            enemy.Event();

        foreach (var arrow in arrows)
            arrow.Move();
    }

    public void PlayerInputAction(KeyCode keyCode)
    {
        if (!player.isMoving)
        {
            if (keyCode == KeyCode.Mouse0)
                player.Attack();

            if (keyCode == KeyCode.W)
                player.SetDestination(Vector3.forward);
            else if (keyCode == KeyCode.A)
                player.SetDestination(Vector3.left);
            else if (keyCode == KeyCode.S)
                player.SetDestination(Vector3.back);
            else if (keyCode == KeyCode.D)
                player.SetDestination(Vector3.right);
        }
    }



    public bool CheckDestination(Vector3 targetPosition, Vector3 direction)
    {
        Vector3 tempPos = targetPosition + direction;

        if (maze.field[(int)tempPos.x, (int)tempPos.z * (-1)].type == CellType.Floor)
        {
            if (zombies.Concat(skeletons).Any(x => x.targetPosition == tempPos) ||
                player.targetPosition == tempPos)
                return false;

            return true;
        }

        return false;
    }

    public bool isPlayerNearby(Vector3 targetPosition)
    {
        if (player.targetPosition.x == targetPosition.x - 1 && player.targetPosition.z == targetPosition.z ||
            player.targetPosition.x == targetPosition.x && player.targetPosition.z == targetPosition.z - 1 ||
            player.targetPosition.x == targetPosition.x + 1 && player.targetPosition.z == targetPosition.z ||
            player.targetPosition.x == targetPosition.x && player.targetPosition.z == targetPosition.z + 1)
            return true;
        else
            return false;
    }

    public void PlayerTakeDamage(int damage)
    {
        player.TakeDamage(damage);
    }

    public void EnemyTakeDamage(Vector3 enemyPos, int damage)
    {
        zombies.Concat(skeletons).First(x => x.targetPosition == enemyPos).TakeDamage(damage);
    }



    #region Публичные методы для получения приватных данных

    public int mazeWidth
    {
        get { return maze.width; }
    }
    public int mazeHeight
    {
        get { return maze.height; }
    }
    public Cell[,] Field { get { return maze.field; } }
    public Vector3 keyPos { get { return maze.keyPos; } }
    public bool isKeyPicked { get { return player.isKeyPicked; } }
    public Vector3 exitPos { get { return maze.exitPos; } }

    public string quest { get { return player.Quest; } }
    public int playerHP { get { return player.HP; } }
    public Vector3 playerCurrentPosition { get { return player.currentPosition; } }
    public List<EnemyRenderData> GetZombiesRenderData()
    {
        List<EnemyRenderData> zombiesRI = new List<EnemyRenderData>();

        foreach (var zombie in zombies)
            zombiesRI.Add(new EnemyRenderData(zombie.HP, zombie.currentPosition));

        return zombiesRI;
    }
    public List<EnemyRenderData> GetSkeletonsRenderData()
    {
        List<EnemyRenderData> skeletonsRI = new List<EnemyRenderData>();

        foreach (var skeleton in skeletons)
            skeletonsRI.Add(new EnemyRenderData(skeleton.HP, skeleton.currentPosition));

        return skeletonsRI;
    }
    public List<Vector3> GetArrowsCurrentPositions()
    {
        List<Vector3> ars = new List<Vector3>();

        if (arrows.Count > 0)
        {
            foreach (var arrow in arrows)
                    ars.Add(arrow.currentPosition);
        } 

        return ars;
    }

    #endregion
}
