import time
import sys
import serial
import atexit

# Import PyQt5 libraries
from PyQt5.QtCore import Qt
from PyQt5.QtWidgets import (QApplication, QCheckBox, QGridLayout, QGroupBox,
                             QMenu, QPushButton, QRadioButton, QVBoxLayout, QWidget, QSlider, QLabel, QLineEdit)
from PyQt5 import QtCore, QtGui
from ast import literal_eval


# Set up the serial connection
ser = serial.Serial('COM5', 9600, timeout=1)

# Set the time delay
timeDelay = 0.5

# Create a window class
class Window(QWidget):

    # Initialize the class
    def __init__(self, parent=None):
        super(Window, self).__init__(parent)

        # Create a grid layout
        grid = QGridLayout()

        # Add groups to the grid layout
        grid.addWidget(self.createExampleGroup("1"), 0, 1)
        grid.addWidget(self.createExampleGroup("2"), 0, 2)
        grid.addWidget(self.createExampleGroup("3"), 1, 0)
        grid.addWidget(self.createExampleGroup("4"), 1, 1)
        grid.addWidget(self.createExampleGroup("5"), 1, 2)
        grid.addWidget(self.createExampleGroup("6"), 1, 3)
        grid.addWidget(self.createExampleGroup("7"), 2, 0)
        grid.addWidget(self.createExampleGroup("8"), 2, 1)
        grid.addWidget(self.createExampleGroup("9"), 2, 2)
        grid.addWidget(self.createExampleGroup("10"), 2, 3)
        grid.addWidget(self.createExampleGroup("11"), 3, 0)
        grid.addWidget(self.createExampleGroup("12"), 3, 1)
        grid.addWidget(self.createExampleGroup("13"), 3, 2)
        grid.addWidget(self.createExampleGroup("14"), 3, 3)
        grid.addWidget(self.createExampleGroup("15"), 4, 0)
        grid.addWidget(self.createExampleGroup("16"), 4, 1)
        grid.addWidget(self.createExampleGroup("17"), 4, 2)
        grid.addWidget(self.createExampleGroup("18"), 4, 3)
        grid.addWidget(self.individualParam("PP"), 0,4)
        grid.addWidget(self.individualParam("Pp"), 1,4)
        grid.addWidget(self.individualParam("IN"), 2,4)
        grid.addWidget(self.individualParam("IP"), 3,4)
        grid.addWidget(self.individualParam("ON"), 4,4)
        # Set the layout of the window
        self.setLayout(grid)

        # Set the title and size of the window
        self.setWindowTitle("Electrodes and Intensities")
        self.resize(400, 400)

    def individualParam(self, name):
        groupBox = QGroupBox(name)
        
        # globals()["param" + name] = QSlider(Qt.Horizontal)
        # globals()["param" + name].setFocusPolicy(Qt.StrongFocus)
        # globals()["param" + name].setTickPosition(QSlider.TicksBothSides)
        # globals()["param" + name].setTickInterval(10)
        # globals()["param" + name].setSingleStep(1)
        # globals()["param" + name].setRange(1, 37)
        globals()["textbox" + name] = QLineEdit()
        vbox = QVBoxLayout()
        #vbox.addWidget(globals()["param" + name])
        vbox.addWidget(globals()["textbox" + name])
        vbox.addStretch(1)
        groupBox.setLayout(vbox)

        return groupBox
        

    # Create a group with a checkbox and slider
    def createExampleGroup(self, name):
