/* Summary: 
 *    Each player (user vs. CPU) begins with five cards. Starting with the user, each player takes turns
 *    asking the other for a card (Rank only). If they have it, it is given to the requesting player and
 *    a pair is made and the player's turn continues. Pairs are placed into a separate pile for each player. 
 *    If the player does not have the requested card, the requesting player must 'go fishing' for a pair by 
 *    drawing one card from the top of the deck. Gameplay continues until all cards from the deck are used
 *    and neither player has a remaining card in their hand.
 * 
 * Winner: 
 *    The winner is the player with the most pairs at the end of gameplay.
 * 
 * Special case:
 *    If a player has no cards left in their hand, but there are still cards in the deck, they must draw
 *    either three cards or the number of cards left in the deck, whichever is fewer.
 * 
 */
namespace GoFish
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using Classes;

  class Program
  {

    #region Globals

    // Global variables that will be referenced throughout the program
    private static Deck deck;
    private static List<Card> cpuHand;
    private static List<Card> cpuPairs;
    private static List<Card> playerHand;
    private static List<Card> playerPairs;
    private static Random randInstance = new Random();

    // Constant time durations in miliseconds for the calls to Thread.Sleep()
    private static int STANDARD_PAUSE = 1000;
    private static int LONG_PAUSE = 1500;
    private static int EXTENDED_PAUSE = 2000;

    #endregion

    #region Main

    /// <summary>
    /// Where gameplay takes place
    /// </summary>
    /// <param name="args">unused</param>
    static void Main(string[] args)
    {
      try
      {
        cpuHand = new List<Card>();
        cpuPairs = new List<Card>(); // Stores the pairs for the cpu player
        playerHand = new List<Card>();
        playerPairs = new List<Card>(); // Stores the pairs for the human player

        bool isCpuTurn = false; // Keeps track of whose turn it is
        int selection; // Used to get which card to the CPU ask for

        Console.WriteLine("Welcome to the game of GoFish!");
        Thread.Sleep(LONG_PAUSE);
        Console.WriteLine("Press enter to begin the game...");
        Console.ReadLine();

        Console.WriteLine("Creating deck...");
        Thread.Sleep(STANDARD_PAUSE);
        deck = new Deck();

        Console.WriteLine("\nShuffling deck...");
        Thread.Sleep(LONG_PAUSE);
        deck.Shuffle();

        Console.WriteLine("\nDealing hands...");
        Thread.Sleep(EXTENDED_PAUSE);
        BeginningDeal();

        ShowPlayerHand();
        Thread.Sleep(LONG_PAUSE);
        Console.WriteLine("\nChecking for player pairs...");
        Thread.Sleep(STANDARD_PAUSE);
        FindPairs(ref playerHand, ref playerPairs);

        Console.WriteLine("\nChecking for cpu pairs...");
        Thread.Sleep(STANDARD_PAUSE);
        FindPairs(ref cpuHand, ref cpuPairs);

        // Gameplay loop
        while (true)
        {
          ShowScores();
          ShowRemainingCards();

          if (!isCpuTurn)
          {
            Console.WriteLine("\n--Your Turn--");
            Console.Write("Press enter to begin your turn...");
            Console.ReadLine();
            ShowPlayerHand();
            selection = GetSelection();
            Console.WriteLine($"Asking the CPU for: {playerHand[selection - 1].CardRank}");
            Thread.Sleep(STANDARD_PAUSE);

            if (AskForCard(playerHand, cpuHand, playerHand[selection - 1].CardRank))
            {
              Console.WriteLine("\nYou got it!");
              Thread.Sleep(LONG_PAUSE);
              Console.WriteLine("\nChecking for pairs...");
              Thread.Sleep(STANDARD_PAUSE);
              FindPairs(ref playerHand, ref playerPairs);
            }
            else
            {
              Console.WriteLine("\nGoing fishing...");
              Thread.Sleep(LONG_PAUSE);

              Card newCard = GoFishing();
              if (newCard != null)
                playerHand.Add(newCard);

              ShowPlayerHand();
              Thread.Sleep(STANDARD_PAUSE);
              Console.WriteLine("\nChecking for pairs...");
              Thread.Sleep(STANDARD_PAUSE);
              FindPairs(ref playerHand, ref playerPairs);

              isCpuTurn = !isCpuTurn; // Change turns
            }
          }
          else
          {
            Console.WriteLine("\n--Computer's Turn--");
            Console.Write("Press enter to begin the CPU's turn...");
            Console.ReadLine();
            selection = randInstance.Next(cpuHand.Count);
            Console.WriteLine($"\nCPU is asking you for: {cpuHand[selection].CardRank}");
            ShowPlayerHand();
            Console.Write("\nPress enter to continue...");
            Console.ReadLine();

            if (AskForCard(cpuHand, playerHand, cpuHand[selection].CardRank))
            {
              Console.WriteLine("\nThank you!");
              Thread.Sleep(LONG_PAUSE);
              Console.WriteLine("\nChecking for pairs...");
              Thread.Sleep(STANDARD_PAUSE);
              FindPairs(ref cpuHand, ref cpuPairs);
            }
            else
            {
              Console.WriteLine("\nGoing fishing...");
              Thread.Sleep(LONG_PAUSE);

              Card newCard = GoFishing();
              if (newCard != null)
                cpuHand.Add(newCard);

              Console.WriteLine("\nChecking for pairs...");
              Thread.Sleep(STANDARD_PAUSE);
              FindPairs(ref cpuHand, ref cpuPairs);
              isCpuTurn = !isCpuTurn; // Change turns
            }
          }

          // Check special cases
          if (cpuHand.Count == 0 && deck.Count > 0)
          {
            List<Card> drawn = deck.DrawMore(deck.Count >= 3 ? 3 : deck.Count);

            cpuHand.AddRange(drawn);

            Console.WriteLine($"CPU ran out of cards and drew {drawn.Count} more...");
            Thread.Sleep(LONG_PAUSE);
            Console.WriteLine("\nChecking for pairs...");
            FindPairs(ref cpuHand, ref cpuPairs);
          }
          if (playerHand.Count == 0 && deck.Count > 0)
          {
            List<Card> drawn = deck.DrawMore(deck.Count >= 3 ? 3 : deck.Count);

            playerHand.AddRange(drawn);

            Console.WriteLine($"You ran out of cards and drew {drawn.Count} more...");
            Thread.Sleep(STANDARD_PAUSE);
            ShowPlayerHand();
          }

          // Check if the current player has no remaining cards and there are no cards left in the deck
          if (isCpuTurn && cpuHand.Count == 0 && deck.Count == 0)
          {
            Console.WriteLine("I have no cards in my hand and none to draw. It is now your turn.");
            isCpuTurn = !isCpuTurn;
          }

          if (!isCpuTurn && playerHand.Count == 0 && deck.Count == 0)
          {
            Console.WriteLine("You have no cards in your hand and none to draw. It is now my turn.");
            isCpuTurn = !isCpuTurn;
          }

          // Check for the end of gameplay
          if (cpuHand.Count == 0 && playerHand.Count == 0 && deck.Count == 0)
            break;
        }

        // End game and display who won
        Console.WriteLine("\nGame Over...");
        Thread.Sleep(EXTENDED_PAUSE * 2);
        Console.Clear();
        ShowScores();
        if (getCpuScore() == getPlayerScore())
          Console.WriteLine("\n\n--You tied!--");
        else if (getCpuScore() > getPlayerScore())
          Console.WriteLine("\n\n--You lose!--");
        else
          Console.WriteLine("\n\n--You win!--");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"\n--There was an error--\n{ex.Message}");
      }
      finally
      {
        Console.WriteLine("\nPress Enter to close the window...");
        Console.ReadLine();
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Called to initially provide each player with five cards
    /// </summary>
    private static void BeginningDeal()
    {
      for (int i = 0; i < 5; i++)
      {
        playerHand.Add(deck.DrawOne());
        cpuHand.Add(deck.DrawOne());
      }
    }

    /// <summary>
    /// Displays the cards that are in the player's hand
    /// </summary>
    private static void ShowPlayerHand()
    {
      Console.WriteLine("\nYour hand:");
      int i = 1;
      foreach (Card card in playerHand)
      {
        Console.WriteLine($"{i++}: {card.ToString()}");
      }
    }

    /// <summary>
    /// Displays the scores for both players
    /// </summary>
    private static void ShowScores()
    {
      Thread.Sleep(STANDARD_PAUSE);
      Console.WriteLine($"\n{"Your Score:", -15} {getPlayerScore()}");
      Console.WriteLine($"{"CPU Score:", -15} {getCpuScore()}");
    }

    /// <summary>
    /// Displays the remaining number of cards in the main deck and the CPU's hand
    /// </summary>
    private static void ShowRemainingCards()
    {
      Thread.Sleep(STANDARD_PAUSE);
      Console.WriteLine($"\n{deck.Count} {(deck.Count == 1 ? "card" : "cards")} remaining in the deck.");
      Console.WriteLine($"{cpuHand.Count} {(cpuHand.Count == 1 ? "card" : "cards")} remaining in CPUs hand.");
    }

    /// <summary>
    /// Called to get the CPU's score
    /// </summary>
    /// <returns>The number of 'pairs' the CPU has made</returns>
    private static int getCpuScore()
    {
      return cpuPairs.Count / 2;
    }

    /// <summary>
    /// Called to get the player's score
    /// </summary>
    /// <returns>The number of 'pairs' the player has made</returns>
    private static int getPlayerScore()
    {
      return playerPairs.Count / 2;
    }

    /// <summary>
    /// Prompts the user to select a number that corresponds to a card in their hand
    /// </summary>
    /// <returns>The index of the card selected plus one</returns>
    private static int GetSelection()
    {
      int result;
      Console.Write("\nWhich card would like to try to pair? (Enter the number next to the card) ");
      while (!int.TryParse(Console.ReadLine(), out result) || result > playerHand.Count || result <= 0)
        Console.WriteLine("Invalid input. Please enter the number next to the card you wish to choose.");
      return result;
    }

    /// <summary>
    /// Looks for any two cards whose ranks match. If a pair is found, they are removed
    /// from the hand and added to the pairs.
    /// </summary>
    /// <param name="hand">The hand to search for pairs</param>
    /// <param name="pairs">The 'pile' to which to add any found pairs</param>
    private static void FindPairs(ref List<Card> hand, ref List<Card> pairs)
    {
      bool pairFound = false;

      for (int i = 0; i < hand.Count - 1; i++)
      {
        for (int j = (i + 1); j < hand.Count; j++)
        {
          if (hand[i].CardRank == hand[j].CardRank)
          {
            Card first = hand[i];
            Card second = hand[j];
            /* Because i is less than j, remove card at index j first to keep the card at 
             * index i in the same position. The other way would shift the positions and 
             * require removing (j - 1)*/
            hand.RemoveAt(j); 
            hand.RemoveAt(i);
            pairs.Add(first);
            pairs.Add(second);
            pairFound = true;
            Console.WriteLine("Found a pair...");
            // Since we removed from the list, we need to make sure noting gets skipped
            if (i != 0)
            {
              i--;
              j--;
            }
          }
        }
      }
      if (!pairFound)
        Console.WriteLine("None found...");
    }

    /// <summary>
    /// Requests a <see cref="Card.Rank"/> from the other player. 
    /// If the requested <see cref="Card.Rank"/> exists in their hand,
    /// the <see cref="Card"/> is removed from their hand and added to the hand of the requester.
    /// </summary>
    /// <param name="myHand">The hand of the requester</param>
    /// <param name="yourHand">The hand of the responder</param>
    /// <param name="value">The <see cref="Card.Rank"/> value requested</param>
    /// <returns>true if the requested card exists in yourHand, false otherwise</returns>
    private static bool AskForCard(List<Card> myHand, List<Card> yourHand, Card.Rank value)
    {
      bool foundMatch = false;
      Card foundCard = new Card();

      foreach (var card in yourHand)
      {
        if (card.CardRank == value) // Match!
        {
          foundCard = card;
          myHand.Add(foundCard);
          foundMatch = true;
          break;
        }
      }

      yourHand.Remove(foundCard);
      return foundMatch;
    }

    /// <summary>
    /// Draws a card from the deck if there is at least one card remaining and adds it to the player's hand
    /// </summary>
    /// <param name="hand">A reference to the hand of the player 'fishing'</param>
    private static Card GoFishing()
    {
      if (deck.Count > 0)
        return deck.DrawOne();
      Console.WriteLine("No cards left in the deck...");
      return null;
    }

    #endregion
  }
}
