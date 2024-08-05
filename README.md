# Nine Men's Morris Games

## Overview
This repository contains three implementations of the Nine Men's Morris game, each developed using a different technology: WinForms, WPF, and MAUI. These applications allow two players to enjoy the Nine Men's Morris on a virtual board. The game is played on a unique 24-position board with three nested squares, each having eight positions at the corners and midpoints. These squares are connected at the midpoints, creating an interconnected grid. Each player has 9 pieces, and they take turns placing and moving these pieces. The goal is to remove the opponent's pieces by forming mills — three of your pieces in a row.

## Gameplay Phases
1. Placement Phase:
   - Players take turns placing their pieces on empty positions on the board.
   - The goal is to form mills while strategically positioning pieces for the movement phase.
2. Movement Phase:
   - Once all pieces are placed, players take turns moving their pieces to adjacent connected positions.
   - Forming mills during this phase allows players to remove opponent pieces.
   - Players must pass their turn if no valid moves are available.
3. Final Phase
   - Once a player has only 3 pieces left, they can move them to any available position (not limited to adjacent positions).
   - If a player has fewer than 3 pieces, they lose the game.

## Additional Features
- Mill Formation and Piece Removal
  - After forming a mill, players can remove one of the opponent's pieces.
  - Only pieces that are not part of a mill can be removed, except when all remaining opponent pieces are part of mills.
- Game Management:
  - Players can save the current game state.
  - Players can load a previously saved game.
  - Players can start a new game.
  - Players can exit the game.
- Turn Indicator and Notifications
  - A text indicator shows whose turn it is, the removal phase, invalid moves, and the winner.
  - Notification shows the end of the game.

## Platforms
This Nine Men's Morris game has been developed using the following technologies:
### **WinForms**
- Architecture: Implemented using a three-tier architecture. The presentation layer is in the NineMensMorris namespace, the model is in the ModelNineMenMorris.Model namespace, and the persistence is in the ModelNineMenMorris.Persistence namespace.
- Project Structure: The solution is divided into two projects for implementation considerations: the Persistence and Model packages are in a platform-independent project, while the NineMensMorris package is in a Windows Forms-dependent project.
### **WPF**
- Architecture: Implemented using the MVVM architecture. The presentation layer is in the NMMView namespace, the view model is in the NMMView.ViewModel namespace, the model is in the ModelNineMenMorris.Model namespace, and the persistence is in the ModelNineMenMorris.Persistence namespace.
- Project Structure: The solution is divided into two projects for implementation considerations: the Persistence and Model packages are in a platform-independent project, while the NMMView package is in a WPF-dependent project.
### **MAUI**
- Architecture: Implemented using the MVVM architecture. The presentation layer is in the NMM.View namespace, the view model is in the NMM.ViewModel namespace, the model is in the NMMModel.Model namespace, and the persistence is in the NMMModel.Persistence namespace.
- Project Structure: The solution consists of two projects: a .NET Standard class library containing the model and persistence, and a .NET MAUI multi-platform project that can be built for both Windows and Android operating systems.
