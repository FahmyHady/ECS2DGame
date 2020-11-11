using System;
using Unity.Collections;

public enum CharacterState { Idle, Attacking, Hit, Dying, None }
public enum CharacterType { Player, EnemyOne, EnemyTwo }
public enum EntityType { Character, Effect }
public enum EffectType { BasicAttack }