﻿using HearthDb.Enums;
using Hearthstone_Deck_Tracker.LogReader.Interfaces;
using Hearthstone_Deck_Tracker.Utility;
using Entity = Hearthstone_Deck_Tracker.Hearthstone.Entities.Entity;

namespace Hearthstone_Deck_Tracker.Hearthstone.CounterSystem.Counters;

public class PlayedDragonsCounter : NumericCounter
{
	public override string LocalizedName => LocUtil.Get("Counter_PlayedDragons", useCardLanguage: true);
	protected override string? CardIdToShowInUI => HearthDb.CardIds.Collectible.Priest.TimewinderZarimi;

	public override string[] RelatedCards => new string[]
	{
		HearthDb.CardIds.Collectible.Priest.TimewinderZarimi,
		HearthDb.CardIds.Collectible.Neutral.Kazakusan,
	};

	public PlayedDragonsCounter(bool controlledByPlayer, GameV2 game) : base(controlledByPlayer, game)
	{
	}

	public override bool ShouldShow()
	{
		if(!Game.IsTraditionalHearthstoneMatch) return false;
		if(IsPlayerCounter)
			return InPlayerDeckOrKnown(RelatedCards);
		return Counter >= 2 && OpponentMayHaveRelevantCards();
	}

	public override string[] GetCardsToDisplay()
	{
		return IsPlayerCounter ?
			GetCardsInDeckOrKnown(RelatedCards).ToArray() :
			FilterCardsByClassAndFormat(RelatedCards, Game.Opponent.OriginalClass);
	}

	public override string ValueToShow() => Counter.ToString();
	public override void HandleTagChange(GameTag tag, IHsGameState gameState, Entity entity, int value, int prevValue)
	{
		if(!Game.IsTraditionalHearthstoneMatch)
			return;

		if(entity.IsControlledBy(Game.Player.Id) != IsPlayerCounter)
			return;

		if(!entity.IsMinion)
			return;

		if(!entity.Card.IsDragon())
			return;

		if(tag != GameTag.ZONE)
			return;

		if(value != (int)Zone.PLAY)
			return;

		if(gameState.CurrentBlock?.Type != "PLAY")
			return;

		Counter++;
	}
}
