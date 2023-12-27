using UnityEngine;


namespace MazeGame
{
    public class Skeleton : Enemy
    {
        [SerializeField] private float shootCooldown;
        private float time;

        public Skeleton(Vector3 startPost) : base(startPost)
        {
            shootCooldown = 1f;
        }

        public override void Event()
        {
            base.Event();

            if (!alive) return;

            time += Time.deltaTime;
            if (time >= shootCooldown)
            {
                time = 0;
                Shoot();
            }
        }

        public void Shoot()
        {
            ViewModel.instance.view.CreateGameObject(ViewModel.instance.view.arrowPrefab, currentPosition);
        }

    }
}
