using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SwinGameSDK;

/// <summary>
/// The battle phase is handled by the DiscoveryController.
/// </summary>
static class DiscoveryController
{
	public static int RandNum (int min, int max)
	{
		Random rnd = new Random ();
		return rnd.Next (min, max);
	}
	/// <summary>
	/// Handles input during the discovery phase of the game.
	/// </summary>
	/// <remarks>
	/// Escape opens the game menu. Clicking the mouse will
	/// attack a location.
	/// </remarks>
	public static void HandleDiscoveryInput()
	{
		//generate random number to randomize the music switching
		int numero = RandNum (0, 4);
		string song = "";

		switch (numero) {
		case 0: 
			song = "Background_3"; //music 1 - ACDC
			break;
		case 1:
			song = "Background_1"; //music 3 - Boom Boom
			break;
		case 2:
			song = "Background_2"; //music 2 - Bang Bang
			break;
		case 3:
			song = "Background_0";//music 0 - dark music
			break;
		}


		//controlling music with 'M' key
		if (SwinGame.KeyTyped (KeyCode.vk_m)) {

			SwinGame.PlayMusic (GameResources.GameMusic (song)); //using random number to switch between music
		}

		if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE)) {
			GameController.AddNewState(GameState.ViewingGameMenu);
			SwinGame.ResumeTimer (GameTimer);

		}
		if (SwinGame.MouseClicked(MouseButton.LeftButton)) {
			DoAttack();
		}
	}

	/// <summary>
	/// Attack the location that the mouse if over.
	/// </summary>
	private static void DoAttack()
	{
		Point2D mouse = default(Point2D);

		mouse = SwinGame.MousePosition();

		//Calculate the row/col clicked
		int row = 0;
		int col = 0;
		row = Convert.ToInt32(Math.Floor((mouse.Y - UtilityFunctions.FIELD_TOP) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
		col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));

		if (row >= 0 & row < GameController.HumanPlayer.EnemyGrid.Height) {
			if (col >= 0 & col < GameController.HumanPlayer.EnemyGrid.Width) {
				GameController.Attack(row, col);
			}
		}
	}

public static Timer GameTimer = SwinGame.CreateTimer ();
public static uint _time;
public static string s;
public static int x = 0;
public static uint min = 0;

	/// <summary>
	/// Draws the game during the attack phase.
	/// </summary>s
	public static void DrawDiscovery()
	{
		const int SCORES_LEFT = 172;
		const int SHOTS_TOP = 157;
		const int HITS_TOP = 206;
		const int SPLASH_TOP = 256;

		if (x == 0) {
			SwinGame.ResetTimer (GameTimer);
			SwinGame.StartTimer (GameTimer);
			x++;
		}

		if ((SwinGame.KeyDown(KeyCode.vk_LSHIFT) | SwinGame.KeyDown(KeyCode.vk_RSHIFT)) & SwinGame.KeyDown(KeyCode.vk_c)) {
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, true);
		} else {
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, false);
		}

			UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
			UtilityFunctions.DrawMessage();

		SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
		SwinGame.DrawText(GameController.HumanPlayer.Hits.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, HITS_TOP);
		SwinGame.DrawText(GameController.HumanPlayer.Missed.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);
		//To Show the value of the remaining enemy's ships on screens
		SwinGame.DrawText (GameController.remainingShip.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, 306);
		s = _time.ToString ();
		_time = SwinGame.TimerTicks (GameTimer) / 1000;

		SwinGame.DrawTextLines("Time: " +s, Color.Blue, Color.Transparent, GameResources.GameFont("Menu"), FontAlignment.AlignCenter, (SwinGame.ScreenWidth()/2)-450, 94, 400, 15);
		if (_time == 300)
		{
			SwinGame.DrawTextLines("Click the mouse to Exit    ", Color.Yellow, Color.Transparent, GameResources.GameFont("Menu"), FontAlignment.AlignRight, 0, 550, SwinGame.ScreenWidth(), SwinGame.ScreenHeight());
			SwinGame.DrawTextLines("TIME UP!!!", Color.Yellow, Color.Transparent, GameResources.GameFont("ArialLarge"), FontAlignment.AlignCenter, 0, 250, SwinGame.ScreenWidth(), SwinGame.ScreenHeight());
			SwinGame.PauseTimer (GameTimer);
			if (SwinGame.MouseClicked(MouseButton.LeftButton)) {
				SwinGame.ResetTimer (GameTimer);
				GameController.AddNewState(GameState.Quitting);
			}
		}
	}

}