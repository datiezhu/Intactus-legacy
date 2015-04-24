﻿
//
//  Program.cs
//
//  Created by Pouya Kary on 2015/4/17
//  Copyright (c) 2015 Pouya Kary. All rights reserved.
//


using System;
using System.Collections.Generic;
using Kary.Calculat;
using Kary.Leopard;
using Kary.Leopard.Text;

namespace Int
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Expression load_expr = new Expression ("23 - 23");
			var ans = load_expr.Evaluate ();

			List<Space> spaces = new List<Space> ();
			List<string> history = new List<string> (); 


		

			//
			// ──────────────────────────────────────────────────────────── I ─────────
			//  :::::: I N T E R F A C E : : :  :  :   :     :        :          :
			// ─────────────────────────────────────────────────────────────────────
			//


			Terminal.Title = "I N T A C T U S";

			Terminal.NewLine ();
			Terminal.PrintLn ("Intactus, The Calculation Machine.");
			Terminal.PrintLn ("Copyright 2015 Kary Systems, Inc. All rights reseved");
			Terminal.PrintLn ("Kary.Intactus, Kary.Calculat, Kary.Leopard and Kary.Numerica");
			Terminal.PrintLn ("Are copyrights of Kary Systems, Inc. All rights reserved.");

			//
			// COUNTERS
			//

			int IO_counter = 1;

			while (true) {

				var input = Interface.Input (IO_counter).Replace (" vs ", "&");


				Terminal.PrintLn ("");

				if (input == "c") {

					Terminal.Clean ();

					history.Add ("0");

				} else if (input == "e") {

					history.Add ("0");
					Terminal.PrintLn ();
					Terminal.Title = "";
					Environment.Exit (0);

				} else if (input.StartsWith ("load ")) {

					try {

						int input_number_to_load = InputToLoadNumberFromString (input, "load");
						EvaluateWithResult (history [input_number_to_load], ref ans, IO_counter, spaces);

						history.Add (history [input_number_to_load]);

					} catch {

						Interface.PrintOut ("bad index number...", IO_counter);
						history.Add ("0");

					}

				
				} else if (input.StartsWith ("simple ")) {

					try {

						int input_number_to_load = InputToLoadNumberFromString (input, "simple");
						Interface.PrintOut (history [input_number_to_load].Replace ("&"," vs "), IO_counter);
						history.Add ("0");

					} catch {

						Interface.PrintOut ("bad index number...", IO_counter);
						history.Add ("0");

					}
				

				} else if (input.Split ('&').Length > 1) {

					history.Add (input);
					var parts = input.Split ('&');
					string[] results = new string[parts.Length];

					for (int i = 0; i < parts.Length; i++) {

						results [i] = Evaluate (parts [i], ref ans, spaces).ToString ();

					}

					Interface.PrintColumnChart (results, IO_counter);

					ans = 0;


				} else if (input.Split ('?').Length == 2) { 

					var parts = input.Split ('?'); // 0 -> name 1 -> value

					EvaluateWithResult (parts[1], ref ans, IO_counter, spaces);

					var space = new Space ();

					space.name = parts [0].Replace (" ", "");
					space.value = ans;

					bool add_the_space = true;

					foreach (var find_space in spaces) {
						if (find_space.name == space.name) {
							add_the_space = false;
						}
					}

					if (add_the_space) {
						spaces.Add (space);
					} else {
						for (int J = 0; J < spaces.Count; J++) {
							if (spaces[J].name == space.name) {
								spaces [J] = space;
								J = spaces.Count;
							}
						}
					}

					history.Add (parts[1]);

				} else {

					EvaluateWithResult (input, ref ans, IO_counter, spaces);
					history.Add (input);
				}

				IO_counter++;
			}
		}




		//
		// ───────────────────────────────────────────────────────────── II ─────────
		//  :::::: E V A L U A T I O N : : :  :  :   :     :        :          :
		// ──────────────────────────────────────────────────────────────────────
		//


		public static void EvaluateWithResult (string input, ref object ans, int IO_counter, List<Space> spaces) {

			try {

				Interface.PrintOut (Evaluate (input, ref ans, spaces).ToString (), IO_counter);

			} catch  {

				Interface.PrintOut ("Operation Failure", IO_counter);

			}
		}




		public static string Evaluate (string input, ref object ans, List<Space> spaces) {

			try {
				Expression expr = new Expression (input);

				expr.Parameters.Add ("ans", ans);
				expr.Parameters.Add ("pi", Math.PI);

				foreach ( var space in spaces ) {

					expr.Parameters.Add (space.name, space.value);

				}

				var result = expr.Evaluate ();
				ans = result;

				return result.ToString ();

			} catch  {

				return "Operation Failure";

			}

		}





		public struct Space {

			public string name;
			public object value;

		}




		public static int InputToLoadNumberFromString(string text, string command) {

			return int.Parse (Utilities.RemoveFromStart (text, command + ' ').TrimEnd ().TrimStart ()) - 1;

		}
	}
}