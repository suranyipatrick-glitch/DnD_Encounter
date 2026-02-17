
from enums import *
from exceptions import *

class CommandParameter:
    def __init__(self, name, description, required=True, default=None, param_type=str):
        if required and default is not None:
            raise ValueError("A required parameter cannot have a default value.")
        self.name = name
        self.description = description
        self.required = required
        self.param_type = param_type
        self.default = default

class Command:
    def __init__(self, name, description, required_state, execute_fn, parameters=None):
        self.name = name
        self.description = description
        self.required_state = required_state
        self.execute_fn = execute_fn
        self.parameters = parameters if parameters is not None else []

    def execute(self, fight, parameters):
        if fight.state != self.required_state and self.required_state != FightState.ANY:
            print(f"Cannot execute '{self.name}' command. Current fight state: {fight.state.value}")
            return
        # print(f"Executing '{self.name}' command.")
        for param in self.parameters:
            if param.required and param.name not in parameters:
                print(f"Missing required parameter: {param.name}")
                return
            if not param.required and param.name not in parameters:
                parameters[param.name] = param.default

        for param in parameters:
            if param not in [p.name for p in self.parameters]:
                print(f"Unknown parameter: {param}")
                return
        self.execute_fn(fight, parameters)

class CommandHandler:
    def __init__(self, fight):
        self.fight = fight
        self.commands = {
            "add": Command(
                "add",
                "Add an entity to the fight",
                FightState.NOT_STARTED,
                lambda fight, params: fight.add_entity(
                    name=params["name"],
                    hp=params["hp"],
                    initiative=params.get("initiative", 0),
                    entity_type=params["type"]
                ),
                parameters=[
                    CommandParameter("type", "Type of the entity (Monster or Player)", param_type=EntityType, required=True),
                    CommandParameter("name", "Name of the entity"),
                    CommandParameter("hp", "HP of the entity", param_type=int),
                    CommandParameter("initiative", "Initiative of the entity", param_type=int, required=False, default=0)
                ]
            ),
            "start": Command(
                "start",
                "Start the fight",
                FightState.NOT_STARTED,
                lambda fight, params: fight.start_fight()
            ),
            "attack": Command(
                "attack",
                "Attack an entity",
                FightState.IN_PROGRESS,
                lambda fight, params: fight.attack(
                    attacker_name=params["attacker"],
                    target_name=params["target"],
                    damage=params["damage"]
                ),
                parameters=[
                    CommandParameter("attacker", "Name of the attacking entity"),
                    CommandParameter("target", "Name of the target entity"),
                    CommandParameter("damage", "Amount of damage to deal", param_type=int)
                ]
            ),
            "status": Command(
                "status",
                "Show current fight status",
                FightState.ANY,
                lambda fight, params: fight.display_status()
            ),
            "help": Command(
                "help",
                "Show available commands",
                FightState.ANY,
                self.handle_help
            )
        }

    def handle_help(self, fight, parameters):
        print("Available commands:")
        for command in self.commands.values():
            print(f"- {command.name}: {command.description}")
            if command.parameters:
                print("  Parameters:")
                for param in command.parameters:
                    req = "Required" if param.required else f"Optional (default={param.default})"
                    print(f"    - {param.name}: {param.description} ({req})")

    def parse_command(self, command_str):
        parts = command_str.split()
        if not parts:
            return None, {}
        command_name = parts[0]
        parameters = {}
        for i, part in enumerate(parts[1:]):
            if "=" in part:
                key, value = part.split("=", 1)
            else:
                key = self.commands[command_name].parameters[i].name
                value = part
            try:
                param_type = self.commands[command_name].parameters[i].param_type
                parameters[key] = param_type(value)
            except (IndexError, ValueError) as e:
                print(f"{key} must be of type {param_type.__name__}. Error: {e}")
                raise HandledException(f"Invalid parameter: {key}")

            
        return command_name, parameters
    
    def handle_command(self, command_str):
        try:
            command_name, parameters = self.parse_command(command_str)
        except HandledException as e:
            print(e.message)
            return
        if command_name in self.commands:
            command = self.commands[command_name]
            command.execute(self.fight, parameters)
        else:
            print(f"Unknown command: {command_name}. Type 'help' for a list of commands.")


