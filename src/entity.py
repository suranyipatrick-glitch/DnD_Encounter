
class Entity:
    def __init__(self, name, hp, initiative=0):
        self.name = name
        self.hp = hp
        self.initiative = initiative

class Player(Entity):
    def __init__(self, name, hp, initiative=0):
        super().__init__(name, hp, initiative)

class Monster(Entity):
    def __init__(self, name, hp, initiative=0):
        super().__init__(name, hp, initiative)