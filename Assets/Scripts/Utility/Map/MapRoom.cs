using UnityEngine;
using System.Collections;

namespace Uniboom.Utility.Map {

	// Room element
	public class MapRoom
	{
		// Some params
		public uint pos_x;
		public uint pos_y;
		public int roomType = 1;
		public bool isGenerated = false;
		
		// Left, right, up or down doors if exists 
		public MapDoor left = null;
		public MapDoor right = null;
		public MapDoor up = null;
		public MapDoor down = null;

		public MapRoom()
		{}
	}
}
