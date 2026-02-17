
from enum import Enum

class FightState(Enum):
    NOT_STARTED = "Not Started"
    IN_PROGRESS = "In Progress"
    COMPLETED = "Completed"
    ANY = "Any"

class EntityType(Enum):
    PLAYER = "Player"
    MONSTER = "Monster"

    def __new__(cls, value):
        # If it's already an EntityType, return it
        if isinstance(value, cls):
            return value
        
        # During enum creation, just create the object normally
        obj = object.__new__(cls)
        obj._value_ = value
        return obj
    
    @classmethod
    def _missing_(cls, value):
        # This is called when EntityType(value) doesn't match any existing member
        if isinstance(value, str):
            value_lower = value.lower()
            for member in cls:
                if member.value.lower() == value_lower:
                    return member
        raise ValueError(f"Unknown entity type: {value}")

    def __str__(self):
        return self.value
    
    @staticmethod    
    def from_string(s):
        s = s.lower()
        if s == "player":
            return EntityType.PLAYER
        elif s == "monster":
            return EntityType.MONSTER
        else:
            raise ValueError(f"Unknown entity type: {s}")