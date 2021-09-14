using System;
using System.Collections.Generic;
using Sandbox;

namespace ActionTag
{
    public abstract partial class BaseRound : Networked
    {
        public virtual int RoundDuration => 0;
        public virtual string RoundName => "";

        public List<ActionTagPlayer> Players = new();
        public List<ActionTagPlayer> Spectators = new();

        public float RoundEndTime { get; set; }

        public float TimeLeft
        {
            get
            {
                return RoundEndTime - Sandbox.Time.Now;
            }
        }

        [Net]
        public string TimeLeftFormatted { get; set; }

        public void Start()
        {
            if (Host.IsServer && RoundDuration > 0)
            {
                RoundEndTime = Sandbox.Time.Now + RoundDuration;
                TimeLeftFormatted = TimeSpan.FromSeconds(TimeLeft).ToString(@"mm\:ss");
            }

            OnStart();
        }

        public void Finish()
        {
            if (Host.IsServer)
            {
                RoundEndTime = 0f;
                Players.Clear();
                Spectators.Clear();
            }

            OnFinish();
        }

        protected void AddPlayer(ActionTagPlayer player)
        {
            Host.AssertServer();

            if (!Players.Contains(player))
            {
                Players.Add(player);
            }
        }
        
        protected void AddSpectator(ActionTagPlayer player)
        {
	        Host.AssertServer();

	        if (!Spectators.Contains(player))
	        {
		        Spectators.Add(player);
	        }
        }

        public virtual void OnPlayerSpawn(ActionTagPlayer player) { }

        public virtual void OnPlayerKilled(ActionTagPlayer player) { }

        public virtual void OnPlayerLeave(ActionTagPlayer player)
        {
            Players.Remove(player);
            Spectators.Remove(player);
        }

        public virtual void OnSecond()
        {
	        if ( !Host.IsServer )
	        {
		        return;
	        }

	        if (RoundEndTime > 0 && Sandbox.Time.Now >= RoundEndTime)
	        {
		        RoundEndTime = 0f;
		        OnTimeUp();
	        }
	        else
	        {
		        TimeLeftFormatted = TimeSpan.FromSeconds(TimeLeft).ToString(@"mm\:ss");
	        }
        }

        protected virtual void OnStart() { }

        protected virtual void OnFinish() { }

        protected virtual void OnTimeUp() { }
    }
}
