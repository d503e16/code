namespace Rankingsystem.Classes.Roles
{
    public class Jungle : Role
    {
        public Jungle(bool fb, bool ft, double kda, double kp, long ownMonsters, 
            long enemyMonsters, long wards) :
            base(fb, ft, kda, kp)
        {
            this.ownMonsters = ownMonsters;
            this.enemyMonsters = enemyMonsters;
            this.wards = wards;
        }
        //Jungle features
        private long ownMonsters;
        private long enemyMonsters;
        private long wards;

        public long Wards
        {
            get { return wards; }
            set { wards = value; }
        }

        public long EnemyMonsters
        {
            get { return enemyMonsters; }
            set { enemyMonsters = value; }
        }

        public long OwnMonsters
        {
            get { return ownMonsters; }
            set { ownMonsters = value; }
        }

    }
}