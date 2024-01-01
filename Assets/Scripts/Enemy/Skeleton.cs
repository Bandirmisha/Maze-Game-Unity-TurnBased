using UnityEngine;


namespace MazeGame
{
    public class Skeleton : Enemy
    {
        private float shootCooldown { get; }
        private float time { get; set; }

        public Skeleton()
        {
            shootCooldown = 1f;
        }

        public override void Event()
        {
            base.Event();

            if (!alive) return;

            time += Time.fixedDeltaTime;
            if (time >= shootCooldown)
            {
                time = 0;
                Shoot();
            }
        }

        public void Shoot()
        {
            //ViewModel.instance.view.CreateGameObject(ViewModel.instance.view.arrowPrefab, currentPosition);
        }

    }
}
