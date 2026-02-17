

from command import CommandHandler
from enums import FightState
from fight import Fight
import readline

if __name__ == "__main__":
    fight = Fight()
    command_handler = CommandHandler(fight)
    
    while fight.state != FightState.COMPLETED:
        try:
            command = input("> ").strip()
            if command.lower() == "quit":
                break
            if command:  # Only process non-empty commands
                command_handler.handle_command(command)
        except (EOFError, KeyboardInterrupt):
            print("\nExiting...")
            break
    print(f"Final fight state: {fight.state.value}")