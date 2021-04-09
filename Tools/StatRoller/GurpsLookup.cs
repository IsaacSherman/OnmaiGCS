﻿using System;

namespace StatRoller
{
	public class GurpsLookup
	{
		private static Lazy<GurpsLookup> _instance = new Lazy<GurpsLookup>();

		public static GurpsLookup Instance => _instance.Value;

		public DieString ThrustDamage(int st)
		{
			string ret = "";
			switch (st)
			{
				case 1: ret = "1d-6"; break;
				case 2: ret = "1d-6"; break;
				case 3: ret = "1d-5"; break;
				case 4: ret = "1d-5"; break;
				case 5: ret = "1d-4"; break;
				case 6: ret = "1d-4"; break;
				case 7: ret = "1d-3"; break;
				case 8: ret = "1d-3"; break;
				case 9: ret = "1d-2"; break;
				case 10: ret = "1d-2"; break;
				case 11: ret = "1d-1"; break;
				case 12: ret = "1d-1"; break;
				case 13: ret = "1d "; break;
				case 14: ret = "1d "; break;
				case 15: ret = "1d+1"; break;
				case 16: ret = "1d+1"; break;
				case 17: ret = "1d+2"; break;
				case 18: ret = "1d+2"; break;
				case 19: ret = "2d-1"; break;
				case 20: ret = "2d-1"; break;
				case 21: ret = "2d "; break;
				case 22: ret = "2d "; break;
				case 23: ret = "2d+1"; break;
				case 24: ret = "2d+1"; break;
				case 25: ret = "2d+2"; break;
				case 26: ret = "2d+2"; break;
				case 27: ret = "3d-1"; break;
				case 28: ret = "3d-1"; break;
				case 29: ret = "3d "; break;
				case 30: ret = "3d "; break;
				case 31: ret = "3d+1"; break;
				case 32: ret = "3d+1"; break;
				case 33: ret = "3d+2"; break;
				case 34: ret = "3d+2"; break;
				case 35: ret = "4d-1"; break;
				case 36: ret = "4d-1"; break;
				case 37: ret = "4d "; break;
				case 38: ret = "4d "; break;
				case 39: ret = "4d+1"; break;
				case 40: ret = "4d+1"; break;
				case 41: ret = "4d+2"; break;
				case 42: ret = "4d+2"; break;
				case 43: ret = "5d"; break;
				case 44: ret = "5d"; break;
				case 45: ret = "5d"; break;
				case 46: ret = "5d+1"; break;
				case 47: ret = "5d+1"; break;
				case 48: ret = "5d+1"; break;
				case 49: ret = "5d+2"; break;
				case 50: ret = "5d+2"; break;
				case 51: ret = "5d+2"; break;
				case 52: ret = "5d+2"; break;
				case 53: ret = "5d+2"; break;
				case 54: ret = "5d+2"; break;
				case 55: ret = "6d"; break;
				case 56: ret = "6d"; break;
				case 57: ret = "6d"; break;
				case 58: ret = "6d"; break;
				case 59: ret = "6d"; break;
				case 60: ret = "7d-1"; break;
				case 61: ret = "7d-1"; break;
				case 62: ret = "7d-1"; break;
				case 63: ret = "7d-1"; break;
				case 64: ret = "7d-1"; break;
				case 65: ret = "7d+1"; break;
				case 66: ret = "7d+1"; break;
				case 67: ret = "7d+1"; break;
				case 68: ret = "7d+1"; break;
				case 69: ret = "7d+1"; break;
				case 70: ret = "8d "; break;
				case 71: ret = "8d "; break;
				case 72: ret = "8d "; break;
				case 73: ret = "8d "; break;
				case 74: ret = "8d "; break;
				case 75: ret = "8d+2"; break;
				case 76: ret = "8d+2"; break;
				case 77: ret = "8d+2"; break;
				case 78: ret = "8d+2"; break;
				case 79: ret = "8d+2"; break;
				case 80: ret = "9d "; break;
				case 81: ret = "9d "; break;
				case 82: ret = "9d "; break;
				case 83: ret = "9d "; break;
				case 84: ret = "9d "; break;
				case 85: ret = "9d+2"; break;
				case 86: ret = "9d+2"; break;
				case 87: ret = "9d+2"; break;
				case 88: ret = "9d+2"; break;
				case 89: ret = "9d+2"; break;
				case 90: ret = "10d "; break;
				case 91: ret = "10d "; break;
				case 92: ret = "10d "; break;
				case 93: ret = "10d "; break;
				case 94: ret = "10d "; break;
				case 95: ret = "10d+2"; break;
				case 96: ret = "10d+2"; break;
				case 97: ret = "10d+2"; break;
				case 98: ret = "10d+2"; break;
				case 99: ret = "10d+2"; break;
				case 100: ret = "11d+2"; break;
				case 101: ret = "11d+2"; break;
				case 102: ret = "11d+2"; break;
				case 103: ret = "11d+2"; break;
				case 104: ret = "11d+2"; break;
				case 105: ret = "12d"; break;
				case 106: ret = "12d"; break;
				case 107: ret = "12d"; break;
				case 108: ret = "12d"; break;
				case 109: ret = "12d"; break;
				case 110: ret = "12d+2"; break;
				case 111: ret = "12d+2"; break;
				case 112: ret = "12d+2"; break;
				case 113: ret = "12d+2"; break;
				case 114: ret = "12d+2"; break;
				case 115: ret = "13d"; break;
				case 116: ret = "13d"; break;
				case 117: ret = "13d"; break;
				case 118: ret = "13d"; break;
				case 119: ret = "13d"; break;
				case 120: ret = "13d+2"; break;
				case 121: ret = "13d+2"; break;
				case 122: ret = "13d+2"; break;
				case 123: ret = "13d+2"; break;
				case 124: ret = "13d+2"; break;
				case 125: ret = "14d"; break;
				case 126: ret = "14d"; break;
				case 127: ret = "14d"; break;
				case 128: ret = "14d"; break;
				case 129: ret = "14d"; break;
				case 130: ret = "14d+2"; break;
				case 131: ret = "14d+2"; break;
				case 132: ret = "14d+2"; break;
				case 133: ret = "14d+2"; break;
				case 134: ret = "14d+2"; break;
				case 135: ret = "14d+2"; break;
				case 136: ret = "14d+2"; break;
				case 137: ret = "14d+2"; break;
				case 138: ret = "14d+2"; break;
				case 139: ret = "14d+2"; break;
				case 140: ret = "14d+2"; break;
				case 141: ret = "14d+2"; break;
				case 142: ret = "14d+2"; break;
				case 143: ret = "14d+2"; break;
				case 144: ret = "14d+2"; break;
				case 145: ret = "15d "; break;
				case 146: ret = "15d "; break;
				case 147: ret = "15d "; break;
				case 148: ret = "15d "; break;
				case 149: ret = "15d "; break;
				case 150: ret = "15d+2"; break;
			}
			return new DieString(ret.Trim());
		}

