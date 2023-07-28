# TactTongue: Prototyping Electrotactile Stimulations on the Tongue

This is the repository that contains the hardware and software ecosystem for the "TactTongue: Prototyping Electrotactile Stimulations on the Tongue" paper (ACM UIST 2023).


# Organization
The repository is organized as follows:

Hardware: this folder contains the design schematics for the Arduino Uno and Nano boards. The designs have been simplified to ensure that there are fewer number of components and parts.

Firmware: contains the Arduino source code for generating the pulses that are sent to the electrode array.

Software: contains the source code for sending stimulation commands to the Arduino. The original TactTongue software has been written in WPF (C#.NET). The software does all the sanity checks before sending the commands to the TactTongue hardware. To ensure a wider adoption, the repository also contains the minimal Python version, allowing for sending commands through a User Interface.


# Note
Due to the concerns about unsafe AC Wall adaptors on the market, we recommend that the device be powered through a computer USB or a battery.

Code cleaning and Optimization are still in progress.

# Acknowledgements
This library was adapted from work by Kurt Kaczmarek and Joel Moritz Jr.
