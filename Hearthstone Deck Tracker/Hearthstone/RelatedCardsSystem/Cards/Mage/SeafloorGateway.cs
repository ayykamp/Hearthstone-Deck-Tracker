using System.Collections.Generic;

namespace Hearthstone_Deck_Tracker.Hearthstone.RelatedCardsSystem.Cards.Mage;

public class SeafloorGateway : ICardWithHighlight
{
	public string GetCardId() => HearthDb.CardIds.Collectible.Mage.SeafloorGateway;

	public HighlightColor ShouldHighlight(Card card, IEnumerable<Card> deck) =>
		HighlightColorHelper.GetHighlightColor(card.IsMech());
}
