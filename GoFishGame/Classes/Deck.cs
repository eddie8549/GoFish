namespace GoFishGame.Classes.Deck
{
  using System;
  using System.Collections.Generic;
  using Card;

  class Deck
  {

    #region Member Variables

    /// <summary>
    /// A list that holds the <see cref="Card"/>s in the deck
    /// </summary>
    private List<Card> theDeck = new List<Card>();

    /// <summary>
    /// Instance of the Random class that will be used to make shuffling more random
    /// </summary>
    private Random randInstance = new Random();

    #endregion

    #region Constructors

    /// <summary>
    /// Creates an instance of a <see cref="Deck"/> and populates it with the standard 52 cards
    /// </summary>
    public Deck()
    {
      try
      {
        // Loop through each suit then each rank to generate all the cards
        foreach (Card.Suit suit in Enum.GetValues(typeof(Card.Suit)))
        {
          foreach (Card.Rank rank in Enum.GetValues(typeof(Card.Rank)))
            theDeck.Add(new Card(rank, suit));
        }
      }
      catch (Exception ex)
      {
        throw new Exception($"--Something when wrong creating the deck--\n--{ex.Message}");
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the number of cards in the deck
    /// </summary>
    public int Count
    {
      get
      {
        return theDeck.Count;
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Called to shuffle the deck
    /// </summary>
    public void Shuffle()
    {
      List<Card> firstHalf;
      List<Card> secondHalf;

      try
      {
        for (int i = 0; i < 4; i++) // Do the shuffle multiple times
        {
          SplitDeck(out firstHalf, out secondHalf);
          // After splitting the deck, both halves represent the entire deck
          // Now I reinitialize the main deck to insert the cards in a shuffled order
          theDeck = new List<Card>();

          /* Here, I loop through the halves of the deck to insert one card at a time
           * into the shuffled version of the deck. After inserting a card, I remove
           * that card from the half from which it came changing its count for the loop control. 
           * Once either half has been fully inserted, I insert whater is remaining from 
           * the other half into the shuffled deck. */
          while (firstHalf.Count > 0 && secondHalf.Count > 0)
          {
            // Generate a random 0 or 1 to determine which half the card comes from
            if (randInstance.Next(2) == 0)
            {
              theDeck.Add(firstHalf[0]);
              firstHalf.RemoveAt(0);
            }
            else
            {
              theDeck.Add(secondHalf[0]);
              secondHalf.RemoveAt(0);
            }
          }
          if (firstHalf.Count != 0)
          {
            foreach (Card card in firstHalf)
              theDeck.Add(card);
            // I don't need to remove from the half now because the count is not my loop control
          }
          if (secondHalf.Count != 0)
          {
            foreach (Card card in secondHalf)
              theDeck.Add(card);
          }
        }
        // Now the deck reflects a shuffled version of itself
      }
      catch (Exception ex)
      {
        throw new Exception($"--Something when wrong shuffling the deck--\n--{ex.Message}");
      }
    }

    /// <summary>
    /// Grabs the card at the 'top' of the deck to return
    /// </summary>
    /// <returns>The card that was at the 'top' of the deck</returns>
    public Card DrawOne()
    {
      Card card = theDeck[Count - 1];
      theDeck.RemoveAt(Count - 1);
      return card;
    }

    /// <summary>
    /// Removes a specified number of cards from the deck to be returned in a List
    /// </summary>
    /// <param name="n">The number of cards to draw</param>
    /// <returns>The <see cref="List<Card>"/> removed from the deck</returns>
    public List<Card> DrawMore(int n)
    {
      List<Card> cards = new List<Card>();
      // loopControl is needed because Count changes on each iteration
      int loopControl = Count - n;
      for (int i = (Count - 1); i >= loopControl; i--)
      {
        cards.Add(theDeck[i]);
        theDeck.RemoveAt(i);
      }

      return cards;
    }

    /// <summary>
    /// Creates a string representation of how many cards remain in the deck
    /// </summary>
    /// <returns>Desciption of how many cards are in the deck</returns>
    public override string ToString()
    {
      // Use the ternary operator to make the output more readable
      return $"{Count} {(Count == 1 ? "card" : "cards")} in the deck.";
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Separates the deck into two halves for use when shuffling
    /// </summary>
    /// <param name="firstHalf">out paramter that will reference the 'first' half after splitting</param>
    /// <param name="secondHalf">out paramter that will reference the 'second' half after splitting</param>
    private void SplitDeck(out List<Card> firstHalf, out List<Card> secondHalf)
    {
      // Required initialization of out parameters
      firstHalf = new List<Card>();
      secondHalf = new List<Card>();

      try
      {
        int midPoint = theDeck.Count / 2;
        if (theDeck.Count > 10)
          // Generate a little bit of randomization for the mid point
          midPoint += randInstance.Next(-4, 5);

        // Now separate the deck into two 'halves' using the mid point
        for (int i = 0; i < midPoint; i++)
          firstHalf.Add(theDeck[i]);

        for (int i = midPoint; i < theDeck.Count; i++)
          secondHalf.Add(theDeck[i]);
        // The deck is now split and each half is 'returned' via the out parameters
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("--Error splitting the deck--\n--{0}", ex.Message));
      }
    }

    #endregion
  }
}
