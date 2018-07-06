# GoFish
Console based Go Fish game written in C#.Net

Game Play:
  Each player (user vs. CPU) begins with five cards. Starting with the user, each player takes turns
  asking the other for a card (Rank only). If they have it, it is given to the requesting player and
  a pair is made and the player's turn continues. Pairs are placed into a separate pile for each player. 
  If the player does not have the requested card, the requesting player must 'go fishing' for a pair by 
  drawing one card from the top of the deck. Gameplay continues until all cards from the deck are used
  and neither player has a remaining card in their hand.

Winner: 
  The winner is the player with the most pairs at the end of gameplay.

Special case:
  If a player has no cards left in their hand, but there are still cards in the deck, they must draw
  either three cards or the number of cards left in the deck, whichever is fewer.
