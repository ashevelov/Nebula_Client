namespace Nebula {
    using System;

    public class InterestArea
    {
        private readonly byte areaId;
        private readonly NetworkGame game;

        [CLSCompliant(false)]
        public InterestArea(byte areaId, NetworkGame game, MyItem avatar)
            : this(areaId, game, avatar.Position)
        {
            this.AttachedItem = avatar;
            avatar.Moved += this.OnItemMoved;
        }

        [CLSCompliant(false)]
        public InterestArea(byte areaId, NetworkGame game, float[] position)
        {
            this.game = game;
            this.areaId = areaId;
            this.Position = position;
        }
        public MyItem AttachedItem { get; private set; }

        [CLSCompliant(false)]
        public NetworkGame Game
        {
            get
            {
                return this.game;
            }
        }
        public byte Id
        {
            get
            {
                return this.areaId;
            }
        }
        public float[] Position { get; private set; }

        public void AttachItem(MyItem item)
        {
            if (this.AttachedItem != null)
            {
                this.AttachedItem.Moved -= this.OnItemMoved;
                this.AttachedItem = null;
            }

            this.AttachedItem = item;
            item.Moved += this.OnItemMoved;

            Operations.AttachInterestArea(this.game, item.Id, item.Type);
            item.SetInterestAreaAttached(true);
        }

        public void Create()
        {
            //this.game.AddCamera(this);
            Operations.AddInterestArea(this.game, this.Id, this.Position, Game.Settings.ViewDistanceEnter, Game.Settings.ViewDistanceExit);
        }

        public void Detach()
        {
            if (this.AttachedItem != null)
            {
                this.AttachedItem.Moved -= this.OnItemMoved;
                this.AttachedItem.SetInterestAreaAttached(false);
                this.AttachedItem = null;
            }

            Operations.DetachInterestArea(this.game);
        }

        public void Move(float[] newPosition)
        {
            if (this.AttachedItem == null)
            {
                this.Position = newPosition;
                Operations.MoveInterestArea(this.game, this.areaId, newPosition);
                return;
            }

            throw new InvalidOperationException("cannot move attached interest area manually");
        }
        public void Remove()
        {
            Operations.RemoveInterestArea(this.game, this.Id);
            //this.game.RemoveCamera(this.areaId);
        }
        public void ResetViewDistance()
        {
            Operations.SetViewDistance(this.game, Game.Settings.ViewDistanceEnter, Game.Settings.ViewDistanceExit);
        }

        private void OnItemMoved(Item item)
        {
            this.Position = item.Position;
        }
    }
}
