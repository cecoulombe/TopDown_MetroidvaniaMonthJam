/* Rework the enemy AI to hopefully make it feel better as a player and seem as 
 * though the enemies are making decisions and acting accordingly
 * 
 * Each state will first be made as a parent class and then each specific enemy type will use children of that class so that they can have nuances that are specific to that class
 * 
 * Each enemy type (broad classes and specific enemies probably) will have a state machine with 
 * the following states (each point will refer to the movement options below it depending on the specific enemy)
 *      * idle/not aggro (with an animation)
 *          * moves out of idle state when the health is not 100% or when player is in the detectable range
 *          * moves into idle state when the player leaves the detected range and they enemy has full health
 *      * aggro-ed behaviours (will cycle through each of the states below based on cooldown and player position, this is where the majority of time will be spent)
 *              * two way relationship with idle state (described above), one way relationship with death state, otherwise it will stay in the group of aggro states
 *          * wake up (when they first notice the behaviour)
 *              * plays when the enemy first enters the aggro state from idle and not again unless they leave to idle and return to aggro)
 *              * an anim during a very short delay to show that they have noticed the player
 *          * movement (depending on what enemy it is, they will always do one type of movement when they are in the movement phase) 
 *              * deciding which way to move 
 *                  * options include walk towards player 
 *                  * walk away from player
 *                  * maintain a specific distance from player (combination of towards and away movement)
 *                      * will have different states then the other enemies because there needs to be a specific measure of how close they are to the player, which will not be in the other states (can probably use inference and add the spacing to the child to determine which state to be in))
 *                  * random movement
 *                          * wont be a separate state for bouncing, just a different movement class because it wont care about the player
 *                      * bounce off walls and other enemies
 *                      * phase through walls and other enemies   
 *                  * path movement (can be the same movement as before aggro but make sure that they are in the aggro phase)
 *          * attacking
 *                  * goes into attack after the cooldown timer has ended, which is also when the attack type is determined if they have mutliple attacks
 *                  * may depend on the enemy, but most will not be interupted if they take damage while attacking 
 *              * telegraph (anim)
 *                      * happens as soon as the enemy transition into attacking (there will be one telegraph per attack, so if they have multiple attacks, they will have multiple telegraphs)
 *                      * changing state to an attack will transition into the telegraph, which will then transition into the attack
 *                  * attacking
 *                          * each type of attack will have a parent but these are likely to have themost variety between enemies in order to make it feel like the enemies are unique
 *                      * punish window (cool down from the attack)
 *                          * short pause where the enemy is open for a punish (anim)
 *          * healing (if the enemy type of able to)
 *                  * 
 *              * anim for healing
 *              * punish window after
 *          * cooldown between attacks/healing (attacks will most likely have a range of cooldown which will have some variability, but this is not the punish window)
 *              * not a state because they will go into movement after the punish window, but maybe there will be a small cooldown pause or this will just start a timer
 *          * deaggro (anim)
 *              * only plays when leaving the aggrod state into the idle state (which will only happen if the player is out of range and the enemy health is 100%)
 *     * death (anim)
 *          * happens when health is <= 0, regardless of what state they are in, this will always have the highest priority
 *          * the only way out of the death state is into drops (which will also "despawn" the enemy)
 *     * drops (will likely be reused between all enemy types
 *          * enters from death, no other transitions in or out because this will turn off the enemy
 *          * disperses the drops based on the enemy size (i.e. random within 0.5x of the center)
 * 
 */