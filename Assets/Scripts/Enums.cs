public enum ShipClass { Squadron = 1, Frigate = 2, Medium = 4, Heavy = 6, Titan = 8 }
public enum StarSides { NONE, Frente, Atras, LADOS, Derecha, Izquierda, TODOS }
public enum WeaponType { Turret, Battery, Beam, Missile }
public enum ModuleType { PASSIVE, ACTIVE, ROUND_START, SHIPSELECT }

public enum ShipState { WAIT_DEPLOY, DEPLOY, IDLE, MOVING, AIM, FIRING, DEAD }


public enum RoundPhase { ROUND_START, DAMAGE_CALC, ATK_ANIM, DAMAGE_APP, ROUND_END  }
public enum GameState { MENU, PREP, DEPLOY, TACTIC, MOVE_POINT, MODULE_SELECT, TARGET_SELECT, SHOOTING, WAIT }

public enum SelectMode { NONE = 0, ACTIVE_SHIP, MOVE_ACTIVE, ACTIVE_WEAPON, TARGET_SHIP  }