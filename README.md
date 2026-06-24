# 3-in-1-LP2B
Welcome to the repository of the 3-in-1-LP2 project, a retro arcade compilation app developed entirely using Unity.


 Each game has an explanation of the rules as a video
  
## 1. Apple Catcher
  
  The player controls the catchboy on a horizontal axis to catch as many falling apples as possible and achieve the highest score before the number of lives is under 0.
  
  ### Controls:
   Left and Right arrow keys.
  
  ### Mechanics & Scoring:
   
    The player starts with 3 lives. Missing a normal apple costs 1 life.
    Normal Apple = +1 pt | Golden Apple = +2 pts | Apple Core = -1 pt.
    A streak multiplier applies if catches are chained together. The streak breaks if you lost normal apple or by catching an apple core / rotten apple.
  
  ### Special Apple Physics: These appear randomly and feature unique inverted falling physics:
  
      Angel Apple: Grants +1 life. Descends slowly down to a specific height, then accelerates.
      Golden Apple: Boosts basket speed. Follows the same slow-then-fast physics pattern.
      Apple Core: Reduces basket speed. Descends rapidly down to height, then slows down.
      Rotten Apple: Inverts movement controls. Follows the same fast-then-slow physics pattern.


## 2. Brick Breaker

A dynamic brick-breaking game where the player must destroy a grid of bricks randomly generated.

### Controls:
 Left and Right arrow keys to move the paddle.
 
### Core Mechanics:
 The ball inflicts 1 point of damage per impact. If the ball falls below the screen, the player loses a life. The game ends when the life counter reaches zero.
 
### Brick Types:
 Simple (1 HP), Hard (2 HP), Giga Hard Block (6 HP), and the Lucky Block (4 HP).
 
### The 7 Random Effects of the Lucky Block:
 Buffs: +1 life, paddle speed boost, paddle size enlargement, or One-shot mode (instant brick destruction).
 Debuffs: Critical ball speed acceleration or healing ball (restores 1 HP to any brick it hits).
 Mixed (Multi-ball): Splits the ball into two. 50% chance they share a single life pool; 50% chance each ball acts completely independently with its own life cost. 
 
Special Feature (Innovation): Includes a surprise special level every 5 completed stages.
 
## 3. The Shooter (Mini UFO Attack)

The main centerpiece of this compilation. A tactical horizontal shoot 'em up / tower defense game where the player pilots a spaceship to defend their base from incoming progressive enemy waves.

### Controls:

#### Arrow Keys: Free movement in all 4 directions (WASD is not configured, controls are centered around the arrows).
 Spacebar: Hold down to fire the main laser machine gun continuously.Mouse Click: Mandatory interaction required to collect and activate floating power-ups on the screen.
 
### Entity Management:
 
 The Base (Left Border): Has 300 HP. If it takes no damage for 5s, it activates a passive regeneration of 5 HP/s.The Player Ship: Has 10 HP. If destroyed, a respawn cutscene acts as a blocking cooldown before the ship reappears.
 
 The ship blinks for 1 to 2 seconds after taking damage to indicate temporary invincibility.
 ### The 3 Horizontal Tactical Zones:
 Zone 1 (Spawn - Right): Enemies spawn and advance in a straight line.
 Zone 2 (Combat - Middle): Enemies activate their offensive attack patterns. Specifically, Monster 5 locks onto the horizontal axis and moves vertically to align with the player's ship, charging up to fire its massive super laser.
 Zone 3 (Assaut - Left): Enemies ignore patterns and charge straight towards the base. Any enemy reaching the wall crashes, dealing its remaining HP plus its attack power directly to the base.Power-up System (Click-to-Collect Drops):

 ### Powerups:
 Shield (Blue): Spawns an arc-shaped barrier in front of the ship for 5s, protecting half the field.
 Fire Rate (Yellow): Boosts the weapon's fire rate.
 Speed (Purple): Boosts the ship's movement speed.
 Heal (Red): Instantly restores HP to the ship.
 Bomb (Green): A special attack (bomb) clears the screen after the player collects 4 green powerups.

 ### Monsters:
 LaserShooter: keeps a distance from the player ( Zone 1) and shoots the laser
 Kamikaze: Follows the player and explodes on contact
 Energy beam thrower: keeps a distance from the player and shoots a beam that paralyzes the player on contact
 Flame Thrower: follows the player while throwing bullets (not flames because we did not have the assets for that)
 Bullet Shower: goes in a single line and shoots quick bullets
 Boss: Has all the capabilities of the other monsters except the kamikaz and the flame thrower. Has another special attack not for the other players
 
 ### Dedicated Pause Menu:
  Available only in the Shooter game ( we used scenes for the other ones), accessing this menu freezes gameplay. It displays the live score, current lives, and active power-up cooldown timers. It offers 4 actions: Resume, Restart, Return to main menu, or Quit application.
