
from enums import *
from entity import Monster, Player

class Fight:
    def __init__(self):
        self.state = FightState.NOT_STARTED
        self.monsters = []
        self.players = []

    def add_entity(self, name, hp, initiative=0, entity_type=EntityType.MONSTER):
        if not name:
            print(f"{entity_type.value} name cannot be empty.")
            return
        
        if hp <= 0:
            print("HP must be greater than 0.")
            return
        
        if name in [monster.name for monster in self.monsters]:
            print(f"A monster with the name '{name}' already exists.")
            return

        if entity_type == EntityType.MONSTER:
            monster = Monster(name, hp, initiative)
            self.monsters.append(monster)
            print(f"Added monster: {name} with HP: {hp} and Initiative: {initiative}")
        elif entity_type == EntityType.PLAYER:
            player = Player(name, hp, initiative)
            self.players.append(player)
            print(f"Added player: {name} with HP: {hp} and Initiative: {initiative}")
        else:
            print("Invalid entity type. Must be 'Monster' or 'Player'.")

    def start_fight(self):
        if self.state != FightState.NOT_STARTED:
            print("Fight has already started or completed.")
            return
        if not self.players:
            print("Cannot start fight without players.")
            return
        if not self.monsters:
            print("Cannot start fight without monsters.")
            return
        self.state = FightState.IN_PROGRESS
        print("Fight started!")

    def find_entity(self, name):
        for monster in self.monsters:
            if monster.name == name:
                return monster
        for player in self.players:
            if player.name == name:
                return player
        return None

    def attack(self, attacker_name, target_name, damage):
        if self.state != FightState.IN_PROGRESS:
            print("Fight is not in progress. Cannot attack.")
            return
        
        attacker = self.find_entity(attacker_name)
        defender = self.find_entity(target_name)

        if not attacker:
            print(f"Attacker '{attacker_name}' not found.")
            return
        if not defender:
            print(f"Defender '{target_name}' not found.")
            return
        
        defender.hp -= damage
        print(f"{attacker.name} attacks {defender.name} for {damage} damage. {defender.name} now has {defender.hp} HP.")

        if defender.hp <= 0:
            print(f"{defender.name} has been defeated!")
            self.remove_entity(defender)
            if not self.players or not self.monsters:
                self.state = FightState.COMPLETED
                print("Fight completed!")

    def remove_entity(self, entity):
        if isinstance(entity, Monster):
            self.monsters.remove(entity)
        elif isinstance(entity, Player):
            self.players.remove(entity)

    def display_status(self):
        print("Current Fight Status:")
        print("Players:")
        for player in self.players:
            print(f"  - {player.name}: {player.hp} HP, Initiative: {player.initiative}")
        print("Monsters:")
        for monster in self.monsters:
            print(f"  - {monster.name}: {monster.hp} HP, Initiative: {monster.initiative}")