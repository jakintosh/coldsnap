﻿using System.Collections.Generic;
using UnityEngine;

public class LevelLoader {

	private Vector2? _player1Start;
	private Vector2? _player2Start;

	public void LoadDefaultLevel () {

		string defaultLayout = 
			"0   00000000000000000   0" +
			"0   0               0   0" +
			"0   0               0   0" +
			"0   0               0   0" +
			"0   a                   0" +
			"0                       0" +
			"0      00000000000      0" +
			"0                       0" +
			"0                       0" +
			"0                       0" +
			"0000                 0000" +
			"        000000000        " +
			"        000000000        " +
			"        000000000        " +
			"0   00000000000000000   0";

		LoadLevel( defaultLayout );
	}
	public void LoadLevel ( string layout ) {

		int pos = -1;
		foreach ( char c in layout ) {

			pos++;

			// if space, move on
			if ( c == ' ' ) {
				continue;
			}

			// get tile position
			var x = pos % SCREEN_W;
			var y = pos / SCREEN_W;
			var tilePos = new Vector2( x, y );

			// if player spawn, move on
			if ( c == 'a' ) {
				_player1Start = tilePos;
				continue;
			} if ( c == 'b' ) {
				_player2Start = tilePos;
				continue;
			}

			// spawn tile
			if ( _tileSymbols.ContainsKey( c ) ) {
				SpawnTile( _tileSymbols[c], tilePos );
			} else {
				Debug.LogError( "Tried to spawn a tile with an invalid symbol." );
				SpawnTile( TileType.INVALID, tilePos );
			}
		}
	}
	public void SpawnPlayers () {

		if ( _player1Start.HasValue ) {

			var prefab = Game.Resources.Player.StandardPlayer;
			var startPos = _player1Start.Value + new Vector2( 0f, 1f );
			var playerGO = Object.Instantiate( prefab, startPos, Quaternion.identity ) as GameObject;
			playerGO.transform.SetParent( Game.View.Environment );
		}

		if ( _player2Start.HasValue ) {

			var prefab = Game.Resources.Player.StandardPlayer;
			var startPos = _player2Start.Value + new Vector2( 0f, 1f );
			var playerGO = Object.Instantiate( prefab, startPos, Quaternion.identity ) as GameObject;
			playerGO.transform.SetParent( Game.View.Environment );
		}
	}

	// ****************** Tile Loading ******************

	private const int SCREEN_W = 25;
	private const int SCREEN_H = 15;

	// define tiles
	private enum TileType {
		INVALID = -1,
		DEFAULT,
		NUM_TILES
	}

	// map chars to tiles
	private Dictionary<char,TileType> _tileSymbols = new Dictionary<char, TileType> () {
		{ '0', TileType.DEFAULT }
	};

	// map tiles to prefabs
	private Dictionary<TileType, GameObject> _tilePrefabs = new Dictionary<TileType, GameObject> () {
		{ TileType.DEFAULT, Game.Resources.Environment.Tiles.Default },
		{ TileType.INVALID, Game.Resources.Environment.Tiles.Invalid }
	};

	private void SpawnTile ( TileType type, Vector2 position ) {

		// get prefab
		GameObject prefab = null;
		if ( _tilePrefabs.ContainsKey( type ) ) {
			prefab = _tilePrefabs[type];
		} else {
			Debug.LogWarning( "Prefab not linked for tile type; couldn't spawn tile." );
			return;
		}

		// get position
		var x = position.x + 0.5f;
		var y = position.y + 0.5f;
		var pos = new Vector3( x, y );

		var tileGO = Object.Instantiate( prefab, pos, Quaternion.identity ) as GameObject;
		tileGO.transform.SetParent( Game.View.Environment );
	}

	// **************************************************
}