# Create a group box with the given name
        groupBox = QGroupBox(name)

        # Create a checkbox with the given name
        globals()["electrode" + name]=QCheckBox(name, self)

        # Create a slider with the given name
        globals()["slider" + name] = QSlider(Qt.Horizontal)
        globals()["slider" + name].setFocusPolicy(Qt.StrongFocus)
        globals()["slider" + name].setTickPosition(QSlider.TicksBothSides)
        globals()["slider" + name].setTickInterval(10)
        globals()["slider" + name].setSingleStep(1)
        globals()["slider" + name].setRange(1, 37)

        # Set the checkbox to unchecked
        globals()["electrode" + name].setChecked(False)

        # Connect the slider to the do_action function
        globals()["slider" + name].valueChanged.connect(lambda: self.do_action())

        # Create a label for the result
        globals()["result_label" + name] = QLabel()
        self.labelname = name

        # Create a vertical layout for the group box
        vbox = QVBoxLayout()
        vbox.addWidget(globals()["electrode" + name])
        vbox.addWidget(globals()["slider" + name])
        vbox.addStretch(1)
        vbox.addWidget(globals()["result_label" + name])
        

        groupBox.setLayout(vbox)

        self.button = QPushButton(self)
        self.button.move(8, 20)
        self.button.setText("Show")
        
        return groupBox
    
    def do_action(self):
        
        # Loop through all sliders and get their values
        for i in range (1,19):
            value = globals()["slider" + str(i)].value()
            globals()["result_label" + str(i)].setText(f'I: {value}')

    def show_values(self):
        
        # Initialize data to an empty byte string
        data = b''
        intStr1 = literal_eval(globals()["textbox" + "PP"].text())
        intStr2 = literal_eval(globals()["textbox" + "Pp"].text())
        intStr3 = literal_eval(globals()["textbox" + "IN"].text())
        intStr41 = 123
        if len(globals()["textbox" + "IP"].text()) > 2:
            intStr4 = literal_eval(globals()["textbox" + "IP"].text()[:2])
            intStr41 = literal_eval(globals()["textbox" + "IP"].text()[-1])
        elif len(globals()["textbox" + "IP"].text()) > 0 and len(globals()["textbox" + "IP"].text())<3:
            intStr4 = literal_eval(globals()["textbox" + "IP"].text())
            intStr41 = 123
        else:
            print("IP is empty")
        
        intStr5 = literal_eval(globals()["textbox" + "ON"].text())

        str1 = globals()["textbox" + "PP"].text()
        str2 = globals()["textbox" + "Pp"].text()
        str3 = globals()["textbox" + "IN"].text()
        str4 = globals()["textbox" + "IP"].text()
        str5 = globals()["textbox" + "ON"].text()
        # Loop through all checkboxes and sliders and add their values to data if checked
        #data = data + globals()["textbox" + "PP"].text().encode('utf-8').hex() + globals()["textbox" + "Pp"].text().encode('utf-8').hex() + globals()["textbox" + "IN"].text().encode('utf-8').hex() + globals()["textbox" + "IP"].text().encode('utf-8').hex() + globals()["textbox" + "ON"].text().encode('utf-8').hex()
     
        #data = data + int(globals()["textbox" + "PP"].text(),16).to_bytes(1,byteorder='big') + int(globals()["textbox" + "Pp"].text(),16).to_bytes(1,byteorder='big') + int(globals()["textbox" + "IN"].text(),16).to_bytes(1,byteorder='big') + int(globals()["textbox" + "IP"].text(),16).to_bytes(1,byteorder='big') + int(globals()["textbox" + "ON"].text(),16).to_bytes(1,byteorder='big') 
        hex1 = hex(intStr1)
        hex2 = hex(intStr2)
        hex3 = hex(intStr3)
        hex4 = hex(intStr4)
        hex5 = hex(intStr5)
        
        if intStr41 == 0:
            intStr41=1
            
        data = data + intStr1.to_bytes(1,byteorder='big') + intStr2.to_bytes(1,byteorder='big') + intStr3.to_bytes(1,byteorder='big') + intStr4.to_bytes(1,byteorder='big') + intStr41.to_bytes(1,byteorder='big') + intStr5.to_bytes(1,byteorder='big')
        print(intStr41.to_bytes(1,byteorder='big'))

        
        
        for i in range(1,19):
            if  globals()["electrode" + str(i)].isChecked():
                data = data + i.to_bytes(1,byteorder='big') #+ b'\x00' + globals()["slider" + str(i)].value().to_bytes(1,byteorder='big')
        

        data += b'\x00'
        
        # Write data to serial port and sleep for timeDelay seconds
        ser.write(data)
        print(data)
        print(len(data))
        response = ser.readline().decode()
        print("response:" + response)
        time.sleep(timeDelay)
        while(len(data)!=response):
            
            ser.write(data)
            
            response = int(float(ser.readline().decode()))
            time.sleep(timeDelay)
            if len(data)==response:
                print("sending again")
                break
    
        print("all good")
        
        
       
        
def exit_handler():
    
        # Write '\x00\x00' to serial port when exiting program
        ser.write( str.encode("\x00\x00"))

if __name__ == '__main__':
    
    # Initialize PyQt5 application and window
    app = QApplication(sys.argv)
    clock = Window()
    clock.button.clicked.connect(clock.show_values)
    clock.show()
    atexit.register(exit_handler)
    sys.exit(app.exec_())



