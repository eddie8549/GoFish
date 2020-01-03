namespace GoFish.Classes
{
  class Card
  {

    #region Constructors

    /// <summary>
    /// Creates a default instance of a <see cref="Card"/> (Ace of Spaces)
    /// </summary>
    public Card()
    {
      CardRank = Rank.Ace;
      CardSuit = Suit.Spades;
    }

    /// <summary>
    /// Creates a new instace of a specified card
    /// </summary>
    /// <param name="r">The <see cref="Rank"/> of the new <see cref="Card"/></param>
    /// <param name="s">The <see cref="Suit"/> of the new <see cref="Card"/> </param>
    public Card(Rank r, Suit s)
    {
      CardRank = r;
      CardSuit = s;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the <see cref="Rank"/>
    /// </summary>
    public Rank CardRank
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets or sets the <see cref="Suit"/>
    /// </summary>
    public Suit CardSuit
    {
      get;
      private set;
    }

    #endregion

    #region Enums

    /// <summary>
    /// Defines the card ranks Ace through King
    /// </summary>
    public enum Rank
    {
      Ace,
      Two,
      Three,
      Four,
      Five,
      Six,
      Seven,
      Eight,
      Nine,
      Ten,
      Jack,
      Queen,
      King
    }

    /// <summary>
    /// Defines the card suits
    /// </summary>
    public enum Suit
    {
      Spades,
      Clubs,
      Hearts,
      Diamonds
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Generates the string representation of the <see cref="Card"/>
    /// </summary>
    /// <returns>A string describing the <see cref="Card"/></returns>
    public override string ToString()
    {
      return $"{CardRank.ToString()} of {CardSuit.ToString()}";
    }

    #endregion

  }
}
