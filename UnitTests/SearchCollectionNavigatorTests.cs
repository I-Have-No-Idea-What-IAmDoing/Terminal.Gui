﻿using Terminal.Gui;
using Xunit;

namespace Terminal.Gui.Core {
	public class SearchCollectionNavigatorTests {

		[Fact]
		public void TestSearchCollectionNavigator_Cycling ()
		{
			var strings = new string []{
    "appricot",
    "arm",
    "bat",
    "batman",
    "candle"
  };

			var n = new SearchCollectionNavigator (strings);
			Assert.Equal (2, n.CalculateNewIndex ( 0, 'b'));
			Assert.Equal (3, n.CalculateNewIndex ( 2, 'b'));

			// if 4 (candle) is selected it should loop back to bat
			Assert.Equal (2, n.CalculateNewIndex ( 4, 'b'));

		}


		[Fact]
		public void TestSearchCollectionNavigator_ToSearchText ()
		{
			var strings = new string []{
    "appricot",
    "arm",
    "bat",
    "batman",
    "bbfish",
    "candle"
  };

			var n = new SearchCollectionNavigator (strings);
			Assert.Equal (2, n.CalculateNewIndex (0, 'b'));
			Assert.Equal (4, n.CalculateNewIndex (2, 'b'));

			// another 'b' means searching for "bbb" which does not exist
			// so we go back to looking for "b" as a fresh key strike
			Assert.Equal (4, n.CalculateNewIndex (2, 'b'));
		}

		[Fact]
		public void TestSearchCollectionNavigator_FullText ()
		{
			var strings = new string []{
    "appricot",
    "arm",
    "ta",
    "target",
    "text",
    "egg",
    "candle"
  };

			var n = new SearchCollectionNavigator (strings);
			Assert.Equal (2, n.CalculateNewIndex (0, 't'));

			// should match "te" in "text"
			Assert.Equal (4, n.CalculateNewIndex (2, 'e'));

			// still matches text
			Assert.Equal (4, n.CalculateNewIndex (4, 'x'));

			// nothing starts texa so it jumps to a for appricot
			Assert.Equal (0, n.CalculateNewIndex (4, 'a'));
		}

		[Fact]
		public void TestSearchCollectionNavigator_Unicode ()
		{
			var strings = new string []{
    "appricot",
    "arm",
    "ta",
    "丗丙业丞",
    "丗丙丛",
    "text",
    "egg",
    "candle"
  };

			var n = new SearchCollectionNavigator (strings);
			Assert.Equal (3, n.CalculateNewIndex (0, '丗'));

			// 丗丙业丞 is as good a match as 丗丙丛
			// so when doing multi character searches we should
			// prefer to stay on the same index unless we invalidate
			// our typed text
			Assert.Equal (3, n.CalculateNewIndex (3, '丙'));

			// No longer matches 丗丙业丞 and now only matches 丗丙丛
			// so we should move to the new match
			Assert.Equal (4, n.CalculateNewIndex (3, '丛'));

			// nothing starts "丗丙丛a" so it jumps to a for appricot
			Assert.Equal (0, n.CalculateNewIndex (4, 'a'));
		}

		[Fact]
		public void TestSearchCollectionNavigator_AtSymbol ()
		{
			var strings = new string []{
    "appricot",
    "arm",
    "ta",
    "@bob",
    "@bb",
    "text",
    "egg",
    "candle"
  };

			var n = new SearchCollectionNavigator (strings);
			Assert.Equal (3, n.CalculateNewIndex (0, '@'));
			Assert.Equal (3, n.CalculateNewIndex (3, 'b'));
			Assert.Equal (4, n.CalculateNewIndex (3, 'b'));
		}
	}
}
