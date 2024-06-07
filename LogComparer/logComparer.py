import argparse
import shutil
import re

def parseSaspCurrentGoalCommand(log: str):
    goal = re.sub(r".*CALL \(\d+\): current goal is ", "", log)
    return f"{goal.strip()} :Current Goal"

def parseApollonCurrentGoalCommand(log: str):
    goal = re.sub(r".*current goal is: ", "", log.lower())
    return f"Current Goal: {goal}".strip()

def getSaspCommands(logData):
  commands = []
  index = 0
  for line in logData:
    index += 1
    if "current goal is" in line.lower():
      commands.append(f"{parseSaspCurrentGoalCommand(line)} Line {index} ")
  
  return commands

def getApollonCommands(logData):  
  commands = []
  index = 0
  for line in logData:
    index += 1
    if "current goal is" in line.lower():
      commands.append(f"Line {index} {parseApollonCurrentGoalCommand(line)}")
      
  return commands

def printCommands(goal1, goal2, index, saspOffset):
    terminal_width = shutil.get_terminal_size().columns
    half_width = terminal_width // 2
    separator = '|'

    formatted_goal1 = f"{goal1[:half_width - 2]:>{half_width - 2}}"
    formatted_goal2 = f"{goal2[:half_width - 2]:<{half_width - 2}}"
    commandIndex = f"Command {index + 1} Offset {saspOffset}"

    print(f"{commandIndex:^{terminal_width - 1}}")
    print(f"{formatted_goal1} {separator} {formatted_goal2}")

def main(apollon, sasp, skip):
    with open(apollon, 'r') as file1, open(sasp, 'r') as file2:
        apollonLogs = file1.readlines()
        saspLogs = file2.readlines()

    apollonCommands = getApollonCommands(apollonLogs)
    saspCommands = getSaspCommands(saspLogs)

    saspOffset = 0
    for i in range(min(len(apollonCommands), len(saspCommands))):
        printCommands(apollonCommands[i], saspCommands[i + saspOffset], i, saspOffset)
        if i + 1 >= skip:
          offset = input()
          if offset.isdecimal():
            saspOffset = int(offset)



if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Compare two log files to find where they diverge in their 'current goal is' lines.")
    parser.add_argument("apollon", help="Path to the apollon log file")
    parser.add_argument("sasp", help="Path to the sasp log file")
    parser.add_argument("--skip", "-s", help="Skip the first n commands of the log files", type=int, default=0)
    args = parser.parse_args()

    main(args.apollon, args.sasp, args.skip)
