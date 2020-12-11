using System;
using Unity.Collections;

public enum CharacterState { Idle, Attacking, Hit, Dying, SpecialAttack,Walking, None }
public enum CharacterType { Player, EnemyOne, EnemyTwo, EnemyThree }
public enum EntityType { Character, Effect }
public enum EffectType { BasicAttack }