		public DieString SwingDamage(int st)
		{
			string ret = "";
			switch (st)
			{
				case 1: ret = "1d-5"; break;
				case 2: ret = "1d-5"; break;
				case 3: ret = "1d-4"; break;
				case 4: ret = "1d-4"; break;
				case 5: ret = "1d-3"; break;
				case 6: ret = "1d-3"; break;
				case 7: ret = "1d-2"; break;
				case 8: ret = "1d-2"; break;
				case 9: ret = "1d-1"; break;
				case 10: ret = "1d"; break;
				case 11: ret = "1d+1"; break;
				case 12: ret = "1d+2"; break;
				case 13: ret = "2d-1"; break;
				case 14: ret = "2d"; break;
				case 15: ret = "2d+1"; break;
				case 16: ret = "2d+2"; break;
				case 17: ret = "3d-1"; break;
				case 18: ret = "3d"; break;
				case 19: ret = "3d+1"; break;
				case 20: ret = "3d+2"; break;
				case 21: ret = "4d-1"; break;
				case 22: ret = "4d"; break;
				case 23: ret = "4d+1"; break;
				case 24: ret = "4d+2"; break;
				case 25: ret = "5d-1"; break;
				case 26: ret = "5d"; break;
				case 27: ret = "5d+1"; break;
				case 28: ret = "5d+1"; break;
				case 29: ret = "5d+2"; break;
				case 30: ret = "5d+2"; break;
				case 31: ret = "6d-1"; break;
				case 32: ret = "6d-1"; break;
				case 33: ret = "6d"; break;
				case 34: ret = "6d"; break;
				case 35: ret = "6d+1"; break;
				case 36: ret = "6d+1"; break;
				case 37: ret = "6d+2"; break;
				case 38: ret = "6d+2"; break;
				case 39: ret = "7d-1"; break;
				case 40: ret = "7d-1"; break;
				case 45: ret = "7d+1"; break;
				case 50: ret = "8d-1"; break;
				case 51: ret = "8d-1"; break;
				case 52: ret = "8d-1"; break;
				case 53: ret = "8d-1"; break;
				case 54: ret = "8d-1"; break;
				case 55: ret = "8d+1"; break;
				case 56: ret = "8d+1"; break;
				case 57: ret = "8d+1"; break;
				case 58: ret = "8d+1"; break;
				case 59: ret = "8d+1"; break;
				case 60: ret = "9d"; break;
				case 61: ret = "9d"; break;
				case 62: ret = "9d"; break;
				case 63: ret = "9d"; break;
				case 64: ret = "9d"; break;
				case 65: ret = "9d+2"; break;
				case 66: ret = "9d+2"; break;
				case 67: ret = "9d+2"; break;
				case 68: ret = "9d+2"; break;
				case 69: ret = "9d+2"; break;
				case 70: ret = "10d"; break;
				case 71: ret = "10d"; break;
				case 72: ret = "10d"; break;
				case 73: ret = "10d"; break;
				case 74: ret = "10d"; break;
				case 75: ret = "10d+2"; break;
				case 76: ret = "10d+2"; break;
				case 77: ret = "10d+2"; break;
				case 78: ret = "10d+2"; break;
				case 79: ret = "10d+2"; break;
				case 80: ret = "11d"; break;
				case 81: ret = "11d"; break;
				case 82: ret = "11d"; break;
				case 83: ret = "11d"; break;
				case 84: ret = "11d"; break;
				case 85: ret = "11d+2"; break;
				case 86: ret = "11d+2"; break;
				case 87: ret = "11d+2"; break;
				case 88: ret = "11d+2"; break;
				case 89: ret = "11d+2"; break;
				case 90: ret = "12d"; break;
				case 91: ret = "12d"; break;
				case 92: ret = "12d"; break;
				case 93: ret = "12d"; break;
				case 94: ret = "12d"; break;
				case 95: ret = "12d+2"; break;
				case 96: ret = "12d+2"; break;
				case 97: ret = "12d+2"; break;
				case 98: ret = "12d+2"; break;
				case 99: ret = "12d+2"; break;
				case 100: ret = "13d"; break;
				case 101: ret = "13d"; break;
				case 102: ret = "13d"; break;
				case 103: ret = "13d"; break;
				case 104: ret = "13d"; break;
				case 105: ret = "13d+2"; break;
				case 106: ret = "13d+2"; break;
				case 107: ret = "13d+2"; break;
				case 108: ret = "13d+2"; break;
				case 109: ret = "13d+2"; break;
				case 110: ret = "14d"; break;
				case 111: ret = "14d"; break;
				case 112: ret = "14d"; break;
				case 113: ret = "14d"; break;
				case 114: ret = "14d"; break;
				case 115: ret = "14d+2"; break;
				case 116: ret = "14d+2"; break;
				case 117: ret = "14d+2"; break;
				case 118: ret = "14d+2"; break;
				case 119: ret = "14d+2"; break;
				case 120: ret = "15d"; break;
				case 121: ret = "15d"; break;
				case 122: ret = "15d"; break;
				case 123: ret = "15d"; break;
				case 124: ret = "15d"; break;
				case 125: ret = "15d+2"; break;
				case 126: ret = "15d+2"; break;
				case 127: ret = "15d+2"; break;
				case 128: ret = "15d+2"; break;
				case 129: ret = "15d+2"; break;
				case 130: ret = "16d"; break;
				case 131: ret = "16d"; break;
				case 132: ret = "16d"; break;
				case 133: ret = "16d"; break;
				case 134: ret = "16d"; break;
				case 135: ret = "16d"; break;
				case 136: ret = "16d"; break;
				case 137: ret = "16d"; break;
				case 138: ret = "16d"; break;
				case 139: ret = "16d"; break;
				case 140: ret = "16d"; break;
				case 141: ret = "16d"; break;
				case 142: ret = "16d"; break;
				case 143: ret = "16d"; break;
				case 144: ret = "16d"; break;
				case 145: ret = "16d+2"; break;
				case 146: ret = "16d+2"; break;
				case 147: ret = "16d+2"; break;
				case 148: ret = "16d+2"; break;
				case 149: ret = "16d+2"; break;
				case 150: ret = "17d"; break;
			}

			return new DieString(ret.Trim());
		}

	}
}