﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace Lockstep
{
	public class LSInfluencer
	{
		#region Static Helpers
		static LSAgent tempAgent;
		static GridNode tempNode;
		#endregion

		#region Collection Helper
		[NonSerialized]
		public int bucketIndex = -1;
		#endregion

        #region ScanNode Helper
        public int NodeTicket;
        #endregion

		public GridNode LocatedNode { get; private set;}

		public LSBody Body { get; private set; }

		public LSAgent Agent { get; private set; }

		public void Setup (LSAgent agent)
		{
			Agent = agent;
			Body = agent.Body;
		}

		public void Initialize ()
		{
			LocatedNode = GridManager.GetNode (Body._position.x, Body._position.y);
			LocatedNode.Add (this);
		}

		public void Simulate ()
		{

			if (Body.PositionChangedBuffer) {
				tempNode = GridManager.GetNode (Body._position.x, Body._position.y);

				if (System.Object.ReferenceEquals (tempNode, LocatedNode) == false) {
                    LocatedNode.Remove (this);
					 tempNode.Add (this);
					LocatedNode = tempNode;
				}
			}
		}

		public void Deactivate ()
		{
			LocatedNode.Remove (this);
			LocatedNode = null;
		}
        
		static AllegianceType TargetAllegiance;
		static PlatformType TargetPlatform;
		public LSAgent Scan (int deltaCount,
            AllegianceType targetAllegiance = AllAllegiance,
            PlatformType targetPlatform = AllPlatforms)
		{
			InfluenceManager.Source = Agent;
			InfluenceManager.TargetAllegiance = targetAllegiance;
			InfluenceManager.TargetPlatform = targetPlatform;
			return InfluenceManager.Scan (LocatedNode.ScanX, LocatedNode.ScanY, deltaCount,
			                              this.Agent,targetAllegiance);
		}

		public LSAgent ScanCone (long radius, long angle,
			AllegianceType targetAllegiance = AllAllegiance,
			PlatformType targetPlatform = AllPlatforms)
		{
			InfluenceManager.Source = Agent;
			InfluenceManager.TargetAllegiance = targetAllegiance;
			InfluenceManager.TargetPlatform = targetPlatform;
			return InfluenceManager.ScanCone (LocatedNode.ScanX, LocatedNode.ScanY, radius, angle,
				this.Agent, targetAllegiance);
		}

		public IEnumerable<LSAgent> ScanAll (int deltaCount,
			AllegianceType targetAllegiance = AllAllegiance,
			PlatformType targetPlatform = AllPlatforms)
		{
			InfluenceManager.Source = Agent;
			InfluenceManager.TargetAllegiance = targetAllegiance;
			InfluenceManager.TargetPlatform = targetPlatform;
			return InfluenceManager.ScanAll (LocatedNode.ScanX, LocatedNode.ScanY, deltaCount,
				this.Agent, targetAllegiance);
		}

        const PlatformType AllPlatforms = (PlatformType)~0;
        const AllegianceType AllAllegiance = (AllegianceType)~0;
	}

	public enum PlatformType
	{
		Air             = 1 << 1,
		Ground          = 1 << 2
	}
}