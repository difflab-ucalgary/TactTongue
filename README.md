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

# Citation

When using or building upon this device in an academic publication, please consider citing as follows:

Dinmukhammed Mukashev, Nimesha Ranasinghe, and Aditya Shekhar Nittala. 2023. TactTongue: Prototyping ElectroTactile Stimulations on the Tongue. In The 36th Annual ACM Symposium on User Interface Software and Technology (UIST ’23), October 29-November 1, 2023, San Francisco, CA, USA. ACM, New York, NY, USA, 13 pages. https://doi.org/10.1145/3586183.3606829


# Acknowledgements
This library was adapted from work by Kurt Kaczmarek and Joel Moritz Jr.
